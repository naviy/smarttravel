using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;




namespace Luxena.Travel.Tests.Parsers
{



	using static Assert;






	//===g






	[TestFixture]
	public class AmadeusConsoleParserTests
	{

		//---g



		private List<AviaDocument> Parse(string content)
		{
			return AmadeusConsoleParser
				.Parse(content, new Currency("UAH"))
				.ToList()
			;
		}



		//---g



		[Test]
		public void TestParseTicket01()
		{

			var docs = Parse(@"
--- TST RLR ---                                                                 
RP/IEVU23401/IEVU23401            MA/SU  20JUN13/0752Z   4RDSMO              
IEVU23401/1988MA/20JUN13                                                        
  1.CHEREPANOVA/DARIA MRS   2.KUBRUSHKO/IURII MR                                
  3  OS 661 E 21JUN 5 VIEKBP HK2  0900    0945 1240   *1A/E*                    
  4  OS 662 K 19OCT 6 KBPVIE HK2  1230 D  1330 1435   *1A/E*                    
  5 AP IEV NA - ARIOLA GROUP LTD - A                                            
  6 APE KUBRUSHKO@YAHOO.COM                                                     
  7 TK TL20JUN/IEVU23401                                                        
  8 FE PAX REF NOT PERM/CHNGS NOT PER NONENDO/S3-4/P1-2                         
  9 FV PAX OS/S3-4/P1-2                                                         


>tqt
TST00001     IEVU23401 MA/20JUN I 0 LD 21JUN13 OD VIEVIE SI                     
T-                                                                              
FXB                                                                             
   1.CHEREPANOVA/DARIA MRS   2.KUBRUSHKO/IURII MR                               
 1   VIE OS  661 E 21JUN 0945  OK ENN30E2S        21JUN21JUN 1PC                
 2 O KBP OS  662 K 19OCT 1330  OK KNN30E5S        19OCT19OCT 1PC                
	 VIE                                                                        
FARE  F EUR     420.00                                                          
EQUIV   UAH       4501                                                          
TX001 X UAH      408YQAC TX002 X UAH      136YKAE TX003 X UAH       16UDDP      
TX004 X UAH       32UASE TX005 X UAH      193ZYAE TX006 X UAH       75QDAP      
TX007 X UAH       83ATSE                                                        
TOTAL   UAH       5444    BSR 10.71541                                          
GRAND TOTAL UAH       5444                                                      
VIE OS IEV306.36OS VIE241.17NUC547.53END ROE0.767069                            
																				
  8.FE REF NOT PERM/CHNGS NOT PER NONENDO                                       
  9.FV OS"
			);



			docs.AssertAll(

				a => a
					.PnrCode("4RDSMO")

			);


			docs.Assert(

				a => a
					.PassengerName("CHEREPANOVA/DARIA MRS")
					.Fares("EUR", 420m, "UAH", 4501m, "UAH", 5444m)
				,

				a => a
					.PassengerName("KUBRUSHKO/IURII MR")
					.Fares("EUR", 420m, "UAH", 4501m, "UAH", 5444m)

			);

		}



		[Test]
		public void TestParseTicket02()
		{

			var docs = Parse(@"
--- TST RLR ---                                                                 
RP/IEVU23561/IEVU23561            HG/SU  23MAY13/1431Z   ZY7W6B                 
IEVU23561/1984LX/18MAY13                                                        
  1.DOVHANYUK/TARAS MR                                                          
  2  PS 471 L 22MAY 3 KBPZRH         FLWN                                       
  3  HG8537 N 22MAY 3 ZRHVIE         FLWN                                       
  4  PS 848 M 19JUN 3 VIEKBP HK1          1750 2045   *1A/E*                    
  5 MIS 1A HK1 BER 13APR                                                        
  6 AP IEV 38044-4902888 - ARIOLA GROUP LTD - A                                 
  7 AP 050 311 55 26                                                            
  8 APE DOVHANYUK@UKR.NET                                                       
  9 TK OK18MAY/IEVU23401//ETAB                                                  
 10 TK PAX OK18MAY/IEVU23401//ETPS/S2,4                                         
 11 SSR OTHS 1A AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY 1049/20MAY                
	   /IEV LT                                                                  
 12 SSR OTHS 1A PLS ISSUE TICKET BY 2359/21MAY2013                              
 13 SSR OTHS 1A AB RESERVES THE RIGHT TO AUTOCANCEL OR SEND ADM                 
	   IF NO TKT IS ISSUED                                                      
 14 SSR DOCS PS HK1 P/UKR/AK279473/UKR/22JUN86/M/28JAN15/DOVGANY                
	   UK/TARAS                                                                 
 15 SSR DOCS HG HK1 P/UKR/AK279473/UKR/22JUN86/M/28JAN15/DOVGANY                
	   UK/TARAS                                                                 
)>tqt
T     P/S  NAME                   TOTAL            FOP                 SEGMENTS 
3    .1  TDOVHANYUK/TARAS MR      UAH        3690  INVOICE                2,4   
4    .1  TDOVHANYUK/TARAS MR      UAH        1702  INVOICE                3     
																				
DELETED TST RECORDS MAY EXIST - PLEASE USE TTH                                  
>tqt/t3
TST00003     IEVU23401 EV/18MAY I 0 LD 19MAY13 OD IEVIEV SI                     
T-                                                                              
FXB/R,UP,VC-PS/S2,4                                                             
   1.DOVHANYUK/TARAS MR                                                         
 1   KBP PS  471 L 22MAY 1010  OK LPX1UA          22MAY22MAY 1PC                
 2   ZRH    ARNK                                                                
 3 O VIE PS  848 M 19JUN 1750  OK MADVUA          19JUN19JUN 1PC                
	 KBP                                                                        
FARE  F USD     283.00                                                          
EQUIV   UAH       2262                                                          
TX001 X UAH      906YQAC TX002 X UAH      136YKAE TX003 X UAH       16UDDP      
TX004 X UAH       32UASE TX005 X UAH      186ZYAE TX006 X UAH       72QDAP      
TX007 X UAH       80ATSE                                                        
TOTAL   UAH       3690    BSR 7.9930                                            
GRAND TOTAL UAH       3690                                                      
IEV PS ZRH137.50/-VIE PS IEV145.00NUC282.50END ROE1.000000                      
																				
*PS *                                                                           
																				
																				
 22.FE NONEND/REF AND RBKG RESTR                                                
 24.FM *M*1A                                                                    
)>tqt/t4
TST00004     IEVU23401 EV/18MAY I 0 LD 21MAY13 OD ZRHVIE SI                     
T-                                                                              
FXB/R,UP,VC-AB/S3                                                               
   1.DOVHANYUK/TARAS MR                                                         
 1   ZRH HG 8537 N 22MAY 1630  OK NNCOW           22MAY22MAY 1PC                
	 VIE                                                                        
FARE  F CHF     113.00                                                          
EQUIV   UAH        944                                                          
TX001 X UAH      412YQAC TX002 X UAH      346CHAE                               
TOTAL   UAH       1702    BSR 1.044277    USD 7.9930                            
GRAND TOTAL UAH       1702                                                      
ZRH HG VIE119.72NUC119.72END ROE0.943800                                        
																				
*AB *                                                                           
																				
																				
 23.FE NO REFUND/RBK RESTRICTED                                                 
 25.FM *M*1                                                                     
 26.FP INVOICE                                                                  
 28.FV AB                                             
"
			);


			docs.AssertAll(a => a.PnrCode("ZY7W6B"));

