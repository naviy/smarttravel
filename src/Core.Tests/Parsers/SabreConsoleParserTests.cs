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

			);


			docs.Assert(

				a => a
					.PassengerName("1TKACH/OLEKSANDR MR")
				,

				a => a
					.PassengerName("1TKACH/NADIIA MISS")

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



		//---g

	}






	//===g



}
