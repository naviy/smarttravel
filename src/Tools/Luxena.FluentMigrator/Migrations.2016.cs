using FluentMigrator;
// ReSharper disable InconsistentNaming


namespace Luxena.Travel.DbMigrator
{


	[Migration(2016022608)]
	public class Migration_2016022608 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_airplane_model")
				.AsEntity3()
				.WithColumn("iatacode").AsText().NotNullable().Unique()
				.WithColumn("icaocode").AsText().Nullable()
			;
		}
	}

	[Migration(2016022609)]
	public class Migration_2016022609 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
insert into lt_airplane_model (id, version, createdby, createdon, iatacode, icaocode, name) values
('4999047ecab64bab8854347c75f2bc81', 1, 'SYSTEM', current_timestamp, '100','F100','Fokker 100'),
('b78f6a871ad849229115ce9255e13d21', 1, 'SYSTEM', current_timestamp, '141','B461','British Aerospace BAe 146-100'),
('3319eb6847f6446ab0ef986d0ec186f8', 1, 'SYSTEM', current_timestamp, '142','B462','British Aerospace BAe 146-200'),
('c765f452273f4f88b64a58863c7b5c0a', 1, 'SYSTEM', current_timestamp, '143','B463','British Aerospace BAe 146-300'),
('3d53ce001a2d41c68511c277bfe3df05', 1, 'SYSTEM', current_timestamp, '146','BA46','British Aerospace BAe 146'),
('d6e2a8830bfd42f1825e5dd882665968', 1, 'SYSTEM', current_timestamp, '14F','BA46','British Aerospace BAe 146 Freighter'),
('52ab1b9f8d4747dab3cce5272c685dcc', 1, 'SYSTEM', current_timestamp, '14X','B461','British Aerospace BAe 146-100QT/QC'),
('cd22363542b74e8392f14b171d8e76c8', 1, 'SYSTEM', current_timestamp, '14Y','B462','British Aerospace BAe 146-200QT/QC'),
('247fbb4ec54f4a558f3d5baf96c6a4bb', 1, 'SYSTEM', current_timestamp, '14Z','B463','British Aerospace BAe 146-300QT/QC'),
('628c093e34c4478a893180335e2d317f', 1, 'SYSTEM', current_timestamp, '310','A310','Airbus A310'),
('0b3db1250a9f4dbd9890a26994753089', 1, 'SYSTEM', current_timestamp, '312','A312','Airbus A310-200'),
('00903535095c4cb9aa81e4f23ec797e7', 1, 'SYSTEM', current_timestamp, '313','A313','Airbus A310-300'),
('40477aa85d0342f5923951bf8292fe50', 1, 'SYSTEM', current_timestamp, '318','A318','Airbus A318'),
('e7612eccf2fd4d1abffc78fd8123a7cd', 1, 'SYSTEM', current_timestamp, '319','A319','Airbus A319'),
('d1f7fbf85301483584e787210708c06a', 1, 'SYSTEM', current_timestamp, '31F','A310','Airbus A310 Freighter'),
('451ee15d768144469255b3fcbcbe9800', 1, 'SYSTEM', current_timestamp, '31X','A312','Airbus A310-200 Freighter'),
('b3b696ea6ee942949ede0b2786083f8e', 1, 'SYSTEM', current_timestamp, '31Y','A313','Airbus A310-300 Freighter'),
('c31775498d3c4b0390782c911d6929bd', 1, 'SYSTEM', current_timestamp, '320','A320','Airbus A320'),
('7129293db2d04400a5fcc87afae7cf54', 1, 'SYSTEM', current_timestamp, '321','A321','Airbus A321'),
('5d2ec7320a254f7587f955f1af3dd173', 1, 'SYSTEM', current_timestamp, '32S',null,'Airbus A318/319/320/321'),
('81a8b04400c3426c85b786b121e4bda2', 1, 'SYSTEM', current_timestamp, '330','A330','Airbus A330'),
('dff40b39e10e4b74961259f1495deab8', 1, 'SYSTEM', current_timestamp, '332','A332','Airbus A330-200'),
('a9d1acabd6f54d51af16017e57821428', 1, 'SYSTEM', current_timestamp, '333','A333','Airbus A330-300'),
('d2b223a96f60458f976660c65c201b5b', 1, 'SYSTEM', current_timestamp, '340','A340','Airbus A340'),
('7cb1aa9a3315424f9f437f31d07448cd', 1, 'SYSTEM', current_timestamp, '342','A342','Airbus A340-200'),
('a353b83553764645becfcbd9ffebf797', 1, 'SYSTEM', current_timestamp, '343','A343','Airbus A340-300'),
('3de110dcdeea4fd7a11917bc3a187983', 1, 'SYSTEM', current_timestamp, '345','A345','Airbus A340-500'),
('1deabd67f83541a797809bb28a097bae', 1, 'SYSTEM', current_timestamp, '346','A346','Airbus A340-600'),
('91405771d0fa4980a0211c80287c5e1c', 1, 'SYSTEM', current_timestamp, '380','A380','Airbus A380'),
('137877db95ba43c18bee04594def7a7f', 1, 'SYSTEM', current_timestamp, '388','A380','Airbus A380-800'),
('d656aa00114c431b94808993b5cdac7a', 1, 'SYSTEM', current_timestamp, '38F','A38F','Airbus A380 Freighter'),
('2aeaa8649cf44df3ad8a5df13e774875', 1, 'SYSTEM', current_timestamp, '703','B703','Boeing 707-300'),
('8540086c1d9e4f77b592bb8d5a6e2d1a', 1, 'SYSTEM', current_timestamp, '707','B707','Boeing 707/720'),
('9e13e68052f54d46b1025c9e500d71fd', 1, 'SYSTEM', current_timestamp, '70F','B703','Boeing 707-300 Freighter'),
('69be3eb8b2cd4cada3e64af0c75c594d', 1, 'SYSTEM', current_timestamp, '70M','B703','Boeing 707-300 Combi'),
('6ecbb55abc0f47efa04e86a1762a8fdc', 1, 'SYSTEM', current_timestamp, '717','B712','Boeing 717-200'),
('d40478f14ca348a99070c36f59b09516', 1, 'SYSTEM', current_timestamp, '721','B721','Boeing 727-100'),
('64699626447c48bc8c5e590231bd9f4f', 1, 'SYSTEM', current_timestamp, '722','B722','Boeing 727-200'),
('1d3bd986a8554bbab89ee6e7ed1d4c59', 1, 'SYSTEM', current_timestamp, '727','B727','Boeing 727'),
('7bbfb18e1ceb4d538c1cc8e70ad83077', 1, 'SYSTEM', current_timestamp, '72A','B727','Boeing 727-200 Advanced'),
('65a28030c3a940b1aa19b506310bb9f1', 1, 'SYSTEM', current_timestamp, '72B','B721','Boeing 727-100 Combi'),
('86ee807f5843456a8ba47890beebd368', 1, 'SYSTEM', current_timestamp, '72C','B722','Boeing 727-200 Combi'),
('dbcb60a144304782bf6413f34b1558ab', 1, 'SYSTEM', current_timestamp, '72F','B727','Boeing 727 Freighter'),
('2d24ea4915114b1bb589380bd468577f', 1, 'SYSTEM', current_timestamp, '72M','B721','Boeing 727 Combi'),
('cc34782e56e545a98a8235a9482f1bd1', 1, 'SYSTEM', current_timestamp, '72S','B722','Boeing 727-200'),
('16b97d6e6fa64959adc263251a367deb', 1, 'SYSTEM', current_timestamp, '72X','B721','Boeing 727-100 Freighter'),
('b7e9f8d113a146e78760082176896806', 1, 'SYSTEM', current_timestamp, '72Y','B722','Boeing 727-200 Freighter'),
('1b544a8dff974467a94bffedb6e34fc1', 1, 'SYSTEM', current_timestamp, '731','B731','Boeing 737-100'),
('0ef11b398de24652adb27192799a1314', 1, 'SYSTEM', current_timestamp, '732','B732','Boeing 737-200'),
('3755ef626ba346d0bc58cf826358297a', 1, 'SYSTEM', current_timestamp, '733','B733','Boeing 737-300'),
('8f64c3d1739e4424b9c857de10ddf6f8', 1, 'SYSTEM', current_timestamp, '734','B734','Boeing 737-400'),
('9486a5a3158c4ef1b8f76e5337fa3fee', 1, 'SYSTEM', current_timestamp, '735','B735','Boeing 737-500'),
('7f81c3f660af4b37b5abb2ab826ab99d', 1, 'SYSTEM', current_timestamp, '736','B736','Boeing 737-600'),
('45e992db0cbe4bc980954eeb1b99b2c7', 1, 'SYSTEM', current_timestamp, '737',null,'Boeing 737'),
('9d0d70a53f79476a88f9e5c8db7cd8b7', 1, 'SYSTEM', current_timestamp, '738','B738','Boeing 737-800'),
('3067d45917fa4f6ea48e44a697fa1787', 1, 'SYSTEM', current_timestamp, '739','B739','Boeing 737-900'),
('d41e8e3530d1443f89abedbb7df1a2cd', 1, 'SYSTEM', current_timestamp, '73A','B732','Boeing 737-200/200C Advanced'),
('1c6204162d2e4d87acaf3d9ad09bcc3d', 1, 'SYSTEM', current_timestamp, '73C','B733','Boeing 737-300'),
('9c8f6cc909cb4b56afc07ae6e391095f', 1, 'SYSTEM', current_timestamp, '73F','B731','Boeing 737 Freighter'),
('021e23cb7eaa42d18f6336484d1b04b1', 1, 'SYSTEM', current_timestamp, '73G','B737','Boeing 737-700'),
('b47af01a869d423b9b2b9fedc19344e5', 1, 'SYSTEM', current_timestamp, '73H','B738','Boeing 737-800 With Winglets'),
('c8d4ab58c5874338a6415acba553c1e1', 1, 'SYSTEM', current_timestamp, '73M','B732','Boeing 737-200 Combi'),
('a428ebe5c56842e1868aaee4f6e8002c', 1, 'SYSTEM', current_timestamp, '73P','B734','Boeing 737-400 Freighter'),
('c508560fa859499782b86698cb833795', 1, 'SYSTEM', current_timestamp, '73S',null,'Boeing 737 Advanced'),
('549cc708c36e49e4955cd2fdf5318f58', 1, 'SYSTEM', current_timestamp, '73W','B737','Boeing 737-700 With Winglets'),
('9aaf0fed14754321ab233b28134ca1d1', 1, 'SYSTEM', current_timestamp, '73X','B732','Boeing 737-200 Freighter'),
('9137097e03974e0ca4879a901e2fca83', 1, 'SYSTEM', current_timestamp, '73Y','B733','Boeing 737-300 Freighter'),
('2d550e51f861406cbc593d40eaea2408', 1, 'SYSTEM', current_timestamp, '741','B741','Boeing 747-100'),
('151f2294a33d4ea295fc1df4ef3f8d43', 1, 'SYSTEM', current_timestamp, '742','B742','Boeing 747-200'),
('6d39c8c27e434423bcf9b6314806e07c', 1, 'SYSTEM', current_timestamp, '743','B743','Boeing 747-300 (including -100SUD and -200SUD)'),
('0b4b483e68a94bdb824dde258946de1c', 1, 'SYSTEM', current_timestamp, '744','B744','Boeing 747-400'),
('2a4e9c7186ba4925ba1e5a08910f3913', 1, 'SYSTEM', current_timestamp, '747','B741','Boeing 747'),
('2283d5d18dd246ecbf6bd41cbf109f2e', 1, 'SYSTEM', current_timestamp, '74C','B742','Boeing 747-200 Combi'),
('f1d5ddd1af054b228a843ece03408c93', 1, 'SYSTEM', current_timestamp, '74D','B743','Boeing 747-300 Combi(including -200SUD)'),
('91623850599340efbe8d421cd4956b93', 1, 'SYSTEM', current_timestamp, '74F',null,'Boeing 747 Freighter'),
('05b0d11c841143f683db618c904859ef', 1, 'SYSTEM', current_timestamp, '74E','B744','Boeing 747-400 Combi'),
('3ec8f49af34f4ef0bdfb453379d419e4', 1, 'SYSTEM', current_timestamp, '74J','B744','Boeing 747-400 Domestic'),
('51ea87d0c08d492083d80b34ba101a12', 1, 'SYSTEM', current_timestamp, '74L','B74S','Boeing 747SP'),
('0aa96ee728384510b0572c6aa83f119f', 1, 'SYSTEM', current_timestamp, '74M','B741','Boeing 747 Combi'),
('25749aa0827d4304a0e919bc1f9b85b7', 1, 'SYSTEM', current_timestamp, '74T','B741','Boeing 747-100 Freighter'),
('ea57fba481a04357984b6e89c6633f86', 1, 'SYSTEM', current_timestamp, '74U','B743','Boeing 747-300 Freighter'),
('73b17a8a66c64cc4a0c77b89ae0ea165', 1, 'SYSTEM', current_timestamp, '74V','B74R','Boeing 747SR Freighter'),
('b1ae86f28fac4f93af7755367cd31819', 1, 'SYSTEM', current_timestamp, '74X','B742','Boeing 747-200 Freighter'),
('ccea1ffc39d842eb9ca88d1a2d10e315', 1, 'SYSTEM', current_timestamp, '74Y','B744','Boeing 747-400 Freighter'),
('884a83032e4d41df9e1edb4719f4a1f7', 1, 'SYSTEM', current_timestamp, '752','B752','Boeing 757-200'),
('dcec544ce6b44ec5b3bcd9492799f6d1', 1, 'SYSTEM', current_timestamp, '753','B753','Boeing 757-300'),
('7eb1a4c5e8f1491298fad3918356f6f2', 1, 'SYSTEM', current_timestamp, '757','B752','Boeing 757'),
('3a0e1b88fa1444fda2e407b8af781b10', 1, 'SYSTEM', current_timestamp, '75F','B752','Boeing 757-200 Freighter'),
('5ac71a0b17004b988b86d4e5dd4b0e4b', 1, 'SYSTEM', current_timestamp, '75M','B752','Boeing 757-200 Combi'),
('738c54d2f0b84d85871c4a85989beacb', 1, 'SYSTEM', current_timestamp, '762','B762','Boeing 767-200'),
('85c26cbf7cef473dba58e8fa3e4b39c5', 1, 'SYSTEM', current_timestamp, '763','B763','Boeing 767-300'),
('60514be4f1f146808562015c4e9bd229', 1, 'SYSTEM', current_timestamp, '764','B764','Boeing 767-400'),
('335c8ba08ccc44a99b02fa94242b4163', 1, 'SYSTEM', current_timestamp, '767','B762','Boeing 767'),
('2c890cf9dc6144728428a669d8df019d', 1, 'SYSTEM', current_timestamp, '76F','B762','Boeing 767 Freighter'),
('29c30a2cce4646878b33f697c2b3e91b', 1, 'SYSTEM', current_timestamp, '76X','B762','Boeing 767-200 Freighter'),
('8226b237ec81440d952fb8df1a7178ca', 1, 'SYSTEM', current_timestamp, '76Y','B763','Boeing 767-300 Freighter'),
('d19484079e714fad9d01b275e75fab97', 1, 'SYSTEM', current_timestamp, '772','B772','Boeing 777-200/200ER'),
('3cf71a9b905a41f983e6ee9a41cf43c3', 1, 'SYSTEM', current_timestamp, '773','B773','Boeing 777-300'),
('aec97cabdeba45ebb1d8d95b1f573f2f', 1, 'SYSTEM', current_timestamp, '777','B777','Boeing 777'),
('916cd8e4088d417bb7b61c92e61a9987', 1, 'SYSTEM', current_timestamp, '77L','B772','Boeing 777-200LR'),
('00440b8951374dc0b97c6965330d7ea0', 1, 'SYSTEM', current_timestamp, '77W','B773','Boeing 777-300ER'),
('cd5606b5108140a39843539f65f21024', 1, 'SYSTEM', current_timestamp, 'A26','AN26','Antonov An-26'),
('098ea5f6e1774e019632c4fb50c5fd6a', 1, 'SYSTEM', current_timestamp, 'A28','AN26','Antonov An-28'),
('5e8aa301d4034d5d916fadef0619b70e', 1, 'SYSTEM', current_timestamp, 'A30','AN30','Antonov An-30'),
('0fb839c1c41044a69be0bc62b5090e75', 1, 'SYSTEM', current_timestamp, 'A32','AN32','Antonov An-32'),
('16fc36b0c5234bf6947a75e52a722472', 1, 'SYSTEM', current_timestamp, 'A40','A140','Antonov An-140'),
('c2496f4b255e468ab56e15b19a349796', 1, 'SYSTEM', current_timestamp, 'A4F','A124','Antonov An-124 Ruslan'),
('44e79aa3fee640b19554ef9fa2f7e199', 1, 'SYSTEM', current_timestamp, 'A81','A148','Antonov An-148-100'),
('52e1713193214d729f6998d66424c793', 1, 'SYSTEM', current_timestamp, 'AB3','A300','Airbus Industrie A300'),
('0c75c643b86d4130bbac0a42e5578e73', 1, 'SYSTEM', current_timestamp, 'AB4','A30B','Airbus Industrie A300B2/B4/C4'),
('ad5486c5169844a1946ebef8778dc966', 1, 'SYSTEM', current_timestamp, 'AB6','A306','Airbus Industrie A300-600'),
('83b452345df749bbafb3de60e622dd71', 1, 'SYSTEM', current_timestamp, 'ABB','A3ST','Airbus Industrie A300-600ST Beluga'),
('a03d11ca4f20434bb02324b308aa26e5', 1, 'SYSTEM', current_timestamp, 'ABF','A30B','Airbus Industrie A300 Freighter'),
('002e1811f05042daa60cdb4ac1f4af57', 1, 'SYSTEM', current_timestamp, 'ABX','A30B','Airbus Industrie A300B4/C4/F4 Freighter'),
('f047f566c7804813b0fa85525ac6d581', 1, 'SYSTEM', current_timestamp, 'ABY','A306','Airbus Industrie A300-600 Freighter'),
('503561dc57994eceb053d7ca0982507c', 1, 'SYSTEM', current_timestamp, 'ACD',null,'Twin Aero Commander/Turbo/Jetprop'),
('33a04bbafc714f2faf20746b857a0d68', 1, 'SYSTEM', current_timestamp, 'AGH',null,'Agusta A109'),
('952cdbca3db64a5daa48bccc26801451', 1, 'SYSTEM', current_timestamp, 'AN4','AN24','Antonov An-24'),
('7deecb389dbc4fc9b443dc045ba31c0d', 1, 'SYSTEM', current_timestamp, 'AN6','AN26','Antonov An-26/30/32'),
('fc324e1d5f5e4593bc7375cd79872a71', 1, 'SYSTEM', current_timestamp, 'AN7','AN72','Antonov An-72/74'),
('0b5f0a2475254ebd955d41b1cdf4c8dd', 1, 'SYSTEM', current_timestamp, 'ANF','AN12','Antonov An-12'),
('539f20b0098f4e5cab21898040d9da4c', 1, 'SYSTEM', current_timestamp, 'AR1','RJ1H','Avro RJ100 Avroliner'),
('ece5691a672b4c62ab02ab09b31a42b7', 1, 'SYSTEM', current_timestamp, 'AR7','RJ70','Avro RJ70 Avroliner'),
('16654adb1a3049b882b44e446f13b02b', 1, 'SYSTEM', current_timestamp, 'AR8','RJ85','Avro RJ85 Avroliner'),
('87e63b8956c24f978be8af99bd1430b2', 1, 'SYSTEM', current_timestamp, 'ARJ','RJ70','Avro RJ Avroliner'),
('f603dc23d2db4dda9bc164c7c83fe137', 1, 'SYSTEM', current_timestamp, 'ARX','RX85','Avro RJX'),
('0a090371f5524846b4c24feaeeeb036a', 1, 'SYSTEM', current_timestamp, 'AT4','AT43','Aerospatiale/Alenia ATR 42-300/320'),
('9a4e1551f4684d1aa60d390fd052130d', 1, 'SYSTEM', current_timestamp, 'AT5','AT45','Aerospatiale/Alenia ATR 42-500'),
('86d36563d0604e1a87da60e46cdb0d26', 1, 'SYSTEM', current_timestamp, 'AT7','AT72','Aerospatiale/Alenia ATR 72'),
('4b17794c401d4a789695c6b5faf62536', 1, 'SYSTEM', current_timestamp, 'ATP','ATP','British Aerospace ATP'),
('4443bd5168904fbb9b27c70529a57938', 1, 'SYSTEM', current_timestamp, 'ATR','AT42','Aerospatiale/Alenia ATR'),
('b45a0904a3e840198d1a6de98d4c4714', 1, 'SYSTEM', current_timestamp, 'AX1','RX1H','Avro RJX100'),
('45111d6d42364277be087af52431cd61', 1, 'SYSTEM', current_timestamp, 'AX8','RX85','Avro RJX85'),
('8f461406c08143dca6c7d3aeb598151a', 1, 'SYSTEM', current_timestamp, 'B11','BA11','British Aerospace BAC One Eleven'),
('27f8ce98da2f4a3c9dbc380df51cd5d1', 1, 'SYSTEM', current_timestamp, 'B12','BA11','British Aerospace BAC One Eleven 200'),
('db5abc50eca84cb58ed05be41f9f70e8', 1, 'SYSTEM', current_timestamp, 'B13','BA11','British Aerospace BAC One Eleven 300'),
('6cf9135be8d146458abbaab7b228676f', 1, 'SYSTEM', current_timestamp, 'B14','BA11','British Aerospace BAC One Eleven 400'),
('3ca9584ee2554cba91b000f632be80fe', 1, 'SYSTEM', current_timestamp, 'B15','BA11','British Aerospace BAC One Eleven 500'),
('64fc0be2590b44528bf96389b7a37312', 1, 'SYSTEM', current_timestamp, 'B72','B720','Boeing 720B'),
('dda4cd6c0b7a49b79d75dd098a4da4ff', 1, 'SYSTEM', current_timestamp, 'BE1','B190','Beechcraft Beech 1900'),
('2e84d2a6e3b04dc0a61e11af8db99b4a', 1, 'SYSTEM', current_timestamp, 'BE9','BE99','Beechcraft Airliner 99'),
('18fc77ae175949a8925402b96136ee34', 1, 'SYSTEM', current_timestamp, 'BEC',null,'Beechcraft Light Aircraft'),
('bb5d631d5f24421b804d77826874bb9b', 1, 'SYSTEM', current_timestamp, 'BEH','B190','Beechcraft Beech 1900D'),
('21646ef7693d476799ce1dbd486b00a6', 1, 'SYSTEM', current_timestamp, 'BES','B190','Beechcraft Beech 1900C'),
('85c8424d1aa14d2ead21f0331c03d238', 1, 'SYSTEM', current_timestamp, 'BET',null,'Beechcraft Light Aircraft Twin Prop'),
('71815b8bad864f5d998636578a390e27', 1, 'SYSTEM', current_timestamp, 'BNI','BN2P','Pilatus Britten-Norman BN-2A/B'),
('5de5ec665be14a38a67594342ab056ca', 1, 'SYSTEM', current_timestamp, 'BNT','TRIS','Pilatus Britten-Norman BN-2A Trislander'),
('7bec1ea89eb34d02abf3e64c5f352c1b', 1, 'SYSTEM', current_timestamp, 'BUS',null,'Non Aircraft – Bus'),
('59fd56e610f54c66b2dd39c0a03cd962', 1, 'SYSTEM', current_timestamp, 'CD2','NOMA','Government Aircraft Factories N22B/N24A Nomad'),
('a0161a3a44af4624a9166bbad5430eaa', 1, 'SYSTEM', current_timestamp, 'CNA',null,'Cessna Light Aircraft'),
('ebd0ab9c2c5c41b790ff6676f37835b5', 1, 'SYSTEM', current_timestamp, 'CNJ','C500','Cessna Citation'),
('e7b6d97717ca4edbb3495ed325fef688', 1, 'SYSTEM', current_timestamp, 'CR1','CRJ1','Canadair Regional Jet 100'),
('e382220540ba4c2095457759255f02cf', 1, 'SYSTEM', current_timestamp, 'CR2','CRJ2','Canadair Regional Jet 200'),
('cafc6de1e7a24364be13c020b2cd00d1', 1, 'SYSTEM', current_timestamp, 'CR7','CRJ7','Canadair Regional Jet 700'),
('d340373f39944486ab2d19f1bf2607ec', 1, 'SYSTEM', current_timestamp, 'CR9','CRJ9','Canadair Regional Jet 900'),
('9ea55e22203d4198a6f022d393c433a2', 1, 'SYSTEM', current_timestamp, 'CRJ','CARJ','Canadair Regional Jet'),
('9484959d4ee34956a2476c22f646c387', 1, 'SYSTEM', current_timestamp, 'CRV','S210','Aerospatiale/SUD SE210 Caravelle'),
('95c4f6f4f5e647a698c30794f28183bc', 1, 'SYSTEM', current_timestamp, 'CS2','C212','CASA/IPTN CN-212 Aviocar'),
('f5b2d820e3074f1c855b2693a76b2c6c', 1, 'SYSTEM', current_timestamp, 'CS5','CN35','CASA/IPTN CN-235'),
('8dc7d7c32d8244d6bbc18ddbee067432', 1, 'SYSTEM', current_timestamp, 'CV5','CVLT','Convair 580'),
('dcf54a4554ca47e9856066993abe59e0', 1, 'SYSTEM', current_timestamp, 'CVF',null,'Convair 580/600/640 Freighter'),
('17b10991d9594729bff39b2ef461394d', 1, 'SYSTEM', current_timestamp, 'CWC','C46','Curtiss C-46 Commando'),
('76e19156323c473fa1dff9107e671376', 1, 'SYSTEM', current_timestamp, 'CVR',null,'Convair 240/440/580'),
('0635a5e484f64c7f90a07279189a2747', 1, 'SYSTEM', current_timestamp, 'D10','DC10','Boeing/McDonnell Douglas DC-10'),
('531c0fb8859c4636bf117b7cfe15ca29', 1, 'SYSTEM', current_timestamp, 'D11','DC10','Boeing/McDonnell Douglas DC-10-10/15'),
('1ffc3eafc9534b6fad262f5098b2d9f3', 1, 'SYSTEM', current_timestamp, 'D1C','DC10','Boeing/McDonnell Douglas DC-10-30/40'),
('e0ba692a65804e00be84bc2d702dc785', 1, 'SYSTEM', current_timestamp, 'D1F','DC10','Boeing/McDonnell Douglas DC-10 Freighter'),
('d4a40c5171ef493aa432b216889b6963', 1, 'SYSTEM', current_timestamp, 'D1X','DC10','Boeing/McDonnell Douglas DC-10-10 Freighter'),
('ae45cb44fb6d46a48f9e918fecefc362', 1, 'SYSTEM', current_timestamp, 'D1Y','DC10','Boeing/McDonnell Douglas DC-10-30/40 Freighter'),
('ae294ff8eac84c12a506054a3385c598', 1, 'SYSTEM', current_timestamp, 'D28','D228','Fairchild Dornier Do-228'),
('5e5ae520e23a4d879483184bdd71326b', 1, 'SYSTEM', current_timestamp, 'D38','D328','Fairchild Dornier Do-328'),
('0cae1e5c06fe4b50a04e143d27065c0c', 1, 'SYSTEM', current_timestamp, 'D3F','DC3','Boeing/Douglas DC-3 Freighter'),
('85c1d98d45a34742b282431ad1fbf20e', 1, 'SYSTEM', current_timestamp, 'D6F','DC6','Boeing/Douglas DC-6A/B/C Freighter'),
('e8e453754a9a45d19815cbfe823ecb86', 1, 'SYSTEM', current_timestamp, 'D8F','DC85','Boeing/McDonnell Douglas DC-8 Freighter'),
('e98fa39ee958424397334ddd20aba09e', 1, 'SYSTEM', current_timestamp, 'D8L','DC86','Boeing/McDonnell Douglas DC-8-62'),
('308464ab5ea646c3ae208c00381e7e48', 1, 'SYSTEM', current_timestamp, 'D8M','DC8','Boeing/McDonnell Douglas DC-8 Combi'),
('cac9cc0983c249959578d758021bd977', 1, 'SYSTEM', current_timestamp, 'D8Q','DC87','Boeing/McDonnell Douglas DC-8-72'),
('81bf926e7b974aeb998389be85a8cd18', 1, 'SYSTEM', current_timestamp, 'D8T','DC85','Boeing/McDonnell Douglas DC-8-50 Freighter'),
('2bddc50654cb4401a6af78670b723962', 1, 'SYSTEM', current_timestamp, 'D8X','DC86','Boeing/McDonnell Douglas DC-8-61/62/63 Freighter'),
('ee1263c3e3544db187d26e59a7d63240', 1, 'SYSTEM', current_timestamp, 'D8Y','DC87','Boeing/McDonnell Douglas DC-8-71/72/73 Freighter'),
('d39dbe1aee684669aed95877a954dfdd', 1, 'SYSTEM', current_timestamp, 'D91','DC91','Boeing/McDonnell Douglas DC-9-10'),
('de91764d507946f0b5678e41887e51ab', 1, 'SYSTEM', current_timestamp, 'D92','DC92','Boeing/McDonnell Douglas DC-9-20'),
('86d5138df5e14450846ffa401cfb9ed3', 1, 'SYSTEM', current_timestamp, 'D93','DC93','Boeing/McDonnell Douglas DC-9-30'),
('c51d5165c6f6400b8cf9db8d4f2cd68f', 1, 'SYSTEM', current_timestamp, 'D94','DC94','Boeing/McDonnell Douglas DC-9-40'),
('5d6d48f23fc24e3a819760bd2de7942e', 1, 'SYSTEM', current_timestamp, 'D95','DC95','Boeing/McDonnell Douglas DC-9-50'),
('a3eac7e89a96470c9f3a85ff988347e9', 1, 'SYSTEM', current_timestamp, 'D9C','DC93','Boeing/McDonnell Douglas DC-9-30 Freighter'),
('1134268ce96443aeac76182fe4e4689f', 1, 'SYSTEM', current_timestamp, 'D9F','DC94','Boeing/McDonnell Douglas DC-9-40 Freighter'),
('188d11fd0cda4e45832019035a110662', 1, 'SYSTEM', current_timestamp, 'D9S','DC93','Boeing/McDonnell Douglas DC-9-30/40/50'),
('2f35e8c7326148e88250c42b8b03d3f4', 1, 'SYSTEM', current_timestamp, 'D9X','DC91','Boeing/McDonnell Douglas DC-9-10 Freighter'),
('ffa25d1b0f8e46a9b94ea8ec7b30b7b2', 1, 'SYSTEM', current_timestamp, 'DC3','DC3','Boeing/Douglas DC-3'),
('61b20640cf2e4cf2837a9e9e1816cce8', 1, 'SYSTEM', current_timestamp, 'DC6','DC6','Boeing/Douglas DC-6'),
('9ec3036c890c489090e8451dd0c922a2', 1, 'SYSTEM', current_timestamp, 'DC8','DC8','Boeing/McDonnell Douglas DC-8'),
('4c7ee19b35a84b81a5e3f43b30612539', 1, 'SYSTEM', current_timestamp, 'DC9','DC9','Boeing/McDonnell Douglas DC-9'),
('d68cbeb243fd4c1bad382cd2e12565f2', 1, 'SYSTEM', current_timestamp, 'DCF','DC91','Boeing/McDonnell Douglas DC-9 Freighter'),
('ed1ab8c28df64d9eb3d680dbcb4d7bce', 1, 'SYSTEM', current_timestamp, 'DH1','DH8A','De Havilland Canada DHC-8-100 Dash 8/8Q'),
('4bb808257df5498b8748d3917ced24b1', 1, 'SYSTEM', current_timestamp, 'DH2','DH8B','De Havilland Canada DHC-8-200 Dash 8/8Q'),
('60b57a389dcd4cd6bbbde117f0c7099c', 1, 'SYSTEM', current_timestamp, 'DH3','DH8C','De Havilland Canada DHC-8-300 Dash 8/8Q'),
('bfc73b6e36ce4343871138da7b342bd7', 1, 'SYSTEM', current_timestamp, 'DH4','DH8D','De Havilland Canada DHC-8-400 Dash 8/8Q'),
('e82bc8f241144597a0a58fba2f1bf35d', 1, 'SYSTEM', current_timestamp, 'DH7','DHC7','De Havilland Canada DHC-7 Dash 7'),
('1dbe2971cc99474497ef26624fc110fb', 1, 'SYSTEM', current_timestamp, 'DH8','DH8A','De Havilland Canada DHC-8 Dash 8 All S.'),
('6c5964498726488a91d5aea982378209', 1, 'SYSTEM', current_timestamp, 'DHC','DHC4','De Havilland Canada DHC-4 Caribou'),
('837d976c84324b4cb17af31e50b25a8e', 1, 'SYSTEM', current_timestamp, 'DHL','DHC3','De Havilland Canada DHC-3 Turbo Otter'),
('050b19d8af5e4a39a8c898b94afe576e', 1, 'SYSTEM', current_timestamp, 'DHP','DHC2','De Havilland Canada DHC-2 Beaver'),
('713333e2717344de961e6179e93c5285', 1, 'SYSTEM', current_timestamp, 'DHT','DHC6','De Havilland Canada DHC-6 Twin Otter'),
('f7f0107b2a8f46959a6b7d123927139c', 1, 'SYSTEM', current_timestamp, 'E70','E170','Embraer ERJ-170'),
('ebccc2c90d6e4480b962c9470c886194', 1, 'SYSTEM', current_timestamp, 'E75','E175','Embraer ERJ-175'),
('5a7cfde86a0946eebe9e1f187a8c710d', 1, 'SYSTEM', current_timestamp, 'E90','E190','Embraer ERJ-190'),
('868028b7ebee44dd90e26c8ee41a9a25', 1, 'SYSTEM', current_timestamp, 'EM2','E120','Embraer EMB-120 Brasilia'),
('d963816f98d247769ef4b0707adf3bc4', 1, 'SYSTEM', current_timestamp, 'EMB','E110','Embraer EMB-110 Bandeirante'),
('c8f7cc42104a4df7ae75764f4686c17f', 1, 'SYSTEM', current_timestamp, 'EMJ','E170','Embraer ERJ-170/190'),
('d3a17a5c8b924975982f1c8462d8d189', 1, 'SYSTEM', current_timestamp, 'ER3','E135','Embraer ERJ-135 Regional Jet'),
('dcdf6723bf0a41d3bf08de45b3a6971b', 1, 'SYSTEM', current_timestamp, 'ER4','E145','Embraer ERJ-145 Regional Jet'),
('6b9d1b1d7e8e4a4d97f1d20a565a1e50', 1, 'SYSTEM', current_timestamp, 'ERD','E140','Embraer ERJ-140 Regional Jet'),
('37e50d5b2d2c4475a179294811fc68f7', 1, 'SYSTEM', current_timestamp, 'ERJ','E135','Embraer ERJ-135/140/145 Regional Jet'),
('b4a0b2b9270e4cffa93f160a0d846c01', 1, 'SYSTEM', current_timestamp, 'F21','F28','Fokker F28 Fellowship 1000'),
('5fcd644554f942afa14ddc7d53d3cf94', 1, 'SYSTEM', current_timestamp, 'F22','F28','Fokker F28 Fellowship 2000'),
('ccb89d8b6a064764b8811e843a6e7cb4', 1, 'SYSTEM', current_timestamp, 'F23','F28','Fokker F28 Fellowship 3000'),
('506f3472811e4e979d089fafab488a3e', 1, 'SYSTEM', current_timestamp, 'F24','F28','Fokker F28 Fellowship 4000'),
('c5f5639d9bc34eef9e71bde94b15781c', 1, 'SYSTEM', current_timestamp, 'F27','F27','Fokker F27 Friendship/Fairchild F27'),
('6a93db95c46049098c25c1cde88e61d6', 1, 'SYSTEM', current_timestamp, 'F28','F28','Fokker F28 Fellowship'),
('a22065baf65c4e768eed9f5e760c2eb5', 1, 'SYSTEM', current_timestamp, 'F50','F50','Fokker 50'),
('9f82c45465b14dd283549cc288d5f149', 1, 'SYSTEM', current_timestamp, 'F70','F70','Fokker 70'),
('ca97a252b0d34cefaead835f22313191', 1, 'SYSTEM', current_timestamp, 'FA7','J728','Fairchild Dornier 728JET'),
('3813c6e2d11a4ae284e0f36f0981e85b', 1, 'SYSTEM', current_timestamp, 'FK7','F27','Fairchild FH227'),
('8079f9ef87434fc396cd75115aa03e40', 1, 'SYSTEM', current_timestamp, 'FRJ','J328','Fairchild Dornier 328JET'),
('11bbcf0a2a984afd8c8e8aff64e9cea9', 1, 'SYSTEM', current_timestamp, 'GRG','G21','Grumman G-21 Goose'),
('7811f398afef46ce97936265bec78082', 1, 'SYSTEM', current_timestamp, 'GRM','G73T','Grumman G-73 Turbo Mallard'),
('0c58204dbcd0407ca532daa56f8aa27d', 1, 'SYSTEM', current_timestamp, 'HEC','COUR','Helio H-250 Courier/H-295/395 Super Courier'),
('ccc44c7fcb8b40118c66c69f10f65e78', 1, 'SYSTEM', current_timestamp, 'HS7','A748','British Aerospace/Hawker Siddeley HS748'),
('abaa7b5c524143c7a9e7bf74cdf4347f', 1, 'SYSTEM', current_timestamp, 'I14','I114','Ilyushin IL-114'),
('bfbd4c9062d542d1be59cbde6e90f855', 1, 'SYSTEM', current_timestamp, 'I93','IL96','Ilyushin IL-96-300'),
('f2048c8d8f3e4576b360e0fdad5acbc4', 1, 'SYSTEM', current_timestamp, 'I9F','IL96','Ilyushin IL-96 Freighter'),
('2cf2f17f86754d4283039948138cba21', 1, 'SYSTEM', current_timestamp, 'I9M','IL96','Ilyushin IL-96M'),
('9d7b0b3b6c014080a435449b0eb3adca', 1, 'SYSTEM', current_timestamp, 'I9Y','IL96','Ilyushin IL-96T'),
('d268d94fc43a46f3b0eb73a9e6018fd4', 1, 'SYSTEM', current_timestamp, 'IL6','IL62','Ilyushin IL-62'),
('d173543d2f64468996a8fe55e3e29894', 1, 'SYSTEM', current_timestamp, 'IL7','IL76','Ilyushin IL-76'),
('c08f35c999824476840daeaf30cd8007', 1, 'SYSTEM', current_timestamp, 'IL8','IL18','Ilyushin IL-18'),
('eaa371d38ad64c798eca73e44f7e50ad', 1, 'SYSTEM', current_timestamp, 'IL9','IL96','Ilyushin IL-96'),
('0f6bb3c512c1489fa85fdaf47d943653', 1, 'SYSTEM', current_timestamp, 'ILW','IL86','Ilyushin IL-86'),
('c7ca77f0e78b4dcd9b886a35ba4d0c61', 1, 'SYSTEM', current_timestamp, 'J31','JS31','British Aerospace Jetstream 31'),
('15d7e508a4e044ca92bbfd0ee2db6f8c', 1, 'SYSTEM', current_timestamp, 'J32','JS32','British Aerospace Jetstream 32'),
('5287fb151e8042bca7fdb5d8ba81c2af', 1, 'SYSTEM', current_timestamp, 'J41','JS41','British Aerospace Jetstream 41'),
('97ea987681134b7199412122f296af21', 1, 'SYSTEM', current_timestamp, 'JST','JS31','British Aerospace Jetstream 31/32/41'),
('a9c60639f0644f8db1c4eb8b78ea331a', 1, 'SYSTEM', current_timestamp, 'JU5','JU52','Junkers Ju52'),
('50f11a1862494b29b6aaed1923422ab0', 1, 'SYSTEM', current_timestamp, 'L10','L101','Lockheed L-1011 Tristar'),
('bd9500ae5fa84db793bad1156f6d5305', 1, 'SYSTEM', current_timestamp, 'L11','L101','Lockheed L-1011-1/50/100/150/200/250'),
('29df65d4eb624b67934a8461e1ebbf12', 1, 'SYSTEM', current_timestamp, 'L15','L101','Lockheed L-1011-500 Tristar'),
('7a6a8bf37e7144819efb2f799ff2862d', 1, 'SYSTEM', current_timestamp, 'L1F','L101','Lockheed L-1011 Tristar Freighter'),
('530bfc8919734ef3b2edd27635f8fd4c', 1, 'SYSTEM', current_timestamp, 'L4T','L410','Let 410 Turbolet'),
('026cdc5a0449487cbe37d20d63da3148', 1, 'SYSTEM', current_timestamp, 'LCH',null,'Non Aircraft – Launch/Boat'),
('6a93fd84ce1f439a99f555d6ce9c5e96', 1, 'SYSTEM', current_timestamp, 'LMO',null,'Non Aircraft – Limousine'),
('230fca5daf7c4ff4944a15de570eb381', 1, 'SYSTEM', current_timestamp, 'LOE','L188','Lockheed L-188 Electra'),
('f00c4ef3bb274a5e813f40671abe9093', 1, 'SYSTEM', current_timestamp, 'LOH','C130','Lockheed L-100 Hercules'),
('4f29f085e4b44f1e94e242702757d5ae', 1, 'SYSTEM', current_timestamp, 'LRJ',null,'Learjet'),
('0eda5cbdf3c14c8b8ea4cb998dd24b46', 1, 'SYSTEM', current_timestamp, 'M11','MD11','Boeing/McDonnell Douglas MD-11'),
('963cb4a18a2d40599ffe337df579e951', 1, 'SYSTEM', current_timestamp, 'M1F','MD11','Boeing/McDonnell Douglas MD-11 Freighter'),
('abe87f72c5844e6da9920833729cdf53', 1, 'SYSTEM', current_timestamp, 'M1M','MD11','Boeing/McDonnell Douglas MD-11 Combi'),
('4c17d0f7842a4ae39145c03a2c2de4d2', 1, 'SYSTEM', current_timestamp, 'M80','MD80','Boeing/McDonnell Douglas MD-80'),
('683e919d41734c6cbcd781b4a053233e', 1, 'SYSTEM', current_timestamp, 'M81','MD81','Boeing/McDonnell Douglas MD-81'),
('93c9555ca9d54cb6b10fe905c968fc49', 1, 'SYSTEM', current_timestamp, 'M82','MD82','Boeing/McDonnell Douglas MD-82'),
('0662b74e38f1461896c8c941957a9279', 1, 'SYSTEM', current_timestamp, 'M83','MD83','Boeing/McDonnell Douglas MD-83'),
('5a29a18d82a14c0494ede03b32e5bfca', 1, 'SYSTEM', current_timestamp, 'M87','MD87','Boeing/McDonnell Douglas MD-87'),
('dcb2abb6517e4e0787e9355b8d9e1d39', 1, 'SYSTEM', current_timestamp, 'M88','MD88','Boeing/McDonnell Douglas MD-88'),
('2f94b59d71bb4971bf23259bde0557b8', 1, 'SYSTEM', current_timestamp, 'M90','MD90','Boeing/McDonnell Douglas MD-90'),
('dd16d52331e841a0b23e5dad7568a254', 1, 'SYSTEM', current_timestamp, 'MIH',null,'Mil'),
('237a95be922e4af1a8261d59b8a32fae', 1, 'SYSTEM', current_timestamp, 'MU2','MU2','Mitsubishi MU-2'),
('594f13ea5e1a497ca8ee2accbb08441b', 1, 'SYSTEM', current_timestamp, 'ND2','N262','Aerospatiale/Nord 262'),
('c009165a147d4ba995d9c12907c815d8', 1, 'SYSTEM', current_timestamp, 'NDC','S601','Aerospatiale SN601 Corvette'),
('36a8254b87504f1ebc5e03e99a8801fb', 1, 'SYSTEM', current_timestamp, 'NDE',null,'Aerospatiale/Eurocopter Ecureuil AS350/AS355'),
('e204394559fb4d3dbc2dce7ef4b28607', 1, 'SYSTEM', current_timestamp, 'PAG',null,'Piper Light Aircraft'),
('ea03bebd2d904178b451009906f0e838', 1, 'SYSTEM', current_timestamp, 'PN6','P68','Partenavia P68'),
('d82202aac3f147bbaaf2534de4ab8f63', 1, 'SYSTEM', current_timestamp, 'S20','SB20','Saab 2000'),
('d343e4a0115a4a4db737c8f0a36c44ab', 1, 'SYSTEM', current_timestamp, 'SF3','SF34','Saab SF-340'),
('5f8f50932e75474ebc30a15cbea8660d', 1, 'SYSTEM', current_timestamp, 'SSC','CONC','Aerospatiale/British Aerospace Concorde (discontinued)'),
('f6e70788912e45a3b5b05d1a579b85a5', 1, 'SYSTEM', current_timestamp, 'SH3','SH33','Shorts SD-330'),
('5fe634a9ae124ed28d588faa69c46468', 1, 'SYSTEM', current_timestamp, 'SH6','SH36','Shorts SD-360'),
('a904168c775f4d00a8ddcb8055e36832', 1, 'SYSTEM', current_timestamp, 'S58','S58T','Sikorsky S-58T'),
('91b5676a0dfd43b7a387e15d2d7275e5', 1, 'SYSTEM', current_timestamp, 'S61','S61','Sikorsky S-61'),
('504da4f95fb7428ea2932c00ddbcc829', 1, 'SYSTEM', current_timestamp, 'S76','S76','Sikorsky S-76'),
('390c2be2a9664adb9bed7cf5ace1ccf9', 1, 'SYSTEM', current_timestamp, 'SWM','SW4','Fairchild Metro/Merlin'),
('5caff0619ca54afd9c3a08c7280ab0e0', 1, 'SYSTEM', current_timestamp, 'T20','T204','Tupolev Tu-204/214'),
('1953eaed9737463d89cdae2e2bbdd18f', 1, 'SYSTEM', current_timestamp, 'TU3','T134','Tupolev Tu-134'),
('eae62defc6264b4f80f9cbebc2d8f4bd', 1, 'SYSTEM', current_timestamp, 'TU5','T154','Tupolev Tu-154'),
('b16b7aeb32f74f6baaffd8377a82aff4', 1, 'SYSTEM', current_timestamp, 'TRN',null,'Non Aircraft – Train'),
('c8ca6e57c3be49b78c26663c2d48b399', 1, 'SYSTEM', current_timestamp, 'VCV','VISC','Vickers Viscount'),
('3a396f0263754a12a0e3841c13d251f1', 1, 'SYSTEM', current_timestamp, 'VCX','VC10','Vickers VC-10'),
('46fe3fbd16e4407ab66916a158b7b8c4', 1, 'SYSTEM', current_timestamp, 'WWP','WW24','Israel Aircraft Industries 1124 Westwind'),
('2a5d8e3d732e4b499b4907c27f34f337', 1, 'SYSTEM', current_timestamp, 'YK2','YK40','Yakovlev Yak-42'),
('af78d83a5b844ed5b6d90bd689d0b862', 1, 'SYSTEM', current_timestamp, 'YK4','YK42','Yakovlev Yak-40'),
('88b756cfc1e3499e805baf16936c5130', 1, 'SYSTEM', current_timestamp, 'YN2','Y12','Yunshuji Y-12/Xian Y-12'),
('a5b2ea91f44d4d078d51022298d978e2', 1, 'SYSTEM', current_timestamp, 'YN7','AN24','Yunshuji Y-7/Harbin Y-7'),
('e8f6d28231ff4c128f737040bebfd50a', 1, 'SYSTEM', current_timestamp, 'YS1','YS11','NAMC YS-11')

