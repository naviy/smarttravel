using System;
using System.IO;
using System.Linq;
using System.ServiceModel;

using Luxena;


namespace AmadeusRSAKeys
{
	class Program
	{

		static void Main(string[] args)
		{
			var config = Config.Default;

			if (args.Length < 2)
			{
				args = new[] { "20200206", "20200319" };
				//Console.WriteLine("Необходимо указать параметры: oldFolder newFolder [agency]");
			}

			var oldFolder = Path.Combine(config.ProfilesFolder, args.By(0));
			var newFolder = Path.Combine(config.ProfilesFolder, args.By(1));
			var selectedAgency = args.By(2);

			var agencies = selectedAgency.Yes()
				? config.Agencies.Where(a => a.Name == selectedAgency).ToList()
				: config.Agencies
			;

			var client = new RsaKeysService.rsa_keyPortTypeClient(
				new BasicHttpsBinding
				{
					Name = "rsa_keyBinding", /*MaxReceivedMessageSize = 0xfffffff*/
				},
				new EndpointAddress("https://webservices.bmp.viaamadeus.com/rsa_key.php")
			);

			foreach (var agency in agencies)
			{
				Console.WriteLine(agency.Name.ToUpper());


				var oldKeyPath = Path.Combine(oldFolder, agency.Name, "conf", "amadeus-sftp.txt");
				var newKeyPath = Path.Combine(newFolder, agency.Name, "conf", "amadeus-sftp.txt");
				var newTokenPath = Path.Combine(newFolder, agency.Name, "conf", "tokens.txt");
				Console.WriteLine("old: " + oldKeyPath);
				Console.WriteLine("new: " + newKeyPath);

				try
				{
					var newToken = client.get_new_token(agency.BmpTerminal, agency.Token);
					Console.WriteLine("newToken: " + newToken);
					File.AppendAllText(newTokenPath, agency.Name + ": " + newToken + "\r\n");

					var oldKey = File.ReadAllText(oldKeyPath);
					var newKey = File.ReadAllText(newKeyPath);

					var sessionId = client.login_token(agency.BmpTerminal, newToken);
					Console.WriteLine("sessionId: " + sessionId);

					//var result = client.changeKeyToken(sessionId, oldKey, newKey);
					//Console.WriteLine("changeKeyToken: " + result);

				}
				catch (Exception ex)
				{
					Console.WriteLine("ERROR: " + ex.FullMessage());
				}
				Console.WriteLine();
			}

			Console.WriteLine("ALL DONE");
			Console.ReadKey();
		}

	}
}
