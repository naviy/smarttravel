SELECT 
this_.issuedate as issuedate41_10_, this_.number_ as number7_41_10_, 
this_.id as id41_10_, this_.version as version41_10_, this_.createdby as createdby41_10_, this_.createdon as createdon41_10_
, this_.modifiedby as modifiedby41_10_, this_.modifiedon as modifiedon41_10_, this_.isvoid as isvoid41_10_, this_.customer as customer41_10_, 
this_.billto as billto41_10_, this_.billtoname as billtoname41_10_, this_.shipto as shipto41_10_, this_.discount_currency as discount14_41_10_, 
this_.discount_amount as discount15_41_10_, this_.vat_currency as vat16_41_10_, this_.vat_amount as vat17_41_10_, 
this_.useservicefeeonlyinvat as useserv18_41_10_, this_.total_currency as total19_41_10_, this_.total_amount as total20_41_10_, 
this_.paid_currency as paid21_41_10_, this_.paid_amount as paid22_41_10_, this_.totaldue_currency as totaldue23_41_10_, 
this_.totaldue_amount as totaldue24_41_10_, this_.ispaid as ispaid41_10_, this_.vatdue_currency as vatdue26_41_10_, 
this_.vatdue_amount as vatdue27_41_10_, this_.deliverybalance as deliver28_41_10_, this_.assignedto as assignedto41_10_, 
this_.bankaccount as bankacc30_41_10_, this_.owner as owner41_10_, this_.note as note41_10_, this_.ispublic as ispublic41_10_, 
this_.issubjectofpaymentscontrol as issubje34_41_10_, this_.invoicelastindex as invoice35_41_10_, this_.consignmentnumbers as consign36_41_10_, 
customer1_.id as id18_0_, customer1_.version as version18_0_, customer1_.createdby as createdby18_0_, customer1_.createdon as createdon18_0_, 
customer1_.modifiedby as modifiedby18_0_, customer1_.modifiedon as modifiedon18_0_, customer1_.type as type18_0_, 
customer1_.legalname as legalname18_0_, customer1_.code as code18_0_, customer1_.phone1 as phone11_18_0_, customer1_.phone2 as phone12_18_0_, 
customer1_.fax as fax18_0_, customer1_.email1 as email14_18_0_, customer1_.email2 as email15_18_0_, customer1_.webaddress as webaddress18_0_, 
customer1_.iscustomer as iscustomer18_0_, customer1_.issupplier as issupplier18_0_, customer1_.reportsto as reportsto18_0_, 
customer1_.defaultbankaccount as default20_18_0_, customer1_.details as details18_0_, customer1_.legaladdress as legalad22_18_0_, 
customer1_.actualaddress as actuala23_18_0_, customer1_.note as note18_0_, customer1_.invoicesuffix as invoice25_18_0_, customer1_.name as name18_0_, 
customer1_.organization as organiz27_18_0_, customer1_.isairline as isairline18_0_, customer1_.airlineiatacode as airline29_18_0_, 
customer1_.airlineprefixcode as airline30_18_0_, customer1_.airlinepassportrequirement as airline31_18_0_, 
customer1_.isaccommodationprovider as isaccom32_18_0_, customer1_.isbusticketprovider as isbusti33_18_0_, 
customer1_.iscarrentalprovider as iscarre34_18_0_, customer1_.ispasteboardprovider as ispaste35_18_0_, customer1_.istourprovider as istourp36_18_0_, 
customer1_.istransferprovider as istrans37_18_0_, customer1_.isgenericproductprovider as isgener38_18_0_, customer1_.isprovider as isprovider18_0_, 
customer1_.isinsurancecompany as isinsur40_18_0_, customer1_.isroamingoperator as isroami41_18_0_, customer1_.milescardsstring as milesca42_18_0_, 
customer1_.birthday as birthday18_0_, customer1_.title as title18_0_, customer1_.bonuscardnumber as bonusca45_18_0_, customer1_.class as class18_0_, 
billto2_.id as id18_1_, billto2_.version as version18_1_, billto2_.createdby as createdby18_1_, billto2_.createdon as createdon18_1_, 
billto2_.modifiedby as modifiedby18_1_, billto2_.modifiedon as modifiedon18_1_, billto2_.type as type18_1_, billto2_.legalname as legalname18_1_, 
billto2_.code as code18_1_, billto2_.phone1 as phone11_18_1_, billto2_.phone2 as phone12_18_1_, billto2_.fax as fax18_1_, 
billto2_.email1 as email14_18_1_, billto2_.email2 as email15_18_1_, billto2_.webaddress as webaddress18_1_, billto2_.iscustomer as iscustomer18_1_, 
billto2_.issupplier as issupplier18_1_, billto2_.reportsto as reportsto18_1_, billto2_.defaultbankaccount as default20_18_1_, 
billto2_.details as details18_1_, billto2_.legaladdress as legalad22_18_1_, billto2_.actualaddress as actuala23_18_1_, billto2_.note as note18_1_, 
billto2_.invoicesuffix as invoice25_18_1_, billto2_.name as name18_1_, billto2_.organization as organiz27_18_1_, billto2_.isairline as isairline18_1_, 
billto2_.airlineiatacode as airline29_18_1_, billto2_.airlineprefixcode as airline30_18_1_, billto2_.airlinepassportrequirement as airline31_18_1_, 
billto2_.isaccommodationprovider as isaccom32_18_1_, billto2_.isbusticketprovider as isbusti33_18_1_, billto2_.iscarrentalprovider as iscarre34_18_1_, 
billto2_.ispasteboardprovider as ispaste35_18_1_, billto2_.istourprovider as istourp36_18_1_, billto2_.istransferprovider as istrans37_18_1_, 
billto2_.isgenericproductprovider as isgener38_18_1_, billto2_.isprovider as isprovider18_1_, billto2_.isinsurancecompany as isinsur40_18_1_, 
billto2_.isroamingoperator as isroami41_18_1_, billto2_.milescardsstring as milesca42_18_1_, billto2_.birthday as birthday18_1_, 
billto2_.title as title18_1_, billto2_.bonuscardnumber as bonusca45_18_1_, billto2_.class as class18_1_, shipto3_.id as id18_2_, 
shipto3_.version as version18_2_, shipto3_.createdby as createdby18_2_, shipto3_.createdon as createdon18_2_, shipto3_.modifiedby as modifiedby18_2_, 
shipto3_.modifiedon as modifiedon18_2_, shipto3_.type as type18_2_, shipto3_.legalname as legalname18_2_, shipto3_.code as code18_2_, 
shipto3_.phone1 as phone11_18_2_, shipto3_.phone2 as phone12_18_2_, shipto3_.fax as fax18_2_, shipto3_.email1 as email14_18_2_, 
shipto3_.email2 as email15_18_2_, shipto3_.webaddress as webaddress18_2_, shipto3_.iscustomer as iscustomer18_2_, 
shipto3_.issupplier as issupplier18_2_, shipto3_.reportsto as reportsto18_2_, shipto3_.defaultbankaccount as default20_18_2_, 
shipto3_.details as details18_2_, shipto3_.legaladdress as legalad22_18_2_, shipto3_.actualaddress as actuala23_18_2_, shipto3_.note as note18_2_, 
shipto3_.invoicesuffix as invoice25_18_2_, shipto3_.name as name18_2_, shipto3_.organization as organiz27_18_2_, shipto3_.isairline as isairline18_2_, 
shipto3_.airlineiatacode as airline29_18_2_, shipto3_.airlineprefixcode as airline30_18_2_, shipto3_.airlinepassportrequirement as airline31_18_2_, 
shipto3_.isaccommodationprovider as isaccom32_18_2_, shipto3_.isbusticketprovider as isbusti33_18_2_, shipto3_.iscarrentalprovider as iscarre34_18_2_, 
shipto3_.ispasteboardprovider as ispaste35_18_2_, shipto3_.istourprovider as istourp36_18_2_, shipto3_.istransferprovider as istrans37_18_2_, 
shipto3_.isgenericproductprovider as isgener38_18_2_, shipto3_.isprovider as isprovider18_2_, shipto3_.isinsurancecompany as isinsur40_18_2_, 
shipto3_.isroamingoperator as isroami41_18_2_, shipto3_.milescardsstring as milesca42_18_2_, shipto3_.birthday as birthday18_2_, 
shipto3_.title as title18_2_, shipto3_.bonuscardnumber as bonusca45_18_2_, shipto3_.class as class18_2_, 
discount_4_.id as id11_3_, discount_4_.version as version11_3_, discount_4_.createdby as createdby11_3_, discount_4_.createdon as createdon11_3_, 
discount_4_.modifiedby as modifiedby11_3_, discount_4_.modifiedon as modifiedon11_3_, discount_4_.name as name11_3_, discount_4_.code as code11_3_, 
discount_4_.numericcode as numericc9_11_3_, discount_4_.cyrilliccode as cyrilli10_11_3_, 
vat_curre5_.id as id11_4_, vat_curre5_.version as version11_4_, vat_curre5_.createdby as createdby11_4_, vat_curre5_.createdon as createdon11_4_, 
vat_curre5_.modifiedby as modifiedby11_4_, vat_curre5_.modifiedon as modifiedon11_4_, vat_curre5_.name as name11_4_, vat_curre5_.code as code11_4_, 
vat_curre5_.numericcode as numericc9_11_4_, vat_curre5_.cyrilliccode as cyrilli10_11_4_, 
total_cur6_.id as id11_5_, total_cur6_.version as version11_5_, total_cur6_.createdby as createdby11_5_, total_cur6_.createdon as createdon11_5_, 
total_cur6_.modifiedby as modifiedby11_5_, total_cur6_.modifiedon as modifiedon11_5_, total_cur6_.name as name11_5_, total_cur6_.code as code11_5_, 
total_cur6_.numericcode as numericc9_11_5_, total_cur6_.cyrilliccode as cyrilli10_11_5_, 
paid_curr7_.id as id11_6_, paid_curr7_.version as version11_6_, paid_curr7_.createdby as createdby11_6_, paid_curr7_.createdon as createdon11_6_, paid_curr7_.modifiedby as modifiedby11_6_, 
 paid_curr7_.modifiedon as modifiedon11_6_, paid_curr7_.name as name11_6_, paid_curr7_.code as code11_6_, paid_curr7_.numericcode as numericc9_11_6_, 
 paid_curr7_.cyrilliccode as cyrilli10_11_6_, totaldue_8_.id as id11_7_, totaldue_8_.version as version11_7_, totaldue_8_.createdby as createdby11_7_, 
 totaldue_8_.createdon as createdon11_7_, totaldue_8_.modifiedby as modifiedby11_7_, 
 totaldue_8_.modifiedon as modifiedon11_7_,  totaldue_8_.name as name11_7_, totaldue_8_.code as code11_7_, totaldue_8_.numericcode as numericc9_11_7_, totaldue_8_.cyrilliccode as cyrilli10_11_7_, 
 assignedt9_.id as id18_8_, assignedt9_.version as version18_8_, assignedt9_.createdby as createdby18_8_, assignedt9_.createdon as createdon18_8_, 
 assignedt9_.modifiedby as modifiedby18_8_, assignedt9_.modifiedon as modifiedon18_8_, assignedt9_.type as type18_8_, 
 assignedt9_.legalname as legalname18_8_, assignedt9_.code as code18_8_, assignedt9_.phone1 as phone11_18_8_, assignedt9_.phone2 as phone12_18_8_, 
 assignedt9_.fax as fax18_8_, assignedt9_.email1 as email14_18_8_, assignedt9_.email2 as email15_18_8_, assignedt9_.webaddress as webaddress18_8_, 
 assignedt9_.iscustomer as iscustomer18_8_, assignedt9_.issupplier as issupplier18_8_, assignedt9_.reportsto as reportsto18_8_, 
 assignedt9_.defaultbankaccount as default20_18_8_, assignedt9_.details as details18_8_, assignedt9_.legaladdress as legalad22_18_8_, 
 assignedt9_.actualaddress as actuala23_18_8_, assignedt9_.note as note18_8_, assignedt9_.invoicesuffix as invoice25_18_8_, 
 assignedt9_.name as name18_8_, assignedt9_.milescardsstring as milesca42_18_8_, assignedt9_.birthday as birthday18_8_, 
 assignedt9_.organization as organiz27_18_8_, assignedt9_.title as title18_8_, assignedt9_.bonuscardnumber as bonusca45_18_8_, owner10_.id as id18_9_, 
 owner10_.version as version18_9_, owner10_.createdby as createdby18_9_, owner10_.createdon as createdon18_9_, owner10_.modifiedby as modifiedby18_9_, 
 owner10_.modifiedon as modifiedon18_9_, owner10_.type as type18_9_, owner10_.legalname as legalname18_9_, owner10_.code as code18_9_, 
 owner10_.phone1 as phone11_18_9_, owner10_.phone2 as phone12_18_9_, owner10_.fax as fax18_9_, owner10_.email1 as email14_18_9_, 
 owner10_.email2 as email15_18_9_, owner10_.webaddress as webaddress18_9_, owner10_.iscustomer as iscustomer18_9_, 
 owner10_.issupplier as issupplier18_9_, owner10_.reportsto as reportsto18_9_, owner10_.defaultbankaccount as default20_18_9_, 
 owner10_.details as details18_9_, owner10_.legaladdress as legalad22_18_9_, owner10_.actualaddress as actuala23_18_9_, owner10_.note as note18_9_, 
 owner10_.invoicesuffix as invoice25_18_9_, owner10_.name as name18_9_, owner10_.organization as organiz27_18_9_, owner10_.isairline as isairline18_9_,
  owner10_.airlineiatacode as airline29_18_9_, owner10_.airlineprefixcode as airline30_18_9_, owner10_.airlinepassportrequirement as airline31_18_9_, 
  owner10_.isaccommodationprovider as isaccom32_18_9_, owner10_.isbusticketprovider as isbusti33_18_9_, 
  owner10_.iscarrentalprovider as iscarre34_18_9_, owner10_.ispasteboardprovider as ispaste35_18_9_, owner10_.istourprovider as istourp36_18_9_, 
  owner10_.istransferprovider as istrans37_18_9_, owner10_.isgenericproductprovider as isgener38_18_9_, owner10_.isprovider as isprovider18_9_, 
  owner10_.isinsurancecompany as isinsur40_18_9_, owner10_.isroamingoperator as isroami41_18_9_, owner10_.milescardsstring as milesca42_18_9_, 
   owner10_.birthday as birthday18_9_, owner10_.title as title18_9_, owner10_.bonuscardnumber as bonusca45_18_9_, owner10_.class as class18_9_ 

  FROM lt_order this_ 