");
		}
	}

	[Migration(2016022610)]
	public class Migration_2016022610 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_flight_segment")
				.AddColumn("equipment").AsRefecence("lt_airplane_model").Nullable()
			;
		}
	}


	[Migration(2016031701)]
	public class Migration_2016031701 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_flight_segment")
				.AddColumn("operator").AsRefecence("lt_party").Nullable()
			;
		}
	}

	[Migration(2016032201)]
	public class Migration_2016032201 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("galileorailwebservice_loadedon").AsDateTime().Nullable()
			;
		}
	}

	[Migration(2016032301)]
	public class Migration_2016032301 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Index().OnTable("lt_product").OnColumn("createdon");
		}
	}

	[Migration(2016032302)]
	public class Migration_2016032302 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Column("class").OnTable("lt_gds_file").AsString(16);
		}
	}


	[Migration(2016032901)]
	public class Migration_2016032901 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddColumn("legalentity").AsRefecence("lt_party").Nullable();
			Alter.Table("lt_gds_agent")
				.AddColumn("provider").AsRefecence("lt_party").Nullable()
				.AddColumn("legalentity").AsRefecence("lt_party").Nullable();
		}
	}

	[Migration(2016032903)]
	public class Migration_2016032903 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddMoneyColumn("handlingn");
		}
	}

	[Migration(2016033001)]
	public class Migration_2016033001 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Index().OnTable("lt_gds_file").OnColumn("createdon");
			Create.Index().OnTable("lt_gds_file").OnColumn("importresult");
			Create.Index().OnTable("lt_gds_file").OnColumn("importoutput");
		}
	}


	[Migration(2016081501)]
	public class Migration_2016081501 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order")
				.AddColumn("intermediary").AsRefecence("lt_party").Nullable();
		}
	}

	[Migration(2016092801)]
	public class Migration_2016092801 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order")
				.AddColumn("bonusdate").AsDate().Nullable()
				.AddColumn("bonusspentamount").AsDecimal(19, 4).Nullable()
				.AddColumn("bonusrecipient").AsRefecence("lt_party").Nullable();


			Alter.Table("lt_party")
				.AddColumn("bonusamount").AsDecimal(19, 4).Nullable();

		}
	}

	[Migration(2016102401)]
	public class Migration_2016102401 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order")
				.AddMoneyColumn("checkpaid")
				.AddMoneyColumn("wirepaid")
				.AddMoneyColumn("creditpaid")
				.AddMoneyColumn("restpaid");
		}
	}

	[Migration(2016102403)]
	public class Migration_2016102403 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_order o set

	checkpaid_currency = paid_currency,
	wirepaid_currency = paid_currency,
	creditpaid_currency = paid_currency,
	restpaid_currency = paid_currency,

	checkpaid_amount = 
		coalesce((select sum(amount_amount) from lt_payment where order_ = o.id and not isvoid and postedon is not null and paymentform = 2), 0),
	wirepaid_amount = 
		coalesce((select sum(amount_amount) from lt_payment where order_ = o.id and not isvoid and postedon is not null and paymentform = 1), 0),
	creditpaid_amount = 
		coalesce((select sum(amount_amount) from lt_payment where order_ = o.id and not isvoid and postedon is not null and paymentform = 3), 0),
	restpaid_amount = 
		coalesce((select sum(amount_amount) from lt_payment where order_ = o.id and not isvoid and postedon is not null and paymentform in (0, 4)), 0)
		+ coalesce((select sum(amount) from lt_internal_transfer where toorder = o.id), 0)
		- coalesce((select sum(amount) from lt_internal_transfer where fromorder = o.id), 0)
