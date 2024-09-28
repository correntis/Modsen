using Library.Application.UseCases.Auth;
using Library.Application.UseCases.Authors;
using Library.Application.UseCases.Books;
using Library.Application.UseCases.Users;

namespace Library.API.Extensions
{
    public static class UseCasesExtensions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.GetName().Name == "Library.Application");

            if (applicationAssembly == null)
            {
                return;
            }

            var useCasesTypes = applicationAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("UseCase") && t.IsClass);

            foreach (var useCase in useCasesTypes)
            {
                services.AddScoped(useCase);
            }
        }
    }
}
