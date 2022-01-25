using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;




namespace Luxena.Travel.Tests.Parsers
{



	//===g






	[TestFixture]
	public class SabreConsoleParserTests
	{

		//---g



		private List<AviaDocument> Parse(string content)
		{
			return SabreConsoleParser
				.Parse(content, new Currency("UAH"))
				.ToList()
			;
		}



		//---g



		[Test]
		public void TestParseTicket01()
		{

			var docs = Parse(@"
TZEZYU
     1.1TKACH/OLEKSANDR MR  2.1TKACH/NADIIA MISS*DOB24DEC16
     1 7W 146K 20OCT 3 HRKKBP HK2  0655  0815  /DC7W*JWV6E /E
     2 AF1753G 20OCT 3 KBPCDG HK2  1450  1710  /DCAF*SK2CC3 /E
     3 AF1752G 30OCT 6 CDGKBP HK2  0935  1345  /DCAF*SK2CC3 /E
    TKT/TIME LIMIT
      1.TAW/
    PHONES
      1.IEV380442067576-A
    PASSENGER EMAIL DATA EXISTS  *PE TO DISPLAY ALL
    PRICE QUOTE RECORD EXISTS - SYSTEM
    PROFILE,POLICY,AND/OR PREFERENCE INDEX DATA EXIST 
    *PI TO DISPLAY ALL
    AA FACTS
      1.SSR OTHS 1S AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY 7W BY 151
        1/29SEP/IEV LT
      2.SSR ADTK 1S TO AF BY 01OCT 1600 IEV OTHERWISE WILL BE XLD
    GENERAL FACTS
      1.SSR CHLD 7W HK1/24DEC16
      2.SSR CHLD AF HK1/24DEC16
    REMARKS
      1.Z¥ID-NF
    RECEIVED FROM - AR
    S1OI.S1OI*AOR 0711/28SEP21 TZEZYU H

*PQS«

            PRICE QUOTE RECORD - SUMMARY BY NAME NUMBER            
                      RETAINED FARE                                
    NAME    PQ TYPE TKT DES        M/S/A CREATED       TKT TTL     
     1.1  S  1  ADT                  S    28SEP UAH      2682      
     1.1  S  3  ADT                  S    28SEP UAH      9319      
     2.1  S  2  CNN CH25             S    28SEP UAH      2169      
     2.1  S  4  CNN CH20             S    28SEP UAH      8089


*PQ1«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 1  S1¥RQ                                                    
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD65.00        UAH1738         944XT                UAH2682ADT
    XT BREAKDOWN                                                   
              268YQ           134YR            41UA            14UD
              107YK           380HF                                
    ADT-01  KP2POW                                                 
    HRK 7W IEV65.00USD65.00END
    VALIDATING CARRIER - 7W                                        
    NONEND/REF REST/RBK REST                                       
    01 O HRK 7W 146K 20OCT  655  KP2POW          20OCT2120OCT21 23K
         KBP                                                       
    FARE SOURCE - ATPC                                             
    S1OI S1OI *AOR 1518/28SEP21                        PRICE-SYSTEM

*PQ2«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 2  S1¥RQ                                                    
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD49.00        UAH1310         859XT                UAH2169C05
    XT BREAKDOWN                                                   
              268YQ           134YR            41UA            14UD
              107YK           295HF                                
    C05-01  KP2POW/CH25                                            
    HRK 7W IEV48.75USD48.75END
    EACH C05 REQUIRES ACCOMPANYING SAME CABIN ADT                  
    VALIDATING CARRIER - 7W                                        
    NONEND/REF REST/RBK REST                                       
    01 O HRK 7W 146K 20OCT  655  KP2POW/CH25     20OCT2120OCT21 23K
         KBP                                                       
    FARE SOURCE - ATPC                                             
    S1OI S1OI *AOR 1518/28SEP21                        PRICE-SYSTEM

*PQ3«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 3  S2-3¥RQ                                                  
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD232.00       UAH6203         3116XT               UAH9319ADT
    XT BREAKDOWN                                                   
              814YQ           107UA            54UD           348YK
              629FR           928QX           142IZ            94O4
    ADT-01  GS50OILG                                               
    LAST DAY TO PURCHASE 11OCT/2359                                
    IEV AF PAR116.00AF IEV116.00NUC232.00END ROE1.00
    VALIDATING CARRIER - AF                                        
    CAT 15 SALES RESTRICTIONS FREE TEXT FOUND - VERIFY RULES       
    01 O KBP AF1753G 20OCT 1450  GS50OILG        20OCT2120OCT21 NIL
    02 O CDG AF1752G 30OCT  935  GS50OILG        30OCT2130OCT21 NIL
         KBP                                                       
    FARE SOURCE - ATPC                                             
    ONE OR MORE FORM OF PAYMENT FEES MAY APPLY                     
    ACTUAL TOTAL WILL BE BASED ON FORM OF PAYMENT USED             
    FEE CODE     DESCRIPTION                        FEE   TKT TOTAL
     OBFCAX    - CC NBR BEGINS WITH 542527            0        9319
     OBFCAX    - CC NBR BEGINS WITH 532728            0        9319
     OBFCAX    - CC NBR BEGINS WITH 516470            0        9319
     OBFCAX    - CC NBR BEGINS WITH 559867            0        9319
     OBFCAX    - CC NBR BEGINS WITH 1611              0        9319
     OBFCAX    - CC NBR BEGINS WITH 900024            0        9319
     OBFDAX    - CC NBR BEGINS WITH 542527            0        9319
     OBFDAX    - CC NBR BEGINS WITH 532728            0        9319
     OBFDAX    - CC NBR BEGINS WITH 516470            0        9319
     OBFDAX    - CC NBR BEGINS WITH 559867            0        9319
     OBFDAX    - CC NBR BEGINS WITH 1611              0        9319
     OBFDAX    - CC NBR BEGINS WITH 900024            0        9319
    S1OI S1OI *AOR 1518/28SEP21                        PRICE-SYSTEM

*PQ4«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 4  S2-3¥RQ                                                  
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD186.00       UAH4973         3116XT               UAH8089C05
    XT BREAKDOWN                                                   
              814YQ           107UA            54UD           348YK
              629FR           928QX           142IZ            94O4
    C05-01  GS50OILG/CH20                                          
    LAST DAY TO PURCHASE 11OCT/2359                                
    IEV AF PAR92.80AF IEV92.80NUC185.60END ROE1.00
    EACH C05 REQUIRES ACCOMPANYING SAME CABIN ADT                  
    VALIDATING CARRIER - AF                                        
    CAT 15 SALES RESTRICTIONS FREE TEXT FOUND - VERIFY RULES       
    01 O KBP AF1753G 20OCT 1450  GS50OILG/CH20   20OCT2120OCT21 NIL
    02 O CDG AF1752G 30OCT  935  GS50OILG/CH20   30OCT2130OCT21 NIL
         KBP                                                       
    FARE SOURCE - ATPC                                             
    ONE OR MORE FORM OF PAYMENT FEES MAY APPLY                     
    ACTUAL TOTAL WILL BE BASED ON FORM OF PAYMENT USED             
    FEE CODE     DESCRIPTION                        FEE   TKT TOTAL
     OBFCAX    - CC NBR BEGINS WITH 542527            0        8089
     OBFCAX    - CC NBR BEGINS WITH 532728            0        8089
     OBFCAX    - CC NBR BEGINS WITH 516470            0        8089
     OBFCAX    - CC NBR BEGINS WITH 559867            0        8089
     OBFCAX    - CC NBR BEGINS WITH 1611              0        8089
     OBFCAX    - CC NBR BEGINS WITH 900024            0        8089
     OBFDAX    - CC NBR BEGINS WITH 542527            0        8089
     OBFDAX    - CC NBR BEGINS WITH 532728            0        8089
     OBFDAX    - CC NBR BEGINS WITH 516470            0        8089
     OBFDAX    - CC NBR BEGINS WITH 559867            0        8089
     OBFDAX    - CC NBR BEGINS WITH 1611              0        8089
     OBFDAX    - CC NBR BEGINS WITH 900024            0        8089
    S1OI S1OI *AOR 1518/28SEP21                        PRICE-SYSTEM
"
			);



			docs.AssertAll(

				a => a
					.PnrCode("TZEZYU")
					.BookerOffice("S1OI")
					.BookerCode("OR")

			);



			docs.Assert(


				a => a

					.PassengerName("TKACH/OLEKSANDR MR")

					.Fare("USD", 65m)
					.EqualFare("UAH", 1738m)
					.FeesTotal("UAH", 944m)
					.Total("UAH", 2682m)

					.FlightSegments(

						seg => seg
							.Position(0)
							//.Stopover(true)
							.FromAirport("HRK")
							.ToAirport("KBP")
							.CarrierIataCode("7W")
							.FlightNumber("146")
							.ServiceClassCode("K")
							.FareBasis("KP2POW")
							.Luggage("23K")

					)

				,


				a => a

					.PassengerName("TKACH/OLEKSANDR MR")

					.Fare("USD", 232m)
					.EqualFare("UAH", 6203m)
					.FeesTotal("UAH", 3116m)
					.Total("UAH", 9319m)

					.FlightSegments(

						seg => seg
							.Position(0)
							.FromAirport("KBP")
							.ToAirport("CDG")
						,

						seg => seg
							.Position(1)
							.FromAirport("CDG")
							.ToAirport("KBP")

					)

				,


				a => a

					.PassengerName("TKACH/NADIIA MISS")

					.Fare("USD", 49m)
					.EqualFare("UAH", 1310m)
					.FeesTotal("UAH", 859m)
					.Total("UAH", 2169m)


					.FlightSegments(

						seg => seg
							.Position(0)
							//.Stopover(true)
							.FromAirport("HRK")
							.ToAirport("KBP")
							.CarrierIataCode("7W")
							.FlightNumber("146")
							.ServiceClassCode("K")
							.FareBasis("KP2POW/CH25")
							.Luggage("23K")

					)

				,


				a => a

					.PassengerName("TKACH/NADIIA MISS")

					.Fare("USD", 186m)
					.EqualFare("UAH", 4973m)
					.FeesTotal("UAH", 3116m)
					.Total("UAH", 8089m)

					.FlightSegments(

						seg => seg
							.Position(0)
							.FromAirport("KBP")
							.ToAirport("CDG")
						,

						seg => seg
							.Position(1)
							.FromAirport("CDG")
							.ToAirport("KBP")

					)


			);


		}



