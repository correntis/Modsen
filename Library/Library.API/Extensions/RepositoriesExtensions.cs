using Library.Core.Abstractions;
using Library.DataAccess;
using System.Reflection;

namespace Library.API.Extensions
{
    public static class RepositoriesExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            var dataAccessAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.GetName().Name == "Library.Application");

            if(dataAccessAssembly == null)
            {
                return;
            }

            var repositoryTypes = dataAccessAssembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                            t.GetInterfaces().Any(i => i.Name.EndsWith("Repository")));

            foreach(var repository in repositoryTypes)
            {
                var interfaceType = repository.GetInterfaces().First(i => i.Name.EndsWith("Repository"));
                services.AddScoped(interfaceType, repository);
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