			docs.Assert(

				a => a
					.PassengerName("DOVHANYUK/TARAS MR")
					.Fares("USD", 283m, "UAH", 2262m, "UAH", 3690m)
				,

				a => a
					.PassengerName("DOVHANYUK/TARAS MR")
					.Fares("CHF", 113m, "UAH", 944m, "UAH", 1702m)

			);

		}



		[Test]
		public void TestParseTicket03()
		{

			var docs = Parse(@"
rt 2tk7ye
--- TST RLR ---                                                                 
RP/LEDR2233B/LEDR2233B            AZ/RM   1JUL13/1304Z   2TK7YE                 
  1.PALCHENOK/ELENA MRS                                                         
  2  FV 203 O 24AUG 6 LEDVIE HK1       1  0840 0940   *1A/E*                    
  3  ARNK                                                                       
  4  AZ7456 W 01SEP 7 VCELED HK1          2315 0425+1 *1A/E*                    
  5 AP LED +78123093500 - ACTIS - A                                             
  6 AP CTC NINA                                                                 
  7 TK OK01JUL/LEDR2233B//ETAZ                                                  
  8 TK PAX OK01JUL/LEDR2233B//ETFV/S2                                           
  9 SSR OTHS 1A TKTL WITHIN 04JUL OTHERWISE WILL BE CNLD                        
 10 SSR DOCS AZ HK1 P/RUS/642672251/RUS/20OCT64/F/27NOV15/PALCHE                
	   NOK/ELENA                                                                
 11 SSR DOCS FV HK1 P/RUS/642672251/RUS/20OCT64/F/27NOV15/PALCHE                
	   NOK/ELENA                                                                
 12 SSR OTHS AP HK/ 01JUL1304 RLOC PY8DQQ                                       
 13 FA PAX 195-3919562552/ETFV/RUB4171/01JUL13/LEDR2233B/9222498                
	   5/S2                                                                     
 14 FA PAX 055-3919562553/ETAZ/RUB6214/01JUL13/LEDR2233B/9222498                
	   5/S4                                                                     
 15 FB PAX 0100003491 TTP/ET/RT/T1 OK ETICKET/S2                                
 16 FB PAX 0100003492 TTP/ET/RT/T2 OK ETICKET/S4                                
)>md
--- TST RLR ---                                                                 
RP/LEDR2233B/LEDR2233B            AZ/RM   1JUL13/1304Z   2TK7YE                 
 10 SSR DOCS AZ HK1 P/RUS/642672251/RUS/20OCT64/F/27NOV15/PALCHE                
	   NOK/ELENA                                                                
 11 SSR DOCS FV HK1 P/RUS/642672251/RUS/20OCT64/F/27NOV15/PALCHE                
	   NOK/ELENA                                                                
 12 SSR OTHS AP HK/ 01JUL1304 RLOC PY8DQQ                                       
 13 FA PAX 195-3919562552/ETFV/RUB4171/01JUL13/LEDR2233B/9222498                
	   5/S2                                                                     
 14 FA PAX 055-3919562553/ETAZ/RUB6214/01JUL13/LEDR2233B/9222498                
	   5/S4                                                                     
 15 FB PAX 0100003491 TTP/ET/RT/T1 OK ETICKET/S2                                
 16 FB PAX 0100003492 TTP/ET/RT/T2 OK ETICKET/S4                                
 17 FE PAX FV ONLY/S2                                                           
 18 FE PAX AZ ONLY/S4                                                           
 19 FG PAX 0100789344 IEV1A098D/S2                                              
 20 FG PAX 0100789346 IEV1A098D/S4                                              
 21 FM PAX *M*43A/S4                                                            
 22 FM PAX *M*4/S2                                                              
 23 FP INVOICE                                                                  
 24 FV PAX FV/S2                                                                
 25 FV PAX AZ/S4                                                                
>tqt/t1
TST00001     LEDR2233B AK/01JUL I 0 LD 02JUL13 OD LEDVIE SI                     
T-                                                                              
FXP/ET/S2                                                                       
   1.PALCHENOK/ELENA MRS                                                        
 1   LED FV  203 O 24AUG 0840  OK OXSALE1         24AUG24AUG 25K                
	 VIE                                                                        
FARE  F EUR      55.00                                                          
EQUIV   RUB       2365                                                          
TX001 X RUB     1806YQAC                                                        
TOTAL   RUB       4171    BSR 43.00                                             
GRAND TOTAL RUB       4171                                                      
LED FV VIE72.31NUC72.31END ROE0.760562                                          
																				
NONREF/SALE                                                                     
																				
																				
 17.FE FV ONLY                                                                  
 22.FM *M*4                                                                     
 23.FP INVOICE                                                                  
 24.FV FV                                                                       
>tqt/t2
TST00002     LEDR2233B AK/01JUL I 0 LD 01SEP13 OD VCELED SI                     
T-                                                                              
FXP/ET/S3                                                                       
   1.PALCHENOK/ELENA MRS                                                        
 1   VCE AZ 7456 W 01SEP 2315  OK WMXP8C          01SEP01SEP 1PC                
	 LED                                                                        
FARE  F EUR     100.00                                                          
EQUIV   RUB       4300                                                          
TX001 X RUB      645YRVB TX002 X RUB      344RIDP TX003 X RUB       43MJAD      
TX004 X RUB       61EXAE TX005 X RUB      280HBCO TX006 X RUB      421ITEB      
TX007 X RUB      120VTSE                                                        
TOTAL   RUB       6214    BSR 43.00                                             
GRAND TOTAL RUB       6214                                                      
VCE AZ LED131.48NUC131.48END ROE0.760562                                        
																				
 18.FE AZ ONLY                                                                  
 21.FM *M*43A                                                                   
 23.FP INVOICE                                                                  
 25.FV AZ                                                                       
>"
			);


			docs.AssertAll(
				a => a
					.PnrCode("2TK7YE")
					.PassengerName("PALCHENOK/ELENA MRS")
			);


			docs.Assert(

				a => a
					.Fares("EUR", 55m, "RUB", 2365m, "RUB", 4171m)
					.AirlinePrefixCode("195")
					.Number("3919562552")
					.IataOffice("92224985")
					.AirlineIataCode("FV")
				,

				a => a
					.Fares("EUR", 100m, "RUB", 4300m, "RUB", 6214m)
					.AirlinePrefixCode("055")
					.Number("3919562553")
					.IataOffice("92224985")
					.AirlineIataCode("AZ")

			);

		}



		[Test]
		public void TestParseTicket06()
		{

			var docs = Parse(@"
--- TST RLR ---                                                                 
RP/LEDR2233B/LEDR2233B            AS/SU  12SEP13/1143Z   ZTV8KB                 
  1.TIRSKIY/NIKOLAY MR                                                          
  2  SU1411 N 18OCT 5 SVXSVO HK1          1300 1330   *1A/E*                    
  3 AP LED +78123093500 - ACTIS - A                                             
  4 TK TL19SEP/LEDR2233B                                                        
  5 FE PAX VALID ON SU/FARE RESTR APL/S2                                        
  6 FV PAX SU/S2                                                                
>
TST00001     LEDR2233B AS/12SEP I 0 LD 19SEP13 OD SVXMOW SI                     
T-                                                                              
FXP                                                                             
   1.TIRSKIY/NIKOLAY MR                                                         
 1   SVX SU 1411 N 18OCT 1300  OK NPXOWRF         18OCT18OCT 1PC                
	 SVO                                                                        
FARE  F RUB       3500                                                          
TX001 X RUB     1500YQAC TX002 X RUB      163YRVB                               
TOTAL   RUB       5163                                                          
GRAND TOTAL RUB       5163                                                      
SVX SU MOW3500.00RUB3500.00END                                                  
																				
  5.FE VALID ON SU/FARE RESTR APL                                               
  6.FV SU                                                                       
>"
			);


			docs.Assert(a => a
				.PnrCode("ZTV8KB")
				.PassengerName("TIRSKIY/NIKOLAY MR")
				.Fares("RUB", 3500m, null, null, "RUB", 5163m)
			);

		}



		[Test]
		public void TestParseTicket07()
		{

			var docs = Parse(@"
--- TST RLR ---                                                                 
RP/IEVU23561/IEVU23561            PS/RM   7OCT13/0645Z   7YC3W9                 
IEVU23561/1962VZ/22SEP13                                                        
  1.BLONSKA/YAROSLAVNA MRS                                                      
  2  PS 711 L 11OCT 5 KBPIST HK1       D  0640 0840   *1A/E*                    
  3  PS9559 W 11OCT 5 ISTKBP HK1          1820 2020   *1A/E*                    
  4 AP IEV 38044-4902888 - ARIOLA GROUP LTD - A                                 
  5 AP 594-85-57                                                                
  6 TK TL08OCT/IEVU23561                                                        
  7 SSR FQTV PS HK1 PS1141619                                                   
  8 SSR OTHS 1A AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY 1134/07OCT                
	   /IEV LT                                                                  
  9 SSR OTHS 1A AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY 0945/09OCT                
	   /IEV LT                                                                  
 10 FE PAX NONEND/NO REF/RBKG RSTR/S2-3                                         
 11 FV PAX PS/S2-3                                                              
>tqt
TST00010     IEVU23561 VZ/07OCT I 0 LD 11OCT13 OD IEVIEV SI                     
T-                                                                              
FXB/R,UP                                                                        
   1.BLONSKA/YAROSLAVNA MRS                                                     
 1   KBP PS  711 L 11OCT 0640  OK LSXUA           11OCT11OCT 1PC                
 2 O IST PS 9559 W 11OCT 1820  OK WPXUA           11OCT11OCT 20K                
	 KBP                                                                        
FARE  F USD     299.00                                                          
EQUIV   UAH       2390                                                          
TX001 X UAH      892YQAC TX002 X UAH      136YKAE TX003 X UAH       16UDDP      
TX004 X UAH       32UASE TX005 X UAH      120TRAE                               
TOTAL   UAH       3586    BSR 7.99300                                           
GRAND TOTAL UAH       3586                                                      
IEV PS IST94.50PS IEV Q50.00 154.50NUC299.00END ROE1.000000                     
																				
 10.FE NONEND/NO REF/RBKG RSTR                                                  
 11.FV PS"
			);


			docs.Assert(a => a
				.PnrCode("7YC3W9")
				.PassengerName("BLONSKA/YAROSLAVNA MRS")
				.Fares("USD", 299m, "UAH", 2390m, "UAH", 3586m)
				.AirlineIataCode("PS")
			);

		}


		[Test]
		public void TestParseTicket08()
		{

			var docs = Parse(@"
--- TST RLR ---                                                                 
RP/IEVU23561/IEVU23561            AL/GS  20MAR15/0835Z   3AMQ2J                 
IEVU23561/1982AL/19MAR15                                                        
  1.PLYNOKOS/MARYNA MISS(CHD/05FEB04)   2.USIKOVA/GALYNA MRS                    
  3  PS 182 G 27MAR 5 VNOKBP HK2          1150 1320   1A/E                    
  4 AP IEV 38044-4902888 - ARIOLA GROUP LTD - A                                 
  5 AP 254 09 38                                                                
  6 TK TL21MAR/IEVU23561                                                        
  7 SSR CHLD PS HK1 05FEB04/P1                                                  
  8 SSR OTHS 1A AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY PS BY 1558                
	   /21MAR/IEV LT                                                            
  9 SSR DOCS PS HK1 P/UKR/AA050970/UKR/05FEB04/F/25DEC17/PLYNOKO                
	   S/MARYNA/P1                                                              
 10 SSR DOCS PS HK1 P/UKR/ES774332/UKR/31MAY59/F/20JAN25/USIKOVA                
	   /GALYNA/P2                                                               
 11 OSI PS CTC 254 09 38                                                        
 12 FE PAX NONEND/NO REF/RBK 75EUR/S3/P1                                        
 13 FE PAX NONEND/NO REF/RBK 75EUR/S3/P2                                        
 14 FV PAX PS/S3/P1                                                             
 15 FV PAX PS/S3/P2                                                             
>tqt/t3
TST00003     IEVU23561 AL/20MAR I 0 LD 25MAR15 OD VNOIEV SI                     
T-                                                                              
FXB                                                                             
   1.PLYNOKOS/MARYNA MISS(CHD/05FEB04)                                          
 1   VNO PS  182 G 27MAR 1150  OK GOWLT    CH25              1PC                
	 KBP                                                                        
FARE  F EUR      42.00                                                          
EQUIV   UAH       1043                                                          
TX001 X UAH      350-YQAC TX002 X UAH       24-YQAD TX003 X UAH        4-D3AP   
TX004 X UAH      159-LTDP                                                       
TOTAL   UAH       1580    BSR 24.8238                                           
GRAND TOTAL UAH       1580                                                      
VNO PS IEV47.78NUC47.78END ROE0.878934                                          
																				
 12.FE NONEND/NO REF/RBK 75EUR                                                  
 14.FV PS                                                                       
>tqt/t4
TST00004     IEVU23561 AL/20MAR I 0 LD 25MAR15 OD VNOIEV SI                     
T-                                                                              
FXB                                                                             
   2.USIKOVA/GALYNA MRS                                                         
 1   VNO PS  182 G 27MAR 1150  OK GOWLT                      1PC                
	 KBP                                                                        
FARE  F EUR      56.00                                                          
EQUIV   UAH       1391                                                          
TX001 X UAH      350-YQAC TX002 X UAH       24-YQAD TX003 X UAH        4-D3AP   
TX004 X UAH      159-LTDP                                                       
TOTAL   UAH       1928    BSR 24.8238                                           
GRAND TOTAL UAH       1928                                                      
VNO PS IEV63.71NUC63.71END ROE0.878934                                          
																				
 13.FE NONEND/NO REF/RBK 75EUR                                                  
 15.FV PS "
			);


			docs.AssertAll(a => a
				.PnrCode("3AMQ2J")
			);


			docs.Assert(

				a => a
					.PassengerName("PLYNOKOS/MARYNA MISS")
					.Fares("EUR", 42m, "UAH", 1043m, "UAH", 1580m)
					.AirlineIataCode("PS")
				,

				a => a
					.PassengerName("USIKOVA/GALYNA MRS")
					.Fares("EUR", 56m, "UAH", 1391m, "UAH", 1928m)
					.AirlineIataCode("PS")

			);

		}



		[Test]
		public void TestParseTicket09()
		{

			var docs = Parse(@"
--- TST RLR ---                                                                 
RP/IEVU23561/IEVU23561            AL/GS  20MAR15/0835Z   3AMQ2J                 
IEVU23561/1982AL/19MAR15                                                        
  1.PLYNOKOS/MARYNA MISS(CHD/05FEB04)   2.USIKOVA/GALYNA MRS                    
  2  PS 056 S 20JUN 2 ODSKBP HK1          0700 0800   1A/E
  3  PS 845 S 20JUN 2 KBPVIE HK1          0925 1030   1A/E
  4  PS 402 S 25JUN 7 FRAKBP HK1       2  1415 1750   1A/E
  5  PS 055 S 25JUN 7 KBPODS HK1          1935 2035   1A/E
  4 AP IEV 38044-4902888 - ARIOLA GROUP LTD - A                                 
  5 AP 254 09 38                                                                
  6 TK TL21MAR/IEVU23561                                                        
  7 SSR CHLD PS HK1 05FEB04/P1                                                  
  8 SSR OTHS 1A AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY PS BY 1558                
	   /21MAR/IEV LT                                                            
  9 SSR DOCS PS HK1 P/UKR/AA050970/UKR/05FEB04/F/25DEC17/PLYNOKO                
	   S/MARYNA/P1                                                              
 10 SSR DOCS PS HK1 P/UKR/ES774332/UKR/31MAY59/F/20JAN25/USIKOVA                
	   /GALYNA/P2                                                               
 11 OSI PS CTC 254 09 38                                                        
 12 FE PAX NONEND/NO REF/RBK 75EUR/S3/P1                                        
 13 FE PAX NONEND/NO REF/RBK 75EUR/S3/P2                                        
 14 FV PAX PS/S3/P1                                                             
 15 FV PAX PS/S3/P2                                                             
>tqt/t3
TST00003     IEVU23561 AL/20MAR I 0 LD 25MAR15 OD VNOIEV SI                     
T-                                                                              
FXB                                                                             
   1.PLYNOKOS/MARYNA MISS(CHD/05FEB04)                                          
 1   ODS PS  056 S 20JUN 0700  OK S2FUP2                     1PC               
 2 X KBP PS  845 S 20JUN 0925  OK S2FUP2                     1PC               
 3   VIE    ARNK                                                               
 4 O FRA PS  402 S 25JUN 1415  OK S2FUP2                     1PC               
 5 X KBP PS  055 S 25JUN 1935  OK S2FUP2                     1PC
FARE  F EUR      42.00                                                          
EQUIV   UAH       1043                                                          
TX001 X UAH      350-YQAC TX002 X UAH       24-YQAD TX003 X UAH        4-D3AP   
TX004 X UAH      159-LTDP                                                       
TOTAL   UAH       1580    BSR 24.8238                                           
GRAND TOTAL UAH       1580                                                      
VNO PS IEV47.78NUC47.78END ROE0.878934                                          
																				
 12.FE NONEND/NO REF/RBK 75EUR                                                  
 14.FV PS                                                                       
>tqt/t4
TST00004     IEVU23561 AL/20MAR I 0 LD 25MAR15 OD VNOIEV SI                     
T-                                                                              
FXB                                                                             
   2.USIKOVA/GALYNA MRS                                                         
 1   VNO PS  182 G 27MAR 1150  OK GOWLT                      1PC                
	 KBP                                                                        
FARE  F EUR      56.00                                                          
EQUIV   UAH       1391                                                          
TX001 X UAH      350-YQAC TX002 X UAH       24-YQAD TX003 X UAH        4-D3AP   
TX004 X UAH      159-LTDP                                                       
TOTAL   UAH       1928    BSR 24.8238                                           
GRAND TOTAL UAH       1928                                                      
VNO PS IEV63.71NUC63.71END ROE0.878934                                          
																				
 13.FE NONEND/NO REF/RBK 75EUR                                                  
 15.FV PS "
			);


			docs.AssertAll(a => a
				.PnrCode("3AMQ2J")
			);


			docs.Assert(

				a => a
					.PassengerName("PLYNOKOS/MARYNA MISS")
					.Fares("EUR", 42m, "UAH", 1043m, "UAH", 1580m)
					.AirlineIataCode("PS")
				,

				a => a
					.PassengerName("USIKOVA/GALYNA MRS")
					.Fares("EUR", 56m, "UAH", 1391m, "UAH", 1928m)
					.AirlineIataCode("PS")

			);

		}



		[Test]
		public void TestParseTicket10()
		{

			var docs = Parse(@"
TICKET REVALIDATION/REISSUE IS RECOMMENDED
--- TST RLR ---
RP/IEVPS2311/IEVPS2311            UF/SU  10MAY17/1347Z   TYFJD7
  1.CHEN/PEILUN MR   2.GONG/PING MR   3.LUO/QIN MR
  4.QIANG/BO MR   5.WANG/CHENGDIAN MR   6.WEI/AN MR
  7.YU/LI MR   8.ZHAI/JIANQUN MR   9.ZHANG/XIAOYONG MR
 10  PS 288 R 26JUN 1 PEKKBP HK9       2  0240 0835   *1A/E*
 11  PS 287 R 05JUL 3 KBPPEK HK9          1025 0110+1 *1A/E*
 12 AP IEV 38044 2067570 - UNIVERSAL FLIGHTS SALES AGENCY - A
 13 APE JLM@UKR.NET
 14 TK PAX OK10MAY/IEVPS2311//ETPS/S10-11/P1-9
 15 SSR DOCS PS HK1 P/CHN/E35063722/CHN/13FEB66/M/28JAN24/ZHANG/
	   XIAOYONG/P9
 16 SSR DOCS PS HK1 P/CHN/G48789836/CHN/07MAY72/M/23JAN21/GONG/P
	   ING/P2
 17 SSR DOCS PS HK1 P/CHN/G56300080/CHN/29AUG53/M/06NOV21/CHEN/P
	   EILUN/P1
 18 SSR DOCS PS HK1 P/CHN/G35460719/CHN/28OCT72/M/06JUL19/ZHAI/J
	   IANQUN/P8
 19 SSR DOCS PS HK1 P/CHN/G60404644/CHN/08MAR56/M/14MAR22/QIANG/
	   BO/P4
 20 SSR DOCS PS HK1 P/CHN/G42054278/CHN/04APR50/M/21APR20/LUO/QI
	   N/P3
)>
>
md
TICKET REVALIDATION/REISSUE IS RECOMMENDED
--- TST RLR ---
RP/IEVPS2311/IEVPS2311            UF/SU  10MAY17/1347Z   TYFJD7
 21 SSR DOCS PS HK1 P/CHN/G45224859/CHN/02NOV61/M/05SEP20/YU/LI
	   /P7
 22 SSR DOCS PS HK1 P/CHN/E39335141/CHN/11FEB49/M/23NOV24/WANG/C
	   HENGDIAN/P5
 23 SSR DOCS PS HK1 P/CHN/G53112125/CHN/01DEC63/M/14JUL21/WEI/AN
	   /P6
 24 SSR OTHS 1A DUPED WITH U2EB2H PS 2 5
 25 OSI PS CTC-38 067 505-36-52
 26 OSI PS CTC-38 099 622-42-44
 27 FA PAX 566-2404581248/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P1
 28 FA PAX 566-2404581249/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P2
 29 FA PAX 566-2404596800/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P3
 30 FA PAX 566-2404596801/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P4
 31 FA PAX 566-2404596802/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P5
 32 FA PAX 566-2404596803/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
)>
>
md
TICKET REVALIDATION/REISSUE IS RECOMMENDED
--- TST RLR ---
RP/IEVPS2311/IEVPS2311            UF/SU  10MAY17/1347Z   TYFJD7
	   0/S10-11/P6
 33 FA PAX 566-2404596804/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P7
 34 FA PAX 566-2404596805/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P8
 35 FA PAX 566-2404596806/ETPS/UAH1995/10MAY17/IEVPS2311/7232022
	   0/S10-11/P9
 36 FHE PAX 566-2404581218/P1
 37 FHE PAX 566-2404581219/P2
 38 FHE PAX 566-2404581220/P3
 39 FHE PAX 566-2404581221/P4
 40 FHE PAX 566-2404581222/P5
 41 FHE PAX 566-2404581223/P6
 42 FHE PAX 566-2404581224/P7
 43 FHE PAX 566-2404581225/P8
 44 FHE PAX 566-2404581226/P9
 45 FB PAX 0000000000 TTP/S10,12/T-PS/RT/P1 OK ETICKET -
	   WARNING: TICKET QUOTA LOW/S10-11/P1
 46 FB PAX 0000000000 TTP/S10,12/T-PS/RT/P2 OK ETICKET -
	   WARNING: TICKET QUOTA LOW/S10-11/P2
)>
>
tqt
T     P/S  NAME                   TOTAL            FOP                 SEGMENTS
3    .1  TCHEN/PEILUN MR          UAH         1995 O/CASH+/CASH           10-11
4    .2  TGONG/PING MR            UAH         1995 O/CASH+/CASH           10-11
5    .3  TLUO/QIN MR              UAH         1995 O/CASH+/CASH           10-11
6    .4  TQIANG/BO MR             UAH         1995 O/CASH+/CASH           10-11
7    .5  TWANG/CHENGDIAN MR       UAH         1995 O/CASH+/CASH           10-11
8    .6  TWEI/AN MR               UAH         1995 O/CASH+/CASH           10-11
9    .7  TYU/LI MR                UAH         1995 O/CASH+/CASH           10-11
10   .8  TZHAI/JIANQUN MR         UAH         1995 O/CASH+/CASH           10-11
11   .9  TZHANG/XIAOYONG MR       UAH         1995 O/CASH+/CASH           10-11
																			   