		[Test]
		public void TestParseTicket02()
		{

			var docs = Parse(@"
SPAXAV
 1.1TKACH/OLEKSANDR MR  2.1TKACH/MARYNA MRS
 3.I/1TKACH/POLINA MISS  4.1STOROZHUK/OLENA MRS*DOB20MAY15
 1 TK 458T 20FEB 7 KBPIST*HK3  0950  1250  /DCTK /E
 2 TK  77T 20FEB 7 ISTMIA*HK3  1520  2000  /DCTK /E
 3   ARNK
 4 TK   4T 27FEB 7 JFKIST*HK3  1310  0625   28FEB 1 /DCTK /E
 5 TK 457T 28FEB 1 ISTKBP*HK3  0740  0830  /DCTK /E
TKT/TIME LIMIT
  1.TAW/
PHONES
  1.IEV380442067576-A
PASSENGER EMAIL DATA EXISTS  *PE TO DISPLAY ALL
PRICE QUOTE RECORD EXISTS - SYSTEM
PROFILE,POLICY,AND/OR PREFERENCE INDEX DATA EXIST 
*PI TO DISPLAY ALL
AA FACTS
  1.SSR ADTK AA TO  TK BY 06DEC 1458 IRC-2/ADV OTO TKT 
  2.SSR OTHS AA   PLS ADV FQTV NUMBER IF AVAILABLE WITH SSR FOR
    MAT
  3.SSR OTHS AA   PLS ADV PSGR MOBILE AND/OR EMAIL AS SSR CTCM/
    CTCE
  4.SSR OTHS AA   DOCS CTCM CTCE ARE MANDATORY FOR TICKETING
  5.SSR OTHS AA   PLS ENTER DOCS CTCM AND CTCE FIELDS WITH SSR 
    FORMAT
  6.SSR OTHS AA   FOR DOCS FIELD DOB GENDER NAME FIELDS ARE MAN
    DATORY
  7.SSR ADPI AA KK3  ADV SECURE FLT PSGR DATA FOR ALL PSGRS
GENERAL FACTS
  1.SSR ADPI AA KK3  ADV SECURE FLT PSGR DATA FOR ALL PSGRS
REMARKS
  1.Z¥ID-NF
RECEIVED FROM - AR
S1OI.S1OI*AOR 0658/03DEC21 SPAXAV H

*PQS«
        PRICE QUOTE RECORD - SUMMARY BY NAME NUMBER            
                                                               
                  RETAINED FARE                                
NAME    PQ TYPE TKT DES        M/S/A CREATED       TKT TTL     
 1.1     1  ADT                  S    03DEC UAH     20509      
 2.1     1  ADT                  S    03DEC UAH     20509      
 3.1     3  INF                  S    03DEC UAH      2352      
 4.1     2  CNN                  S    03DEC UAH     18813


*PQ1«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 1  RQ                                                       
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD247.00       UAH6755         13754XT             UAH20509ADT
    XT BREAKDOWN                                                   
            11130YR           110UA            55UD           356YK
              312TR          1046US           167YC           192XY
              109XA           154AY           123XF                
    ADT-01  TLN2XPB                                                
    IEV TK X/IST TK MIA99.50/-NYC TK X/IST TK IEV147.50NUC247.00END
     ROE1.00 XFJFK4.5
    VALIDATING CARRIER - TK                                        
    CAT 15 SALES RESTRICTIONS FREE TEXT FOUND - VERIFY RULES       
    NONEND/TK ONLY                                                 
    01 O KBP TK 458T 20FEB  950  TLN2XPB         20FEB2220FEB22 02P
    02 X IST TK  77T 20FEB 1520  TLN2XPB         20FEB2220FEB22 02P
    03   MIA                     VOID                              
    04 O JFK TK   4T 27FEB 1310  TLN2XPB         27FEB2227FEB22 02P
    05 X IST TK 457T 28FEB  740  TLN2XPB         28FEB2228FEB22 02P
         KBP                                                       
    FARE SOURCE - ATPC                                             
    BAG ALLOWANCE     -KBPMIA-02P/TK/EACH PIECE UP TO 50 POUNDS/23 
    KILOGRAMS AND UP TO 62 LINEAR INCHES/158 LINEAR CENTIMETERS    
    BAG ALLOWANCE     -JFKKBP-02P/TK/EACH PIECE UP TO 50 POUNDS/23 
    KILOGRAMS AND UP TO 62 LINEAR INCHES/158 LINEAR CENTIMETERS    
    CARRY ON ALLOWANCE                                             
    KBPIST ISTMIA JFKIST ISTKBP-01P/TK                             
    01/UP TO 18 POUNDS/8 KILOGRAMS AND UP TO 45 LINEAR INCHES/115 L
    INEAR CENTIMETERS                                              
    CARRY ON CHARGES                                               
    KBPIST ISTMIA JFKIST ISTKBP-TK-CARRY ON FEES UNKNOWN-CONTACT CA
    RRIER                                                          
    ADDITIONAL ALLOWANCES AND/OR DISCOUNTS MAY APPLY DEPENDING ON  
    FLYER-SPECIFIC FACTORS /E.G. FREQUENT FLYER STATUS/MILITARY/   
    CREDIT CARD FORM OF PAYMENT/EARLY PURCHASE OVER INTERNET,ETC./ 
    S1OI S1OI *AOR 1501/03DEC21                        PRICE-SYSTEM

*PQ2«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 2  RQ                                                       
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD185.00       UAH5059         13754XT             UAH18813C06
    XT BREAKDOWN                                                   
            11130YR           110UA            55UD           356YK
              312TR          1046US           167YC           192XY
              109XA           154AY           123XF                
    C06-01  TLN2XPBCH                                              
    IEV TK X/IST TK MIA74.62/-NYC TK X/IST TK IEV110.62NUC185.24END
     ROE1.00 XFJFK4.5
    EACH C06 REQUIRES ACCOMPANYING SAME CABIN ADT                  
    VALIDATING CARRIER - TK                                        
    CAT 15 SALES RESTRICTIONS FREE TEXT FOUND - VERIFY RULES       
    NONEND/TK ONLY                                                 
    01 O KBP TK 458T 20FEB  950  TLN2XPBCH       20FEB2220FEB22 02P
    02 X IST TK  77T 20FEB 1520  TLN2XPBCH       20FEB2220FEB22 02P
    03   MIA                     VOID                              
    04 O JFK TK   4T 27FEB 1310  TLN2XPBCH       27FEB2227FEB22 02P
    05 X IST TK 457T 28FEB  740  TLN2XPBCH       28FEB2228FEB22 02P
         KBP                                                       
    FARE SOURCE - ATPC                                             
    BAG ALLOWANCE     -KBPMIA-02P/TK/EACH PIECE UP TO 50 POUNDS/23 
    KILOGRAMS AND UP TO 62 LINEAR INCHES/158 LINEAR CENTIMETERS    
    BAG ALLOWANCE     -JFKKBP-02P/TK/EACH PIECE UP TO 50 POUNDS/23 
    KILOGRAMS AND UP TO 62 LINEAR INCHES/158 LINEAR CENTIMETERS    
    CARRY ON ALLOWANCE                                             
    KBPIST ISTMIA JFKIST ISTKBP-01P/TK                             
    01/UP TO 18 POUNDS/8 KILOGRAMS AND UP TO 45 LINEAR INCHES/115 L
    INEAR CENTIMETERS                                              
    CARRY ON CHARGES                                               
    KBPIST ISTMIA JFKIST ISTKBP-TK-CARRY ON FEES UNKNOWN-CONTACT CA
    RRIER                                                          
    ADDITIONAL ALLOWANCES AND/OR DISCOUNTS MAY APPLY DEPENDING ON  
    FLYER-SPECIFIC FACTORS /E.G. FREQUENT FLYER STATUS/MILITARY/   
    CREDIT CARD FORM OF PAYMENT/EARLY PURCHASE OVER INTERNET,ETC./ 
    S1OI S1OI *AOR 1501/03DEC21                        PRICE-SYSTEM

*PQ3«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 3  RQ                                                       
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD25.00        UAH684          1668XT               UAH2352INF
    XT BREAKDOWN                                                   
             1046US           167YC           192XY           109XA
              154AY                                                
    INF-01  TLN2XPBIN                                              
    IEV TK X/IST TK MIA9.95/-NYC TK X/IST TK IEV14.75NUC24.70END RO
    E1.00
    REQUIRES ACCOMPANYING ADT PASSENGER                            
    EACH INF REQUIRES ACCOMPANYING ADT PASSENGER                   
    VALIDATING CARRIER - TK                                        
    CAT 15 SALES RESTRICTIONS FREE TEXT FOUND - VERIFY RULES       
    NONEND/TK ONLY                                                 
    01 O KBP TK 458T 20FEB  950  TLN2XPBIN       20FEB2220FEB22 01P
    02 X IST TK  77T 20FEB 1520  TLN2XPBIN       20FEB2220FEB22 01P
    03   MIA                     VOID                              
    04 O JFK TK   4T 27FEB 1310  TLN2XPBIN       27FEB2227FEB22 01P
    05 X IST TK 457T 28FEB  740  TLN2XPBIN       28FEB2228FEB22 01P
         KBP                                                       
    FARE SOURCE - ATPC                                             
    BAG ALLOWANCE     -KBPMIA-01P/TK/EACH PIECE UP TO 50 POUNDS/23 
    KILOGRAMS AND UP TO 45 LINEAR INCHES/115 LINEAR CENTIMETERS    
    2NDCHECKED BAG FEE-KBPMIA-UAH5333/TK/UP TO 50 POUNDS/23 KILOGRA
    MS AND UP TO 62 LINEAR INCHES/158 LINEAR CENTIMETERS           
    BAG ALLOWANCE     -JFKKBP-01P/TK/EACH PIECE UP TO 50 POUNDS/23 
    KILOGRAMS AND UP TO 45 LINEAR INCHES/115 LINEAR CENTIMETERS    
    2NDCHECKED BAG FEE-JFKKBP-UAH5333/TK/UP TO 50 POUNDS/23 KILOGRA
    MS AND UP TO 62 LINEAR INCHES/158 LINEAR CENTIMETERS           
    CARRY ON ALLOWANCE                                             
    KBPIST ISTMIA JFKIST ISTKBP-01P/TK                             
    01/UP TO 18 POUNDS/8 KILOGRAMS AND UP TO 45 LINEAR INCHES/115 L
    INEAR CENTIMETERS                                              
    CARRY ON CHARGES                                               
    KBPIST ISTMIA JFKIST ISTKBP-TK-CARRY ON FEES UNKNOWN-CONTACT CA
    RRIER                                                          
    ADDITIONAL ALLOWANCES AND/OR DISCOUNTS MAY APPLY DEPENDING ON  
    FLYER-SPECIFIC FACTORS /E.G. FREQUENT FLYER STATUS/MILITARY/   
    CREDIT CARD FORM OF PAYMENT/EARLY PURCHASE OVER INTERNET,ETC./ 
    S1OI S1OI *AOR 1501/03DEC21                        PRICE-SYSTEM
Copy as textPrint"
			);



			docs.AssertAll(a => a
				.PnrCode("SPAXAV")
				.BookerOffice("S1OI")
				.BookerCode("OI")
			);



			docs.Assert(


				a => a
					.PassengerName("TKACH/OLEKSANDR MR")
				,


				a => a
					.PassengerName("TKACH/MARYNA MRS")
				,


				a => a
					.PassengerName("TKACH/POLINA MISS")
				,


				a => a
					.PassengerName("STOROZHUK/OLENA MRS")

			);

		}



