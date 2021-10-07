/*
GRANT USAGE ON SCHEMA ufsa TO utb;
GRANT SELECT ON ALL TABLES IN SCHEMA ufsa TO utb;
*/

-- Function: utb.lt_sp_import_avia_documents_from_ufsa()

-- DROP FUNCTION utb.lt_sp_import_avia_documents_from_ufsa();

CREATE OR REPLACE FUNCTION utb.lt_sp_import_avia_documents_from_ufsa(in ticketId varchar(32) default null)
  RETURNS integer AS
$BODY$

declare
    rowCount integer;
    ticketCount integer;
begin

    begin  -- using document
        if documentId is not null then
            create temp table document on commit drop 
            as
                select d.* 
                  from ufsa.lt_avia_document d
                 where d.id = documentId;
        else
            create temp table document on commit drop 
            as
                select d.* 
                  from ufsa.lt_avia_document d
                    inner join ufsa.lt_avia_ticket t on d.id = t.id
                    left join lt_avia_document d2 on d.id = d2.id
                 where d.owner = '006382cbfb92403ca8613fbde34da5da' -- 'УТБ'
                   and d2.id is null;
        end if;
        raise info 'Using Documents: %', (select count(*) from document);
    end;

    if (select count(*) from document) = 0 then
        return null;
    end if;


    begin  -- using airline
        create temp table airline on commit drop 
        as
            select o.id as oldId, coalesce(n.id, o.id) as newId, n.id as curId, o.organization
              from (
                select distinct airline as id from document union
                select distinct carrier
                  from ufsa.lt_flight_segment fs inner join document d on d.id = fs.ticket
              ) c
                inner join ufsa.lt_airline o on c.id = o.id
                left join lt_airline n on o.iatacode = n.iatacode;

        raise info 'Using Airlines: % (% + %)'
            , (select count(*) from airline)
            , (select count(*) from airline where curId is not null)
            , (select count(*) from airline where curId is null);
    end;


    begin  -- using airport
        create temp table airport on commit drop 
        as
            select o.id as oldId, coalesce(n.id, o.id) as newId, n.id as curId
              from (
                select distinct fromAirport as id 
                  from ufsa.lt_flight_segment fs inner join document d on d.id = fs.ticket
                union
                select distinct toAirport as id 
                  from ufsa.lt_flight_segment fs inner join document d on d.id = fs.ticket
              ) c
                inner join ufsa.lt_airport o on c.id = o.id
                left join lt_airport n on o.code = n.code;

        raise info 'Using Airports: % (% + %)'
            , (select count(*) from airport)
            , (select count(*) from airport where curId is not null)
            , (select count(*) from airport where curId is null);
    end;


    begin  -- using currency
        create temp table currency on commit drop 
        as
            select o.id as oldId, coalesce(n.id, o.id) as newId, n.id as curId
              from (
                select distinct commission_currency as id from document union
                select distinct commissiondiscount_currency as id from document union
                select distinct discount_currency as id from document union
                select distinct equalfare_currency as id from document union
                select distinct fare_currency as id from document union
                select distinct feestotal_currency as id from document union
                select distinct grandtotal_currency as id from document union
                select distinct handling_currency as id from document union
                select distinct servicefee_currency as id from document union
                select distinct total_currency as id from document union
                select distinct vat_currency as id from document
              ) c
                inner join ufsa.lt_currency o on c.id = o.id
                left join lt_currency n on o.code = n.code;

        --raise info '%', (select count(distinct airline) as id from document);
        raise info 'Using Currencies: % (% + %)'
            , (select count(*) from currency)
            , (select count(*) from currency where curId is not null)
            , (select count(*) from currency where curId is null);
    end;


    begin  -- using party
        create temp table party on commit drop
        as
            with recursive paties (id, reportsTo) as (
                select o.id, o.reportsTo
                  from (
                    select organization as id from airline union 
                    select booker from document union 
                    select customer from document union 
                    select intermediary from document union 
                    select owner from document union 
                    select passenger from document union 
                    select seller from document union 
                    select ticketer from document                    
                  ) c
                    inner join ufsa.lt_party o on c.id = o.id
                union all
                select o.id, o.reportsTo
                  from paties c
                    inner join ufsa.lt_party o on c.reportsTo = o.id
                 where o.id <> c.id
            )            
            select distinct 
                c.id as oldId, coalesce(n.id, o.id) as newId, n.id as curId, coalesce(n.reportsTo, o.reportsTo) as reportsTo
              from paties c
                inner join ufsa.lt_party o on c.id = o.id
                left join lt_party n on o.name = n.name;

        --raise notice '%', (select curId from party where oldId = 'e5089b73498b403aa6bb27f4f49ef7d7');        
        raise info 'Using Parties: % (% + %)'
            , (select count(*) from party)
            , (select count(*) from party where curId is not null)
            , (select count(*) from party where curId is null);
    end;


    begin  -- new party
        insert into lt_party (
            id, version, createdby, createdon, 
            name, legalname, phone1, phone2, fax, email1, email2, webaddress, iscustomer, 
            issupplier, legaladdress, actualaddress, note, reportsto
        )
        select 
            id, 1, 'SYSTEM', current_timestamp,
            name, legalname, phone1, phone2, fax, email1, email2, webaddress, iscustomer, 
            issupplier, legaladdress, actualaddress, note, o.reportsto
          from ufsa.lt_party o
            inner join party t on t.oldId = o.id
         where t.curId is null;

        get diagnostics rowCount = row_count;
        raise info 'New Parties: %', rowCount;
    end;


    begin  -- new organization
        insert into lt_organization (id, code)
        select id, code
          from ufsa.lt_organization o
            inner join party t on t.oldId = o.id
         where t.curId is null;

        get diagnostics rowCount = row_count;
        raise info 'New Organizations: %', rowCount;
    end;


    begin  -- new person
        insert into lt_person (id, birthday, milescardsstring, title, organization)
        select id, birthday, milescardsstring, title, org.newId as organization
          from ufsa.lt_person o
            inner join party t on t.oldId = o.id
            left join party org on o.organization = org.oldId
         where t.curId is null;

        get diagnostics rowCount = row_count;
        raise info 'New Persons: %', rowCount;
        
        --raise notice '%', (select id from lt_person where id = 'e5089b73498b403aa6bb27f4f49ef7d7');
        --raise notice '%', (select curId from party where oldId = 'e5089b73498b403aa6bb27f4f49ef7d7');        
    end;


    begin  -- new airline
        insert into lt_airline (
            id, version, createdby, createdon, 
            iatacode, prefixcode, passportrequirement, name, organization
        )
        select 
            id, 1, 'SYSTEM', current_timestamp,
            iatacode, prefixcode, passportrequirement, name, org.newId as organization
          from ufsa.lt_airline o          
            inner join airline t on t.oldId = o.id
            left join party org on org.oldId = o.organization
         where t.curId is null;
            
        get diagnostics rowCount = row_count;
        raise info 'New Airlines: %', rowCount;
    end;


    begin  -- new airport
        insert into lt_airport (
            id, version, createdby, createdon, 
            code, settlement, localizedsettlement, name, country, longitude, latitude
        )
        select 
            id, 1, 'SYSTEM', current_timestamp,
            code, settlement, localizedsettlement, name, country, longitude, latitude
          from ufsa.lt_airport o          
            inner join airport t on t.oldId = o.id
         where t.curId is null;
            
        get diagnostics rowCount = row_count;
        raise info 'New Airports: %', rowCount;
    end;


    begin  -- new currency
        insert into lt_currency (
            id, version, createdby, createdon, 
            name, code, cyrilliccode, numericcode
        )
        select 
            id, 1, 'SYSTEM', current_timestamp,
            name, code, cyrilliccode, numericcode
          from ufsa.lt_currency o          
            inner join currency t on t.oldId = o.id
         where t.curId is null;
            
        get diagnostics rowCount = row_count;
        raise info 'New Currencies: %', rowCount;
    end;


    begin  -- new document

        insert into lt_avia_document (
            id, version, createdby, createdon, 
            type, issuedate, airlineiatacode, airlineprefixcode, airlinename, number_, reissuefor, 
            conjunctionnumbers, isprocessed, isvoid, requiresprocessing, 
            passengername, gdspassportstatus, gdspassport, itinerary, commissionpercent, 
            paymenttype, paymentform, bookeroffice, bookercode, ticketeroffice, 
            ticketercode, originator, origin, pnrcode, airlinepnrcode, tourcode, 
            note, remarks, displaystring, airline, passenger, booker, ticketer, 
            seller, owner, customer, intermediary, order_, originaldocument, 
            fare_amount, fare_currency, equalfare_amount, equalfare_currency, 
            commission_amount, commission_currency, commissiondiscount_amount, 
            commissiondiscount_currency, feestotal_amount, feestotal_currency, 
            vat_amount, vat_currency, total_amount, total_currency, servicefee_amount, 
            servicefee_currency, handling_amount, handling_currency, discount_amount, 
            discount_currency, grandtotal_amount, grandtotal_currency, ticketingiataoffice, 
            isticketerrobot, paymentdetails
        )
        select 
            id, 1, 'SYSTEM', current_timestamp,
            type, issuedate, airlineiatacode, airlineprefixcode, airlinename, number_, reissuefor, 
            conjunctionnumbers, isprocessed, isvoid, requiresprocessing, 
            passengername, gdspassportstatus, gdspassport, itinerary, commissionpercent, 
            paymenttype, paymentform, bookeroffice, bookercode, ticketeroffice, 
            ticketercode, originator, origin, pnrcode, airlinepnrcode, tourcode, 
            note, remarks, displaystring, al.newId as airline, pp.newId as passenger, pb.newId as booker, pt.newId as ticketer, 
            ps.newId as seller, po.newId as owner, pc.newId as customer, pi.newId as intermediary, null as order_, null as originaldocument, 
            fare_amount, cf.newId as fare_currency, equalfare_amount, cef.newId as equalfare_currency, 
            commission_amount, cc.newId as commission_currency, commissiondiscount_amount, 
            ccd.newId as commissiondiscount_currency, feestotal_amount, cft.newId as feestotal_currency, 
            vat_amount, cv.newId as vat_currency, total_amount, ct.newId as total_currency, servicefee_amount, 
            csf.newId as servicefee_currency, handling_amount, ch.newId as handling_currency, discount_amount, 
            cd.newId as discount_currency, grandtotal_amount, cgt.newId as grandtotal_currency, ticketingiataoffice, 
            isticketerrobot, paymentdetails
          from document d
          
            left join airline al on al.oldId = d.airline

            left join currency cc on cc.oldId = d.commission_currency 
            left join currency ccd on ccd.oldId = d.commissiondiscount_currency 
            left join currency cd on cd.oldId = d.discount_currency 
            left join currency cef on cef.oldId = d.equalfare_currency 
            left join currency cf on cf.oldId = d.fare_currency 
            left join currency cft on cft.oldId = d.feestotal_currency 
            left join currency cgt on cgt.oldId = d.grandtotal_currency
            left join currency ch on ch.oldId = d.handling_currency 
            left join currency csf on csf.oldId = d.servicefee_currency 
            left join currency ct on ct.oldId = d.total_currency 
            left join currency cv on cv.oldId = d.vat_currency 

            left join party pb on pb.oldId = d.booker 
            left join party pc on pc.oldId = d.customer 
            left join party pi on pi.oldId = d.intermediary 
            left join party po on po.oldId = d.owner 
            left join party pp on pp.oldId = d.passenger 
            left join party ps on ps.oldId = d.seller 
            left join party pt on pt.oldId = d.ticketer 
          
            --inner join document t on t.oldId = o.id

        ;
        get diagnostics rowCount = row_count;
        raise info 'New Documents: %', rowCount;
    
    end;


    begin  -- new ticket

        insert into lt_avia_ticket (
            id, domestic, interline, segmentclasses, departure, endorsement, 
            faretotal_amount, faretotal_currency
        )
        select 
            o.id, o.domestic, o.interline, o.segmentclasses, o.departure, o.endorsement, 
            o.faretotal_amount, cft.newId as faretotal_currency
          from ufsa.lt_avia_ticket o
            inner join document t on t.id = o.id
            left join currency cft on cft.oldId = o.faretotal_currency;
            
        get diagnostics rowCount = row_count;
        raise info 'New Tickets: %', rowCount;
        
        ticketCount := rowCount;
    
    end;

    
    begin  -- new flight_segment

        insert into lt_flight_segment (
            id, version, createdby, createdon, 
            "position", type, fromairportcode, fromairportname, toairportcode, toairportname, 
            carrieriatacode, carrierprefixcode, carriername, flightnumber, 
            serviceclasscode, serviceclass, departuretime, arrivaltime, mealcodes, 
            mealtypes, numberofstops, luggage, checkinterminal, checkintime, 
            duration, arrivalterminal, seat, farebasis, stopover, ticket, 
            fromairport, toairport, carrier, surcharges, isinclusive, fare, 
            stopoverortransfercharge, issidetrip, distance, amount_amount, 
            amount_currency
       )
        select 
            o.id, 1, 'SYSTEM', current_timestamp,
            o."position", o.type, o.fromairportcode, o.fromairportname, o.toairportcode, o.toairportname,
            o.carrieriatacode, o.carrierprefixcode, o.carriername, o.flightnumber, 
            o.serviceclasscode, o.serviceclass, o.departuretime, o.arrivaltime, o.mealcodes, 
            o.mealtypes, o.numberofstops, o.luggage, o.checkinterminal, o.checkintime, 
            o.duration, o.arrivalterminal, o.seat, o.farebasis, o.stopover, o.ticket, 
            af.newId as fromairport, at.newId as toairport, o.carrier, o.surcharges, o.isinclusive, o.fare, 
            o.stopoverortransfercharge, o.issidetrip, o.distance, o.amount_amount, 
            ca.newId as amount_currency
          from ufsa.lt_flight_segment o
            inner join document t on t.id = o.ticket

            left join airport af on af.oldId = o.fromAirport
            left join airport at on at.oldId = o.toAirport
            
            left join currency ca on ca.oldId = o.amount_currency;
            
        get diagnostics rowCount = row_count;
        raise info 'New Flight Segments: %', rowCount;
    
    end;

    

    raise info 'TOTAL TICKETS: %', (select count(*) from lt_avia_ticket);

    return ticketCount;
    
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION utb.lt_sp_import_avia_documents_from_ufsa(varchar(32))
  OWNER TO utb;
