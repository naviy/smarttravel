using System.Collections.Generic;




namespace AmadeusRSAKeys
{



	//===g




	public class Config
	{

		public static Config Default = new Config
		{
			ProfilesFolder = @"D:\data\git\Luxena.Travel\profiles\_amadeus-sftp\",

			Agencies =
			{
				//new Agency("atlastour", "11395", "b41f66611b903c58b5a7fbcaf9c4a29d570bffd3a35e1244c54f8be4"),
					//new Agency("bsv", "10583",  "f605a6889d3f7615f134815d33984f26f337d70035cb573e3a9f1968"),
				//new Agency("egoist", "7969",  "88317642a600f2868ebb31a546ffc817b179eb79ae61f5d315a52dd6"),
				////new Agency("fgr", "1229",  ""), // 494
				//new Agency("persey", "28474",  "d566945d5bfca3d4ffa3db6af9fcc51f816a2b4985dcc5f5c57de"), //7974
				new Agency("ufsa", "510",  "cd8e484949d7939cea910f711d093a49aa541ff2c864634d43226b7d"),
				//new Agency("utb", "9907",  "4ba478d50c8aa7fb6a94e315f6bde357c1af2be4235c3062235ed9cb"),
			}
		};


		public string ProfilesFolder { get; set; }

		public List<Agency> Agencies { get; set; } = new List<Agency>();

	}




	public class Agency
	{

		public Agency(string name, string bmpTerminal, string token)
		{
			Name = name;
			BmpTerminal = bmpTerminal;
			Token = token;
		}


		public string Name { get; set; }

		public string BmpTerminal { get; set; }

		public string Token { get; set; }

	}




	//===g



}
