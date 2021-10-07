drop view if exists olap_gds_dim;


create view olap_gds_dim
as
select 0 as type, 'Unknown' as name union all 
select 1, 'Amadeus' union all 
select 2, 'Galileo' union all 
select 3, 'Sirena' union all 
select 4, 'Airline' union all 
select 5, 'Gabriel' union all 

select 6, 'WizzAir' union all 
select 7, 'IATI' union all 
select 8, 'ETravels' union all 
select 9, 'TicketConsolidator' union all 
select 10, 'DeltaTravel' union all 
select 11, 'TicketsUA' union all 
select 12, 'FlyDubai' union all 
select 13, 'AirArabia' union all 
select 14, 'Pegasus' union all 
select 15, 'ВіаКиїв' union all 
select 16, 'AirLife' union all 
select 17, 'Sabre' union all 
select 18, 'Amadeus Altea' union all 
select 19, 'SPRK' union all 
select 20, 'Travel & Marketing' union all 
select 21, 'Travel Point' union all 
select 22, 'Crazy Llama' union all 
select 23, 'Drct Aero'


;