");
		}
	}


	[Migration(2017022701)]
	public class Migration_2017022701 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_airline_month_commission")
				.AsEntity2()
				.WithColumn("airline").AsRefecence("lt_party").NotNullable()
				.WithColumn("datefrom").AsDate().NotNullable()
				.WithColumn("dateto").AsDate().NotNullable()
				.WithColumn("commissionpc").AsDecimal().Nullable()
			;
		}
	}

	[Migration(2017022702)]
	public class Migration_2017022702 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Index().OnTable("lt_airline_month_commission").OnColumn("datefrom");
			Create.Index().OnTable("lt_airline_month_commission").OnColumn("dateto");
		}
	}

	[Migration(2017032901)]
	public class Migration_2017032901 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddMoneyColumn("bookingfee");

			Alter.Table("lt_system_configuration")
				.AddColumn("consignment_separatebookingfee").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("pasterboard_defaultpaymenttype").AsInt32().Nullable()
			;
		}
	}

	[Migration(2018060701)]
	public class Migration_2018060701 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_party")
				.AddColumn("cannotbecustomer").AsBoolean().NotNullable().WithDefaultValue(false)
			;
		}
	}

	[Migration(2018120501)]
	public class Migration_2018120501 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_identity")
				.AddColumn("allowcustomerreport").AsBoolean().NotNullable().WithDefaultValue(true)
				.AddColumn("allowregistryreport").AsBoolean().NotNullable().WithDefaultValue(true)
				.AddColumn("allowunbalancedreport").AsBoolean().NotNullable().WithDefaultValue(true)
			;
		}
	}

	[Migration(2019010302)]
	public class Migration_2019010302 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("consignment_numbermode")
				.AsInt32().NotNullable().WithDefaultValue(0);

			Alter.Table("lt_order")
				.AddColumn("consignmentlastindex")
				.AsInt32().Nullable();
		}
	}


	[Migration(2019012101)]
	public class Migration_2019012101 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("ticket_noprintreservations").AsBoolean().NotNullable().WithDefaultValue(false);
		}
	}


	[Migration(2019021204)]
	public class Migration_2019021204 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_user_visit")
				.AsEntity()
				.WithColumn("user_").AsRefecence("lt_identity").NotNullable()
				.WithColumn("startdate").AsDateTime().NotNullable().Indexed()
				.WithColumn("ip").AsText().NotNullable().Indexed()
				.WithColumn("sessionid").AsText().Nullable()
				.WithColumn("request").AsText().Nullable()
			;
		}
	}


	[Migration(2019101501)]
	public class Migration_2019101501 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("galileobuswebservice_loadedon").AsDateTime().Nullable()
			;
		}
	}


	[Migration(2020081801)]
	public class Migration_2020081801 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("travelpointwebservice_loadedon").AsDateTime().Nullable()
			;
		}
	}


	[Migration(2021032401)]
	public class Migration_2021032401 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_country")

				.AddColumn("note").AsText().Nullable()
			;
		}
	}


	[Migration(2021052501)]
	public class Migration_2021052501 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order")
				.AddColumn("allowaddproductsinclosedperiod").AsBoolean().NotNullable().WithDefaultValue(false)
			;
		}
	}

	[Migration(2021072001)]
	public class Migration_2021072001 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("allowotheragentstomodifyproduct").AsBoolean().NotNullable().WithDefaultValue(false)
			;
		}
	}


	[Migration(2021080201)]
	public class Migration_2021080201 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("drctwebservice_loadedon").AsDateTime().Nullable()
			;
		}
	}


	[Migration(2021101101)]
	public class Migration_2021101101 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_invoice")
				.AddColumn("fileextension").AsString(8).Nullable()
			;
		}
	}


	[Migration(2022102001)]
	public class Migration_2022102001 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_party")
				.AddColumn("signature").AsText().Nullable()
			;
		}
	}


	[Migration(2021120701)]
	public class Migration_2021120701 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("invoiceprinter_showvat").AsBoolean().WithDefaultValue(true).NotNullable()
			;
		}
	}


	[Migration(2021121301)]
	public class Migration_2021121301 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("useconsolidatorcommission").AsBoolean().WithDefaultValue(false).NotNullable()
				.AddColumn("usebonuses").AsBoolean().WithDefaultValue(false).NotNullable()
				.AddMoneyColumn("defaultconsolidatorcommission");
			;
		}
	}


	[Migration(2021121302)]
	public class Migration_2021121302 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddMoneyColumn("consolidatorcommission");
			;
		}
	}


	[Migration(2022011701)]
	public class Migration_2022011701 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order_item")
				.AddColumn("isforcedelivered").AsBoolean().NotNullable().WithDefaultValue(false);
			;
		}
	}


	[Migration(20222071401)]
	public class Migration_20222071401 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("invoice_canownerselect").AsBoolean().WithDefaultValue(false).NotNullable()
			;
		}
	}


	[Migration(20222071402)]
	public class Migration_20222071402 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_document_owner")
				.AddColumn("isdefault").AsBoolean().WithDefaultValue(false).NotNullable()
			;
		}
	}


	[Migration(20222071403)]
	public class Migration_20222071403 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_payment")
				.AddColumn("bankaccount").AsRefecence("lt_bank_account").Nullable()
			;
		}
	}


	[Migration(2023030901)]
	public class Migration_2023030901 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_amadeus_avia_sftp_rsa_key")
				.AsEntity2()
				.WithColumn("sftpusername").AsText().NotNullable()
				.WithColumn("keypassword").AsText().NotNullable()
				.WithColumn("ppk").AsText().NotNullable()
				.WithColumn("oppk").AsText().Nullable()
			;
		}
	}


	[Migration(2023031001)]
	public class Migration_2023031001 : AutoReversingMigration
	{
		public override void Up()
		{
			Delete.Table("lt_amadeus_avia_sftp_rsa_key");
		}
	}


	[Migration(2023031002)]
	public class Migration_2023031002 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_amadeus_avia_sftp_rsa_key")
				.AsEntity2()
				.WithColumn("oppk").AsText().NotNullable()
			;
		}
	}





}