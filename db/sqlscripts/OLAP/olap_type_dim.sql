drop view if exists olap_type_dim;


create view olap_type_dim
as 

select 0 as type, 'Авиабилет' as name union all
select 1 as type, 'Возврат авиабилета' as name union all
select 2 as type, 'MCO' as name union all
select 13 as type, 'Автобусный билет' as name union all
select 16 as type, 'Возврат автобусного билета' as name union all
select 11 as type, 'Аренда авто' as name union all
select 3 as type, 'Ж/д билет' as name union all
select 14 as type, 'Возврат ж/д билета' as name union all
select 5 as type, 'ISIC' as name union all
select 8 as type, 'Проживание' as name union all
select 9 as type, 'Трансфер' as name union all
select 7 as type, 'Тур (готовый)' as name union all
select 4 as type, 'SIM-карта' as name union all
select 10 as type, 'Страховка' as name union all
select 15 as type, 'Возврат страховки' as name union all
select 6 as type, 'Экскурсия' as name union all
select 12 as type, 'Дополнительная услуга' as name
;