		[Test]
		public void TestParseTicket03()
		{

			var docs = Parse(@"
CMWLOH
     1.1YOSEF/GADIEL MR  2.1YOSEF/SHLOMO MR
     1 LY2651Z 23JAN 7 TLVKBP HK2  0630  0955  SPM HRS /DCLY*KMT82G
                                                                /E
     2 LY2652Z 25JAN 2 KBPTLV HK2  1100  1415  SPM HRS /DCLY*KMT82G
                                                                /E
    TKT/TIME LIMIT
      1.TAW/
    PHONES
      1.IEV380442067576-A
    PASSENGER EMAIL DATA EXISTS  *PE TO DISPLAY ALL
    PRICE QUOTE RECORD EXISTS - SYSTEM
    PROFILE,POLICY,AND/OR PREFERENCE INDEX DATA EXIST 
    *PI TO DISPLAY ALL
    AA FACTS
      1.SSR OTHS 1S MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SS
        R CTCR NON-CONSENT FOR LY
      2.SSR ADTK 1S TO LY BY 13JAN 2300 IEV TIME ZONE OTHERWISE WIL
        L BE XLD
      3.SSR KSML LY NO1 TLVKBP2651Z23JAN/ALL LY MEALS ARE KOSHER ME
        ALS
      4.SSR KSML LY NO1 KBPTLV2652Z25JAN/ALL LY MEALS ARE KOSHER ME
        ALS
      5.SSR KSML LY NO1 TLVKBP2651Z23JAN/ALL LY MEALS ARE KOSHER ME
        ALS
      6.SSR KSML LY NO1 KBPTLV2652Z25JAN/ALL LY MEALS ARE KOSHER ME
        ALS
    GENERAL FACTS
      1.SSR KSML LY NN1 TLVKBP2651Z23JAN
      2.SSR KSML LY NN1 KBPTLV2652Z25JAN
      3.SSR KSML LY NN1 TLVKBP2651Z23JAN
      4.SSR KSML LY NN1 KBPTLV2652Z25JAN
      5.SSR KSML LY NO1 TLVKBP2651Z23JAN/ALL LY MEALS ARE KOSHER ME
        ALS
      6.SSR KSML LY NO1 KBPTLV2652Z25JAN/ALL LY MEALS ARE KOSHER ME
        ALS
      7.SSR KSML LY NO1 TLVKBP2651Z23JAN/ALL LY MEALS ARE KOSHER ME
        ALS
      8.SSR KSML LY NO1 KBPTLV2652Z25JAN/ALL LY MEALS ARE KOSHER ME
        ALS
    REMARKS
      1.Z?ID-NF
    RECEIVED FROM - AR
    S1OI.S1OI*AOR 0657/06JAN22 CMWLOH H B

*PQS«

            PRICE QUOTE RECORD - SUMMARY BY NAME NUMBER            
                      RETAINED FARE                                
    NAME    PQ TYPE TKT DES        M/S/A CREATED       TKT TTL     
     1.1     1  ADT                  S    06JAN UAH     32554      
     2.1     1  ADT                  S    06JAN UAH     32554

*PQ1«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 1                                                           
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD1130.00      UAH30979        1575XT              UAH32554ADT
    XT BREAKDOWN                                                   
              110UA            55UD           357YK           833IL
              220AP                                                
    ADT-01  ZFIPEU                                                 
    LAST DAY TO PURCHASE 13JAN/2359                                
    TLV LY IEV565.00LY TLV565.00NUC1130.00END ROE1.00
    VALIDATING CARRIER - LY                                        
    FARE RESTRICTIONS APPLY                                        
    01 O TLV LY2651Z 23JAN  630  ZFIPEU          23JAN2223JAN22 02P
    02 O KBP LY2652Z 25JAN 1100  ZFIPEU          25JAN2225JAN22 02P
         TLV                                                       
    FARE SOURCE - ATPC                                             
    S1OI S1OI *AOR 1458/06JAN22                        PRICE-SYSTEM
Copy as textPrint
Copy as textPrint"
			);



			docs.AssertAll(a => a
				.IssueDate("2022-01-06")
				.PnrCode("CMWLOH")
				.BookerOffice("S1OI")
				.BookerCode("OR")
			);



			docs.Assert(

				a => a
					.PassengerName("YOSEF/GADIEL MR")


				, a => a
					.PassengerName("YOSEF/SHLOMO MR")

			);

		}