DELETED TST RECORDS MAY EXIST - PLEASE USE TTH                                 
>
tqt/t3
TST00003     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   1.CHEN/PEILUN MR                                                            
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 54.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 64.FO 566-2404581218IEV08MAY17/72320220/566-24045812184E1                     
 73.FP O/CASH+/CASH                                                            
 74.FV PS                                                                      
>
tqt/t4
TST00004     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   2.GONG/PING MR                                                              
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 55.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 65.FO 566-2404581219IEV08MAY17/72320220/566-24045812195E1                     
 73.FP O/CASH+/CASH                                                            
 75.FV PS                                                                      
>
tqt/t5
TST00005     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   3.LUO/QIN MR                                                                
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 56.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 66.FO 566-2404581220IEV08MAY17/72320220/566-24045812206E1                     
 73.FP O/CASH+/CASH                                                            
 76.FV PS                                                                      
>
tqt/t6
TST00006     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   4.QIANG/BO MR                                                               
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 57.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 67.FO 566-2404581221IEV08MAY17/72320220/566-24045812210E1                     
 73.FP O/CASH+/CASH                                                            
 77.FV PS                                                                      
>
tqt/t7
TST00007     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   5.WANG/CHENGDIAN MR                                                         
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 58.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 68.FO 566-2404581222IEV08MAY17/72320220/566-24045812221E1                     
 73.FP O/CASH+/CASH                                                            
 78.FV PS                                                                      
