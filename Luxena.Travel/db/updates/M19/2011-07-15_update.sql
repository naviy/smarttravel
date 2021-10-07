alter table lt_currency
add `numericcode` int(11) DEFAULT NULL after code;

create table lt_currency_new as select * from lt_currency where 0 = 1;

insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('004440fd14e24adc9cad2caa49ac2402',1,'SYSTEM','2010-04-29 00:00:00',null,null,'NPR','Nepalese Rupee',524);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0195c99c32b743d69c6fed8f9691a565',1,'SYSTEM','2010-04-29 00:00:00',null,null,'NAD','Namibia Dollar',516);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('026ddda5735848b3974b287b1fce66b0',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SIT','Slovenia, Tolars',91);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('02b39ad1e40148c387f83fbd483fa281',1,'sergey.buturlakin','2011-07-11 19:33:16',null,null,'CHE','WIR Euro',947);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('03b88c53f0584e3788bfda5aae756cc8',1,'SYSTEM','2010-04-29 00:00:00',null,null,'PHP','Philippine Peso',608);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('05b3416cb9054f49be60a469e0218628',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MXN','Mexican Peso',484);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('070e2b0a9bd7434f882fe4731b2fe585',1,'SYSTEM','2010-04-29 00:00:00',null,null,'TRY','Turkish Lira',949);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0a39e628264b470daed5632c807e3f01',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ZAR','Rand',710);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0a6a3abf42f54c119c46a2a84196e075',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GBP','Pound Sterling',826);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0c8a09dc8e5949e483b9e622362b908c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MOP','Pataca',446);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0d3ca8a1fbe6475da6833259b9679bd5',1,'SYSTEM','2010-04-29 00:00:00',null,null,'HUF','Forint',348);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0d43861b5f6a4e50bebd5e5ea2e17a55',1,'sergey.buturlakin','2011-07-11 19:31:48',null,null,'CLF','Unidades de fomento',990);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0e2a178dda8b4f9595a352dc414b0acd',1,'SYSTEM','2010-04-29 00:00:00',null,null,'FJD','Fiji Dollar',242);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0e5c20a5a015486dbc86aad14ba48cc1',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KYD','Cayman Islands Dollar',136);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0e6129464f26452eb242a00cc819c97c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'AWG','Aruban Guilder',533);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0f06b2d6cf3b42a99715e1d5efa81a36',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SYP','Syrian Pound',760);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0f93b1741413471b85e0c1f3d4b6f598',1,'SYSTEM','2010-04-29 00:00:00',null,null,'TTD','Trinidata and Tobago Dollar',780);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('0fb4d7f3008e43fda58e3b7a28a476d5',1,'SYSTEM','2010-04-29 00:00:00',null,null,'STD','Dobra',678);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('10922e428d67412e9ec77ac3a8abc92e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ERN','Nakfa',232);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('121b177046a54083b5a90694b05020c4',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MZM','Mozambique, Meticais',366);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('136721ef7f6e437480975ec182930206',1,'SYSTEM','2010-04-29 00:00:00',null,null,'JMD','Jamaican Dollar',388);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('13d345ee89ff4cb8aec6b85fc37fc61d',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ARS','Argentine Peso',32);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('17734151d97149dfb589046a307fb1e5',1,'sergey.buturlakin','2011-07-11 19:29:00',null,null,'TMT','Manat',934);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('1788f2e318a0401a9928a3efee6d0808',1,'sergey.buturlakin','2011-07-11 19:34:17',null,null,'CHW','WIR Franc',948);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('19cfd575f27e4a45b6eb1dbb67c8826b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GTQ','Quetzal',320);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('1a878798913d4cd3a0a1a9677b2ec6df',1,'SYSTEM','2010-04-29 00:00:00',null,null,'VUV','Vatu',548);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('1b35f684a4d04e2aa2c5e1730e117037',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CVE','Cape Verde Escudo',132);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('1e63ca9094e140adace1eeb05996496c',1,'sergey.buturlakin','2011-07-11 19:25:21',null,null,'GHS','Cedi',936);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('1e7eaa3b03b04a7fa28c3fa6c02f907a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XAF','CFA Franc BEAC',950);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('251c27402ae04fac886d5bcdbe5b796c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'RON','New Leu',946);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('29849f25d99541c0a0e3abbddc14f7bb',1,'SYSTEM','2010-04-29 00:00:00',null,null,'FKP','Falkland Island Pound',238);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('29a66d4ce5494620a4e0beb448ff25ca',1,'SYSTEM','2010-04-29 00:00:00',null,null,'UZS','Uzbekistan Sum',860);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('2a0ea54fddbe403d8add8b957e9671f2',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MUR','Mauritius Rupee',480);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('2e86d90f9ae243bbb0cba3c79ae72f7c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'EUR','Euro',978);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('2ede8accfc3b4f72bec44244bf17d364',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LSL','Loti',426);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('2f94ecd3ed2a414baaf9910e45eeba2e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'RUB','Russian Ruble',643);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('31a091ca29374f23bf7b8accea1efbcd',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ILS','New Israeli Sheqel',376);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('342491db15054bb7886af95c9abff390',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GEL','Lari',981);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('36172337d74044a3afdc5c8811dd68f0',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SBD','Solomon Islands Dollar',90);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('362e6f2dceeb41c3951ca8ae552efb54',1,'SYSTEM','2010-04-29 00:00:00',null,null,'PEN','Nuevo Sol',604);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('3883e41a68dc443c844353e78625ea52',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SZL','Lilangeni',748);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('395ac4ef5a8a4231b70cd2b85eb9cb72',1,'sergey.buturlakin','2011-07-11 19:29:34',null,null,'MXV','Mexican Unidad de Inversion (UDI)',979);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('3adcac83cd2b4063929108e4971db320',1,'SYSTEM','2010-04-29 00:00:00',null,null,'DOP','Dominican Peso',214);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('3d3d38736fb4410693f26c6f7b4e5bba',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SCR','Seychelles Rupee',690);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('3d66d277e15c495c9f45a9411bd02a63',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MAD','Moroccan Dirham',504);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('3e1d67ff44e44b1584a3f24578d0d130',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GIP','Gibraltar Pound',292);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('3fcefaad4e824454a2fd38e969b4ec29',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MYR','Malaysian Ringgit',458);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('409ad043aeb748079138505f6d9ef52b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'IQD','Iraqi Dinar',368);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('411db11c99b34c1daff79623ccee8fc3',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LBP','Lebanese Pound',422);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('453f5f4973984eabb858620d2709c054',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LVL','Latvian Lats',428);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('454e3ec4f9074de79fbd6d4ed9a84a2b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MGA','Malagasy Ariary',969);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('4704490ee131462db0ab3f8fa048f385',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BWP','Pula',72);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('48111242305a4556aa9a10d297998167',1,'SYSTEM','2010-04-29 00:00:00',null,null,'NIO','Cordoba Oro',558);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('488e9be47aad4092bebe960a87e3060a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'JOD','Jordanian Dinar',400);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('49e2c21cc7734974a769b959e54318d9',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CNY','Yuan Renminbi',156);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('49e7924dbbbb48daaa1e133113e47a1e',1,'sergey.buturlakin','2011-07-11 19:29:20',null,null,'MZN','Metical',943);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('4bb4fc3c4d414299ac2c7cabf262b71b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CAD','Canadian Dollar',124);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('4c83a02dee4148a18b5f17a92c5dcd82',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SOS','Somali Shilling',706);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('4c9fe5e73c7d450fb74c80099e89dd5a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SRD','Surinam Dollar',968);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('4df05d1c2ce6437b8661f0471ded9da8',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CRC','Costa Rican Colon',188);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('4ef41b17afce4162aff5fe12b6088538',1,'sergey.buturlakin','2011-07-11 19:29:56',null,null,'BOV','Mvdol',984);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('52f83089cb514a6caa63aaf096eb39d4',1,'SYSTEM','2010-04-29 00:00:00',null,null,'HKD','Hong Kong Dollar',344);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('53203def24f842c78aa5dd7c85c02426',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XPD','Palladium',964);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('53bc8686012c44c1a1d3b02f95228a0a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XDR','SDR',960);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('575ee37600e748ed96782f8287aa319f',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MWK','Kwacha',454);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('5959a4b786254f39b529847dfb54d0fb',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BBD','Barbados Dollar',52);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('5a40b1ee61c04690b39c3b002705bc78',1,'SYSTEM','2010-04-29 00:00:00',null,null,'AMD','Armenian Dram',51);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('5b92accf7e2746a8a1dd5461b9381aff',1,'SYSTEM','2010-04-29 00:00:00',null,null,'AFN','Afghani',971);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('5c17467782fb45658dc98596df4c36f8',1,'sergey.buturlakin','2011-07-11 19:26:11',null,null,'XBB','European Monetary Unit (E.M.U.-6)',956);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('5c183385557045d99fc0c3750f82a83b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'DZD','Algerian Dinar',12);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('5d77b6c8cefd4927852e0cccf8d88c63',1,'SYSTEM','2010-04-29 00:00:00',null,null,'PLN','Zloty',985);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('5ef28ab9626c44a4848811ba492e65e9',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BMD','Bermudian Dollar',60);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('60643b2441ea4ddd84b800e850c24ef9',1,'SYSTEM','2010-04-29 00:00:00',null,null,'VND','Dong',704);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('606f3a58566c46828ae28df221936d0c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'JPY','Yen',392);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('61de0e8a27cc4e8dba22c9132e4bb455',1,'SYSTEM','2010-04-29 00:00:00',null,null,'DKK','Danish Krone',208);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('651773db40f149539a41aa0c92acf804',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SEK','Swedish Krona',752);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('67984c94a3c8406aa8561c87e7651bb2',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SVC','El Salvador Colon',222);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('68139f13ca3b4745a2f816e3f269991b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'AOA','Kwanza',973);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('6bbfc3855d114105a4c6cf1d8af4cfd2',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BIF','Burundi Franc',108);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('6d7f798474ba4f1985186af11594cf7c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SHP','Saint Helena Pound',654);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('6dcf3ee7fc6b42c9ac2ec7de4e7515c2',1,'SYSTEM','2010-04-29 00:00:00',null,null,'HTG','Gourde',332);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('708437c01eaf47519969b8d40b640690',1,'sergey.buturlakin','2011-07-11 19:32:28',null,null,'USN','US Dollar (Next Day)',997);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('71186c33a31349fb8142f507662c90ce',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BHD','Bahraini Dinar',48);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('740c5e5fe70d47d8852252b2842a21a3',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MKD','Denar',807);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7449184c05cc42fd8cbf885473f121de',1,'SYSTEM','2010-04-29 00:00:00',null,null,'NZD','New Zealand Dollar',554);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7596207357114c8280e1d5a24e9d4bc3',1,'SYSTEM','2010-04-29 00:00:00',null,null,'PYG','Guarani',600);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7b02f75731c9472d94237acff9c3079f',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GMD','Dalasi',270);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7ba62d4e3b0547f693281686647e33cb',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BOB','Boliviano',68);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7cae96a9f6a846e487e63b04ad022964',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ANG','Netherlands Antillian Guilder',532);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7d5dc90fb5a54255a47f184b57581e35',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LRD','Liberian Dollar',430);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7ea1aad5d2834aea98d471cd4acc2ee4',1,'SYSTEM','2010-04-29 00:00:00',null,null,'INR','Indian Rupee',356);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('7f0c80ddd4ae4fd4b5699daa5f6d7749',1,'SYSTEM','2010-04-29 00:00:00',null,null,'EGP','Egyptian Pound',818);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('82e2e194d06e48c38c6f0c654b471786',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XPF','CFP Franc',953);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('83a42d146f5a44b2817f7c384d6bc129',1,'SYSTEM','2010-04-29 00:00:00',null,null,'USD','US Dollar',840);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('84ab278688bd4601b047a8af4a270323',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GHC','Ghana, Cedis',276);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('8dd76f2813f4463597cdd7029213e081',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CLP','Chilean Peso',152);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('8f86f85496e2479ea382dabc70da53a0',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BRL','Brazilian Real',986);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('90f61b276ddc44a68105c76089078ff8',1,'SYSTEM','2010-04-29 00:00:00',null,null,'HNL','Lempira',340);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('925849616d884292a66a930c23728602',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BSD','Bahamian Dollar',44);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('939a3797c4244a259169252c133beff7',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SKK','Slovakia, Koruny',63);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('9480c5ead9bf4ed8b9d3b765f6edbd41',1,'SYSTEM','2010-04-29 00:00:00',null,null,'COP','Colombian Peso',170);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('9543d88d884949c0ba6e27b660e4e4c6',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CUP','Cuban Peso',192);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('978e3ceb15fe4c028e36841a79d33d1e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ISK','Iceland Krona',352);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('98181e0f32494827b9e85d92af5eae48',1,'SYSTEM','2010-04-29 00:00:00',null,null,'HRK','Croatian Kuna',191);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('98bbb24b5aa441fa9809842018be52a4',1,'SYSTEM','2010-04-29 00:00:00',null,null,'THB','Baht',764);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('9a9f8cc464c34a048b6bce3677071722',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LTL','Lithuanian Litas',440);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('9babd5384dec481083090da9bea1b383',1,'SYSTEM','2010-04-29 00:00:00',null,null,'PGK','Kina',598);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('a26114a2cde84f029ddd5001323d88c9',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KWD','Kuwaiti Dinar',414);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('a3c8595663d54a7088f3410b4eb06e54',1,'SYSTEM','2010-04-29 00:00:00',null,null,'QAR','Qatari Rial',634);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('a9f24e9029224f6a820427e04cab8fae',1,'sergey.buturlakin','2011-07-11 19:32:45',null,null,'USS','US Dollar (Same Day)',998);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('aad5f1f8893d4a5ca96aa8d136bb807b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ZWD','Zimbabwe, Zimbabwe Dollars',382);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ab01a5783ba94890a45d70081b3b39ff',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BDT','Taka',50);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('af011fb824ac4d58bc69e9fcec60802c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'TZS','Tanzanian Shilling',834);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('af496e4b4de647afa7b61eacfd1a6f4f',1,'SYSTEM','2010-04-29 00:00:00',null,null,'UAH','Hryvnia',980);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b167476f5e3f478da906f8e86c4bcd90',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BAM','Convertible Marks',977);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b359d55553c74dc38d8d8f264cac57a6',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ROL','Romania, Lei [being phased out]',66);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b438f561a12646368232565be0528469',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ETB','Ethiopian Birr',230);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b51a704eadaf42e29f51e27611c47a5f',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BND','Brunei Dollar',96);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b586da2243174d8a8fa1674ff98e4887',1,'sergey.buturlakin','2011-07-11 19:26:42',null,null,'XBD','European Unit of Account 17 (E.U.A.-17)',958);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b58cb8c115a44a829dc0d87edafd5d0e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XCD','East Caribbean Dollar',951);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b8be91dbaa844a9eb451dd56cc90aebf',1,'SYSTEM','2010-04-29 00:00:00',null,null,'TND','Tunisian Dinar',788);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('b92b04d28454478dbc6d6984d2725136',1,'sergey.buturlakin','2011-07-11 19:32:06',null,null,'UYI','Uruguay Peso en Unidades Indexadas',940);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('bab7187ca602474eb1ffd5b598ad3342',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BTN','Ngultrum',64);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('bb1539b43b3d4c76abec8d8d95523066',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MTL','Malta, Liri',46);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('bb18fbb9ea7b4c30a77880402427f2b7',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SAR','Saudi Riyal',682);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('bbeb9dea6cb84839a303de2111fdf53a',1,'sergey.buturlakin','2011-07-11 19:25:45',null,null,'XTS','Codes specifically reserved for testing purposes',963);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('bca303dbda474ff8bb41d15394d3f61d',1,'SYSTEM','2010-04-29 00:00:00',null,null,'NOK','Norwegian Krone',578);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('bdc46af8625943b68c3bcf9a0619587f',2,'sergey.buturlakin','2011-07-11 19:24:00','sergey.buturlakin','2011-07-11 19:24:15','VEF','Bolivar Fuerte',937);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('c1a06adb4e4f4de8b522eb14a4f82c52',1,'SYSTEM','2010-04-29 00:00:00',null,null,'NGN','Naira',566);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('c207ceb5fdc6440c81f47b16507e97d8',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CHF','Swiss Franc',756);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('c3195b776d614f5bb3356277c348a47b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'IRR','Iranian Rial',364);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('c6188a8e93934b35a3ea1c59da27144a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XAU','Gold',959);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('c67c70aff8484592a2a97f723adbadc4',1,'sergey.buturlakin','2011-07-11 19:27:32',null,null,'GWP','Guinea-Bissau Peso',624);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('c67df08a75704a398ebd8dda7cc9c4c7',1,'sergey.buturlakin','2011-07-11 19:24:49',null,null,'XBA','Bond Markets Units European Composite Unit (EURCO)',955);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('c790df28b5da45e588caed8e2263500f',1,'sergey.buturlakin','2011-07-11 19:33:28',null,null,'ZWL','Zimbabwe Dollar',932);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ca9d5e359fdd4ee49c8336265d0fff19',1,'SYSTEM','2010-04-29 00:00:00',null,null,'PAB','Balboa',590);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('cbd5e98e1f3b4a9eb590ee21d584526b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'RWF','Rwanda Franc',646);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('cc30b24c610c40ad9b1117e295f2eb34',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GYD','Guyana Dollar',328);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('cd24e8aa65cd40ac9b0e3ecf2c82255d',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KZT','Tenge',398);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('cf40ec4e75e346f799d42f984a649912',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SDG','Sudanese Pound',938);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('cf49fff95e7c4ca2a59376266c6e5cb7',1,'SYSTEM','2010-04-29 00:00:00',null,null,'OMR','Rial Omani',512);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('cf55966ecc5d49cb879f33bc73a8f456',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KMF','Comoro Franc',174);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('cf98ebd514db48bba12dfc0daa1780c7',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KHR','Riel',116);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('d399295ab8224f8baa65e93ec8dbaeea',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LYD','Libyan Dinar',434);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('d3ecc69a32bf4dc88cece855d2496713',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MNT','Tugrik',496);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('d47cd03777bf40ed9416006c0aba62c3',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BGN','Bulgarian Lev',975);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('d71b85c552ee485fac6dbd259e79f81c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'PKR','Pakistan Rupee',586);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('d86ee11ac2fd4ae99529afa4c813e68f',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XAG','Silver',961);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('d90bf8482d38466da7d2a4d6bc403297',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SGD','Singapore Dollar',702);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('dbf931fce18643608a39ce4d6e42c4b9',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LKR','Sri Lanka Rupee',144);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('dca4f7cf0ed6432ea6d114d0027e72ad',1,'SYSTEM','2010-04-29 00:00:00',null,null,'AUD','Australian Dollar',36);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('dcbf0c2ef443431c86bad620ebdbae2a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MVR','Rufiyaa',462);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('df025af74b394b43ab3dea8eea045648',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BYR','Belarussian Ruble',974);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('df0bf4a5a7494b50af36d44162989f4e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'EEK','Kroon',233);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('df97c5ee8f1c416bb2f694070456c93c',1,'SYSTEM','2010-04-29 00:00:00',null,null,'TOP','Pa''anga',776);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e0aa812f698d42d5a9a801ac9a88700b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MMK','Kyat',104);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e11a45ced9e246c79c36d2f21917590b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'AZN','Azerbaijanian Manat',944);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e2141a7bba4d42e7975d33786dd28983',1,'SYSTEM','2010-04-29 00:00:00',null,null,'AED','UAE Dirham',784);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e2e4bccf068849e2ae0842d5aa60ae01',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KRW','Won',410);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e6eeb733372f40ee8006302bba8bab7a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CZK','Czech Koruna',203);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e73ec8a5be164733b769022b0d647d2c',1,'sergey.buturlakin','2011-07-11 19:30:37',null,null,'CUC','Peso Convertible',931);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e78e240368004137b2fd5380b68f9d11',1,'SYSTEM','2010-04-29 00:00:00',null,null,'BZD','Belize Dollar',84);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e81587a11c744172b26911e18c832b5b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'UYU','Peso Uruguayo',858);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('e8ddefbc49df4d11bdce59db9a626853',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KGS','Som',417);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('eb48b56a661941268447842e349e4bd2',1,'SYSTEM','2010-04-29 00:00:00',null,null,'TJS','Somoni',972);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ebc9246b3c0f43648dc8f71835799fe2',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KES','Kenyan Shilling',404);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ee00d9eb88ca4996a5fad6ece4d0eaa9',1,'sergey.buturlakin','2011-07-11 19:31:27',null,null,'COU','Unidad de Valor real',970);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ef896fc80aad4061952fe8b4a7d7043e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'IDR','Rupiah',360);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ef9ecb7f0ae54230bd633657e82e2d32',1,'SYSTEM','2010-04-29 00:00:00',null,null,'DJF','Djibouti Franc',262);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('efc3ae417f4e4d01ba348db0813c6272',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ZMK','Zambian Kwacha',894);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f298fea05b7c459f8c726ea3361c6630',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MDL','Moldovan Leu',498);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f2c31ad45b0f4458ab669f77895c25a9',1,'SYSTEM','2010-04-29 00:00:00',null,null,'WST','Tala',882);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f2e6b217d587436b930663fba39905b6',1,'SYSTEM','2010-04-29 00:00:00',null,null,'MRO','Ouguiya',478);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f3743ec2ea72421083a345c74003d32d',1,'SYSTEM','2010-04-29 00:00:00',null,null,'UGX','Uganda Shilling',800);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f38dc1211e694f9397fe5ea750e79f8e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'SLL','Leone',694);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f43a0844e3a243cc870c6686da8c579d',1,'SYSTEM','2010-04-29 00:00:00',null,null,'GNF','Guinea Franc',324);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f55ff94884cd45f5ad3a66f23c1023d7',1,'SYSTEM','2010-04-29 00:00:00',null,null,'YER','Yemeni Rial',886);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f5f17a31dc194c08bd7c767d2a291fa9',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XOF','CFA Franc BCEAO',952);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f6375954a16347ae9cdd000f95cfda63',1,'SYSTEM','2010-04-29 00:00:00',null,null,'LAK','Kip',418);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f7ec21d3ff594393aaec6f5b560f8cf1',1,'sergey.buturlakin','2011-07-11 19:31:08',null,null,'XXX','The codes assigned for transactions where no currency is involved are:',999);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('f86d2ab781a54931b56d46e1ddc774d8',1,'SYSTEM','2010-04-29 00:00:00',null,null,'CDF','Franc Congolais',976);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('fb214dc1979a49c8bee43d8c72f3d881',1,'sergey.buturlakin','2011-07-11 19:26:59',null,null,'XBC','European Unit of Account 9 (E.U.A.-9)',957);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('fc4b05fa2f034a7a976c1f317956d15b',1,'SYSTEM','2010-04-29 00:00:00',null,null,'XPT','Platinum',962);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('fc5417a6b2e747e28fee81813a20647f',1,'sergey.buturlakin','2011-07-11 19:35:13',null,null,'RSD','Serbian Dinar',941);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('fe2675ccdccd4c1a982fbd1d9ec52f6a',1,'SYSTEM','2010-04-29 00:00:00',null,null,'KPW','North Korean Won',408);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ff53ceb8297b48689fc9daca8023216e',1,'SYSTEM','2010-04-29 00:00:00',null,null,'TWD','New Taiwan Dollar',901);
insert into `lt_currency_new`(`id`,`version`,`createdby`,`createdon`,`modifiedby`,`modifiedon`,`code`,`name`,`numericcode`) values ('ffcd99b1c38c4054a9b2c155c964c652',1,'SYSTEM','2010-04-29 00:00:00',null,null,'ALL','Lek',8);

