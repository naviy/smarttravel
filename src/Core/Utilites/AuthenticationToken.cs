namespace Luxena.Travel.Security
{
	public class AuthenticationToken
	{
		public AuthenticationToken(string userName, string sessionId)
		{
			_userName = userName;
			_sessionId = sessionId;
		}

		public string UserName => _userName;

		public string SessionId => _sessionId;

		public static AuthenticationToken Parse(string str)
		{
			var parts = str.Split(';');

			return parts.Length != 2 ? null : new AuthenticationToken(parts[0], parts[1]);
		}

		public override string ToString()
		{
			return _userName + ";" + _sessionId;
		}

		private readonly string _userName;
		private readonly string _sessionId;
	}
}