>
tqt/t8
TST00008     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   6.WEI/AN MR                                                                 
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 59.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 69.FO 566-2404581223IEV08MAY17/72320220/566-24045812232E1                     
 73.FP O/CASH+/CASH                                                            
 79.FV PS                                                                      
>
tqt/t9
TST00009     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   7.YU/LI MR                                                                  
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 60.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 70.FO 566-2404581224IEV08MAY17/72320220/566-24045812243E1                     
 73.FP O/CASH+/CASH                                                            
 80.FV PS                                                                      
>
tqt/t10
TST00010     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   8.ZHAI/JIANQUN MR                                                           
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 61.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 71.FO 566-2404581225IEV08MAY17/72320220/566-24045812254E1                     
 73.FP O/CASH+/CASH                                                            
 81.FV PS                                                                      
>
tqt/t11
TST00011     IEVPS2311 UF/10MAY I 0 LD 26JUN17 2359  OD BJSBJS                 
T-                                                                             
FXQ/S10,12                                                                     
   9.ZHANG/XIAOYONG MR                                                         
 1   PEK PS  288 R 26JUN 0240  OK R2LFP5               26JUN 2PC               
 2 O KBP PS  287 R 05JUL 1025  OK R2LFP5               26JUN 2PC               
	 PEK                                                                       
