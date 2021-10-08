namespace Luxena.Travel.Domain
{

	public class AuthenticationToken
	{

		public AuthenticationToken(string userName, string sessionId)
		{
			UserName = userName;
			SessionId = sessionId;
		}

		public string UserName { get; private set; }

		public string SessionId { get; private set; }


		public override string ToString()
		{
			return UserName + ";" + SessionId;
		}


		public static AuthenticationToken Parse(string str)
		{
			var parts = str.Split(';');

			return parts.Length != 2 ? null : new AuthenticationToken(parts[0], parts[1]);
		}

	}

}