left outer join lt_party customer1_ on this_.customer=customer1_.id 
left outer join lt_party billto2_ on this_.billto=billto2_.id 
left outer join lt_party shipto3_ on this_.shipto=shipto3_.id 
left outer join lt_currency discount_4_ on this_.discount_currency=discount_4_.id 
left outer join lt_currency vat_curre5_ on this_.vat_currency=vat_curre5_.id 
left outer join lt_currency total_cur6_ on this_.total_currency=total_cur6_.id 
left outer join lt_currency paid_curr7_ on this_.paid_currency=paid_curr7_.id 
left outer join lt_currency totaldue_8_ on this_.totaldue_currency=totaldue_8_.id 
left outer join lt_party assignedt9_ on this_.assignedto=assignedt9_.id 
left outer join lt_party owner10_ on this_.owner=owner10_.id 

ORDER BY this_.issuedate desc, this_.number_ desc
--ORDER BY this_.issuedate desc, this_.createdon desc
--ORDER BY this_.issuedate desc, this_.number_ desc
--ORDER BY this_.number_ desc--, this_.issuedate desc 
--ORDER BY this_.issuedate desc 
limit 25 offset 50

/*

DROP INDEX "IX_lt_order_issuedate";
DROP INDEX "IX_lt_order_number_";
CREATE INDEX "IX_lt_order_issuedate" ON lt_order USING gin (issuedate);
CREATE INDEX "IX_lt_order_number_" ON lt_order USING gin (number_);


CREATE INDEX "IX_lt_order_number_" ON lt_order (lower(number_));

DROP INDEX "IX_lt_order_sort";
CREATE INDEX "IX_lt_order_sort" ON lt_order (issuedate, number_, owner, createdon);

ALTER TABLE ufsa.lt_order DROP CONSTRAINT lt_order_number__key;
ALTER TABLE ufsa.lt_order ADD CONSTRAINT lt_order_number__key UNIQUE(number_);


DROP INDEX "IX_lt_order_issuedate_number_";
CREATE INDEX "IX_lt_order_issuedate_number_" ON lt_order (issuedate, number_);

DROP INDEX "IX_lt_order_createdon";
CREATE INDEX "IX_lt_order_createdon" ON lt_order (createdon);


*/