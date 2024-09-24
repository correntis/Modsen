namespace Library.Core.Exceptions
{

	[Serializable]
	public class InvalidPasswordException : Exception
	{
		public InvalidPasswordException() { }
		public InvalidPasswordException(string message) : base(message) { }
		public InvalidPasswordException(string message, Exception inner) : base(message, inner) { }
	}
}
