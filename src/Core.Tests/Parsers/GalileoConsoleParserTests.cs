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
	public class GalileoConsoleParserTests
    {

		//---g



		private List<AviaDocument> Parse(string content)
		{
			return GalileoConsoleParser
				.Parse(content, new Currency("UAH"))
				.ToList()
			;
		}



		//---g



		[Test]
		public void TestParseTicket01()
		{

			var docs = Parse(@"
--- TST ---
RP/FLXMIA/                       F1/AGMW       F1:2L2RYX   LXA:VFWE6I
RF AGMX_ASMIRNOVA      19JAN2022 1138Z
  1. DUDAR/VALERII MR
  2. GAIDAIEVA/MARGARYTA MRS
  3. GLUKHOVSKA/OLENA MRS
  4. LERIT/ALINA MRS
  5  LX2291 K 03FEB D KBPZRH HK4           0920   1115   *LXA:VFWE6I/E*
  6  LX568  K 03FEB   ZRHNCE HK4        2  1250   1405   *LXA:VFWE6I/E*
  7  LX569  K 06FEB 2 NCEZRH HK4           1510   1625   *LXA:VFWE6I/E*
  8  LX2290 K 06FEB   ZRHKBP HK4        D  1705   2045   *LXA:VFWE6I/E*
  9  AP M -380675002557-A
  10 AP M -380675001991-A
  11 APE BSV@BSV.COM.UA
  12 FP CASH
  13 FH LXA TKNE 72494159076172S5-8.P1
  14 FH LXA TKNE 72494159076183S5-8.P2
  15 FH LXA TKNE 72494159076194S5-8.P3
  16 FH LXA TKNE 72494159076205S5-8.P4
  17 SSR OTHS F1   MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SSR CTCR NON-CONSENT FOR LX/P
  18 SSR OTHS F1   PLS ADV TKT NBR BY 20JAN22/1138Z OR LX OPTG/MKTG FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMANDS EARLIER TKTG/P
  19 SSR DOCS LX  HK P/UA/FP550670/UA/16APR73/F/02JUL28/GLUKHOVSKA/OLENA/P3
  20 SSR DOCS LX  HK P/UA/FJ352892/UA/29NOV88/F/24OCT27/LERIT/ALINA/P4
  21 SSR DOCS LX  HK P/UA/FU590767/UA/30JUN82/M/25MAR29/DUDAR/VALERII/P1
  22 SSR DOCS LX  HK P/UA/FA846391/UA/09APR65/F/28SEP25/GAIDAIEVA/MARGARYTA/P2
  23 SSR TKNE LX  HK 7249415907617C1/S5.P1
  24 SSR TKNE LX  HK 7249415907617C2/S6.P1
  25 SSR TKNE LX  HK 7249415907617C3/S7.P1
  26 SSR TKNE LX  HK 7249415907617C4/S8.P1
  27 SSR TKNE LX  HK 7249415907618C1/S5.P2
  28 SSR TKNE LX  HK 7249415907618C2/S6.P2
  29 SSR TKNE LX  HK 7249415907618C3/S7.P2
  30 SSR TKNE LX  HK 7249415907618C4/S8.P2
  31 SSR TKNE LX  HK 7249415907619C1/S5.P3
  32 SSR TKNE LX  HK 7249415907619C2/S6.P3
  33 SSR TKNE LX  HK 7249415907619C3/S7.P3
  34 SSR TKNE LX  HK 7249415907619C4/S8.P3
  35 SSR TKNE LX  HK 7249415907620C1/S5.P4
  36 SSR TKNE LX  HK 7249415907620C2/S6.P4
  37 SSR TKNE LX  HK 7249415907620C3/S7.P4
  38 SSR TKNE LX  HK 7249415907620C4/S8.P4
>tqt
TST00001                 AGMX_ASMIRNOVALD 20JAN22

FXB
   1.DUDAR/VALERII LERIT/ALINA GLUKHOVSKA/OLENA GAIDAIEVA/MARGARYTA
     KBP-ZRH LX  2291K 03FEB 0920 OK  K05CLSE8             03FEB03FEB  1PC
     ZRH-NCE LX  568 K 03FEB 1250 OK  K05CLSE8             03FEB03FEB  1PC
     NCE-ZRH LX  569 K 06FEB 1510 OK  K05CLSE8             06FEB06FEB  1PC
     ZRH-KBP LX  2290K 06FEB 1705 OK  K05CLSE8             06FEB06FEB  1PC
     KBP
FARE    USD    181.00
EQUIV   UAH    5139.00
TX001   UAH    162.00YQ
TX002   UAH    162.00YQ
TX003   UAH    162.00YQ
TX004   UAH    162.00YQ
TX005   UAH    992.00CH
TX006   UAH    270.00FR
TX007   UAH    265.00FR
TX008   UAH    146.00IZ
TX009   UAH     97.00O4
TX0010  UAH    188.00QX
TX0011  UAH    114.00UA
TX0012  UAH     57.00UD
TX0013  UAH    369.00YK
TOTAL   UAH    8285.00    BSR 28.392265
GRAND TOTAL UAH    33140.00
IEV LX X/ZRH LX NCE90.50 LX X/ZRH LX IEV90.50 NUC181.00END ROE1.000000
FE FARE RESTRICTION MAY APPLY
FM *F*1.00A
FV *F*LX
>
"
			);



			docs.Assert(

				a => a
					.PassengerName("DUDAR/VALERII MR")
					.Number("9415907617")
				,

				a => a
					.PassengerName("GAIDAIEVA/MARGARYTA MRS")
					.Number("9415907618")
				,

				a => a
					.PassengerName("GLUKHOVSKA/OLENA MRS")
					.Number("9415907619")
				,

				a => a
					.PassengerName("LERIT/ALINA MRS")
					.Number("9415907620")

			);


			docs.AssertAll(a => a

				.IssueDate("2022-01-19")
				.AirlinePrefixCode("724")
				.PnrCode("2L2RYX")
				.TourCode("VFWE6I")
				.BookerOffice("AGMX")
				.BookerCode("ASMIRNOVA")

				.FlightSegments(
					
					seg => seg
						.FromAirport("KBP")
						.ToAirport("ZRH")
						.FlightNumber("2291")
						.DepartureTime("2022-02-03T09:20")
						.ArrivalTime("2022-02-03T11:15")
					,

					seg => seg
						.FromAirport("ZRH")
						.ToAirport("NCE")
						.FlightNumber("568")
						.DepartureTime("2022-02-03T12:50")
						.ArrivalTime("2022-02-03T14:05")
					,

					seg => seg
						.FromAirport("NCE")
						.ToAirport("ZRH")
						.FlightNumber("569")
						.DepartureTime("2022-02-06T15:10")
						.ArrivalTime("2022-02-06T16:25")
					,

					seg => seg
						.FromAirport("ZRH")
						.ToAirport("KBP")
						.FlightNumber("2290")
						.DepartureTime("2022-02-06T17:05")
						.ArrivalTime("2022-02-06T20:45")

				)

				.FlightSegmentsAll(seg => seg
					.CarrierIataCode("LX")
					.ServiceClassCode("K")
					.FareBasis("K05CLSE8")
					.Luggage("1PC")
				)

			);


		}



		[Test]
		public void TestParseTicket02()
		{

			var docs = Parse(@"
--- TST ---
RP/FLXMIA/                       F1/AGMW       F1:R7GTMX
  1. NAGREBELNA/ZORIANA MRS
  2. NAGREBELNYI/ANDRII MR
  3. NAGREBELNYI/BOGUMYR MSTR  (CNN)
  4  AP M -0673817878-A
  5  AP 0675001991-B
  6  FP CASH
  7  FH LH TKNE 22095439922920S4-5.P1
  8  FH LH TKNE 22095439922931S4-5.P2
  9  FH LH TKNE 22095439922942S4-5.P3
  10 SSR OTHS F1   PLS ADV TKT NBR BY 29AUG21/1513Z OR LH OPTG/MKTG FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMANDS EARLIER TKTG/P
  11 SSR TKNE LH  HK 2209543992292C1/P1
  12 SSR TKNE LH  HK 2209543992292C2/P1
  13 SSR TKNE LH  HK 2209543992293C1/P2
  14 SSR TKNE LH  HK 2209543992293C2/P2
  15 SSR TKNE LH  HK 2209543992294C1/P3
  16 SSR TKNE LH  HK 2209543992294C2/P3
  17 SSR OTHS F1   PLS PROVIDE PAX CTC DATA VIA SSR CTCM OR CTCE/P
  18 SSR CHLD LH  HK /P3
  19 SSR OTHS F1   PLS ADV EMD NBR FOR RQST BY 09SEP21/0931Z OR RQST WILL BE CNLD // 09SEP210631/P
  20 SSR OTHS F1   SSR RQST CANCELLED DUE TO TIME LIMIT EXPIRY//09SEP210931/P
>tqt
T    P/S  NAME                             TOTAL            FOP                SEGMENTS

1         NAGREBELNYI/BOGUMYR MSTR [CNN]   UAH 7425.00                            4
2         NAGREBELNA/ZORIANA [ADT]         UAH 8714.00                            4
          NAGREBELNYI/ANDRII [ADT]         UAH 8714.00                            4
>tqt/t1
TST00001                 IOVA/         LD 29AUG21

FXB
   1.NAGREBELNYI/BOGUMYR MSTR                    (CNN)
     KBP-MUC LH  2545H 10SEP 1525 OK  HEULGTX0/CH25        10SEP10SEP  0PC
     MUC-KBP LH  2546K 13SEP 1810 OK  KEULGTX4/CH25        13SEP13SEP  0PC
     KBP
FARE    USD    144.00
EQUIV   UAH    3867.00
TX001   UAH   1420.00YQ
TX002   UAH    158.00YQ
TX003   UAH    314.00DE
TX004   UAH    407.00OY
TX005   UAH    748.00RA
TX006   UAH    108.00UA
TX007   UAH     54.00UD
TX008   UAH    349.00YK
TOTAL   UAH    7425.00    BSR 26.854167
GRAND TOTAL UAH    7425.00
IEV LH MUC132.37 LH IEV11.25 NUC143.62END ROE1.000000
FE FARE RESTRICTION MAY APPLY
FE FARE RESTRICTION MAY APPLY
FM *F*1.00A
FV *F*LH
>tqt/t2
TST00002                 IOVA/         LD 29AUG21

FXB
   2.NAGREBELNA/ZORIANA NAGREBELNYI/ANDRII       (ADT)
     KBP-MUC LH  2545H 10SEP 1525 OK  HEULGTX0             10SEP10SEP  0PC
     MUC-KBP LH  2546K 13SEP 1810 OK  KEULGTX4             13SEP13SEP  0PC
     KBP
FARE    USD    192.00
EQUIV   UAH    5156.00
TX001   UAH   1420.00YQ
TX002   UAH    158.00YQ
TX003   UAH    314.00DE
TX004   UAH    407.00OY
TX005   UAH    748.00RA
TX006   UAH    108.00UA
TX007   UAH     54.00UD
TX008   UAH    349.00YK
TOTAL   UAH    8714.00    BSR 26.854167
GRAND TOTAL UAH    17428.00
IEV LH MUC176.50 LH IEV15.00 NUC191.50END ROE1.000000
FE FARE RESTRICTION MAY APPLY
FE FARE RESTRICTION MAY APPLY
FM *F*1.00A
FV *F*LH

"
			);


			docs.AssertAll(a => a

				.IssueDate(DateTime.Today)
				.AirlinePrefixCode("220")
				.PnrCode("R7GTMX")

			);



			docs.Assert(

				a => a
					.PassengerName("NAGREBELNYI/BOGUMYR MSTR")
					.Number("9543992294")
				,

				a => a
					.PassengerName("NAGREBELNA/ZORIANA MRS")
					.Number("9543992292")
				,

				a => a
					.PassengerName("NAGREBELNYI/ANDRII MR")
					.Number("9543992293")

			);


		}



		//---g

	}






	//===g



}