FARE  R CNY       3120                                                         
EQUIV   UAHUAH                                                                 
TX001 X UAH        6-YRVB TX002 X UAH     1989-CP   TX003 O UAH      347-CNAE  
TX004 O UAH      106-UASE TX005 O UAH      451-YKAE TX006 O UAH     6570-YRVB  
TOTAL   UAH       1995    BSR 3.848556                                         
GRAND TOTAL UAH       1995                                                     
BJS PS IEV225.87PS BJS225.87NUC451.74END ROE6.906520                           
																			   
 62.FE NONEND/NO REF/RBK 75USD                                                 
 63.FM *M*1.5                                                                  
 72.FO 566-2404581226IEV08MAY17/72320220/566-24045812265E1                     
 73.FP O/CASH+/CASH                                                            
 82.FV PS   "
			);


			docs.AssertAll(a => a
				.PnrCode("TYFJD7")
				.AirlineIataCode("PS")
			);


			docs.Assert(

				a => a
					.PassengerName("CHEN/PEILUN MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
						,

				a => a
					.PassengerName("GONG/PING MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
				,

				a => a
					.PassengerName("LUO/QIN MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
				,

				a => a
					.PassengerName("QIANG/BO MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
				,

				a => a
					.PassengerName("WANG/CHENGDIAN MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
				,

				a => a
					.PassengerName("WEI/AN MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
				,

				a => a
					.PassengerName("YU/LI MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
				,

				a => a
					.PassengerName("ZHAI/JIANQUN MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)
				,

				a => a
					.PassengerName("ZHANG/XIAOYONG MR")
					.Fares("CNY", 3120m, null, null, "UAH", 1995m)

			);

		}



		[Test]
		public void TestParseTicket11()
		{


			var docs = Parse(@"
--- TST TSM RLR MSC ---
RP/IEVPS2316/IEVPS2316            KB/SU  15MAY17/0709Z   TLXDUG
  1.STOLITNIY/DMYTRO MSTR(CHD)(IDDOB27SEP10)
  2  PS 024 W 05AUG 6 HRKKBP HK1          0700 0800   1A/E
  3  PS 485 W 05AUG 6 KBPGVA HK1          1045 1245   1A/E
  4 AP IEV 38044 2067570 - UFSA UA - A
  5 AP HRK +380503881481
  6 TK OK15MAY/IEVPS2316//ETPS
  7 SSR CHLD PS HK1
  8 SSR DOCS PS HK1 P/UKR/FA959774/UKR/27SEP10/M/10NOV19/STOLITN
	   IY/DMYTRO/S2
  9 SSR DOCS PS HK1 P/UKR/FA959774/UKR/27SEP10/M/10NOV19/STOLITN
	   IY/DMYTRO/S3
 10 /SSR PDBG PS HK1 UPTO 50LB 23KG BAGGAGE/S2
 11 /SSR PDBG PS HK1 UPTO 50LB 23KG BAGGAGE/S3
 12 OSI PS CTC 380503881481
 13 RII TOTAL TICKET COST 7765.00 UAH
 14 FA PAX 566-2404611214/ETPS/UAH7498/15MAY17/IEVPS2316/7232022
	   0/S2-3
 15 FA PAX 566-8207017233/DTPS/UAH714/15MAY17/IEVPS2316/72320220
	   /E10-11
 16 FB PAX 0000000000 TTP/T-PS/RT OK ETICKET - WARNING: TICKET
	   QUOTA LOW/S2-3
>
tqt
TST00001     IEVPS2316 KB/15MAY I 0 LD 05AUG17 2359  OD HRKGVA                 
T-                                                                             
FXB                                                                            
   1.STOLITNIY/DMYTRO MSTR(CHD)(IDDOB27SEP10)                                  
 1   HRK PS  024 W 05AUG 0700  OK WL1SUP2  CH25              1PC               
 2 X KBP PS  485 W 05AUG 1045  OK WL1SUP2  CH25              1PC               
	 GVA                                                                       
FARE  F USD     236.00                                                         
EQUIV   UAH       6240                                                         
TX001 X UAH      556-YRVB TX002 X UAH      146-UASE TX003 X UAH      556-YKAE  
TOTAL   UAH       7498    BSR 26.44                                            
GRAND TOTAL UAH       7498                                                     
HRK PS X/IEV PS GVA235.50NUC235.50END ROE1.000000                              
																			   
 18.FE NONEND/REF RSTR/RBK 50USD                                               
 19.FM *M*1.5                                                                  
 20.FP CASH                                                                    
 21.FV PS"
			);


			docs.Assert(a => a
				.PnrCode("TLXDUG")
				.PassengerName("STOLITNIY/DMYTRO MSTR")
				.Fares("USD", 236m, "UAH", 6240m, "UAH", 7498m)
				.AirlineIataCode("PS")
			);

		}



		[Test]
		public void TestParseTicket12()
		{

			var docs = Parse(@"
--- TST RLR RLP SFP ---
RP/IEVPS2326/IEVPS2326            LS/SU  30MAY17/0715Z   U9JAE9
  1.MAKHINIA/OLENA MRS   2.MOKHON/ALISA MISS(CHD)(IDDOB14DEC14)
  3.MOKHON/ANTON MSTR(CHD)(IDDOB22FEB07)   4.MOKHON/TARAS MR
  5  PS 231 M 03AUG 4 KBPJFK HK4          1100 1420   1A/E
  6  PS 232 M 29AUG 2 JFKKBP HK4       7  0030 1705   1A/E
  7 AP IEV 38 044 300 01 75 - FLAMINGO TRAVEL - A
  8 AP 380675636229-M/P4
  9 AP 380675636229-M/P3
 10 AP 380675620821-M/P1
 11 APE TARAS1980@ICLOUD.COM-H/P4
 12 APE ANT.MOKHON@MAIL.RU-H/P3
 13 APE ELLEN_KIEV@MAIL.RU-H/P1
 14 TK PAX OK30MAY/IEVPS2326//ETPS/S5-6/P1,4
 15 TK OK30MAY/IEVPS2326//ETPS
 16 SSR CHLD PS HK1/P2
 17 SSR CHLD PS HK1/P3
 18 SSR DOCS PS HK1 P/UKR/AA276631/UKR/22FEB07/M/03MAR18/MOKHON/
	   ANTON/P3
 19 *SSR FQTV PS HK/ PS1002793400/3/P4
 20 *SSR FQTV PS HK/ PS1004198963/3/P3
 21 *SSR FQTV PS HK/ PS1002793422/3/P1
 22 SSR DOCS PS HK1 P/UKR/EA893494/UKR/05NOV79/F/17APR18/MAKHINI
)>
>
md
--- TST RLR RLP SFP ---
RP/IEVPS2326/IEVPS2326            LS/SU  30MAY17/0715Z   U9JAE9
	   A/OLENA/P1
 23 SSR DOCS PS HK1 P/UKR/AA276630/UKR/14DEC14/F/03MAR18/MOKHON/
	   ALISA/P2
 24 SSR DOCS PS HK1 P/UKR/EE607318/UKR/27FEB80/M/15OCT18/MOKHON/
	   TARAS/P4
 25 RM NOTIFY PASSENGER PRIOR TO TICKET PURCHASE & CHECK-IN:
	   FEDERAL LAWS FORBID THE CARRIAGE OF HAZARDOUS MATERIALS -
	   GGAMAUSHAZ/S5-6
 26 FA PAX 566-2404628224/ETPS/UAH23831/30MAY17/IEVPS2326/723234
	   95/S5-6/P1
 27 FA PAX 566-2404628225/ETPS/UAH23831/30MAY17/IEVPS2326/723234
	   95/S5-6/P4
 28 FA PAX 566-2404628226/ETPS/UAH20122/30MAY17/IEVPS2326/723234
	   95/S5-6/P2
 29 FA PAX 566-2404628227/ETPS/UAH20122/30MAY17/IEVPS2326/723234
	   95/S5-6/P3
 30 FB PAX 0000000000 TTP/T1/RT/T-PS OK ETICKET - WARNING:
	   TICKET QUOTA LOW/S5-6/P1,4
 31 FB PAX 0000000000 TTP/T2/RT/T-PS OK ETICKET - WARNING:
	   TICKET QUOTA LOW/S5-6/P2-3
 32 FE PAX NONENDO/NO REF/RBK USD200 -BG:PS/S5-6/P1,4
)>
>
tqt/t1
TST00001     IEVPS2326 LS/29MAY I 0 LD 04JUL17 2359  OD IEVIEV                 
T-                                                                             
FXB                                                                            
   1.MAKHINIA/OLENA MRS   4.MOKHON/TARAS MR                                    
 1   KBP PS  231 M 03AUG 1100  OK MH2A4U5         03AUG03AUG 1PC               
 2 O JFK PS  232 M 29AUG 0030  OK MH2A4U5         29AUG29AUG 1PC               
	 KBP                                                                       
FARE  F USD     564.00                                                         
EQUIV   UAH      14839                                                         
TX001 X UAH     6788-YRVB TX002 X UAH      106-UASE TX003 X UAH      448-YKAE  
TX004 X UAH      145-YCAE TX005 X UAH      474-USAP TX006 X UAH      474-USAS  
TX007 X UAH      105-XACO TX008 X UAH      185-XYCR TX009 X UAH      148-AYSE  
TX010 X UAH      119-XF                                                        
TOTAL   UAH      23831    BSR 26.31                                            
GRAND TOTAL UAH      23831                                                     
IEV PS NYC282.00PS IEV282.00NUC564.00END ROE1.000000 XF JFK4.5                 
																			   
 32.FE NONENDO/NO REF/RBK USD200 -BG:PS                                        
 34.FM *M*1A                                                                   
 35.FP CASH                                                                    
 36.FV PS                                                                      
>
tqt/t2
TST00002     IEVPS2326 LS/29MAY I 0 LD 04JUL17 2359  OD IEVIEV                 
T-                                                                             
FXB                                                                            
   2.MOKHON/ALISA MISS(CHD)(IDDOB14DEC14)                                      
   3.MOKHON/ANTON MSTR(CHD)(IDDOB22FEB07)                                      
 1   KBP PS  231 M 03AUG 1100  OK MH2A4U5  CH25   03AUG03AUG 1PC               
 2 O JFK PS  232 M 29AUG 0030  OK MH2A4U5  CH25   29AUG29AUG 1PC               
	 KBP                                                                       
FARE  F USD     423.00                                                         
EQUIV   UAH      11130                                                         
TX001 X UAH     6788-YRVB TX002 X UAH      106-UASE TX003 X UAH      448-YKAE  
TX004 X UAH      145-YCAE TX005 X UAH      474-USAP TX006 X UAH      474-USAS  
TX007 X UAH      105-XACO TX008 X UAH      185-XYCR TX009 X UAH      148-AYSE  
TX010 X UAH      119-XF                                                        
TOTAL   UAH      20122    BSR 26.31                                            
GRAND TOTAL UAH      20122                                                     
IEV PS NYC211.50PS IEV211.50NUC423.00END ROE1.000000 XF JFK4.5                 
																			   
 33.FE NONENDO/NO REF/RBK USD200 -BG:PS                                        
 34.FM *M*1A                                                                   
 35.FP CASH                                                                    
 37.FV PS"
			);


			docs.AssertAll(a => a
				.PnrCode("U9JAE9")
				.AirlineIataCode("PS")
			);


			docs.Assert(

				a => a
					.PassengerName("MAKHINIA/OLENA MRS")
					.Fares("USD", 564m, "UAH", 14839m, "UAH", 23831m)
				,

				a => a
					.PassengerName("MOKHON/TARAS MR")
					.Fares("USD", 564m, "UAH", 14839m, "UAH", 23831m)
				,

				a => a
					.PassengerName("MOKHON/ALISA MISS")
					.Fares("USD", 423m, "UAH", 11130m, "UAH", 20122m)
				,

				a => a
					.PassengerName("MOKHON/ANTON MSTR")
					.Fares("USD", 423m, "UAH", 11130m, "UAH", 20122m)

			);

		}




		[Test]
		public void TestParseTicket13()
		{

			var docs = Parse(@"
RP/IEVPS2326/IEVPS2326            TV/SU  31MAY17/0647Z   JW5EXP
  1.DANYLIUK/VALENTYN MR(ID DOB20FEB50)
  2.PRISHCHENKO/LIUDMYLA MRS(ID DOB31DEC50)
  3  PS 315 M 12AUG 6 KBPBGY HK2          2035 2215   1A/E
  4  PS 316 M 26AUG 6 BGYKBP HK2          0440 0810   1A/E
  5 AP IEV 38 044 300 01 75 - FLAMINGO TRAVEL - A
  6 APN M+380674409956/P1
  7 TK OK31MAY/IEVPS2326//ETPS
  8 SSR DOCS PS HK1 P/UKR/FG014716/UKR/20FEB50/M/04APR27/DANYLIU
	   K/VALENTYN//H/P1
  9 SSR DOCS PS HK1 P/UKR/FG014803/UKR/31DEC50/F/04APR27/PRISHCH
	   ENKO/LIUDMYLA//H/P2
 10 RC IEVPS2326-W/380958416848
 11 RC IEVPS2326-W/DANVAL1111@UKR.NET
 12 FA PAX 566-2404628237/ETPS/UAH4628/31MAY17/IEVPS2326/7232349
	   5/S3-4/P1
 13 FA PAX 566-2404628238/ETPS/UAH4628/31MAY17/IEVPS2326/7232349
	   5/S3-4/P2
 14 FB PAX 0000000000 TTP/T-PS OK ETICKET - WARNING: TICKET
	   QUOTA LOW/S3-4/P1-2
 15 FE PAX NONEND/NON REF/CHNG USD50/S3-4/P1-2
 16 FM *M*1A
)>
>
tqt
TST00001     IEVPS2326 TV/31MAY I 0 LD 29JUL17 2359  OD IEVIEV                 
T-                                                                             
FXB                                                                            
   1.DANYLIUK/VALENTYN MR(ID DOB20FEB50)                                       
   2.PRISHCHENKO/LIUDMYLA MRS(ID DOB31DEC50)                                   
 1   KBP PS  315 M 12AUG 2035  OK MN2Z3U1         12AUG12AUG 0PC               
 2 O BGY PS  316 M 26AUG 0440  OK MN2Z3U1         26AUG26AUG 0PC               
	 KBP                                                                       
FARE  F USD      90.00                                                         
EQUIV   UAH       2374                                                         
TX001 X UAH     1108-YRVB TX002 X UAH      106-UASE TX003 X UAH      449-YKAE  
TX004 X UAH       35-EXAE TX005 X UAH      192-HBCO TX006 X UAH      300-ITEB  
TX007 X UAH       17-MJAD TX008 X UAH       47-VTSE                            
TOTAL   UAH       4628    BSR 26.37                                            
GRAND TOTAL UAH       4628                                                     
IEV PS MIL45.00PS IEV45.00NUC90.00END ROE1.000000                              
																			   
 15.FE NONEND/NON REF/CHNG USD50                                               
 16.FM *M*1A                                                                   
 17.FP CASH                                                                    
 18.FV PS"
			);


			docs.AssertAll(a => a
				.PnrCode("JW5EXP")
				.AirlineIataCode("PS")
			);


			docs.Assert(

				a => a
					.PassengerName("DANYLIUK/VALENTYN MR")
					.Fares("USD", 90m, "UAH", 2374m, "UAH", 4628m)
				,

				a => a
					.PassengerName("PRISHCHENKO/LIUDMYLA MRS")
					.Fares("USD", 90m, "UAH", 2374m, "UAH", 4628m)

			);

		}



		[Test]
		public void TestParseTicket14()
		{

			var docs = Parse(@"
--- TST RLR ---
RP/IEVPS2326/IEVPS2326            YS/SU  14JUN17/1006Z   WDHI2J
  1.HERASIMOVA/NATALIIA MRS
  2  PS 713 M 28JUN 3 KBPIST HK1          1240 1440   1A/E
  3  PS 714 M 02JUL 7 ISTKBP HK1       I  1540 1740   1A/E
  4 AP IEV 38 044 300 01 75 - FLAMINGO TRAVEL - A
  5 TK OK14JUN/IEVPS2326//ETPS
  6 SSR DOCS PS HK1 P/UKR/FB998198/UKR/20DEC69/F/16DEC25/HERASIM
	   OVA/NATALIIA
  7 OSI PS CTC 380674018268
  8 RC IEVPS2326-W/YRA
  9 FA PAX 566-2404687184/ETPS/14JUN17/IEVPS2326/72323495/S2-3
 10 FB PAX 0000000000 TTP/T-PS OK ETICKET/S2-3
 11 FE PAX NONEND/ REF/ CHNG RESTR/PS/S2-3
 12 FM PAX *F*0.00/S2-3
 13 FP CASH
 14 FT PAX *F*TP20017/S2-3
 15 FV PAX *F*PS/S2-3
>
tqt
TST00002     IEVPS2326 YS/14JUN F N LD 28JUN17 2359  OD IEVIEV                 
T-                                                                             
FXB/R,U                                                                        
   1.HERASIMOVA/NATALIIA MRS                                                   
 1   KBP PS  713 M 28JUN 1240  OK MN2ZUP1  D15         28JUL 0PC               
 2 O IST PS  714 M 02JUL 1540  OK MN2ZUP1  D15         28JUL 0PC               
	 KBP                                                                       
FARE  I USD     125.00                                                         
EQUIV   UAH       3257                                                         
TX001 X UAH     1094-YRVB TX002 X UAH      105-UASE TX003 X UAH      443-YKAE  
TX004 X UAH      391-TRAE                                                      
TOTAL   UAH       5290    BSR 26.05                                            
GRAND TOTAL UAH       5290                                                     
IEV PS IST M/IT PS IEV M/IT END                                                
																			   
 11.FE NONEND/ REF/ CHNG RESTR/PS                                              
 12.FM *F*0.00                                                                 
 13.FP CASH                                                                    
 14.FT *F*TP20017                                                              
 15.FV *F*PS
"
			);


			docs.Assert(a => a
				.PnrCode("WDHI2J")
				.PassengerName("HERASIMOVA/NATALIIA MRS")
				.Fares("USD", 125m, "UAH", 3257m, "UAH", 5290m)
				.AirlineIataCode("PS")
			);

		}



		[Test]
		public void TestParseTicket15()
		{

			var docs = Parse(@"
--- TST RLR ---
RP/IEVPS2311/IEVPS2311            YR/SU  15JUN17/1008Z   MKNFGI
  1.FAN/XINNAN MS
  2  PS 288 Q 23JUL 7 PEKKBP HK1       2  0240 0835   1A/E
  3  PS 311 Q 23JUL 7 KBPMXP HK1          1040 1230   1A/E
  4  PS 316 J 04AUG 5 BGYKBP HK1          0410 0740   1A/E
  5  PS 287 J 04AUG 5 KBPPEK HK1          1025 0110+1 1A/E
  6 APE UA.OKSANA@MAIL.UA
  7 TK OK15JUN/IEVPS2311//ETPS
  8 SSR DOCS PS HK1 P/CHN/E35710419/CHN/13JUN92/F/16FEB24/FAN/XI
	   NNAN/S2
  9 SSR DOCS PS HK1 P/CHN/E35710419/CHN/13JUN92/F/16FEB24/FAN/XI
	   NNAN/S3
 10 SSR DOCS PS HK1 P/CHN/E35710419/CHN/13JUN92/F/16FEB24/FAN/XI
	   NNAN/S4
 11 SSR DOCS PS HK1 P/CHN/E35710419/CHN/13JUN92/F/16FEB24/FAN/XI
	   NNAN/S5
 12 FA PAX 566-2404744921-22/DTPS/UAH17088/15JUN17/IEVPS2311/723
	   20220/S2-5
 13 FB PAX 0000000000 TTP/ET/RT/T-PS OK ETICKET - WARNING:
	   TICKET QUOTA LOW/S2-5
 14 FE PAX NONEND/NO REF/RBK 100EUR/S2-5
 15 FM *M*1.5
)>
>tqt
TST00001     IEVPS2311 YR/15JUN I 0 LD 23JUL17 2359  OD BJSBJS                 
T-                                                                             
FXB                                                                            
   1.FAN/XINNAN MS                                                             
 1   PEK PS  288 Q 23JUL 0240  OK QH2SFP5                    2PC               
 2 X KBP PS  311 Q 23JUL 1040  OK QH2SFP5                    2PC               
 3   MXP    ARNK                                                               
 4 O BGY PS  316 J 04AUG 0410  OK JH2LFP5                    2PC               
 5 X KBP PS  287 J 04AUG 1025  OK JH2LFP5                    2PC               
	 PEK                                                                       
FARE  F CNY       2080                                                         
EQUIV   UAH       7963                                                         
TX001 X UAH     7546-YRVB TX002 X UAH      208-UASE TX003 X UAH      442-YKAE  
TX004 X UAH      345-CNAE TX005 X UAH       35-EXAE TX006 X UAH      190-HBCO  
TX007 X UAH      297-ITEB TX008 X UAH       16-MJAD TX009 X UAH       46-VTSE  
TOTAL   UAH      17088    BSR 3.828255                                         
GRAND TOTAL UAH      17088                                                     
BJS PS X/IEV PS MIL170.12PS X/IEV PS BJS131.03NUC301.15END                     
 ROE6.906520                                                                   
																			   
 14.FE NONEND/NO REF/RBK 100EUR                                                
 15.FM *M*1.5                                                                  
 16.FP CASH
"
			);


			docs.Assert(a => a
				.PnrCode("MKNFGI")
				.PassengerName("FAN/XINNAN MS")
				.Fares("CNY", 2080m, "UAH", 7963m, "UAH", 17088m)
				.AirlineIataCode("PS")
			);

		}




		[Test]
		public void TestParseTicket17()
		{

			var docs = Parse(@"
--- TST RLR MSC SFP ---                                                         
RP/IEVU23561/IEVU23561            AA/SU   3JUL17/1128Z   UINW8L                 
IEVU23561/2005SV/3JUL17                                                         
  1.KUDRENKO/MARYNA MRS                                                         
  2  AC9594 V 01SEP 5 KBPFRA HK1          0640 0820   1A/E                    
  3  AC9105 V 01SEP 5 FRAYYZ HK1       1  1410 1635   1A/E                    
  4  LH 495 K 25APR 3 YYZMUC HK1       1  2005 1010+1 1A/E                    
  5  LH2544 K 26APR 4 MUCKBP HK1       2  1230 1540   1A/E                    
  6 AP IEV 38044-4902888 - ARIOLA GROUP LTD - A                                 
  7 APE NATALIYA.KUDRENKO@GMAIL.COM                                             
  8 TK TL03JUL/IEVU23561                                                        
  9 OSI AC CTCP IEV 38044-4902888 - ARIOLA GROUP LTD - A                        
 10 OSI AC CTCT IEV 38044-4902888 ARIOLA GROUP LTD                              
 11 RC IEVU23561-W/!!!!!!!!!!!!!!!!!!!!!!!!ATTN STALO DESHEVLE P                
	   AX NE ZNAET*********************************                             
 12 FE PAX NONREF/FL/CHG RESTRICTED CHECK FARE NOTE -BG:AC/S2-5                 
 13 FV PAX LH/S2-5                                                              
>tqt
TST00001     IEVU23561 SV/03JUL I 0 LD 06JUL17 1428  OD IEVIEV                  
T-E                                                                             
FXB                                                                             
   1.KUDRENKO/MARYNA MRS                                                        
 1   KBP AC 9594 V 01SEP 0640  OK VLRCUAW         01SEP01SEP 1PC                
 2 X FRA AC 9105 V 01SEP 1410  OK VLRCUAW         01SEP01SEP 1PC                
 3 O YYZ LH  495 K 25APR 2005  OK KLNCUAW         25APR25APR 1PC                
 4 X MUC LH 2544 K 26APR 1230  OK KLNCUAW         26APR26APR 1PC                
	 KBP                                                                        
FARE  F USD     420.00                                                          
EQUIV   UAH      10967                                                          
TX001 X UAH     5818-YQAD TX002 X UAH      477-YRVB TX003 X UAH      105-UASE   
TX004 X UAH       53-UDDP TX005 X UAH      444-YKAE TX006 X UAH      285-DESE   
TX007 X UAH     1184-RAEB TX008 X UAH      519-CAAE TX009 X UAH       66-RCAB   
TX010 X UAH      501-SQAP                                                       
TOTAL   UAH      20419    BSR 26.11                                             
GRAND TOTAL UAH      20419                                                      
IEV AC X/FRA AC YTO300.00LH X/MUC LH IEV120.00NUC420.00END                      
 ROE1.000000                                                                    
																				
 12.FE NONREF/FL/CHG RESTRICTED CHECK FARE NOTE -BG:AC                          
 13.FV LH
"
			);


			docs.Assert(a => a
				.PnrCode("UINW8L")
				.PassengerName("KUDRENKO/MARYNA MRS")
				.Fares("USD", 420m, "UAH", 10967m, "UAH", 20419m)
				.AirlineIataCode("LH")
			);

		}



		[Test]
		public void TestParseTicket18()
		{

			var docs = Parse(@"
--- TST RLR RLP ---
RP/IEVPS2332/IEVPS2332            SD/GS  18SEP17/0850Z   OPEDS5
  1.GRIGA/MATVII MSTR
  2  PS 111 L 18FEB 7 KBPLGW HK1          1000 1125   1A/E
  3  PS 112 M 23MAR 5 LGWKBP HK1       S  1220 1740   1A/E
  4 AP IEV 38044-4902888 - ARIOLA GROUP LTD - A
  5 AP 8-050-331-81-10
  6 AP 380442348104
  7 APE OLGA@MIRELLE.COM.UA
  8 TK OK18SEP/IEVPS2332//ETPS
  9 *SSR FQTV PS HK/ PS1000825556/3
 10 SSR DOCS PS HK1 P/UKR/FE343973/UKR/18OCT00/M/04APR20/GRIGA/M
	   ATVII
 11 OP IEVPS2332/18SEP/16FEB!!!CHECKIN
 12 RC IEVPS2332-W/!!!!!!!!!!!!3RAZ
 13 FA PAX 566-2405101650/ETPS/UAH6647/18SEP17/IEVPS2332/7232044
	   1/S2-3
 14 FB PAX 0000000000 TTP/RT/T-PS OK ETICKET - WARNING: TICKET
	   QUOTA LOW/S2-3
 15 FE PAX NONEND/REF RST/RBK 100USD/S2-3
 16 FM *M*1A
 17 FP CASH
 18 FV PAX PS/S2-3
>
tqt
TST00007     IEVPS2332 SD/18SEP I 5 LD 18FEB18 2359  OD IEVIEV                 
T-                                                                             
FXB/SBF/RZZ                                                                    
   1.GRIGA/MATVII MSTR                                                         
 1   KBP PS  111 L 18FEB 1000  OK LL2SUP1  ZZ10        18MAY 1PC               
 2 O LGW PS  112 M 23MAR 1220  OK ML2LUP1         25FEB18MAY 1PC               
	 KBP                                                                       
FARE  F USD     160.00                                                         
EQUIV   UAH       4189                                                         
TX001 X UAH     1100-YRVB TX002 X UAH      105-UASE TX003 X UAH      341-YKAE  
TX004 X UAH      454-GBAD TX005 X UAH      458-UBAS                            
TOTAL   UAH       6647    BSR 26.18                                            
GRAND TOTAL UAH       6647                                                     
IEV PS LON94.50PS IEV65.00NUC159.50END ROE1.000000                             
																			   
 15.FE NONEND/REF RST/RBK 100USD                                               
 16.FM *M*1A                                                                   
 17.FP CASH                                                                    
 18.FV PS
"
			);


			docs.Assert(a => a
				.PnrCode("OPEDS5")
				.PassengerName("GRIGA/MATVII MSTR")
				.Fares("USD", 160m, "UAH", 4189m, "UAH", 6647m)
				.AirlineIataCode("PS")
			);

		}



		[Test]
		public void TestParseTicket19()
		{

			var docs = Parse(@"
--- TST RLR DCS ---
RP/IEVPS2332/IEVPS2332            WS/GS  21SEP17/0911Z   KV8XYR
  1.SAVCHENKO/OLEKSII MR   2.TATARCHENKO/KOSTIANTYN MR
  3  PS9814 S 22SEP 5 KBPVIE HK2 A        0700 0800   1A/E
	 OPERATED BY OS668 B
  4  PS 848 K 23SEP 6 VIEKBP HK2          1540 1835   1A/E
  5 AP IEV 38044-4902888 - ARIOLA GROUP LTD - A
  6 APE ELENA.ZHUKOVSKAYA@GMAIL.COM
  7 APM 067-448-16-73 ELENA
  8 TK OK20SEP/IEVPS2332//ETPS
  9 SSR DOCS PS HK1 P/UKR/FF543610/UKR/05APR73/M/23JAN27/SAVCHEN
	   KO/OLEKSII/P1
 10 SSR DOCS PS HK1 P/UKR/ES579421/UKR/25DEC72/M/05DEC24/TATARCH
	   ENKO/KOSTIANTYN/P2
 11 SSR DOCA OS HK1 R/UKR/////S3/P1
 12 SSR DOCA OS HK1 R/UKR/////S3/P2
 13 SSR DOCS OS HK1 P/UKR/FF543610/UKR/05APR73/M/23JAN27/SAVCHEN
	   KO/OLEKSII/S3/P1
 14 SSR DOCS OS HK1 P/UKR/ES579421/UKR/25DEC72/M/05DEC24/TATARCH
	   ENKO/KOSTIANTYN/S3/P2
 15 SSR DOCO OS HK1 /V/ES579421/UKR/05DEC14/AUT/S3/P2
 16 OSI PS CTCM 067-448-16-73 ELENA
 17 OSI PS CTCE ELENA.ZHUKOVSKAYA//GMAIL.COM
)>
>
tqt
TST00001     IEVPS2332 SL/20SEP I 0 LD 22SEP17 2359  OD IEVIEV                 
T-                                                                             
FXB/R,UP                                                                       
   1.SAVCHENKO/OLEKSII MR   2.TATARCHENKO/KOSTIANTYN MR                        
 1   KBP PS 9814 S 22SEP 0700  OK SN2FUP1              05NOV 1PC               
 2 O VIE PS  848 K 23SEP 1540  OK KD2ZUP1              05NOV 0PC               
	 KBP                                                                       
FARE  F USD     543.00                                                         
EQUIV   UAH      14211                                                         
TX001 X UAH     1676-YRVB TX002 X UAH      105-UASE TX003 X UAH      341-YKAE  
TX004 X UAH      579-ZYAE TX005 X UAH      219-QDAP TX006 X UAH      263-ATSE  
TOTAL   UAH      17394    BSR 26.17                                            
GRAND TOTAL UAH      17394                                                     
IEV PS VIE Q4.00 380.00PS IEV Q4.00 155.00NUC543.00END                         
 ROE1.000000                                                                   
																			   
 22.FE NON END/REF RSTR/RBK 75USD                                              
 23.FM *M*1A                                                                   
 24.FP INV                                                                     
 25.FV PS
"
			);


			docs.AssertAll(a => a
				.PnrCode("KV8XYR")
				.AirlineIataCode("PS")
			);


			docs.Assert(

				a => a
					.PassengerName("SAVCHENKO/OLEKSII MR")
					.Fares("USD", 543m, "UAH", 14211m, "UAH", 17394m)
				,

				a => a
					.PassengerName("TATARCHENKO/KOSTIANTYN MR")
					.Fares("USD", 543m, "UAH", 14211m, "UAH", 17394m)

			);

		}



		[Test]
		public void TestParseTicket20()
		{

			var docs = Parse(@"
--- TST RLR ---
RP/IEVU23667/IEVU23667            TV/SU   3NOV17/0856Z   QCPZGP
IEVU23667/5750TV/3NOV17
  1.NAZAROV/ERVIN MR
  2  LY2651 O 14NOV 2 TLVKBP HK1       1  0630 0955   1A/E
  3 AP IEV 38 044 3000175 - FLAMINGO TRAVEL - A
  4 TK OK03NOV/IEVU23667//ETLY
  5 RM *AMA 380039793
  6 FA PAX 114-5917779498/ETLY/UAH2947/03NOV17/IEVU23667/7232349
	   5/S2
  7 FB PAX 0000000000 TTP/RT OK ETICKET/S2
  8 FE PAX /C1 FARE RESTRICTIONS APPLY/S2
  9 FM *M*1A
 10 FP CASH
 11 FV PAX LY/S2
>
tqt
TST00001     IEVU23667 TV/03NOV I 0 LD 03NOV17 2359  OD TLVIEV                  
T-                                                                              
FXB/S2                                                                          
   1.NAZAROV/ERVIN MR                                                           
 1   TLV LY 2651 O 14NOV 0630  OK OLIESHBO        14NOV14NOV 0PC                
	 KBP                                                                        
FARE  F USD      94.00                                                          
EQUIV   UAH       2529                                                          
TX001 X UAH      310-ILEB TX002 X UAH      108-APSE                             
TOTAL   UAH       2947    BSR 26.895                                            
GRAND TOTAL UAH       2947                                                      
TLV LY IEV Q42.00 52.00NUC94.00END ROE1.000000                                  
																				
  8.FE /C1 FARE RESTRICTIONS APPLY                                              
  9.FM *M*1A                                                                    
 10.FP CASH                                                                     
 11.FV LY
"
			);


			docs.Assert(a => a
				.PnrCode("QCPZGP")
				.PassengerName("NAZAROV/ERVIN MR")
				.Fares("USD", 94m, "UAH", 2529m, "UAH", 2947m)
				.AirlineIataCode("LY")
			);

		}



		[Test]
		public void TestParseTicket21()
		{

			var docs = Parse(@"
--- TST RLR ---
RP/IEVPS2332/IEVPS2332            AS/SU   9FEB18/0913Z   U3R5TE
  1.BABYCH/MYKOLA MR
  2  PS 079 V 13FEB 2 KBPIFO HK1          1040 1210   1A/E
  3  PS 082 N 15FEB 4 IFOKBP HK1          0700 0815   1A/E
  4 AP B +38 067 224 88 84
  5 TK TL10FEB/IEVPS2332
  6 OPW-09FEB:1200/1C7/PS REQUIRES TICKET ON OR BEFORE
		10FEB:1200/S2-3
  7 OPC-10FEB:1200/1C8/PS CANCELLATION DUE TO NO TICKET/S2-3
  8 FE PAX NONEND/REF RSTR/RBK 10USD/S2
  9 FE PAX NON END/NO REF/RBK USD10/S3
 10 FV PAX PS/S2
 11 FV PAX PS/S3
>
tqt/t1
TST00001     IEVPS2332 AS/09FEB I 0 LD 13FEB18 2359  OD IEVIFO                 
T-                                                                             
FXB/S2/R,UP                                                                    
   1.BABYCH/MYKOLA MR                                                          
 1   KBP PS  079 V 13FEB 1040  OK VH1LUP1         13FEB13FEB 1PC               
	 IFO                                                                       
FARE  F USD      65.00                                                         
EQUIV   UAH       1765                                                         
TX001 X UAH      570-YRVB TX002 X UAH      375-HFGO TX003 X UAH       41-UASE  
TX004 X UAH       68-YKAE                                                      
TOTAL   UAH       2819    BSR 27.14                                            
GRAND TOTAL UAH       2819                                                     
IEV PS IFO Q10.00 55.00USD65.00END                                             
																			   
  8.FE NONEND/REF RSTR/RBK 10USD                                               
 10.FV PS                                                                      
>
tqt/t2
TST00002     IEVPS2332 AS/09FEB I 0 LD 15FEB18 2359  OD IFOIEV                 
T-                                                                             
FXB/S3/R,UP                                                                    
   1.BABYCH/MYKOLA MR                                                          
 1   IFO PS  082 N 15FEB 0700  OK NH1ZUP1         15FEB15FEB 0PC               
	 KBP                                                                       
FARE  F USD      25.00                                                         
EQUIV   UAH        679                                                         
TX001 X UAH      570-YRVB TX002 X UAH      164-HFGO TX003 X UAH       44-UASE  
TX004 X UAH       95-YKAE                                                      
TOTAL   UAH       1552    BSR 27.14                                            
GRAND TOTAL UAH       1552                                                     
IFO PS IEV25.00USD25.00END                                                     
																			   
  9.FE NON END/NO REF/RBK USD10                                                
 11.FV PS
"
			);


			docs.AssertAll(a => a
				.PnrCode("U3R5TE")
				.PassengerName("BABYCH/MYKOLA MR")
				.AirlineIataCode("PS")
			);


			docs.Assert(

				a => a
					.Fares("USD", 65m, "UAH", 1765m, "UAH", 2819m)
				,

				a => a
					.Fares("USD", 25m, "UAH", 679m, "UAH", 1552m)

			);

		}



		[Test]
		public void TestParseTicket22()
		{

			var docs = Parse(@"
--- TST RLR MSC ---
RP/IEVU2273F/IEVU2273F            AN/SU  26FEB20/1433Z   STXMSB
IEVU2273F/6868IG/26FEB20
  1.DUDNYK/YURIY MR   2.KARPENKO/OKSANA MRS
  3  TP8210 T 16MAR 1 KBPZRH HK2  0900 D  1030 1220   1A/E
  4  TP 931 T 16MAR 1 ZRHLIS HK2  1245    1330 1520   1A/E
  5  TP1040 U 21MAR 6 LISBCN HK2  0755 1  0855 1145   1A/E
  6  TP8223 U 21MAR 6 BCNKBP HK2  1245 1  1330 1800   1A/E
  7 AP IEV 38044 4902888 - ARIOLA GROUP LTD - A
  8 TK OK26FEB/IEVU23561//ETTP
  9 SSR DOCS TP HK1 P/UKR/FY580650/UKR/14AUG68/M/20FEB30/DUDNYK/
	   YURIY/P1
 10 SSR DOCS TP HK1 P/UKR/FL704815/UKR/14SEP78/F/08FEB28/KARPENK
	   O/OKSANA/P2
 11 SSR CTCE TP HK2 DIRECTOUR1//GMAIL.COM
 12 SSR CTCM TP HK1 380965785167/P1
 13 RM *AMA 380043489
 17 FE PAX FARE RESTR APPLY/NON REF/S3-6/P1-2
)>
>
tqt
TST00001     IEVU2273F IG/26FEB I 0 LD 16MAR20 2359  OD IEVIEV                  
T-                                                                              
FXB                                                                             
   1.DUDNYK/YURIY MR   2.KARPENKO/OKSANA MRS                                    
 1   KBP TP 8210 T 16MAR 1030  OK T53CLC0D        16MAR16MAR 1PC                
 2 X ZRH TP  931 T 16MAR 1330  OK T53CLC0D        16MAR16MAR 1PC                
 3 O LIS TP 1040 U 21MAR 0855  OK U53CLC0D        21MAR21MAR 1PC                
 4 X BCN TP 8223 U 21MAR 1330  OK U53CLC0D        21MAR21MAR 1PC                
	 KBP                                                                        
FARE  F USD     197.00                                                          
EQUIV   UAH       4831                                                          
TX001 X UAH     2182-YQAD TX002 X UAH       98-UASE TX003 X UAH       49-UDDP   
TX004 X UAH      319-YKAE TX005 X UAH      401-CHAE TX006 X UAH       93-PTSE   
TX007 X UAH      319-YPAE TX008 X UAH      286-JDAE TX009 X UAH       17-OGCO   
TX010 X UAH       56-QVDP                                                       
TOTAL   UAH       8651    BSR 24.52                                             
GRAND TOTAL UAH       8651                                                      
IEV TP X/ZRH TP LIS89.00TP X/BCN TP IEV108.00NUC197.00END                       
 ROE1.000000                                                                    
																				
 17.FE FARE RESTR APPLY/NON REF                                                 
 19.FM *M*1                                                                     
 20.FP CASH
"
			);


			docs.AssertAll(a => a
				.PnrCode("STXMSB")
				.AirlineIataCode("TP")
				.Fares("USD", 197m, "UAH", 4831m, "UAH", 8651m)
			);


			docs.Assert(

				a => a
					.PassengerName("DUDNYK/YURIY MR")
				,

				a => a
					.PassengerName("KARPENKO/OKSANA MRS")

			);

		}



		[Test]
		public void TestParseTicket23()
		{

			var docs = Parse(@"
--- TST TSM RLR ---
RP/IEVPS28GM/IEVPS28GM            VK/SU  12MAY21/1128Z   T576DF
0.  0PERSEY  NM:14
 15  PS 117 G 17MAY 1 KBPLHR HK14      D  0930 1055       E*
 16 SVC PS HK1 DEPO LHR 17MAY/P1
 17 APA 443039570- AGCY
 18 APE sales@perseytravel.com.ua
 19 TK OK12MAY/IEVPS2356//ETPS
 20 SSR GRPF YY GWRUA
 21 SSR DOCS PS HK1 P/UA/FV330542/UA/04JAN89/M/05JUN29/BONDARENK
	   O/ANTON MR//P1
 22 SSR DOCS PS HK1 P/UA/FZ464431/UA/12NOV00/F/02OCT30/HORBATIUK
	   /VERONIKA MRS//P2
 23 SSR DOCS PS HK1 P/UA/FF131586/UA/22OCT90/M/27OCT26/IVONIN/KY
	   RYLO MR//P3
 24 SSR DOCS PS HK1 P/UA/FF956683/UA/15MAR82/F/24MAR27/KOZAK/MYR
	   OSLAVA MRS//P4
 25 SSR DOCS PS HK1 P/UA/FV192766/UA/29MAY83/F/23MAY29/LAN/MARIA
	   NA MRS//P5
 26 SSR DOCS PS HK1 P/UA/FZ620918/UA/09MAY01/M/20NOV30/PALAMARCH
	   UK/MAKSYM MR//P8
 27 SSR DOCS PS HK1 P/UA/FU274193/UA/01SEP01/M/22FEB29/PARCHUK/K
	   OSTIANTYN MR//P9
)>
>
md
--- TST TSM RLR ---
RP/IEVPS28GM/IEVPS28GM            VK/SU  12MAY21/1128Z   T576DF
 28 SSR DOCS PS HK1 P/UA/FB492206/UA/07APR98/M/18MAY25/TYMOSHENK
	   O/ROSTYSLAV MR//P13
 29 SSR DOCS PS HK1 P/UA/FJ691865/UA/04JUN96/F/27OCT27/ZAKHARENK
	   O/KHRYSTYNA MRS//P14
 30 SSR DOCS PS HK1 P/UA/FY507010/UA/02AUG82/M/13FEB30/TURUTA/OL
	   EKSANDR MR//P12
 31 SSR DOCS PS HK1 P/UA/FS244339/UA/13AUG88/M/20AUG28/MARTYNOVY
	   CH/ANDRII MR//P6
 32 SSR DOCS PS HK1 P/UA/FP155990/UA/22FEB94/M/07JUN28/TARCHYNSK
	   YI/PHILIP MR//P11
 33 SSR DOCS PS HK1 P/UA/FS137726/UA/09MAY94/M/10AUG28/RUDZIK/VL
	   ADYSLAV MR//P10
 34 SSR DOCS PS HK1 P/UA/FF984456/UA/18APR98/M/29MAR27/MYKHAILIU
	   K/VADYM MR//P7
 35 OSI PS GROUP ACCEPTED
 36 SK RQID PS 6604-1-20210511
 37 SK TYPE PS ADHOC
 38 SK STYP PS WORKERS
 39 SK IATA PS 72324350
 40 RM GRPF USD161.00
 41 RM ADULT GRPF USD161.00
)>
>
md
--- TST TSM RLR ---
RP/IEVPS28GM/IEVPS28GM            VK/SU  12MAY21/1128Z   T576DF
 42 RM PNR POS IS IEV
 43 RM TA PERSEY TRAVEL
 44 RM NON ENDO/NO REF/PS ONLY/CHNG RESTR/CHNG NAME FREE
 45 RM THIS PNR IS VALID
 46 FA PAX 566-9800031321/DTPS/UAH69958/12MAY21/IEVPS2356/723243
	   50/S16/P1
 47 FA PAX 566-2407078233/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P1
 48 FA PAX 566-2407078234/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P2
 49 FA PAX 566-2407078235/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P3
 50 FA PAX 566-2407078236/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P4
 51 FA PAX 566-2407078237/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P5
 52 FA PAX 566-2407078238/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P6
 53 FA PAX 566-2407078239/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P7
 54 FA PAX 566-2407078240/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
)>
>
md
--- TST TSM RLR ---
RP/IEVPS28GM/IEVPS28GM            VK/SU  12MAY21/1128Z   T576DF
	   0/S15/P8
 55 FA PAX 566-2407078241/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P9
 56 FA PAX 566-2407078242/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P10
 57 FA PAX 566-2407078243/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P11
 58 FA PAX 566-2407078244/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P12
 59 FA PAX 566-2407078245/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P13
 60 FA PAX 566-2407078246/ETPS/UAH4997/12MAY21/IEVPS2356/7232435
	   0/S15/P14
 61 FB PAX 0000000000 TTM/T-PS/RT OK EMD/S16/P1
 62 FB PAX 0000000000 TTP/T-PS/RT OK ETICKET/S15/P1-14
 63 FE *M*NON ENDO/NO REF/PS ONLY/CHNG RESTR/CHNG NAME FREE
 64 FM *M*0
 65 FP MO*5669800031321/UAH4997
 66 FV PS
 67 AM NA-OLEG KUCHERENKO/A1-OLEG KUCHERENKO/A2-KOPERNYKA STR 
	   29/CO-UA


