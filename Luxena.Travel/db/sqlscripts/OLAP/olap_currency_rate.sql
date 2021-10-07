create or replace function get_currency_rate(date, citext2, citext2) returns numeric(18,8)
as $$
   select

        case $3
            when 'EUR' then 
                case $2
                    when 'EUR' then 1
                    when 'RUB' then 1 / rub_eur
                    when 'UAH' then 1 / uah_eur
                    when 'USD' then eur_usd
                    when 'NUC' then eur_usd
                end
        
            when 'RUB' then 
                case $2
                    when 'EUR' then rub_eur
                    when 'RUB' then 1
                    when 'UAH' then 1 / uah_rub
                    when 'USD' then 1 / rub_usd
                    when 'NUC' then 1 / rub_usd
                end

            when 'UAH' then 
                case $2
                    when 'EUR' then uah_eur
                    when 'RUB' then uah_rub
                    when 'UAH' then 1
                    when 'USD' then uah_usd
                    when 'NUC' then uah_usd
                end

            when 'USD' then 
                case $2
                    when 'EUR' then 1 / eur_usd
                    when 'RUB' then 1 / rub_usd
                    when 'UAH' then 1 / uah_usd
                    when 'USD' then 1
                    when 'NUC' then 1
                end
                
        end::numeric(18, 8)
   
     from lt_currency_daily_rate
    where date_ = $1
$$
language sql;


-- select citext2_like('2015-08-1', 'NUC', 'UAH') as rate