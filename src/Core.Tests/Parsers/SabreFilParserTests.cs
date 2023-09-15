using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;




namespace Luxena.Travel.Tests.Parsers
{



	//===g






	[TestFixture]
	public class SabreFilParserTests
	{

		//---g



		[Test]
		public void Test_00_Ticket()
		{
			var r = ParseTicket(
@"AA29DEC0242M0117                    000284899 99999 2  OKOTWT        611111110000000001 N7GG * OV 4C6C59            29DEC 0240N7GG * OV   00011MARSVOMOSCOW SHEREMET  SVOMOSCOW SHEREMET  001001002002001001000000002000  01 104229DEC











0442354433-A


OV
M101VERBELCHUK/IRYNA MRS                                                                                                    021 01000002  
0102
01


0102
M201ADT30034013603X 2   RST       EUR 1199.00    2144YQ     452GB    1447UB UAH   34626                             UAH   30583       1      306    30583  F    4043               SVO  N N7GG A OV N7GG A OV                      0201SU65636416940
0102
01
CA


BSR25.50





M3011 0HK11MARAIRNSVOMOSCOW SHEREMET  LHRLONDON HEATHROW  SU 2574Y 0830 0945  4.15       000                    320001550      00TERMINAL D - DOMESTIC/INTL    TERMINAL 4                         0                                     1002016LRFXLP  



M3021 0HK18MARAIRNLHRLONDON HEATHROW  SVOMOSCOW SHEREMET  SU  262Y 1055 1745  3.50       000                    321001550      00TERMINAL 4                    TERMINAL D - DOMESTIC/INTL         0                                     1002016LRFXLP  



M401ADT       11MAROK2PCYNRT           00668.911YNRT                                  
M402ADTO      11MAROK2PCYNRT           00668.911YNRT                                  
M50101  SU#6563641694/    306/   30583/   4043/ONE/CA 1.1VERBELCHUK IRYNA MRS/1/F/E
M6ADT1    MOW SU LON668.91SU MOW668.91NUC1337.82END ROE0.896229
M901PT-SSR TKNE SU HK1 SVOLHR2574Y11MAR/5556563641694C1
M902PT-SSR TKNE SU HK1 LHRSVO0262Y18MAR/5556563641694C2
");


			Assert.AreEqual(DateTime.Parse("2023-12-29"), r.IssueDate);


			//Assert.AreEqual("GMEXBL", r.PnrCode);

			//Assert.AreEqual("N7GG", r.BookerOffice);
			//Assert.AreEqual("OV", r.BookerCode);
			//Assert.AreEqual("N7GG", r.TicketerOffice);
			//Assert.AreEqual("OV", r.TicketerCode);
		}



		[Test]
		public void Test_01_Ticket()
		{
			var r = ParseTicket(
@"AA10FEB0301M0117               Y    000285599 99999 2  GPUIGG        611111110000010001 N7GG * KD EDEC4D            08FEB 0111N7GG * KD   00022FEBSVOMOSCOW SHEREMET  SVOMOSCOW SHEREMET  003001002002004001000000011000  01 110110FEB











00380676748009-H


DIK
M101DIK/KATERYNA MRS                                                                                                        021 02000009  
0102
0104


010204050608091011
M102DIK/MAKSYM MSTR                                                 DOB16FEB09                                              02  01000001  
0102
02


03
M103DIK/SNIZHANA MSS                                                DOB20DEC15                                              02  01000001  
0102
03


07
M201ADT81103969102X 2   RST       EUR 1550.00 PD 5376YQ PD  512TS PD   52E7 UAH  15645A                             UAH   44799    0.10       15    15645  F                       SVO  N N7GG A KD N7GG A KD                      0201SU65636417060
0102
01
CASH CASH
5556563641697IEV08FEB1699999992
5556563641697/12
BSR28.90






M3011 0HK22FEBAIRNSVOMOSCOW SHEREMET  BKKBANGKOK SUVARNABHSU  270Y 1940 0830  8.50       110                    77W004382      00TERMINAL F - INTERNATIONAL                                       0                                     1002016DVXIAH  



M3021 0HK25FEBAIRNBKKBANGKOK SUVARNABHSVOMOSCOW SHEREMET  SU  271K 1015 1610  9.55       000                    77W004382      00                              TERMINAL F - INTERNATIONAL         0                                     1002016DVXIAH  



M401ADT       11FEBOK2PCYNRT           001306611YNRT                                  
M402ADTO 25FEB25FEBOK1PCKEX            000381091KEX                                   
M50101  SU#6563641697/     29/   29111/   5983/ONE/CA 1.1DIK KATERYNA MRS/1/F/E
M50202  SU#6563641698/     22/   21833/   5983/ONE/CA 2.1DIK MAKSYM MSTR CHD/1/F/E
M50303  SU#6563641699/      3/    2911/     52/ONE/CA 3.1DIK SNIZHANA MSS INF/1/F/E
M50401 ASU#6563641706/     15/   15645/      0/ONE/CA 1.1DIK KATERYNA MRS/1/F/E-@5556563641697/12
M6ADT1    MOW SU BKK1306.61SU MOW381.09NUC1687.70END ROE0.918404
M901PT-SSR INFT SU NN1 BKKSVO0271K25FEB/DIK/SNIZHANA MSS/20DEC15
M902PT-SSR TKNE SU HK1 BKKSVO0271K25FEB/5556563641697C2
M903PT-SSR TKNE SU HK1 BKKSVO0271K25FEB/5556563641698C2
M904PT-SSR TKNE SU HK1 BKKSVO0271K25FEB/INF5556563641699C2
M905PT-SSR TKNE SU HK1 SVOBKK0270Y22FEB/5556563641706C1
M906PT-SSR TKNE SU HK1 BKKSVO0271K25FEB/5556563641706C2
M907PT-OSI AA INF
M908PT-SSR INFT SU KK1 SVOBKK0270Y20FEB/DIK/SNIZHANA MSS/20DEC15 
M909PT-SSR INFT SU KK1 BKKSVO0271Y25FEB/DIK/SNIZHANA MSS/20DEC15 
M910PT-SSR INFT SU KK1 SVOBKK0270M20FEB/DIK/SNIZHANA MSS/20DEC15 
M911PT-SSR INFT SU KK1 BKKSVO0271K25FEB/DIK/SNIZHANA MSS/20DEC15 
MF01 #DIK.K@TICKETS.UA#
");


			//Assert.AreEqual(DateTime.Parse("2015-12-29"), r.IssueDate);


			//Assert.AreEqual("GMEXBL", r.PnrCode);

			//Assert.AreEqual("N7GG", r.BookerOffice);
			//Assert.AreEqual("OV", r.BookerCode);
			//Assert.AreEqual("N7GG", r.TicketerOffice);
			//Assert.AreEqual("OV", r.TicketerCode);
		}



//		[Test]
//		public void Test_02_Ticket()
//		{
//			var r = ParseVoiding(
//@"AA21MAR0444M0217               Y    000317599 99999 2  VUOPUN        611111110000001000 N7GG * VB D52F98            21MAR 0420N7GG * VB   00020APRLEDST PETERSBURG LEDSVOMOSCOW SHEREMET  001000001000001000000000001000  01 114421MAR











//380444982650


//VL
//M101BEREGOVOY/VLADIMIR MR                                                                                                   01  01000001  
//01
//01


//01
//M3011 0HK20APRAIRNLEDST PETERSBURG LEDSVOMOSCOW SHEREMET  SU   33Y 0615 0730  1.15       000                    321000396      00PULKOVO 1                     TERMINAL D - DOMESTIC/INTL         0                                      002016PHKCVM  



//M50101 RSU#1711394919/     P0/7821/587/ONE/CA 1.1BEREGOVOY VLADIMIR MR/1/1/F0003174/F/E-TEST BACKOFFICE
//M901PT-SSR TKNE SU HK1 LEDSVO0033Y20APR/5551711394919C1
//");


//			//Assert.AreEqual(DateTime.Parse("2015-12-29"), r.IssueDate);


//			//Assert.AreEqual("GMEXBL", r.PnrCode);

//			//Assert.AreEqual("N7GG", r.BookerOffice);
//			//Assert.AreEqual("OV", r.BookerCode);
//			//Assert.AreEqual("N7GG", r.TicketerOffice);
//			//Assert.AreEqual("OV", r.TicketerCode);
//		}



		[Test]
		public void Test_03_Ticket()
		{
			var r = ParseVoiding(
@"AA29DEC0302M0F17                           99 99999 2  OKOTWT        611111110000000001 N7GG * OV 4C6C59            29DEC 0240N7GG * OV   00011MARSVOMOSCOW SHEREMET  SVOMOSCOW SHEREMET  001001002002001001000000002000  01 104229DEC











0442354433-A


OV
M101VERBELCHUK/IRYNA MRS                                                                                                    021 01000002  
0102
01


0102
M201ADT30034013603X 2   RST       EUR 1199.00    2144YQ     452GB    1447UB UAH   34626                             UAH   30583       1      306    30583  F    4043               SVO  N N7GG A OV N7GG A OV                      0201SU65636416940
0102
01
CA


BSR25.50





M3011 0HK11MARAIRNSVOMOSCOW SHEREMET  LHRLONDON HEATHROW  SU 2574Y 0830 0945  4.15       000                    320001550      00TERMINAL D - DOMESTIC/INTL    TERMINAL 4                         0                                     1002016LRFXLP  



M3021 0HK18MARAIRNLHRLONDON HEATHROW  SVOMOSCOW SHEREMET  SU  262Y 1055 1745  3.50       000                    321001550      00TERMINAL 4                    TERMINAL D - DOMESTIC/INTL         0                                     1002016LRFXLP  



M401ADT       11MAROK2PCYNRT           00668.911YNRT                                  
M402ADTO      11MAROK2PCYNRT           00668.911YNRT                                  
M50101  SU#6563641694/    306/   30583/   4043/ONE/CA 1.1VERBELCHUK IRYNA MRS/1/F/E
M6ADT1    MOW SU LON668.91SU MOW668.91NUC1337.82END ROE0.896229
M901PT-SSR TKNE SU HK1 SVOLHR2574Y11MAR/5556563641694C1
M902PT-SSR TKNE SU HK1 LHRSVO0262Y18MAR/5556563641694C2
");


			//Assert.AreEqual(DateTime.Parse("2015-12-29"), r.IssueDate);


			//Assert.AreEqual("GMEXBL", r.PnrCode);

			//Assert.AreEqual("N7GG", r.BookerOffice);
			//Assert.AreEqual("OV", r.BookerCode);
			//Assert.AreEqual("N7GG", r.TicketerOffice);
			//Assert.AreEqual("OV", r.TicketerCode);
		}



		[Test]
		public void Test_04_Ticket()
		{
			var r = ParseTicket(
				@"AA05FEB0418M0317                           99 99999 2  LZKPMW        611111110000000000 I7GG * SS 52D809            05FEB 0407N7GG * VB   00025SEPDMEMOSCOW DOMODEDOVOMSQMINSK            001000001000000000000001000000  01 121805FEB











0442388298


SS
M101SAVCHENKO/SVITLANA                                                                                                      01  00000100  
01


01

M3011 0HK25SEPAIRNDMEMOSCOW DOMODEDOVOMSQMINSK            B2  954Y 0540 0700  1.20       000                    733000423      00                                                                 0                                      002018NJEORL  



M801I FARE 7500UAH  TAX 850UAH/S1
");


			//Assert.AreEqual(DateTime.Parse("2015-12-29"), r.IssueDate);


			//Assert.AreEqual("GMEXBL", r.PnrCode);

			//Assert.AreEqual("N7GG", r.BookerOffice);
			//Assert.AreEqual("OV", r.BookerCode);
			//Assert.AreEqual("N7GG", r.TicketerOffice);
			//Assert.AreEqual("OV", r.TicketerCode);
		}



		[Test]
		public void Test_05_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA28MAR0204M0317                           72 32022 0  LQSMUB        611111110000000000 S1OI * IK 06C778            28MAR 0157S1OI * IK   00030MAYFCOROME FIUMICINO   IEVKIEV     ZHULHANY003000001000000000000006007000  01 100428MAR











0380442067576-A


IK
M101KAGADIY/IRYNA MRS                                                                                                       01  00000606  
03


010203040506
010204050607
M102VOLODKIN/KYRYLO MSTR                                                                                                    01  00000602  
03


010203040506
0607
M103VOLODKINA/ZHANNA MISS                                                                                                   01  00000603  
03


010203040506
030607
M3031 0HK30MAYAIRNFCOROME FIUMICINO   IEVKIEV     ZHULHANYAZ  590K 2235 0220  2.45       110                    320001053      00TERMINAL 1                    TERMINAL B                         0                                      002018RKKDSM  



M801TTL2245/PQ1/S1/1.1
M802TTL1926/PQ2/S1/2.1
M803TTL129/PQ3/S1/3.1
M804TTL11633/PQ4/S3/1.1
M805TTL8225/PQ5/S3/2.1
M806TTL1163/PQ6/S3/3.1
M901PT-SSR INFT PS NN1 KBPODS0057M20MAY/VOLODKINA/ZHANNA MISS/25AUG17
M902PT-SSR INFT AZ NN1 FCOIEV0590K30MAY/VOLODKINA/ZHANNA MISS/25AUG17
M903PT-OSI AA INF
M904PT-SSR INFT AZ KK1 FCOIEV0590K30MAY/VOLODKINA/ZHANNA MISS/25AUG17
M905PT-SSR INFT PS KK1 KBPODS0057M20MAY/VOLODKINA/ZHANNAMISS/25AUG17
M906PT-SSR ADTK 1S TO PS BY 30MAR 1000 OTHERWISE WILL BE XLD
M907PT-SSR ADTK 1S TO AZ BY 02APR 0957 IEV OR AZ FLTS WILL BE CNLD
", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(3, docs.Count);

			Assert.AreEqual(11633m, ((AviaTicket)docs[0]).EqualFare.Amount);
			Assert.AreEqual(8225m, ((AviaTicket)docs[1]).EqualFare.Amount);
			Assert.AreEqual(1163m, ((AviaTicket)docs[2]).EqualFare.Amount);
		}



		[Test]
		public void Test_06_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA28MAR0204M0317                           72 32022 0  LQSMUB        611111110000000000 S1OI * IK 06C778            28MAR 0157S1OI * IK   00030MAYFCOROME FIUMICINO   IEVKIEV     ZHULHANY003000001000000000000006007000  01 100428MAR











0380442067576-A


IK
M101KAGADIY/IRYNA MRS                                                                                                       01  00000606  
03


010203040506
010204050607
M102VOLODKIN/KYRYLO MSTR                                                                                                    01  00000602  
03


010203040506
0607
M103VOLODKINA/ZHANNA MISS                                                                                                   01  00000603  
03


010203040506
030607
M3031 0HK30MAYAIRNFCOROME FIUMICINO   IEVKIEV     ZHULHANYAZ  590K 2235 0220  2.45       110                    320001053      00TERMINAL 1                    TERMINAL B                         0                                      002018RKKDSM  
M3041 0HK30MAYAIRNFCOROME FIUMICINO   IEVKIEV     ZHULHANYAZ  590K 2235 0220  2.45       110                    320001053      00TERMINAL 1                    TERMINAL B                         0                                      002018RKKDSM  



M801TTL2245/PQ1/S1/1.1
M802TTL1926/PQ2/S1/2.1
M803TTL129/PQ3/S1/3.1
M804TTL11633/PQ4/S3,4/1.1
M805TTL8225/PQ5/S3,4/2.1
M806TTL1163/PQ6/S3,4/3.1
M901PT-SSR INFT PS NN1 KBPODS0057M20MAY/VOLODKINA/ZHANNA MISS/25AUG17
M902PT-SSR INFT AZ NN1 FCOIEV0590K30MAY/VOLODKINA/ZHANNA MISS/25AUG17
M903PT-OSI AA INF
M904PT-SSR INFT AZ KK1 FCOIEV0590K30MAY/VOLODKINA/ZHANNA MISS/25AUG17
M905PT-SSR INFT PS KK1 KBPODS0057M20MAY/VOLODKINA/ZHANNAMISS/25AUG17
M906PT-SSR ADTK 1S TO PS BY 30MAR 1000 OTHERWISE WILL BE XLD
M907PT-SSR ADTK 1S TO AZ BY 02APR 0957 IEV OR AZ FLTS WILL BE CNLD
", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(3, docs.Count);

			Assert.AreEqual(11633m, ((AviaTicket)docs[0]).EqualFare.Amount);
			Assert.AreEqual(8225m, ((AviaTicket)docs[1]).EqualFare.Amount);
			Assert.AreEqual(1163m, ((AviaTicket)docs[2]).EqualFare.Amount);
		}



		[Test]
		public void Test_07_Refund()
		{
			var docs = SabreFilParser.Parse(
@"AA27DEC0848M0217               Y    000051372 32487 3  KINBUP        611111110000011000 XV8J * WS 06C778            13NOV 0210XV8J * KI   00016FEBKBPKYIV    BORISPOL DOHDOHA HAMAD INTL  001000001000001000000000009000  01 164827DEC
380971084588-M-1.1
XV8J
M101TKACHENKO/ALLA                                                                                                          01  01000009  
01
01
010203040506070809
M3011 0DS16FEBAIRNKBPKYIV    BORISPOL DOHDOHA HAMAD INTL  **     T                                                 002062      00                                                                 0                                                    
M50101 RQR#5522959911/     P5/UAH6042/9194/ONE/CA 1.1TKACHENKO ALLA/1/1234/F0000394/F/E
M901PT-SSR CTCE QR HK1/ANN.DM4NK//GMAIL.COM
M902PT-SSR DOCS QR HK1/P/UA/FF556677/UA/14AUG1980/F/12DEC2028/TKACHENKO/ALLA/H
M903PT-SSR DOCS QR HK1/P/UA/FE527050/UA/14AUG83/F/25MAY2026/TKACHENKO/ALLA
M904PT-SSR OTHS 1S PLS TICKET BY 2359/18NOV2019 LCLT AT BOARD POINT OR QR WILL CXL
M905PT-SSR OTHS 1S TKNA/TKNM/FA PT/FH TKT ENTRY PROHIBITED FOR BSP/ARC/EDIRECT MRKT
M906PT-SSR OTHS 1S TICKETING DEADLINE IN FARE RULE MUST BE OBSERVED IF MORE RESTRICTIVE
M907PT-SSR OTHS 1S NOREC
M908PT-SSR OTHS 1S PASSIVE REJECTED DUE NAME NOT FOUND
M909PT-SSR OTHS 1S PLEASE CANCEL ALL REJECTED PASSIVE SEGMENTS

", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(1, docs.Count);

			Assert.AreEqual("UAH", ((AviaRefund)docs[0]).EqualFare.Currency.Code);
			Assert.AreEqual(6042m, ((AviaRefund)docs[0]).EqualFare.Amount);
			Assert.AreEqual(9194m, ((AviaRefund)docs[0]).FeesTotal.Amount);
		}



		[Test]
		public void Test_08_EMD()
		{
			var docs = SabreFilParser.Parse(
				@"AA05FEB0746M0A17               Y    000279372 32487 3  XIXSUO        611111110000001000 S1OI * IS B30471            05FEB 0740S1OI * IS   00021FEBGVAGENEVA           TXLBERLIN    TEGEL  001000002000002000000000004000  01 154605FEB











380442067576-A


IP
M101KUZMA/MYKOLA MR                                                                                                         02  02000004  
0102
0102


01020304
M3011 0HK21FEBAIRNGVAGENEVA           MUCMUNICH           LH 5751V 1310 1420  1.10   B   000                  LX221000305      01TERMINAL 1                    TERMINAL 2                         0                                      002020OUIU48  
01 09CN



M3021 0HK21FEBAIRNMUCMUNICH           TXLBERLIN    TEGEL  LH 2056V 1630 1740  1.10   S   000                    32A000292      01TERMINAL 2                                                       0                                      002020OUIU48  
01 05DN



M50101  LH#5523031311/      1/    6044/   3082/ONE/CA 1.1KUZMA MYKOLA MR/1/F/E
M50201  LH#3096088141/      0/      801/        0/ONE/CA 1.1KUZMA MYKOLA MR/1/I/EMD
M901PT-SSR DOCS LH HK1/P/UKR/FJ940789/UKR/25AUG69/M/10NOV27/KUZMA/MYKOLA

M902PT-SSR TKNE LH HK1 GVAMUC5751V21FEB/2205523031311C1

M903PT-SSR TKNE LH HK1 MUCTXL2056V21FEB/2205523031311C2

M904PT-SSR OTHS 1S PLS PROVIDE PAX CTC DATA VIA SSR CTCM OR CTCE

ME01CS# GVA-MUC OPERATED BY SWISS                   

MG01ADT05FEB202015461SLH 220309608814140220552303131102 N  S MUC  TXL  A                UAH               801                 0                 0                     CA                                            Y                       01
 
REFUNDABLE
01               801                      021FEB2020                  0B5SEAT RESERVATION                                  2056 MUC  TXL  21FEB2020                

", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(1, docs.Count);

			var mco = (AviaMco) docs[0];

			Assert.AreEqual("220-3096088141", mco.Name);

			Assert.AreEqual(801m, mco.EqualFare.Amount);
			//Assert.AreEqual(9194m, ((AviaRefund)docs[0]).FeesTotal.Amount);
		}



		[Test]
		public void Test_09_Refund()
		{
			var docs = SabreFilParser.Parse(
@"AA24FEB0408M0217               Y    000289272 32487 3  AGFGIH        611111110000111000 S1OI * GT 06C777            10FEB 0117S1OI * GT   00026FEBKBPKYIV    BORISPOL KBPKYIV    BORISPOL 001000004000002000002000014000  02 120824FEB
380444943366
380442067575
TG
M101KUKHTA/ANDRIY MR                                                                                                        04  02020014  
01020304
0102
0102
0102030405060708091011121314
M3011 0HK26FEBAIRNKBPKYIV    BORISPOL VIEVIENNA           OS 7172Q 0925 1030  2.05       000                  PS737000655      00TERMINAL D - INTL/DOMESTIC                                       0                                      002020LJJT5R  
M3021 0HK26FEBAIRNVIEVIENNA           KRKKRAKOW           OS  597Q 1300 1405  1.05   S   000                    E95000200      00                                                                 0                                      002020LJJT5R  
M3031 0HK28FEBAIRNKRKKRAKOW           MUCMUNICH           LH 1625T 1720 1840  1.20   S   000                    320000380      00                              TERMINAL 2                         0                                      002020LJJT5R  
M3041 0HK28FEBAIRNMUCMUNICH           KBPKYIV    BORISPOL LH 2546T 1950 2305  2.15   S   000                    32A000859      00TERMINAL 2                    TERMINAL D - INTL/DOMESTIC         0                                      002020LJJT5R  
M50101  OS#5523031368/      1/    2853/   5208/ONE/CA 1.1KUKHTA ANDRIY MR/1/F/E
M50201 ROS#5523031368/      0/0/1961/ONE/CA 1.1KUKHTA ANDRIY MR/1/1234/F0002825/F/E
M701TICKET PRICE IS USD359 WITH BAGGAGE
M702LAST TICKETING DATE 10FEB
M901PT-SSR DOCS OS HK1/P/UKR/FE271018/UKR/23SEP62/M/12MAR26/KUKHTA/ANDRIY
M902PT-SSR DOCS LH HK1/P/UKR/FE271018/UKR/23SEP62/M/12MAR26/KUKHTA/ANDRIY
M903PT-SSR TKNE OS HK1 KBPVIE7172Q26FEB/2575523031368C1
M904PT-SSR TKNE OS HK1 VIEKRK0597Q26FEB/2575523031368C2
M905PT-SSR TKNE LH HK1 KRKMUC1625T28FEB/2575523031368C3
M906PT-SSR TKNE LH HK1 MUCKBP2546T28FEB/2575523031368C4
M907PT-SSR CTCE OS HK1/ALEXANDERSOROKA//KCK.UA
M908PT-SSR CTCE LH HK1/ALEXANDERSOROKA//KCK.UA
M909PT-SSR CTCM OS HK1/380444943355
M910PT-SSR CTCM LH HK1/380444943355
M911PT-SSR OTHS 1S PLS ADV TKT NBR BY 11FEB20/0717Z OR LH OPTG/MKTG FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMANDS EARLIER TKTG
M912PT-SSR OTHS 1S PLS ADV TKT NBR BY 13FEB20/0717Z OR OS OPTG/MKTG FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMANDS EARLIER TKTG
M913PT-SSR OTHS 1S PLS ADV TKT NBR BY 11FEB20/0802Z OR LH OPTG/MKTG FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMANDS EARLIER TKTG
M914PT-SSR OTHS 1S PLS PROVIDE PAX CTC DATA VIA SSR CTCM OR CTCE
ME01CS# KBP-VIE OPERATED BY UKRAINE INTL AIRL       
MF01 #ALEXANDER==SOROKA@KCK.UA#TO/@U
", "UAH").ToList();

			Assert.NotNull(docs);

			Assert.AreEqual(1, docs.Count);

			var refund = (AviaRefund)docs[0];
			//Assert.AreEqual("220-3096088141", mco.Name);
			//Assert.AreEqual(801m, mco.Total.Amount);
			//Assert.AreEqual(9194m, ((AviaRefund)docs[0]).FeesTotal.Amount);
		}



		[Test]
		public void Test_10_Refund()
		{
			var docs = SabreFilParser.Parse(
@"AA28FEB0617M0217               Y    000293072 32487 3  EKDDCH        611111110000001000 S1OI * YD 06C778            20FEB 0522S1OI * KI   000     HRKKHARKIV          ISTISTANBUL AIRPORT 003000001000004000000000009000  01 141728FEB
380442067576-A
YD
M101BABICHEV/ROMAN MR                                                                                                       01  01000007  
01
01
01020306070809
M102PLETIN/LIUDMYLA MRS                                                                                                     01  01000005  
01
02
0406070809
M103BALOBAN/TETIANA MRS                                                                                                     01  02000005  
01
0304
0506070809
M3011 0DS     AIRNHRKKHARKIV          ISTISTANBUL AIRPORT **     Y                                                 000713      00                                                                 0                                                    
M50101  TK#5523031460/      1/    2666/   3055/ONE/CA 1.1BABICHEV ROMAN MR/1/F/E
M50202  TK#5523031461/      1/    2666/   3055/ONE/CA 2.1PLETIN LIUDMYLA MRS/1/F/E
M50303  TK#5523031462/      1/    2666/   3055/ONE/CA 3.1BALOBAN TETIANA MRS/1/F/E
M50403 RTK#5523031462/      1/1062/3055/ONE/CA 3.1BALOBAN TETIANA MRS/1/12/F0002884/F/E-VOL REF WITH FEE
M901PT-SSR CTCM TK HK1/380508129343
M902PT-SSR CTCE TK HK1/MVOROBYOVA//NOVAAGRO.COM.UA
M903PT-SSR DOCS TK HK1/P/UA/FX964254/UA/03MAR83/M/02DEC29/BABICHEV/ROMAN
M904PT-SSR DOCS TK HK1/P/UA/FS728787/UA/11SEP81/F/01OCT28/PLETIN/LIUDMYLA
M905PT-SSR DOCS TK HK1/P/UA/FJ509090/UA/17JUN87/F/25OCT27/BALOBAN/TETIANA
M906PT-SSR ADTK AA TO  TK BY 23FEB 1322 IRC-2/ADV OTO TKT 
M907PT-SSR OTHS AA   PLS ADV FQTV NUMBER IF AVAILABLE WITH SSR FORMAT
M908PT-SSR OTHS AA   PLS ADV PSGR MOBILE AND/OR EMAIL AS SSR CTCM/CTCE
M909PT-SSR ADTK AA TO  TK BY 28FEB 1907 IRC-2/ADV MORE TKT 

", "UAH").ToList();

			Assert.NotNull(docs);

			Assert.AreEqual(3, docs.Count);

			var refund0 = (AviaRefund)docs[2];

			Assert.AreEqual("UAH", refund0.EqualFare.Currency.Code);
			Assert.AreEqual(1062m, refund0.EqualFare.Amount);
			//Assert.AreEqual("220-3096088141", mco.Name);
			//Assert.AreEqual(801m, mco.Total.Amount);
			//Assert.AreEqual(9194m, ((AviaRefund)docs[0]).FeesTotal.Amount);
		}



		[Test]
		public void Test_11_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA16MAR0500M0117               Y    000300872 32487 3  THYAEO        611111110000011001 S1OI * YS 2E9AEB            02MAR 1036S1OI * YS   00005MAYHRKKHARKIV          HRKKHARKIV          002001004004003001000000011000  02 120016MAR











380577666089
38067347077-M

IR
M101BASANETS/ANASTASIIA MRS                                                                                                 041 02000010  
01020304
0103


01020405060708091011
M102BASANETS/OLENA MRS                                                                                                      04  01000005  
01020304
02


0308091011
M201ADT42733973415X 2   RST       USD  449.00 PD 9430YR PD  134UA PD 1055XT    NO ADC                               UAH UAH                                F                       HRK  N S1OI A YS S1OI A YS                      0401TK38854162520
01020304
01
CASH
2353885416144IEV03MAR2072324873
2353885416144/1234
BSR24.68/INVOL RER DUE COVID-19





M3011 0HK05MAYAIRNHRKKHARKIV          ISTISTANBUL AIRPORT TK 1476V 2145 0005  2.20   M   110                    73H000713      00                                                                 0                                     1002020S3B4L6  



M3021 0HK06MAYAIRNISTISTANBUL AIRPORT DPSDENPASAR BALI    TK   66V 0200 1930 12.30   M   000                    789006413      00                                                                 0                                     1002020S3B4L6  



M3031 0HK26MAYAIRNDPSDENPASAR BALI    ISTISTANBUL AIRPORT TK   67V 2100 0455 12.55   M   110                    789006413      00                                                                 0                                     1002020S3B4L6  



M3041 0HK27MAYAIRNISTISTANBUL AIRPORT HRKKHARKIV          TK 1473V 0650 0855  2.05   M   000                    73H000713      00                                                                 0                                     1002020S3B4L6  



M401ADT  05MAY05MAYOK20KVN2XPB                 1VN2XPB                                
M402ADTX 06MAY06MAYOK20KVN2XPB                 1VN2XPB                                
M403ADTO 26MAY26MAYOK20KVN2XPB                 1VN2XPB                                
M404ADTX 27MAY27MAYOK20KVN2XPB                 1VN2XPB                                
M50101  TK#3885416144/      1/   11082/  10619/ONE/CA 1.1BASANETS ANASTASIIA MRS/1/F/E
M50202  TK#3885416145/      1/   11082/  10619/ONE/CA 2.1BASANETS OLENA MRS/1/F/E
M50301 ETK#3885416252/      0/       0/      0/ONE/CA 1.1BASANETS ANASTASIIA MRS/1/F/E-@2353885416144/1234
M6ADT1   I-HRK TK X/IST TK DPS224.50TK X/IST TK HRK224.50NUC449.00END ROE1.00 PD 50UD346YK272TR387D5
M901PT-SSR CTCM TK HK1/38067347077

M902PT-SSR DOCS TK HK1/P/UA/FS165538/UA/23SEP98/F/14AUG28/BASANETS/ANASTASIIA//H

M903PT-SSR DOCS TK HK1/P/UA/FV842386/UA/10APR70/F/17JUL29/BASANETS/OLENA//H

M904PT-SSR TKNE TK HK1 HRKIST1476V05MAY/2353885416252C1

M905PT-SSR TKNE TK HK1 ISTDPS0066V06MAY/2353885416252C2

M906PT-SSR TKNE TK HK1 DPSIST0067V26MAY/2353885416252C3

M907PT-SSR TKNE TK HK1 ISTHRK1473V27MAY/2353885416252C4

M908PT-SSR ADTK AA TO  TK BY 05MAR 1836 IRC-2/ADV OTO TKT 

M909PT-SSR OTHS AA   PLS ADV FQTV NUMBER IF AVAILABLE WITH SSR FORMAT

M910PT-SSR OTHS AA   PLS ADV PSGR MOBILE AND/OR EMAIL AS SSR CTCM/CTCE

M911PT-SSR ADTK AA TO  TK BY 17MAR 1156 IRC-2/ADV OTO TKT 

MF01 #OWENTOUR@UKR.NET#TO/@U
MF02 #AVIAKAS@UKR.NET#TO/@U



", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(2, docs.Count);

			//Assert.AreEqual(11633m, ((AviaTicket)docs[0]).EqualFare.Amount);
			//Assert.AreEqual(8225m, ((AviaTicket)docs[1]).EqualFare.Amount);
			//Assert.AreEqual(1163m, ((AviaTicket)docs[2]).EqualFare.Amount);
		}



		[Test]
		public void Test_12_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA22DEC1207M0117                                       LOEGXG        1                  IEVPS28ND                   22DEC0606 IEVPS28ND      25FEBKBPKYIV             ODSKYIV             003003001000000000000000000000  01 140722DEC 











00[380]961234567-M   



M101BASEVICH/EVGENIY MR                                             DOB10OCT90                                              1 1 00000000 





M102BASEVICH/INFANTIK MR                                            DOB10OCT20                                              1 1 00000000 





M103BASEVICH/CHILD MR                                               DOB10OCT15                                              1 1 00000000 






M201ADT           X 2 P RST       USD   22.00   24.80                       USD   46.80                             USD   22.00    3.00     0.66    22.00      24.80                    NIEVPS28ND IEVPS28ND                       0000PS24070067270 











M202INF           X 2 P RST       USD   17.00   23.80                       USD   40.80                             USD   17.00    3.00     0.51    17.00      23.80                    NIEVPS28ND IEVPS28ND                       0000PS24070067280 











M203CNN           X 2 P RST       USD    2.00    0.40                       USD    2.40                             USD    2.00    3.00     0.06     2.00       0.40                    NIEVPS28ND IEVPS28ND                       0000PS24070067290 












M3011 0HK25FEBAIRNKBPKYIV             ODSODESSA           PS  55 Y 1930 2030 60          000                    E90            00D                                                                0                                     1002021LOEGXG   



MF1 #B2B-DEMO-IL@WENORTECH.COM#                                     ", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(3, docs.Count);

			//Assert.AreEqual(11633m, ((AviaTicket)docs[0]).EqualFare.Amount);
			//Assert.AreEqual(8225m, ((AviaTicket)docs[1]).EqualFare.Amount);
			//Assert.AreEqual(1163m, ((AviaTicket)docs[2]).EqualFare.Amount);
		}



		[Test]
		public void Test_13_Ticket()
		{
			var docs = SabreFilParser.Parse(
				"AA20JAN1122M0117                                       LIKDOC        1                  IEVPS28PT                   20JAN0514 IEVPS28PT      18FEBKBPKYIV             LWOKYIV             001001001000000000000000000000  01 132220JAN \n\n\n\n\n\n\n\n\n\n\n\n00[380]673339570-M   \n\n\n\nM101SPIRI/PAVEL MR                                                  DOB12DEC00                                              1 1 00000000 \n\n\n\n\n\n\nM201ADT           X 2 P RST       UAH 1131.00  381.00                       UAH 1512.00                             UAH 1131.00    0.00     0.00  1131.00     381.00                    NIEVPS28PT IEVPS28PT                       0000PS24070243520 \n\n\n\n\n\n\n\n\n\n\n\n\nM3011 0HK18FEBAIRNKBPKYIV             LWOLVIV             PS  906Y 2030 2205 95      5   000                    AT7            00D                                                                0                                     1002021LIKDOC   \n\n\n\nMF1 #P.SPIRIDONOV@WENORTECH.COM#                                    ", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(1, docs.Count);

			Assert.AreEqual(1131m, ((AviaTicket)docs[0]).EqualFare.Amount);
			//Assert.AreEqual(8225m, ((AviaTicket)docs[1]).EqualFare.Amount);
			//Assert.AreEqual(1163m, ((AviaTicket)docs[2]).EqualFare.Amount);
		}




		[Test]
		public void Test_14_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA29MAR0951M0117               Y    000000572 32487 3  JMUDDY        611111110000001001 M47K * SS 121750            27MAR 0609M47K * SS   00030MARODSODESSA           LEDST PETERSBURG LED001001002002002001000000015000  01 175129MAR











380675177830-A


SS
M101FAYNBERG/FELIKS MR                                                                                                      021 02000014  
0102
0102


0102030405060708091011121314
M201ADT80951352700X 2   RST       USD  199.00 PD 2848XT     812YR     632XT UAH   2564AXT 166TR466RI                UAH UAH                          1120  F    1444               ODS  N M47K A SS M47K A SS                      0201TK38854856060
0102
01
CASH CASH
2353885485605ODS27MAR2172324873
2353885485605/12
BSR28/NONEND/TK ONLY





M3011 0HK30MARAIRNODSODESSA           ISTISTANBUL AIRPORT TK  468S 2130 2255  1.25   M   000                    321000392      00                                                                 0                                     1002021SYYBA8  



M3021 0HK31MARAIRNISTISTANBUL AIRPORT LEDST PETERSBURG LEDTK  401S 1125 1440  3.15   M   000                    332001306      00                                                                 0                                     1002021SYYBA8  



M401ADT  30MAR30MAROK20KSN2XOX                 1SN2XOX                                
M402ADTX 31MAR31MAROK20KSN2XOX         000199001SN2XOX                                
M50101  TK#3885485605/      1/    4452/   2881/ONE/CA 1.1FAYNBERG FELIKS MR/1/F/E
M50201 ATK#3885485606/      0/    1120/   1444/ONE/CA 1.1FAYNBERG FELIKS MR/1/F/E-@2353885485605/12
M6ADT1   ODS TK X/IST TK LED199.00NUC199.00END ROE1.00 PD 2184YR146UA56UD462YK XT166TR466RI
M901PT-OSI YY CTCP380482393245 M

M902PT-SSR CTCE TK HK1/AVIA..ODS//UFSA.COM.UA

M903PT-SSR CTCM TK HK1/380674886694

M904PT-SSR DOCS TK HK1/P/UKR/FX780174/UKR/01DEC72/M/06NOV29/FAYNBERG/FELIKS

M905PT-SSR CTCE TK HK1/FAYNBERG//MAIL.RU

M906PT-SSR TKNE TK HK1 ODSIST0468S30MAR/2353885485606C1

M907PT-SSR TKNE TK HK1 ISTLED0401S31MAR/2353885485606C2

M908PT-SSR ADTK AA TO  TK BY 27MAR 1809 IRC-2/ADV OTO TKT 

M909PT-SSR OTHS AA   PLS ADV FQTV NUMBER IF AVAILABLE WITH SSR FORMAT

M910PT-SSR OTHS AA   PLS ADV PSGR MOBILE AND/OR EMAIL AS SSR CTCM/CTCE

M911PT-SSR OTHS AA   DOCS CTCM CTCE ARE MANDATORY FOR TICKETING

M912PT-SSR OTHS AA   PLS ENTER DOCS CTCM AND CTCE FIELDS WITH SSR FORMAT

M913PT-SSR OTHS AA   FOR DOCS FIELD DOB GENDER NAME FIELDS ARE MANDATORY

M914PT-SSR ADTK AA TO  TK BY 29MAR 2249 IRC-2/ADV OTO TKT 

M915 TK     337776153            HK TK           1.1  FAYNBERG/FELIKS MR


", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(1, docs.Count);

			Assert.AreEqual(1120m, ((AviaTicket)docs[0]).EqualFare.Amount);
			Assert.AreEqual(1444m, ((AviaTicket)docs[0]).FeesTotal.Amount);
			Assert.AreEqual(2564m, ((AviaTicket)docs[0]).Total.Amount);
		}


		
		[Test]
		public void Test_15_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA09AUG0504M0A17               Y    000351072 32487 3  ILSLGE        611111110000011000 S1OI * OR 06C775            06AUG 0252S1OI * OR   00023AUGKBPKYIV    BORISPOL KBPKYIV    BORISPOL 009000004000018000000000019000  02 130409AUG











0380674446027
380442067576-A

AR
M101YERKO/VLADYSLAV MR                                                                                                      04  02000001  
01020304
0110


19
M102IVASHCHENKO/DENYS MR                                                                                                    04  02000001  
01020304
0211


19
M103PLISKO/STANISLAV MR                                                                                                     04  02000001  
01020304
0312


19
M104FUNKENDORF/VLADYSLAV MR                                                                                                 04  02000001  
01020304
0413


19
M105DZOZ/VLADYSLAV MR                                                                                                       04  02000001  
01020304
0514


19
M106YURKO/OLEKSII MR                                                                                                        04  02000001  
01020304
0615


19
M107TYMCHUK/VALERIIA MRS                                                                                                    04  02000001  
01020304
0716


19
M108BONDAR/NATALIIA MRS                                                                                                     04  02000007  
01020304
0817


01020304050619
M109KHOMENKO/YEKATERYNA MRS                                                                                                 04  02000013  
01020304
0918


07080910111213141516171819
M3011 0HK23AUGAIRNKBPKYIV    BORISPOL RIXRIGA             BT  407E 0430 0620  1.50   G   000                    223000529      09TERMINAL D - INTL/DOMESTIC                                       0                                      002021N6S3ZJ  
01 05EN
02 03DN
03 03EN
04 03FN
05 04DN
06 04EN
07 04FN
08 05FN
09 05DN



M3021 0HK23AUGAIRNRIXRIGA             TLLTALLINN          BT  311E 0745 0835   .50   G   000                    223000178      09                                                                 0                                      002021N6S3ZJ  
01 03DN
02 03EN
03 03FN
04 04DN
05 04EN
06 04FN
07 05DN
08 05EN
09 05FN



M3031 0HK28AUGAIRNTLLTALLINN          RIXRIGA             BT  312A 0935 1025   .50   G   000                    223000178      09                                                                 0                                      002021N6S3ZJ  
01 06DN
02 06EN
03 06FN
04 07FN
05 07EN
06 07DN
07 08DN
08 08EN
09 08FN



M3041 0HK28AUGAIRNRIXRIGA             KBPKYIV    BORISPOL BT  402A 1250 1445  1.55   G   000                    223000529      09                              TERMINAL D - INTL/DOMESTIC         0                                      002021N6S3ZJ  
01 03DN
02 03EN
03 03FN
04 06DN
05 06EN
06 06FN
07 07DN
08 07EN
09 07FN



M50101  BT#6503417933/     71/    7083/   4646/ONE/CA 1.1YERKO VLADYSLAV MR/1/F/E
M50202  BT#6503417934/     71/    7083/   4646/ONE/CA 2.1IVASHCHENKO DENYS MR/1/F/E
M50303  BT#6503417935/     71/    7083/   4646/ONE/CA 3.1PLISKO STANISLAV MR/1/F/E
M50404  BT#6503417936/     71/    7083/   4646/ONE/CA 4.1FUNKENDORF VLADYSLAV MR/1/F/E
M50505  BT#6503417937/     71/    7083/   4646/ONE/CA 5.1DZOZ VLADYSLAV MR/1/F/E
M50606  BT#6503417938/     71/    7083/   4646/ONE/CA 6.1YURKO OLEKSII MR/1/F/E
M50707  BT#6503417939/     71/    7083/   4646/ONE/CA 7.1TYMCHUK VALERIIA MRS/1/F/E
M50808  BT#6503417940/     71/    7083/   4646/ONE/CA 8.1BONDAR NATALIIA MRS/1/F/E
M50909  BT#6503417941/     71/    7083/   4646/ONE/CA 9.1KHOMENKO YEKATERYNA MRS/1/F/E
M51001  BT#1864661575/      0/        0/        0/ONE/CA 1.1YERKO VLADYSLAV MR/1/I/EMD
M51102  BT#1864661576/      0/        0/        0/ONE/CA 2.1IVASHCHENKO DENYS MR/1/I/EMD
M51203  BT#1864661577/      0/        0/        0/ONE/CA 3.1PLISKO STANISLAV MR/1/I/EMD
M51304  BT#1864661578/      0/        0/        0/ONE/CA 4.1FUNKENDORF VLADYSLAV MR/1/I/EMD
M51405  BT#1864661579/      0/        0/        0/ONE/CA 5.1DZOZ VLADYSLAV MR/1/I/EMD
M51506  BT#1864661580/      0/        0/        0/ONE/CA 6.1YURKO OLEKSII MR/1/I/EMD
M51607  BT#1864661581/      0/        0/        0/ONE/CA 7.1TYMCHUK VALERIIA MRS/1/I/EMD
M51708  BT#1864661582/      0/        0/        0/ONE/CA 8.1BONDAR NATALIIA MRS/1/I/EMD
M51809  BT#1864661583/      0/        0/        0/ONE/CA 9.1KHOMENKO YEKATERYNA MRS/1/I/EMD
M901PT-SSR CKBG BT KK1 TLLRIX0312A28AUG

M902PT-SSR CKFE BT KK1 TLLRIX0312A28AUG

M903PT-SSR PRIO BT KK1 TLLRIX0312A28AUG

M904PT-SSR CKBG BT KK1 RIXKBP0402A28AUG

M905PT-SSR CKFE BT KK1 RIXKBP0402A28AUG

M906PT-SSR PRIO BT KK1 RIXKBP0402A28AUG

M907PT-SSR CKBG BT KK1 KBPRIX0407E23AUG

M908PT-SSR CKFE BT KK1 KBPRIX0407E23AUG

M909PT-SSR PRIO BT KK1 KBPRIX0407E23AUG

M910PT-SSR CKBG BT KK1 RIXTLL0311E23AUG

M911PT-SSR CKFE BT KK1 RIXTLL0311E23AUG

M912PT-SSR PRIO BT KK1 RIXTLL0311E23AUG

M913PT-SSR CKBG BT KK1 TLLRIX0312A28AUG

M914PT-SSR CKFE BT KK1 TLLRIX0312A28AUG

M915PT-SSR PRIO BT KK1 TLLRIX0312A28AUG

M916PT-SSR CKBG BT KK1 RIXKBP0402A28AUG

M917PT-SSR CKFE BT KK1 RIXKBP0402A28AUG

M918PT-SSR PRIO BT KK1 RIXKBP0402A28AUG

M919PT-SSR ADMD 1S KK1 TO BT BY 10AUG 1048 OTHERWISE WILL BE CANCELLED

MF01 #SALE@UFSA.COM.UA#

MG01ADT09AUG202113041SBT 657186466157530657650341793352 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG02ADT09AUG202113041SBT 657186466157640657650341793462 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG03ADT09AUG202113041SBT 657186466157750657650341793502 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG04ADT09AUG202113041SBT 657186466157860657650341793612 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG05ADT09AUG202113041SBT 657186466157900657650341793722 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG06ADT09AUG202113041SBT 657186466158010657650341793832 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG07ADT09AUG202113041SBT 657186466158120657650341793942 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG08ADT09AUG202113041SBT 657186466158230657650341794052 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                



MG09ADT09AUG202113041SBT 657186466158340657650341794162 N  S KBP  KBP  A                UAH                 0                 0                 0                     CA                                            Y                       04
 
REFUNDABLE
01                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      407  KBP  RIX  23AUG2021                



02                 0                      023AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      311  RIX  TLL  23AUG2021                



03                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      312  TLL  RIX  28AUG2021                



04                 0                      028AUG2021                  0B5PRE RESERVED SEAT ASSIGNMENT                      402  RIX  KBP  28AUG2021                




", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(9, docs.Count);

			Assert.AreEqual("1864661575", ((AviaMco)docs[0]).Number);
			Assert.AreEqual("1864661576", ((AviaMco)docs[1]).Number);
			Assert.AreEqual("1864661577", ((AviaMco)docs[2]).Number);
			Assert.AreEqual("1864661578", ((AviaMco)docs[3]).Number);
			Assert.AreEqual("1864661579", ((AviaMco)docs[4]).Number);
			Assert.AreEqual("1864661580", ((AviaMco)docs[5]).Number);
			Assert.AreEqual("1864661581", ((AviaMco)docs[6]).Number);
			Assert.AreEqual("1864661582", ((AviaMco)docs[7]).Number);
			Assert.AreEqual("1864661583", ((AviaMco)docs[8]).Number);

			//Assert.AreEqual(1120m, ((AviaTicket)docs[0]).EqualFare.Amount);
			//Assert.AreEqual(1444m, ((AviaTicket)docs[0]).FeesTotal.Amount);
			//Assert.AreEqual(2564m, ((AviaTicket)docs[0]).Total.Amount);
		}



		[Test]
		public void Test_16_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA10SEP0452M0117                    000357572 32487 3  EYPUFY        611111110000011001 S1OI * YR 68A265            09SEP 0708S1OI * YR   00014SEPDNKDNIPRO           DNKDNIPRO           005004002002004001000000027000  01 125210SEP











380442067575


YR
M101GLICK/SHMAYA MR                                                                                                         021 01000008  
0102
01


0607081718192027
M102GLICK/CHANA MRS                                                                                                         021 01000008  
0102
02


0509101718212227
M103GLICK/MARYASHA BADANA MRS                                       DOB28MAY11                                              02  00000007  
0102



01041112171827
M104GLICK/JOSEPH ISAAC MR                                                                                                   021 01000008  
0102
03


0313141718232427
M105GLICK/MOSHE MR                                                                                                          021 01000008  
0102
04


0215161718252627
M201ADT48875776204X 2   RST       USD  100.00     536YQ     268YR     909XT UAH    4391                             UAH    2678       1       27     2678  D    1713               DNK  N S1OI A YR S1OI A YR                      02017W65034180340
0102
01
CA


BSR26.77/NONEND/REF REST/RBK REST





M202ADT48875776300X 2   RST       USD  100.00     536YQ     268YR     909XT UAH    4391                             UAH    2678       1       27     2678  D    1713               DNK  N S1OI A YR S1OI A YR                      02017W65034180350
0102
01
CA


BSR26.77/NONEND/REF REST/RBK REST





M204ADT48875776403X 2   RST       USD  100.00     536YQ     268YR     909XT UAH    4391                             UAH    2678       1       27     2678  D    1713               DNK  N S1OI A YR S1OI A YR                      02017W65034180360
0102
01
CA


BSR26.77/NONEND/REF REST/RBK REST





M205ADT48875776506X 2   RST       USD  100.00     536YQ     268YR     909XT UAH    4391                             UAH    2678       1       27     2678  D    1713               DNK  N S1OI A YR S1OI A YR                      02017W65034180370
0102
01
CA


BSR26.77/NONEND/REF REST/RBK REST





M3011 0HK14SEPAIRNDNKDNIPRO           KBPKYIV    BORISPOL 7W  104K 1745 1910  1.25   N   000                    AT7000243      00                              TERMINAL D - INTL/DOMESTIC         0                                     1002021H7E5P   



M3021 0HK17SEPAIRNKBPKYIV    BORISPOL DNKDNIPRO           7W  101M 1010 1130  1.20   N   000                    AT7000243      00TERMINAL D - INTL/DOMESTIC                                       0                                     1002021H7E5P   



M401ADT  14SEP14SEPOK23KKP2PRT         000060001KP2PRT                                
M402ADTO 17SEP17SEPOK23KMP2PRT         000040001MP2PRT                                
M50101  7W#6503418034/     27/    2678/   1713/ONE/CA 1.1GLICK SHMAYA MR/1/D/E
M50202  7W#6503418035/     27/    2678/   1713/ONE/CA 2.1GLICK CHANA MRS/1/D/E
M50304  7W#6503418036/     27/    2678/   1713/ONE/CA 4.1GLICK JOSEPH ISAAC MR/1/D/E
M50405  7W#6503418037/     27/    2678/   1713/ONE/CA 5.1GLICK MOSHE MR/1/D/E
M6ADT1   DNK 7W IEV60.00 7W DNK40.00USD100.00END  XT82UA28UD201YK598HF
M901PT-SSR CHLD 7W HK1/28MAY11

M902PT-SSR DOCS 7W HK1/P/USA/548611582/USA/01JUL08/M/26MAR22/GLICK/MOSHE//H

M903PT-SSR DOCS 7W HK1/P/USA/548611579/AUS/14OCT72/M/26MAR27/GLICK/JOSEPH ISAAC//H

M904PT-SSR DOCS 7W HK1/P/USA/568092766/USA/28MAY11/F/28SEP25/GLICK/MARYASHA BADANA//H

M905PT-SSR DOCS 7W HK1/P/USA/521956501/USA/12JUL98/F/24AUG24/GLICK/CHANA/H

M906PT-SSR DOCS 7W HK1/P/USA/568137877/USA/13MAY06/M/11NOV25/GLICK/SHMAYA//H

M907PT-SSR CTCM 7W HK1/380676358004

M908PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M909PT-SSR CTCM 7W HK1/380676358004

M910PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M911PT-SSR CTCM 7W HK1/380676358004

M912PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M913PT-SSR CTCM 7W HK1/380676358004

M914PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M915PT-SSR CTCM 7W HK1/380676358004

M916PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M917PT-OSI 7W CTCP 380676358004

M918PT-OSI 7W CTCE YESHIVA//YKUKRAINE.ORG

M919PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418034C1

M920PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418034C2

M921PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418035C1

M922PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418035C2

M923PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418036C1

M924PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418036C2

M925PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418037C1

M926PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418037C2

M927PT-SSR OTHS 1S AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY 7W BY 1508/10SEP/IEV LT

MF01 #Y.RUDCHUK@UFSA.COM.UA#


", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(4, docs.Count);


			Assert.AreEqual("6503418034", ((AviaTicket)docs[0]).Number);
			Assert.AreEqual("GLICK/SHMAYA MR", ((AviaTicket)docs[0]).PassengerName);

			Assert.AreEqual("6503418035", ((AviaTicket)docs[1]).Number);
			Assert.AreEqual("GLICK/CHANA MRS", ((AviaTicket)docs[1]).PassengerName);

			Assert.AreEqual("6503418036", ((AviaTicket)docs[2]).Number);
			Assert.AreEqual("GLICK/JOSEPH ISAAC MR", ((AviaTicket)docs[2]).PassengerName);

			Assert.AreEqual("6503418037", ((AviaTicket)docs[3]).Number);
			Assert.AreEqual("GLICK/MOSHE MR", ((AviaTicket)docs[3]).PassengerName);


			//Assert.AreEqual(1120m, ((AviaTicket)docs[0]).EqualFare.Amount);
			//Assert.AreEqual(1444m, ((AviaTicket)docs[0]).FeesTotal.Amount);
			//Assert.AreEqual(2564m, ((AviaTicket)docs[0]).Total.Amount);

		}



		[Test]
		public void Test_17_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA10SEP0456M0117               Y    000357772 32487 3  EYPUFY        611111110000011001 S1OI * YR 68A265            09SEP 0708S1OI * YR   00014SEPDNKDNIPRO           DNKDNIPRO           005001002002005001000000029000  01 125610SEP











380442067575


YR
M101GLICK/SHMAYA MR                                                                                                         02  01000008  
0102
01


0607081718192029
M102GLICK/CHANA MRS                                                                                                         02  01000008  
0102
02


0509101718212229
M103GLICK/MARYASHA BADANA MRS                                       DOB28MAY11                                              021 01000009  
0102
05


010411121718272829
M104GLICK/JOSEPH ISAAC MR                                                                                                   02  01000008  
0102
03


0313141718232429
M105GLICK/MOSHE MR                                                                                                          02  01000008  
0102
04


0215161718252629
M203C1048875794404X 2   RST       USD   75.00     536YQ     268YR     775XT UAH    3588                             UAH    2009       1       20     2009  D    1579               DNK  N S1OI A YR S1OI A YR                      02017W65034180400
0102
01
CA


BSR26.77/NONEND/REF REST/RBK REST





M3011 0HK14SEPAIRNDNKDNIPRO           KBPKYIV    BORISPOL 7W  104K 1745 1910  1.25   N   000                    AT7000243      00                              TERMINAL D - INTL/DOMESTIC         0                                     1002021H7E5P   



M3021 0HK17SEPAIRNKBPKYIV    BORISPOL DNKDNIPRO           7W  101M 1010 1130  1.20   N   000                    AT7000243      00TERMINAL D - INTL/DOMESTIC                                       0                                     1002021H7E5P   



M401C10  14SEP14SEPOK23KKP2PRT/CH25    000045001KP2PRT      CH25                      
M402C10O 17SEP17SEPOK23KMP2PRT/CH25    000030001MP2PRT      CH25                      
M50101  7W#6503418034/     27/    2678/   1713/ONE/CA 1.1GLICK SHMAYA MR/1/D/E
M50202  7W#6503418035/     27/    2678/   1713/ONE/CA 2.1GLICK CHANA MRS/1/D/E
M50304  7W#6503418036/     27/    2678/   1713/ONE/CA 4.1GLICK JOSEPH ISAAC MR/1/D/E
M50405  7W#6503418037/     27/    2678/   1713/ONE/CA 5.1GLICK MOSHE MR/1/D/E
M50503  7W#6503418040/     20/    2009/   1579/ONE/CA 3.1GLICK MARYASHA BADANA MRS CHD/1/D/E
M6C101   DNK 7W IEV45.00 7W DNK30.00USD75.00END  XT82UA28UD201YK464HF
M901PT-SSR CHLD 7W HK1/28MAY11

M902PT-SSR DOCS 7W HK1/P/USA/548611582/USA/01JUL08/M/26MAR22/GLICK/MOSHE//H

M903PT-SSR DOCS 7W HK1/P/USA/548611579/AUS/14OCT72/M/26MAR27/GLICK/JOSEPH ISAAC//H

M904PT-SSR DOCS 7W HK1/P/USA/568092766/USA/28MAY11/F/28SEP25/GLICK/MARYASHA BADANA//H

M905PT-SSR DOCS 7W HK1/P/USA/521956501/USA/12JUL98/F/24AUG24/GLICK/CHANA/H

M906PT-SSR DOCS 7W HK1/P/USA/568137877/USA/13MAY06/M/11NOV25/GLICK/SHMAYA//H

M907PT-SSR CTCM 7W HK1/380676358004

M908PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M909PT-SSR CTCM 7W HK1/380676358004

M910PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M911PT-SSR CTCM 7W HK1/380676358004

M912PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M913PT-SSR CTCM 7W HK1/380676358004

M914PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M915PT-SSR CTCM 7W HK1/380676358004

M916PT-SSR CTCE 7W HK1/YESHIVA//YKUKRAINE.ORGKUKRAINE.ORG

M917PT-OSI 7W CTCP 380676358004

M918PT-OSI 7W CTCE YESHIVA//YKUKRAINE.ORG

M919PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418034C1

M920PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418034C2

M921PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418035C1

M922PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418035C2

M923PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418036C1

M924PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418036C2

M925PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418037C1

M926PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418037C2

M927PT-SSR TKNE 7W HK1 DNKKBP0104K14SEP/4616503418040C1

M928PT-SSR TKNE 7W HK1 KBPDNK0101M17SEP/4616503418040C2

M929PT-SSR OTHS 1S AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY 7W BY 1508/10SEP/IEV LT

MF01 #Y.RUDCHUK@UFSA.COM.UA#


", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(1, docs.Count);


			Assert.AreEqual("6503418040", ((AviaTicket)docs[0]).Number);
			Assert.AreEqual("GLICK/MARYASHA BADANA MRS", ((AviaTicket)docs[0]).PassengerName);

		}



		[Test]
		public void Test_18_Ticket()
		{
			var docs = SabreFilParser.Parse(
@"AA11SEP0928M0217               Y    000047172 32501 3  GPBXET        611111110000011000 1MTI * IO 1FB8EB            06SEP 05081MTI 9 DM   00019SEPVIEVIENNA           VIEVIENNA           003000002000003000000000013000  01 172811SEP











380679135847


IO
M101CHURILOVA/GALYNA MRS                                                                                                    02  01000006  
0102
01


010406071213
M102KOVAL/ARTEM MR                                                                                                          02  01000006  
0102
02


020508091213
M103PIDGAINA/IRYNA MRS                                                                                                      02  01000005  
0102
03


0310111213
M3011 0HK19SEPAIRNVIEVIENNA           BUDBUDAPEST         OS  713Q 0955 1040   .45   B   000                    E95000134      00TERMINAL 3                    TERMINAL 2A                        0                                      002023LDHO92  



M3021 0HK24SEPAIRNBUDBUDAPEST         VIEVIENNA           OS  714W 1120 1210   .50   B   000                    E95000134      00TERMINAL 2A                   TERMINAL 3                         0                                      002023LDHO92  



M50101  OS#9340301419/      0/    9596/   7007/ONE/CCCA5169330000006584 1.1CHURILOVA GALYNA MRS/1/F/E
M50202  OS#9340301420/      0/    9596/   7007/ONE/CCCA5169330000006584 2.1KOVAL ARTEM MR/1/F/E
M50303  OS#9340301421/      0/    9596/   7007/ONE/CCCA5169330000006584 3.1PIDGAINA IRYNA MRS/1/F/E
M901PT-SSR DOCS OS HK1/P/UA/FU127195/UA/06MAY58/F/13FEB29/CHURILOVA/GALYNA

M902PT-SSR DOCS OS HK1/P/UA/GA893463/UA/26AUG04/M/06AUG31/KOVAL/ARTEM

M903PT-SSR DOCS OS HK1/P/UA/GE785140/UA/24SEP08/F/04JAN27/PIDGAINA/IRYNA

M904PT-SSR CTCM OS HK1/380674446027

M905PT-SSR CTCE OS HK1/IGOR//UFSA.COM.UA

M906PT-SSR TKNE OS HK1 VIEBUD0713Q19SEP/2579340301419C1

M907PT-SSR TKNE OS HK1 BUDVIE0714W24SEP/2579340301419C2

M908PT-SSR TKNE OS HK1 VIEBUD0713Q19SEP/2579340301420C1

M909PT-SSR TKNE OS HK1 BUDVIE0714W24SEP/2579340301420C2

M910PT-SSR TKNE OS HK1 VIEBUD0713Q19SEP/2579340301421C1

M911PT-SSR TKNE OS HK1 BUDVIE0714W24SEP/2579340301421C2

M912PT-SSR OTHS 1S MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SSR CTCR NON-CONSENT FOR OS

M913PT-SSR OTHS 1S PLS ADV TKT NBR BY 09SEP23/1008Z OR OS OPTG/MKTG FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMANDS EARLIER TKTG


", "UAH").ToList();


			Assert.NotNull(docs);
			Assert.AreEqual(3, docs.Count);

			var t0 = (AviaTicket)docs[0];
			var t1 = (AviaTicket)docs[1];
			var t2 = (AviaTicket)docs[2];


			Assert.AreEqual("9340301419", t0.Number);
			Assert.AreEqual("9340301420", t1.Number);
			Assert.AreEqual("9340301421", t2.Number);

			Assert.AreEqual("CHURILOVA/GALYNA MRS", t0.PassengerName);
			Assert.AreEqual("KOVAL/ARTEM MR", t1.PassengerName);
			Assert.AreEqual("PIDGAINA/IRYNA MRS", t2.PassengerName);

		}
		


		//---g



		#region Utils

		private AviaTicket ParseTicket(string documentContent)
		{
			var docs = SabreFilParser.Parse(documentContent, "UAH");

			Assert.NotNull(docs);
			Assert.GreaterOrEqual(docs.Count(), 1);
			var r = (AviaTicket)docs.First();
			Assert.NotNull(r);

			return r;
		}

		private AviaDocumentVoiding ParseVoiding(string documentContent)
		{
			var docs = SabreFilParser.Parse(documentContent, "UAH");

			Assert.NotNull(docs);
			Assert.GreaterOrEqual(docs.Count(), 1);
			var r = (AviaDocumentVoiding)docs.First();
			Assert.NotNull(r);

			return r;
		}

		#endregion

	}
}