tqt
TST00001     IEVPS2356 VK/12MAY M+1                  OD IEVLON                 
T-                                                                             
   1.BONDARENKO/ANTON MR(ADT)   2.HORBATIUK/VERONIKA MRS(ADT)                  
   3.IVONIN/KYRYLO MR(ADT)   4.KOZAK/MYROSLAVA MRS(ADT)                        
   5.LAN/MARIANA MRS(ADT)   6.MARTYNOVYCH/ANDRII MR(ADT)                       
   7.MYKHAILIUK/VADYM MR(ADT)   8.PALAMARCHUK/MAKSYM MR(ADT)                   
   9.PARCHUK/KOSTIANTYN MR(ADT)  10.RUDZIK/VLADYSLAV MR(ADT)                   
  11.TARCHYNSKYI/PHILIP MR(ADT)  12.TURUTA/OLEKSANDR MR(ADT)                   
  13.TYMOSHENKO/ROSTYSLAV MR(ADT)                                              
  14.ZAKHARENKO/KHRYSTYNA MRS(ADT)                                             
 1   KBP PS  117 G 17MAY 0930  OK GWRUA           17MAY17MAY 1PC               
	 LHR                                                                       
FARE  F USD     161.00                                                         
EQUIV   UAH       4469                                                         
TX001 X UAH      111-UA   TX002 X UAH       56-UD   TX003 X UAH      361-YK    
TOTAL   UAH       4997    BSR 27.76                                            
GRAND TOTAL UAH       4997                                                     
IEV PS LON161.00NUC161.00END ROE1.000000                                       
																			   
 63.FE *M*NON ENDO/NO REF/PS ONLY/CHNG RESTR/CHNG NAME FREE                    
 64.FM *M*0                                                                    
 65.FP MO*5669800031321/UAH4997                                                
 66.FV PS 
