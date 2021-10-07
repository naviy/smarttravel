-- select concat('when ''', id, ''' then ''', code, '''') from lt_currency order by 1

--begin work;

create or replace function currencyCodeById(varchar(32)) returns varchar(32)
as $$
   select case $1
        when '004440fd14e24adc9cad2caa49ac2402' then 'NPR'
        when '0195c99c32b743d69c6fed8f9691a565' then 'NAD'
        when '026ddda5735848b3974b287b1fce66b0' then 'SIT'
        when '02b39ad1e40148c387f83fbd483fa281' then 'CHE'
        when '03b88c53f0584e3788bfda5aae756cc8' then 'PHP'
        when '05b3416cb9054f49be60a469e0218628' then 'MXN'
        when '070e2b0a9bd7434f882fe4731b2fe585' then 'TRY'
        when '0a39e628264b470daed5632c807e3f01' then 'ZAR'
        when '0a6a3abf42f54c119c46a2a84196e075' then 'GBP'
        when '0c8a09dc8e5949e483b9e622362b908c' then 'MOP'
        when '0d3ca8a1fbe6475da6833259b9679bd5' then 'HUF'
        when '0d43861b5f6a4e50bebd5e5ea2e17a55' then 'CLF'
        when '0e2a178dda8b4f9595a352dc414b0acd' then 'FJD'
        when '0e5c20a5a015486dbc86aad14ba48cc1' then 'KYD'
        when '0e6129464f26452eb242a00cc819c97c' then 'AWG'
        when '0f06b2d6cf3b42a99715e1d5efa81a36' then 'SYP'
        when '0f93b1741413471b85e0c1f3d4b6f598' then 'TTD'
        when '0fb4d7f3008e43fda58e3b7a28a476d5' then 'STD'
        when '10922e428d67412e9ec77ac3a8abc92e' then 'ERN'
        when '121b177046a54083b5a90694b05020c4' then 'MZM'
        when '136721ef7f6e437480975ec182930206' then 'JMD'
        when '13d345ee89ff4cb8aec6b85fc37fc61d' then 'ARS'
        when '17734151d97149dfb589046a307fb1e5' then 'TMT'
        when '1788f2e318a0401a9928a3efee6d0808' then 'CHW'
        when '19cfd575f27e4a45b6eb1dbb67c8826b' then 'GTQ'
        when '1a878798913d4cd3a0a1a9677b2ec6df' then 'VUV'
        when '1b35f684a4d04e2aa2c5e1730e117037' then 'CVE'
        when '1e63ca9094e140adace1eeb05996496c' then 'GHS'
        when '1e7eaa3b03b04a7fa28c3fa6c02f907a' then 'XAF'
        when '251c27402ae04fac886d5bcdbe5b796c' then 'RON'
        when '2692cc37551942d385ff67544867eabe' then 'NUC'
        when 'd010c85ac1b34fafb33147b4b7d6dfdf' then 'NUC'
        when 'ddcbc527686944e992d8b800be6876f9' then 'NUC'
        when '29849f25d99541c0a0e3abbddc14f7bb' then 'FKP'
        when '29a66d4ce5494620a4e0beb448ff25ca' then 'UZS'
        when '2a0ea54fddbe403d8add8b957e9671f2' then 'MUR'
        when '2e86d90f9ae243bbb0cba3c79ae72f7c' then 'EUR'
        when '2ede8accfc3b4f72bec44244bf17d364' then 'LSL'
        when '2f85317b4285415dbddbc5f8cabd7385' then 'AUH'
        when '2f94ecd3ed2a414baaf9910e45eeba2e' then 'RUB'
        when '31a091ca29374f23bf7b8accea1efbcd' then 'ILS'
        when '328a3adf0cf14df1aafa50b4b697b248' then 'IUA'
        when '342491db15054bb7886af95c9abff390' then 'GEL'
        when '36172337d74044a3afdc5c8811dd68f0' then 'SBD'
        when '362e6f2dceeb41c3951ca8ae552efb54' then 'PEN'
        when '3883e41a68dc443c844353e78625ea52' then 'SZL'
        when '395ac4ef5a8a4231b70cd2b85eb9cb72' then 'MXV'
        when '3adcac83cd2b4063929108e4971db320' then 'DOP'
        when '3d3d38736fb4410693f26c6f7b4e5bba' then 'SCR'
        when '3d66d277e15c495c9f45a9411bd02a63' then 'MAD'
        when '3e1d67ff44e44b1584a3f24578d0d130' then 'GIP'
        when '3fcefaad4e824454a2fd38e969b4ec29' then 'MYR'
        when '409ad043aeb748079138505f6d9ef52b' then 'IQD'
        when '411db11c99b34c1daff79623ccee8fc3' then 'LBP'
        when '453f5f4973984eabb858620d2709c054' then 'LVL'
        when '454e3ec4f9074de79fbd6d4ed9a84a2b' then 'MGA'
        when '4704490ee131462db0ab3f8fa048f385' then 'BWP'
        when '48111242305a4556aa9a10d297998167' then 'NIO'
        when '488e9be47aad4092bebe960a87e3060a' then 'JOD'
        when '49e2c21cc7734974a769b959e54318d9' then 'CNY'
        when '49e7924dbbbb48daaa1e133113e47a1e' then 'MZN'
        when '4bb4fc3c4d414299ac2c7cabf262b71b' then 'CAD'
        when '4c83a02dee4148a18b5f17a92c5dcd82' then 'SOS'
        when '4c9fe5e73c7d450fb74c80099e89dd5a' then 'SRD'
        when '4df05d1c2ce6437b8661f0471ded9da8' then 'CRC'
        when '4ef41b17afce4162aff5fe12b6088538' then 'BOV'
        when '52f83089cb514a6caa63aaf096eb39d4' then 'HKD'
        when '53203def24f842c78aa5dd7c85c02426' then 'XPD'
        when '53bc8686012c44c1a1d3b02f95228a0a' then 'XDR'
        when '575ee37600e748ed96782f8287aa319f' then 'MWK'
        when '5959a4b786254f39b529847dfb54d0fb' then 'BBD'
        when '5a40b1ee61c04690b39c3b002705bc78' then 'AMD'
        when '5b92accf7e2746a8a1dd5461b9381aff' then 'AFN'
        when '5c17467782fb45658dc98596df4c36f8' then 'XBB'
        when '5c183385557045d99fc0c3750f82a83b' then 'DZD'
        when '5d77b6c8cefd4927852e0cccf8d88c63' then 'PLN'
        when '5ef28ab9626c44a4848811ba492e65e9' then 'BMD'
        when '60643b2441ea4ddd84b800e850c24ef9' then 'VND'
        when '606f3a58566c46828ae28df221936d0c' then 'JPY'
        when '61de0e8a27cc4e8dba22c9132e4bb455' then 'DKK'
        when '651773db40f149539a41aa0c92acf804' then 'SEK'
        when '67984c94a3c8406aa8561c87e7651bb2' then 'SVC'
        when '68139f13ca3b4745a2f816e3f269991b' then 'AOA'
        when '6bbfc3855d114105a4c6cf1d8af4cfd2' then 'BIF'
        when '6d7f798474ba4f1985186af11594cf7c' then 'SHP'
        when '6dcf3ee7fc6b42c9ac2ec7de4e7515c2' then 'HTG'
        when '708437c01eaf47519969b8d40b640690' then 'USN'
        when '71186c33a31349fb8142f507662c90ce' then 'BHD'
        when '740c5e5fe70d47d8852252b2842a21a3' then 'MKD'
        when '7449184c05cc42fd8cbf885473f121de' then 'NZD'
        when '7596207357114c8280e1d5a24e9d4bc3' then 'PYG'
        when '7b02f75731c9472d94237acff9c3079f' then 'GMD'
        when '7ba62d4e3b0547f693281686647e33cb' then 'BOB'
        when '7cae96a9f6a846e487e63b04ad022964' then 'ANG'
        when '7d5dc90fb5a54255a47f184b57581e35' then 'LRD'
        when '7ea1aad5d2834aea98d471cd4acc2ee4' then 'INR'
        when '7f0c80ddd4ae4fd4b5699daa5f6d7749' then 'EGP'
        when '82e2e194d06e48c38c6f0c654b471786' then 'XPF'
        when '83a42d146f5a44b2817f7c384d6bc129' then 'USD'
        when '84ab278688bd4601b047a8af4a270323' then 'GHC'
        when '8dd76f2813f4463597cdd7029213e081' then 'CLP'
        when '8f86f85496e2479ea382dabc70da53a0' then 'BRL'
        when '90f61b276ddc44a68105c76089078ff8' then 'HNL'
        when '925849616d884292a66a930c23728602' then 'BSD'
        when '939a3797c4244a259169252c133beff7' then 'SKK'
        when '9480c5ead9bf4ed8b9d3b765f6edbd41' then 'COP'
        when '9543d88d884949c0ba6e27b660e4e4c6' then 'CUP'
        when '978e3ceb15fe4c028e36841a79d33d1e' then 'ISK'
        when '98181e0f32494827b9e85d92af5eae48' then 'HRK'
        when '98bbb24b5aa441fa9809842018be52a4' then 'THB'
        when '9a9f8cc464c34a048b6bce3677071722' then 'LTL'
        when '9babd5384dec481083090da9bea1b383' then 'PGK'
        when 'a26114a2cde84f029ddd5001323d88c9' then 'KWD'
        when 'a3c8595663d54a7088f3410b4eb06e54' then 'QAR'
        when 'a9f24e9029224f6a820427e04cab8fae' then 'USS'
        when 'aad5f1f8893d4a5ca96aa8d136bb807b' then 'ZWD'
        when 'ab01a5783ba94890a45d70081b3b39ff' then 'BDT'
        when 'af011fb824ac4d58bc69e9fcec60802c' then 'TZS'
        when 'af496e4b4de647afa7b61eacfd1a6f4f' then 'UAH'
        when 'b167476f5e3f478da906f8e86c4bcd90' then 'BAM'
        when 'b359d55553c74dc38d8d8f264cac57a6' then 'ROL'
        when 'b438f561a12646368232565be0528469' then 'ETB'
        when 'b51a704eadaf42e29f51e27611c47a5f' then 'BND'
        when 'b586da2243174d8a8fa1674ff98e4887' then 'XBD'
        when 'b58cb8c115a44a829dc0d87edafd5d0e' then 'XCD'
        when 'b8be91dbaa844a9eb451dd56cc90aebf' then 'TND'
        when 'b92b04d28454478dbc6d6984d2725136' then 'UYI'
        when 'bab7187ca602474eb1ffd5b598ad3342' then 'BTN'
        when 'bb1539b43b3d4c76abec8d8d95523066' then 'MTL'
        when 'bb18fbb9ea7b4c30a77880402427f2b7' then 'SAR'
        when 'bbeb9dea6cb84839a303de2111fdf53a' then 'XTS'
        when 'bca303dbda474ff8bb41d15394d3f61d' then 'NOK'
        when 'bdc46af8625943b68c3bcf9a0619587f' then 'VEF'
        when 'c1a06adb4e4f4de8b522eb14a4f82c52' then 'NGN'
        when 'c207ceb5fdc6440c81f47b16507e97d8' then 'CHF'
        when 'c3195b776d614f5bb3356277c348a47b' then 'IRR'
        when 'c6188a8e93934b35a3ea1c59da27144a' then 'XAU'
        when 'c67c70aff8484592a2a97f723adbadc4' then 'GWP'
        when 'c67df08a75704a398ebd8dda7cc9c4c7' then 'XBA'
        when 'c790df28b5da45e588caed8e2263500f' then 'ZWL'
        when 'ca9d5e359fdd4ee49c8336265d0fff19' then 'PAB'
        when 'cbd5e98e1f3b4a9eb590ee21d584526b' then 'RWF'
        when 'cc30b24c610c40ad9b1117e295f2eb34' then 'GYD'
        when 'cd24e8aa65cd40ac9b0e3ecf2c82255d' then 'KZT'
        when 'cf40ec4e75e346f799d42f984a649912' then 'SDG'
        when 'cf49fff95e7c4ca2a59376266c6e5cb7' then 'OMR'
        when 'cf55966ecc5d49cb879f33bc73a8f456' then 'KMF'
        when 'cf98ebd514db48bba12dfc0daa1780c7' then 'KHR'
        when 'd399295ab8224f8baa65e93ec8dbaeea' then 'LYD'
        when 'd3ecc69a32bf4dc88cece855d2496713' then 'MNT'
        when 'd47cd03777bf40ed9416006c0aba62c3' then 'BGN'
        when 'd71b85c552ee485fac6dbd259e79f81c' then 'PKR'
        when 'd86ee11ac2fd4ae99529afa4c813e68f' then 'XAG'
        when 'd90bf8482d38466da7d2a4d6bc403297' then 'SGD'
        when 'dbf931fce18643608a39ce4d6e42c4b9' then 'LKR'
        when 'dca4f7cf0ed6432ea6d114d0027e72ad' then 'AUD'
        when 'dcbf0c2ef443431c86bad620ebdbae2a' then 'MVR'
        when 'df025af74b394b43ab3dea8eea045648' then 'BYR'
        when 'df0bf4a5a7494b50af36d44162989f4e' then 'EEK'
        when 'df97c5ee8f1c416bb2f694070456c93c' then 'TOP'
        when 'e0aa812f698d42d5a9a801ac9a88700b' then 'MMK'
        when 'e11a45ced9e246c79c36d2f21917590b' then 'AZN'
        when 'e2141a7bba4d42e7975d33786dd28983' then 'AED'
        when 'e2e4bccf068849e2ae0842d5aa60ae01' then 'KRW'
        when 'e6eeb733372f40ee8006302bba8bab7a' then 'CZK'
        when 'e73ec8a5be164733b769022b0d647d2c' then 'CUC'
        when 'e78e240368004137b2fd5380b68f9d11' then 'BZD'
        when 'e81587a11c744172b26911e18c832b5b' then 'UYU'
        when 'e8ddefbc49df4d11bdce59db9a626853' then 'KGS'
        when 'eb48b56a661941268447842e349e4bd2' then 'TJS'
        when 'ebc9246b3c0f43648dc8f71835799fe2' then 'KES'
        when 'ee00d9eb88ca4996a5fad6ece4d0eaa9' then 'COU'
        when 'ef896fc80aad4061952fe8b4a7d7043e' then 'IDR'
        when 'ef9ecb7f0ae54230bd633657e82e2d32' then 'DJF'
        when 'efc3ae417f4e4d01ba348db0813c6272' then 'ZMK'
        when 'f298fea05b7c459f8c726ea3361c6630' then 'MDL'
        when 'f2c31ad45b0f4458ab669f77895c25a9' then 'WST'
        when 'f2e6b217d587436b930663fba39905b6' then 'MRO'
        when 'f3743ec2ea72421083a345c74003d32d' then 'UGX'
        when 'f38dc1211e694f9397fe5ea750e79f8e' then 'SLL'
        when 'f43a0844e3a243cc870c6686da8c579d' then 'GNF'
        when 'f55ff94884cd45f5ad3a66f23c1023d7' then 'YER'
        when 'f5f17a31dc194c08bd7c767d2a291fa9' then 'XOF'
        when 'f6375954a16347ae9cdd000f95cfda63' then 'LAK'
        when 'f7ec21d3ff594393aaec6f5b560f8cf1' then 'XXX'
        when 'f86d2ab781a54931b56d46e1ddc774d8' then 'CDF'
        when 'fb214dc1979a49c8bee43d8c72f3d881' then 'XBC'
        when 'fc4b05fa2f034a7a976c1f317956d15b' then 'XPT'
        when 'fc5417a6b2e747e28fee81813a20647f' then 'RSD'
        when 'fe2675ccdccd4c1a982fbd1d9ec52f6a' then 'KPW'
        when 'ff53ceb8297b48689fc9daca8023216e' then 'TWD'
        when 'ffcd99b1c38c4054a9b2c155c964c652' then 'ALL'
        else $1
    end;
$$
language sql;

/*
-- RUN CHECK !!!

select * from lt_currency
 where currencyCodeById(id) = id;

*/



alter table lt_invoice drop constraint "FK_lt_invoice_vat_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_bonusaccumulation_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_bonusdiscount_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_faretotal_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_refundservicefee_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_servicefeepenalty_currency_lt_currency_id";
alter table lt_avia_document_fee drop constraint "aviadocumentfee_amount_currency_fk";
alter table lt_consignment drop constraint "consignment_discount_currency_fk";
alter table lt_consignment drop constraint "consignment_grandtotal_currency_fk";
alter table lt_consignment drop constraint "consignment_vat_currency_fk";
alter table lt_flight_segment drop constraint "flightsegment_amount_currency_fk";
alter table lt_invoice drop constraint "invoice_total_currency_fk";
alter table lt_order drop constraint "order_discount_currency_fk";
alter table lt_order drop constraint "order_paid_currency_fk";
alter table lt_order drop constraint "order_total_currency_fk";
alter table lt_order drop constraint "order_totaldue_currency_fk";
alter table lt_order drop constraint "order_vat_currency_fk";
alter table lt_order drop constraint "order_vatdue_currency_fk";
alter table lt_order_item drop constraint "orderitem_discount_currency_fk";
alter table lt_order_item drop constraint "orderitem_givenvat_currency_fk";
alter table lt_order_item drop constraint "orderitem_grandtotal_currency_fk";
alter table lt_order_item drop constraint "orderitem_price_currency_fk";
alter table lt_order_item drop constraint "orderitem_taxedtotal_currency_fk";
alter table lt_payment drop constraint "payment_amount_currency_fk";
alter table lt_payment drop constraint "payment_vat_currency_fk";
alter table lt_product drop constraint "product_cancelcommission_currency_fk";
alter table lt_product drop constraint "product_cancelfee_currency_fk";
alter table lt_product drop constraint "product_commission_currency_fk";
alter table lt_product drop constraint "product_commissiondiscount_currency_fk";
alter table lt_product drop constraint "product_discount_currency_fk";
alter table lt_product drop constraint "product_equalfare_currency_fk";
alter table lt_product drop constraint "product_fare_currency_fk";
alter table lt_product drop constraint "product_feestotal_currency_fk";
alter table lt_product drop constraint "product_grandtotal_currency_fk";
alter table lt_product drop constraint "product_handling_currency_fk";
alter table lt_product drop constraint "product_servicefee_currency_fk";
alter table lt_product drop constraint "product_total_currency_fk";
alter table lt_product drop constraint "product_vat_currency_fk";
alter table lt_system_configuration drop constraint "systemconfiguration_defaultcurrency_fkey";



update lt_avia_document_fee set 
    amount_currency = currencyCodeById(amount_currency);
    
update lt_consignment set 
    grandtotal_currency = currencyCodeById(grandtotal_currency),
    vat_currency = currencyCodeById(vat_currency),
    discount_currency = currencyCodeById(discount_currency);
    
update lt_flight_segment set 
    amount_currency = currencyCodeById(amount_currency);
    
update lt_invoice set 
    vat_currency = currencyCodeById(vat_currency),
    total_currency = currencyCodeById(total_currency);
    
update lt_order set 
    discount_currency = currencyCodeById(discount_currency),
    vatdue_currency = currencyCodeById(vatdue_currency),
    vat_currency = currencyCodeById(vat_currency),
    totaldue_currency = currencyCodeById(totaldue_currency),
    total_currency = currencyCodeById(total_currency),
    paid_currency = currencyCodeById(paid_currency);
    
update lt_order_item set 
    givenvat_currency = currencyCodeById(givenvat_currency),
    discount_currency = currencyCodeById(discount_currency),
    grandtotal_currency = currencyCodeById(grandtotal_currency),
    price_currency = currencyCodeById(price_currency),
    taxedtotal_currency = currencyCodeById(taxedtotal_currency);
    
update lt_payment set 
    vat_currency = currencyCodeById(vat_currency),
    amount_currency = currencyCodeById(amount_currency);

update lt_product set
    total_currency = currencyCodeById(total_currency),
    vat_currency = currencyCodeById(vat_currency),
    bonusdiscount_currency = currencyCodeById(bonusdiscount_currency),
    bonusaccumulation_currency = currencyCodeById(bonusaccumulation_currency),
    servicefee_currency = currencyCodeById(servicefee_currency),
    cancelcommission_currency = currencyCodeById(cancelcommission_currency),
    cancelfee_currency = currencyCodeById(cancelfee_currency),
    commission_currency = currencyCodeById(commission_currency),
    commissiondiscount_currency = currencyCodeById(commissiondiscount_currency),
    discount_currency = currencyCodeById(discount_currency),
    equalfare_currency = currencyCodeById(equalfare_currency),
    fare_currency = currencyCodeById(fare_currency),
    feestotal_currency = currencyCodeById(feestotal_currency),
    grandtotal_currency = currencyCodeById(grandtotal_currency),
    handling_currency = currencyCodeById(handling_currency),
    servicefeepenalty_currency = currencyCodeById(servicefeepenalty_currency),
    refundservicefee_currency = currencyCodeById(refundservicefee_currency),
    faretotal_currency = currencyCodeById(faretotal_currency);

update lt_system_configuration set 
    defaultcurrency = currencyCodeById(defaultcurrency);

update lt_currency set 
    id = code;



drop view olap_fare_currency_dim cascade;
drop view olap_currency_dim cascade;
drop view olap_document cascade;
drop view olap_fare_segment_dim cascade;
drop view olap_transaction_dim cascade;


alter table lt_currency 
    alter column id type char(3);

alter table lt_invoice 
    alter column vat_currency type char(3),
    alter column total_currency type char(3);

alter table lt_product 
    alter column bonusaccumulation_currency type char(3),
    alter column bonusdiscount_currency type char(3),
    alter column faretotal_currency type char(3),
    alter column refundservicefee_currency type char(3),
    alter column servicefeepenalty_currency type char(3),
    alter column cancelcommission_currency type char(3),
    alter column cancelfee_currency type char(3),
    alter column commission_currency type char(3),
    alter column commissiondiscount_currency type char(3),
    alter column discount_currency type char(3),
    alter column equalfare_currency type char(3),
    alter column fare_currency type char(3),
    alter column feestotal_currency type char(3),
    alter column grandtotal_currency type char(3),
    alter column handling_currency type char(3),
    alter column servicefee_currency type char(3),
    alter column total_currency type char(3),
    alter column vat_currency type char(3);

alter table lt_avia_document_fee 
    alter column amount_currency type char(3);
    
alter table lt_consignment 
    alter column discount_currency type char(3),
    alter column grandtotal_currency type char(3),
    alter column vat_currency type char(3);

alter table lt_flight_segment 
    alter column amount_currency type char(3);
    
alter table lt_order 
    alter column discount_currency type char(3),
    alter column paid_currency type char(3),
    alter column total_currency type char(3),
    alter column totaldue_currency type char(3),
    alter column vat_currency type char(3),
    alter column vatdue_currency type char(3);
    
alter table lt_order_item 
    alter column discount_currency type char(3),
    alter column givenvat_currency type char(3),
    alter column grandtotal_currency type char(3),
    alter column price_currency type char(3),
    alter column taxedtotal_currency type char(3);

alter table lt_payment 
    alter column amount_currency type char(3),
    alter column vat_currency type char(3);
    
alter table lt_system_configuration 
    alter column defaultcurrency type char(3);



truncate table lt_currency_daily_rate;

alter table lt_currency_daily_rate 
    alter column uah_eur type numeric(16,8),
    alter column uah_rub type numeric(16,8),
    alter column uah_usd type numeric(16,8),
    alter column rub_eur type numeric(16,8),
    alter column rub_usd type numeric(16,8),
    alter column eur_usd type numeric(16,8);

reindex table lt_invoice;
reindex table lt_product;
reindex table lt_avia_document_fee;
reindex table lt_consignment;
reindex table lt_flight_segment;
reindex table lt_order;
reindex table lt_order_item;
reindex table lt_payment;

--rollback;