		[Test]
		public void TestParseTicket04()
		{

			var docs = Parse(@"
QLIKEN
     1.1CILA/OLENA MRS
     1 LH1495T 16FEB 3 KBPFRA*HK1  1815  2005  /DCLH*S35GTH /E
     2 LH 506T 16FEB 3 FRAGRU*HK1  2155  0555   17FEB 4
                                                   /DCLH*S35GTH /E
     3 OB 735X 17FEB 4 GRUVVI HK1  1340  1530  /DCOB*S35GTH /E
     4 OB 736X 01MAR 2 VVIGRU HK1  0830  1210  /DCOB*S35GTH /E
     5 LH 507T 01MAR 2 GRUFRA*HK1  1845  1015   02MAR 3
                                                   /DCLH*S35GTH /E
     6 LH1492T 02MAR 3 FRAKBP*HK1  1710  2035  /DCLH*S35GTH /E
    TKT/TIME LIMIT
      1.T-19JAN-S1OI*AOR
    PHONES
      1.IEV380442067576-A
    PASSENGER EMAIL DATA EXISTS  *PE TO DISPLAY ALL
    INVOICED 
    PRICE QUOTE RECORD EXISTS - SYSTEM
    PROFILE,POLICY,AND/OR PREFERENCE INDEX DATA EXIST 
    *PI TO DISPLAY ALL
    SECURITY INFO EXISTS *P3D OR *P4D TO DISPLAY
    AA FACTS
      1.SSR OTHS 1S MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SS
        R CTCR NON-CONSENT FOR LH
      2.SSR OTHS 1S MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SS
        R CTCR NON-CONSENT FOR OB
      3.SSR ADTK 1S TO OB BY 21JAN 2300 IEV TIME ZONE OTHERWISE WIL
        L BE XLD
      4.SSR OTHS 1S PLS ADV TKT NBR BY 21JAN22/0833Z OR LH OPTG/MKT
        G FLTS WILL BE CANX / APPLIC FARE RULE APPLIES IF IT DEMAND
        S EARLIER TKTG
    GENERAL FACTS
      3.SSR CTCM LH HK1/380678137416
      4.SSR CTCM OB HK1/380678137416
    REMARKS
      1.Z?ID-NF
      2.XXTAW/
     RECEIVED FROM - AR
    S1OI.S1OI*AOR 0233/18JAN22 QLIKEN H

*PQS«

            PRICE QUOTE RECORD - SUMMARY BY NAME NUMBER            
                      RETAINED FARE                                
    NAME    PQ TYPE TKT DES        M/S/A CREATED       TKT TTL     
     1.1     1  ADT                  S    19JAN UAH     33598      
    DELETED RECORD EXISTS - *PQD

*PQ1«

                     PRICE QUOTE RECORD - DETAILS                  
    FARE NOT GUARANTEED UNTIL TICKETED                             
    PQ 1                                                           
    BASE FARE       EQUIV AMT     TAXES/FEES/CHARGES          TOTAL
    USD592.00       UAH16745        16853XT             UAH33598ADT
    XT BREAKDOWN                                                   
            12912YQ           614YR           114UA            57UD
              368YK           644DE          1436RA           708A7
    ADT-01  TNCUA                                                  
    LAST DAY TO PURCHASE 21JAN/1033                                
    IEV LH X/FRA LH X/SAO OB SRZ296.00OB X/SAO LH X/FRA LH IEV296.0
    0NUC592.00END ROE1.00
    VALIDATING CARRIER - LH                                        
    FARE RESTRICTION MAY APPLY                                     
    01 O KBP LH1495T 16FEB 1815  TNCUA                  16FEB23 01P
    02 X FRA LH 506T 16FEB 2155  TNCUA                  16FEB23 01P
    03 X GRU OB 735X 17FEB 1340  TNCUA                  16FEB23 01P
    04 O VVI OB 736X 01MAR  830  TNCUA           23FEB2216FEB23 01P
    05 X GRU LH 507T 01MAR 1845  TNCUA           23FEB2216FEB23 01P
    06 X FRA LH1492T 02MAR 1710  TNCUA           23FEB2216FEB23 01P
         KBP                                                       
    FARE SOURCE - ATPC                                             
    ONE OR MORE FORM OF PAYMENT FEES MAY APPLY                     
    ACTUAL TOTAL WILL BE BASED ON FORM OF PAYMENT USED             
    FEE CODE     DESCRIPTION                        FEE   TKT TOTAL
     OBFCAX    - CC NBR BEGINS WITH 122088            0       33598
     OBFCAX    - CC NBR BEGINS WITH 14121             0       33598
     OBFCAX    - CC NBR BEGINS WITH 14122             0       33598
     OBFCAX    - CC NBR BEGINS WITH 14123             0       33598
     OBFCAX    - CC NBR BEGINS WITH 1611              0       33598
     OBFCAX    - CC NBR BEGINS WITH 1620              0       33598
     OBFCAX    - CC NBR BEGINS WITH 192088            0       33598
     OBFCAX    - CC NBR BEGINS WITH 516470            0       33598
     OBFCAX    - CC NBR BEGINS WITH 528159            0       33598
     OBFCAX    - CC NBR BEGINS WITH 532728            0       33598
     OBFCAX    - CC NBR BEGINS WITH 542527            0       33598
     OBFCAX    - CC NBR BEGINS WITH 559867            0       33598
     OBFCAX    - CC NBR BEGINS WITH 900024            0       33598
     OBFCAX    - CC NBR BEGINS WITH 34                0       33598
     OBFCAX    - CC NBR BEGINS WITH 37                0       33598
     OBFCAX    - CC NBR BEGINS WITH 1112              0       33598
     OBFDAX    - CC NBR BEGINS WITH 122088            0       33598
     OBFDAX    - CC NBR BEGINS WITH 14121             0       33598
     OBFDAX    - CC NBR BEGINS WITH 14122             0       33598
     OBFDAX    - CC NBR BEGINS WITH 14123             0       33598
     OBFDAX    - CC NBR BEGINS WITH 1611              0       33598
     OBFDAX    - CC NBR BEGINS WITH 1620              0       33598
     OBFDAX    - CC NBR BEGINS WITH 192088            0       33598
     OBFDAX    - CC NBR BEGINS WITH 516470            0       33598
     OBFDAX    - CC NBR BEGINS WITH 528159            0       33598
     OBFDAX    - CC NBR BEGINS WITH 532728            0       33598
     OBFDAX    - CC NBR BEGINS WITH 542527            0       33598
     OBFDAX    - CC NBR BEGINS WITH 559867            0       33598
     OBFDAX    - CC NBR BEGINS WITH 900024            0       33598
     OBFDAX    - CC NBR BEGINS WITH 34                0       33598
     OBFDAX    - CC NBR BEGINS WITH 37                0       33598
     OBFDAX    - CC NBR BEGINS WITH 1112              0       33598
    S1OI S1OI *AOR 1331/19JAN22                        PRICE-SYSTEM"
            );



			docs.AssertAll(a => a
				.IssueDate("2022-01-19")
				.PnrCode("QLIKEN")
				.BookerOffice("S1OI")
				.BookerCode("OR")
			);



			docs.Assert(

				a => a
					.PassengerName("CILA/OLENA MRS")

					.Fare("USD", 592m)
					.EqualFare("UAH", 16745m)
					.FeesTotal("UAH", 16853m)
					.Total("UAH", 33598m)

					.FlightSegments(

						seg => seg
							.Position(0)
							.FromAirport("KBP")
							.ToAirport("FRA")
							.CarrierIataCode("LH")
							.DepartureTime("2022-02-16T18:15")
							.ArrivalTime("2022-02-16T20:05")
						,

                        seg => seg
							.Position(1)
							.FromAirport("FRA")
							.ToAirport("GRU")
							.CarrierIataCode("LH")
							.DepartureTime("2022-02-16T21:55")
							.ArrivalTime("2022-02-17T05:55")
						,

                        seg => seg
							.Position(2)
							.FromAirport("GRU")
							.ToAirport("VVI")
							.CarrierIataCode("OB")
							.DepartureTime("2022-02-17T13:40")
							.ArrivalTime("2022-02-17T15:30")
						,

                        seg => seg
							.Position(3)
							.FromAirport("VVI")
							.ToAirport("GRU")
							.CarrierIataCode("OB")
						,

						seg => seg
							.Position(4)
							.FromAirport("GRU")
							.ToAirport("FRA")
							.CarrierIataCode("LH")
						,

						seg => seg
							.Position(5)
							.FromAirport("FRA")
							.ToAirport("KBP")
							.CarrierIataCode("LH")

                    )

            );

		}



		//---g

	}






	//===g



}