"
			);


			AreEqual(14, docs.Count);

		}



		[Test]
		public void TestParseTicket24()
		{

			var docs = Parse(@"
RP/FLXMIA/                       F1/AGMW       F1:R6WQ8T   LH:W466RZ
RF AGMX_ASMIRNOVA      15SEP2021 1635Z
  1. HUZYK/OKSANA
  2  LH1539 K 29SEP   LWOFRA HK1        1  1350   1455   LH:W466RZ/E
  3  LH1412 K 29SEP 1 FRASPU HK1           1615   1755   LH:W466RZ/E
  4  LH1413 K 04OCT   SPUFRA HK1        1  1840   2030   LH:W466RZ/E
  5  LH1536 K 04OCT 1 FRALWO HK1           2145   0055+1 LH:W466RZ/E
  6  AP M -380675001991-A
  7  APE bsv@bsv.com.ua
  8  SSR OTHS F1   PLS ADV TKT NBR BY 16SEP21/1635Z OR LH OPTG/MKTG FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMANDS EARLIER TKTG/P
>TQT
TST00001                 IOVA/15SEP    LD 16SEP21

FXB
   1.HUZYK/OKSANA                                (ADT)
	 LWO-FRA LH  1539K 29SEP 1350 OK  K06LGTU8             29SEP29SEP  0PC
	 FRA-SPU LH  1412K 29SEP 1615 OK  K06LGTU8             29SEP29SEP  0PC
	 SPU-FRA LH  1413K 04OCT 1840 OK  K06LGTU8             04OCT04OCT  0PC
	 FRA-LWO LH  1536K 04OCT 2145 OK  K06LGTU8             04OCT04OCT  0PC
	 LWO
FARE    USD    30.00
EQUIV   UAH    801.00
TX001   UAH    158.00YQ
TX002   UAH    158.00YQ
TX003   UAH    158.00YQ
TX004   UAH    158.00YQ
TX005   UAH    314.00DE
TX006   UAH   1378.00RA
TX007   UAH    518.00HR
TX008   UAH     44.00MI
TX009   UAH    131.00UA
TX0010  UAH     54.00UD
TX0011  UAH    294.00YK
TOTAL   UAH    4166.00    BSR 26.700000
GRAND TOTAL UAH    4166.00
LWO LH X/FRA LH SPU15.00 LH X/FRA LH LWO15.00 NUC30.00END ROE1.000000
FE FARE RESTRICTION MAY APPLY
FM *F*1.00A
FV *F*LH
>
"
			);


			AreEqual(1, docs.Count);

		}



		//---g


	}






	//===g



}
