using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Exceptions
{

	[Serializable]
	public class EntityAlreadyExistsException : Exception
	{
		public EntityAlreadyExistsException() { }
		public EntityAlreadyExistsException(string message) : base(message) { }
		public EntityAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
	}
}