/*ufsa only*/
delete from lt_currency where not code in (select code from lt_currency_new) and code not in ('IUA', 'AUH');
/**/

update lt_currency c
   set name = (select n.name from lt_currency_new n where n.code = c.code),
       numericcode = (select n.numericcode from lt_currency_new n where n.code = c.code)
where code in (select code from lt_currency_new);

insert into lt_currency
 select * from lt_currency_new where not code in (select code from lt_currency);

drop table lt_currency_new;

/*ufsa only*/
update lt_currency
set numericcode = -1
where code = 'IUA';

update lt_currency
set numericcode = -2
where code = 'AUH';
/**/

alter table lt_currency
change `numericcode` `numericcode` int(11) not NULL,
add UNIQUE INDEX `numericcode` ( `numericcode` );

alter table lt_system_configuration
  add useaviadocumentvatinorder tinyint(1) NOT NULL after allowagentsetinvoicevat,
  add aviadocumentvatoptions int(11) NOT NULL;

update lt_system_configuration
  set useaviadocumentvatinorder = if(invoicevatoptions = 0, 0, 1),
      aviadocumentvatoptions = if(invoicevatoptions = 0 or invoicevatoptions = 1, 0, 1);

alter table lt_system_configuration
  drop column invoicevatoptions;

  
alter table lt_system_configuration
  add accountantdisplaystring varchar(255) after aviadocumentvatoptions;

 alter table lt_consignment
 add `discount_amount` decimal(19,5) DEFAULT NULL,
 add `discount_currency` varchar(32) DEFAULT NULL,
 add KEY `discount_currency` ( `discount_currency` );
 
 ALTER TABLE `lt_consignment` ADD CONSTRAINT `consignment_discount_currency_fk`
 FOREIGN KEY ( `discount_currency` ) REFERENCES `lt_currency` ( `id` );

 
 