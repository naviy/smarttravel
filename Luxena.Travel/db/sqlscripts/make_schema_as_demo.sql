SET search_path = demo;


--begin work;

truncate table lt_modification_items;
truncate table lt_modification;


update lt_product set originaldocument = null;
truncate table lt_file;
delete from lt_gds_file;

truncate table lt_internal_transfer;

delete from lt_task
 where order_ is not null;


/*** Удаление продуктов ***/
--begin work;

drop table if exists delProducts;

create temp table delProducts -- on commit drop
as
with recursive docs(id) as (
    select id from lt_product
     where issueDate > '2019-1-1'
  union
    select q.id
      from docs d, (
		select distinct id, reissuefor as id2 from lt_product where reissuefor is not null
		union
		select distinct id, inconnectionwith as id2 from lt_product where inconnectionwith is not null
		union
		select distinct id, refundedproduct as id2 from lt_product where refundedproduct is not null
      ) q
     where d.id = q.id2
)
select distinct id from docs;

create index id_delProducts1 on delProducts (id);

-- select * from delProducts


delete from lt_avia_document_fee
 where document in (select id from delProducts);

delete from lt_avia_document_voiding
 where document in (select id from delProducts);

delete from lt_flight_segment
 where ticket in (select id from delProducts);

delete from lt_penalize_operation
 where ticket in (select id from delProducts);

delete from lt_order_item
 where product in (select id from delProducts);

delete from lt_product_passenger
 where product in (select id from delProducts);

delete from lt_product
 where id in (select id from delProducts);

--rollback;

/*** Удаление заказов ***/

drop table if exists delOrders cascade;

create temp table delOrders -- on commit drop
as
select id from lt_order o 
 where not exists(select id from lt_order_item where order_ = o.id);

create index id_delOrders1 on delOrders (id);

-- select * from delOrders


delete from lt_payment
 where order_ in (select id from delOrders);

update lt_payment set invoice = null;

delete from lt_invoice;
-- where order_ in (select id from delOrders);

delete from lt_order_check;


update lt_product 
   set order_ = null
 where order_ in (select id from delOrders);

delete from lt_order o
 where id in (select id from delOrders);

delete from lt_issued_consignment;
update lt_order_item set consignment = null;
delete from lt_consignment;



/*** Удаление персон ***/

--begin work;

drop table if exists delPersons;

create temp table delPersons -- on commit drop
as
    select id from lt_party where class = 'Person'
    except select booker from lt_product 
    except select ticketer from lt_product 
    except select owner from lt_document_owner
    except select customer from lt_product
    except select intermediary from lt_product
    except select passenger from lt_product_passenger
    except select seller from lt_product
    except select person from lt_document_access
    except select person from lt_gds_agent
    except select person from lt_identity
    except select relatedto from lt_task
    except select assignedto from lt_task
    except select reportsto from lt_party
    except select assignedto from lt_order
    except select billto from lt_order
    except select customer from lt_order
    except select shipto from lt_order
    except select intermediary from lt_order
    except select payer from lt_payment
    except select agent from lt_avia_document_voiding
    except select owner from lt_miles_card
;

delete from lt_passport
 where owner in (select id from delPersons); 

delete from lt_opening_balance
 where party in (select id from delPersons); 

delete from lt_party
 where id in (select id from delPersons); 


drop table if exists delOrgs;

create temp table delOrgs -- on commit drop
as
    select id from lt_party where class = 'Organization'
    except select organization from lt_party
    except select owner from lt_document_owner
    except select provider from lt_product
    except select producer from lt_product
    except select customer from lt_product
    except select intermediary from lt_product
    except select relatedto from lt_task
    except select assignedto from lt_task
    except select assignedto from lt_order
    except select billto from lt_order
    except select customer from lt_order
    except select shipto from lt_order
    except select intermediary from lt_order
    except select payer from lt_payment
    except select organization from lt_miles_card
    except select reportsto from lt_party
	except select airline from lt_airline_service_class
	except select operator from lt_flight_segment
; 


delete from lt_opening_balance
 where party in (select id from delOrgs);

delete from lt_party
 where id in (select id from delOrgs); 

--rollback;



-- -- /*** Переименовывание персон ***/

-- -- create or replace function explode_array(in_array anyarray) 
-- -- returns setof anyelement as
-- -- $$
-- --     select ($1)[s] from generate_series(1, array_upper($1, 1)) as s;
-- -- $$
-- -- language sql immutable;


-- -- update lt_party pt set name = pt.id
-- --   from newPersonNames n
-- --  where pt.id = n.id;


-- -- drop table if exists newPersonNames;

-- -- create temp table newPersonNames --on commit drop
-- -- as
-- -- select
-- -- -- 	distinct name
-- -- 	id, sex, name,
-- -- 	split_part(name, ' ', 1) as name1, 
-- -- 	split_part(name, ' ', 2) as name2
-- --   from (
-- -- 	select
-- -- 		p.id, p.sex,
		
-- -- 		case sex when 0 
-- -- 			then namesm[p.rowno]
-- -- 			else namesw[p.rowno]
-- -- 			--then namesm[round(p.rowno * array_length(namesm, 1) / p2.cnt)]
-- -- 			--else namesw[round(p.rowno * array_length(namesw, 1) / p2.cnt)]
-- -- 		end as name
		
		
-- -- 	  from
-- -- 		(select id, random()::int as sex, row_number() over(order by id) as rowno
-- -- 		  from lt_person
-- -- 		) p,
-- -- 		--(select count(*)::float as cnt from lt_person) p2,
-- -- 		(select
-- -- 			(select array_agg(distinct concat(name2, ' ', name1))
-- -- 				from 
-- -- 					explode_array(array[
-- -- 						'Aleksandr', 'Aleksej', 'Anatolij', 'Andrej', 'Boris', 'Valerij', 'Vasilij', 'Viktor', 'Vitalij', 'Vladimir', 'Gennadij', 'Georgij', 'Grigorij', 'Denis', 'Dmitrij', 'Evgenij', 'Ivan', 'Igor', 'Ilya', 'Konstantin', 'Maksim', 'Mixail', 'Nikita', 'Nikolaj', 'Oleg', 'Pavel', 'Petr', 'Roman', 'Sergej', 'Stepan', 'Fedor', 'Yurij'
-- -- 					]) as name1
-- -- 				cross join
-- -- 					explode_array(array[
-- -- 						'Kovalenko', 'Shevchenko', 'Bondarenko', 'Kravchenko', 'Tkachenko', 'Savchenko', 'Boyko', 'Lysenko', 'Marchenko', 'Sidorenko', 'Vlasenko', 'Mishchenko', 'Fomenko', 'Levchenko', 'Rudenko', 'Karpenko', 'Petrenko', 'Miroshnichenko', 'Kharchenko', 'Klimenko', 'Yurchenko', 'Romanenko', 'Gerasimenko', 'Vasilenko', 'dyachenko', 'Yeremenko', 'Ponomarenko', 'Gavrilenko', 'Denisenko', 'Pavlenko', 'Tarasenko', 'Kuzmenko', 'Alekseyenko', 'Nazarenko', 'Litvinenko', 'Tishchenko', 'Panchenko', 'Tereshchenko', 'Borisenko', 'Doroshenko', 'Korniyenko', 'Prikhodko', 'Didenko', 'Makarenko', 'Yakovenko', 'Velichko', 'Demchenko', 'Kostenko', 'Nesterenko', 'Radchenko', 'Sergiyenko', 'Zinchenko', 'Ivashchenko', 'Kondratenko', 'Taran', 'Shcherbina', 'Babenko', 'Vasilchenko', 'Ostapenko', 'Voytenko', 'Goncharenko', 'Maksimenko', 'Prokopenko', 'Chernyavskiy', 'Kirichenko', 'Martynenko', 'Chernenko', 'Ignatenko', 'Lutsenko', 'Oleynik', 'Parkhomenko', 'Fisenko', 'Simonenko', 'Golovko', 'Kolesnichenko', 'Mironenko', 'Yatsenko', 'Pilipenko', 'Serdyuk', 'Fedorenko', 'Gordiyenko', 'Ishchenko', 'Melnik', 'Ivanenko', 'Moskalenko', 'Ovcharenko', 'Berezhnoy', 'Bondar', 'Redko', 'Shulga', 'Dmitriyenko', 'Sereda', 'Dzyuba', 'Yermolenko', 'Zelenskiy', 'Koval', 'Onishchenko', 'Timoshenko', 'Bozhko', 'Galushko', 'Gritsenko', 'Yevtushenko', 'Stepanenko', 'Yefimenko', 'Kolomiyets', 'Nikitenko', 'Samoylenko', 'Sviridenko', 'Timchenko', 'Davydenko', 'Zakharchenko', 'Garmash', 'Glushchenko', 'Rybalko', 'Savenko', 'Babich', 'Gavrish', 'Matviyenko', 'Moroz', 'Artemenko', 'Kovtun', 'Kolesnik', 'Kulik', 'Kucherenko', 'Nikolenko', 'Omelchenko', 'Taranenko', 'Berezovskiy', 'Grigorenko', 'Gritsay', 'Zhuk', 'Ivanchenko', 'Levitskiy', 'Nikolayenko', 'Protsenko', 'Chumak', 'Grebenyuk', 'Grishchenko', 'Dobrovolskiy', 'Dolzhenko', 'Kalinichenko', 'Kovalchuk', 'Kostyuk', 'Osadchiy', 'Sayenko', 'Andryushchenko', 'Gorobets', 'Danilchenko', 'Dotsenko', 'Kramarenko', 'Krivenko', 'Kutsenko', 'Moiseyenko', 'Pisarenko', 'Sakhno', 'Shvets', 'Gerashchenko', 'Lyashenko', 'Mikhaylenko', 'Novak', 'Reznichenko', 'Romanchenko', 'Rybalchenko', 'Sokol', 'Cherednichenko', 'Antonenko', 'Derevyanko', 'Zozulya', 'Kosenko', 'Lavrinenko', 'Lyakh', 'Titarenko', 'Chumachenko', 'Yakimenko', 'Andriyenko', 'Vakulenko', 'Voloshchenko', 'Golub', 'Kulish', 'Lukyanenko', 'Osipenko', 'Fesenko', 'Shpak', 'Avramenko', 'Logvinenko', 'Lyashko', 'Mozgovoy', 'Piven', 'Ruban', 'Fedchenko', 'Khomenko', 'Shevchuk', 'Yaroshenko', 'Verbitskiy', 'Vishnevskiy', 'Davidenko', 'Derkach', 'Zaporozhets', 'Ivchenko', 'Korzh', 'Litovchenko', 'Boychenko', 'Gorbenko', 'Danilenko', 'Vorushilo', 'Kozlovskiy', 'Kulinich', 'Lebedinskiy', 'Lozovoy', 'Reshetnyak', 'Suprun', 'Abramenko', 'Bublik', 'Butenko', 'Butko', 'Vashchenko', 'Dubovik', 'Dudnik', 'Zubenko', 'Krivonos', 'Lazarenko', 'Leshchenko', 'Mikhaylichenko', 'Pasechnik', 'Petrovskiy', 'Statsenko', 'Storozhenko', 'Chayka', 'Chub', 'Shepel', 'Yarovoy', 'Gavrilko', 'Gontar', 'Guba', 'Demidenko', 'Zima', 'Korol', 'Lebed', 'Pavlovskiy', 'Pashchenko', 'Stetsenko', 'Teslenko', 'Trofimenko', 'Usenko', 'Fedorchenko', 'Buryak', 'Gorban', 'Grishko', 'Ilchenko', 'Korolenko', 'Kocherga', 'Litvin', 'Semenchenko', 'Sklyar', 'Troyan', 'Khizhnyak', 'Chirva', 'Shumeyko', 'Yanovskiy', 'Anishchenko', 'Beletskiy', 'Glushko', 'Grechko', 'Gurskiy', 'Dmitrenko', 'Donskoy', 'Yeroshenko', 'Zaychenko', 'Isayenko', 'Kamenskiy', 'Kuts', 'Linnik', 'Lugovoy', 'Manko', 'Mikhaylyuk', 'Musiyenko', 'Podolskiy', 'Popovich', 'Reva', 'Silchenko', 'Sirota', 'Tkach', 'Troitskiy', 'Shinkarenko', 'Belokon', 'Belous', 'Vovk', 'Volovik', 'Galenko', 'Galushka', 'Gonchar', 'Grinko', 'Drobyazko', 'Zubko', 'Kirilenko', 'Kiyashko', 'Kostyuchenko', 'Kukharenko', 'Lyubchenko', 'Malinovskiy', 'Minenko', 'Mirgorodskiy', 'Mikhaylovskiy', 'Nemchenko', 'Orel', 'Ostrovskiy', 'Okhrimenko', 'Panasenko', 'Perederiy', 'Pokrovskiy', 'Rud', 'Sobol', 'Stupak', 'Tretyak', 'Ustimenko', 'Chernyak', 'Sheremet', 'Aleshchenko', 'Belinskiy', 'Bondarchuk', 'But', 'Vdovenko', 'Volk', 'Grushko', 'Degtyarenko', 'Deynega', 'Zheleznyak', 'Kalita', 'Kobzar', 'Korotkiy', 'Kochura', 'Kravchuk', 'Lugovskoy', 'Nosenko', 'Pavlyuk', 'Polishchuk', 'Sushko', 'Tsokur', 'Shramko', 'Arkhipenko', 'Babeshko', 'Bakumenko', 'Gorbatenko', 'Gorskiy', 'Dudka', 'Zhurba', 'Zabara', 'Klochko', 'Kravets', 'Kucher', 'Kushnarenko', 'Loboda', 'Makhno', 'Novitskiy', 'Opryshko', 'Pelipenko', 'Pogrebnoy', 'Podoprigora', 'Poltavskiy', 'Potapenko', 'Priymak', 'Savchuk', 'Semak', 'Skiba', 'Skorik', 'Spivak', 'Tikhonenko', 'Shulzhenko', 'Shcherbak',
-- -- 						'Abramov', 'Avdeev', 'Agapov', 'Agafonov', 'Ageev', 'Akimov', 'Aksenov', 'Aleksandrov', 'Alekseev', 'Alexin', 'Aleshin', 'Ananev', 'Andreev', 'Andrianov', 'Anikin', 'Anisimov', 'Anoxin', 'Antipov', 'Antonov', 'Artamonov', 'Artemov', 'Arxipov', 'Astafev', 'Astaxov', 'Afanasev', 'Babushkin', 'Bazhenov', 'Balashov', 'Baranov', 'Barsukov', 'Basov', 'Bezrukov', 'Belikov', 'Belkin', 'Belov', 'Belousov', 'Belyaev', 'Belyakov', 'Berezin', 'Bespalov', 'Bessonov', 'Biryukov', 'Blinov', 'Bloxin', 'Bobrov', 'Bogdanov', 'Bogomolov', 'Boldyrev', 'Bolshakov', 'Bondarev', 'Borisov', 'Borodin', 'Bocharov', 'Bulatov', 'Bulgakov', 'Burov', 'Bykov', 'Bychkov', 'Vavilov', 'Vasilev', 'Vdovin', 'Vereshhagin', 'Veshnyakov', 'Vinogradov', 'Vinokurov', 'Vishnevskij', 'Vladimirov', 'Vlasov', 'Volkov', 'Voloshin', 'Vorobev', 'Voronin', 'Voronkov', 'Voronov', 'Voroncov', 'Vysockij', 'Gavrilov', 'Galkin', 'Gerasimov', 'Gladkov', 'Glebov', 'Gluxov', 'Glushkov', 'Golikov', 'Golovanov', 'Golovin', 'Golubev', 'Goncharov', 'Gorbachev', 'Gorbunov', 'Gordeev', 'Gorelov', 'Gorlov', 'Goroxov', 'Gorshkov', 'Goryunov', 'Goryachev', 'Grachev', 'Grekov', 'Gribov', 'Grigorev', 'Grishin', 'Gromov', 'Gubanov', 'Gulyaev', 'Gurov', 'Gusev', 'Gushhin', 'Davydov', 'Danilov', 'Degtyarev', 'Dementev', 'Demidov', 'Demin', 'Demyanov', 'Denisov', 'Dmitriev', 'Dobrynin', 'Dolgov', 'Dorofeev', 'Doroxov', 'Drozdov', 'Druzhinin', 'Dubinin', 'Dubov', 'Dubrovin', 'Dyakov', 'Dyakonov', 'Evdokimov', 'Evseev', 'Egorov', 'Ezhov', 'Elizarov', 'Eliseev', 'Emelyanov', 'Eremeev', 'Eremin', 'Ermakov', 'Ermilov', 'Ermolaev', 'Ermolov', 'Erofeev', 'Ershov', 'Efimov', 'Efremov', 'Zharov', 'Zhdanov', 'Zhilin', 'Zhukov', 'Zhuravlev', 'Zavyalov', 'Zajcev', 'Zaxarov', 'Zverev', 'Zvyagincev', 'Zelenin', 'Zimin', 'Zinovev', 'Zlobin', 'Zolotarev', 'Zolotov', 'Zorin', 'Zotov', 'Zubkov', 'Zubov', 'Zuev', 'Zykov', 'Ivanov', 'Ignatov', 'Ignatev', 'Izmajlov', 'Ilin', 'Ilinskij', 'Isaev', 'Isakov', 'Kazakov', 'Kazancev', 'Kalachev', 'Kalashnikov', 'Kalinin', 'Kalmykov', 'Kalugin', 'Kapustin', 'Karasev', 'Karpov', 'Kartashov', 'Kasatkin', 'Kasyanov', 'Kireev', 'Kirillov', 'Kiselev', 'Klimov', 'Klyuev', 'Knyazev', 'Kovalev', 'Kozhevnikov', 'Kozin', 'Kozlov', 'Kozlovskij', 'Kozyrev', 'Kolesnikov', 'Kolesov', 'Kolosov', 'Kolpakov', 'Kolcov', 'Komarov', 'Komissarov', 'Kondratov', 'Kondratev', 'Kondrashov', 'Konovalov', 'Kononov', 'Konstantinov', 'Kopylov', 'Kornev', 'Korneev', 'Kornilov', 'Korovin', 'Korolev', 'Korolkov', 'Korotkov', 'Korchagin', 'Korshunov', 'Kosarev', 'Kostin', 'Kotov', 'Kochergin', 'Kochetkov', 'Kochetov', 'Koshelev', 'Kravcov', 'Krasnov', 'Kruglov', 'Krylov', 'Kryukov', 'Kryuchkov', 'Kudryavcev', 'Kudryashov', 'Kuzin', 'Kuznecov', 'Kuzmin', 'Kukushkin', 'Kulagin', 'Kulakov', 'Kuleshov', 'Kulikov', 'Kupriyanov', 'Kurochkin', 'Lavrentev', 'Lavrov', 'Lazarev', 'Lapin', 'Laptev', 'Lapshin', 'Larin', 'Larionov', 'Latyshev', 'Lebedev', 'Levin', 'Leonov', 'Leontev', 'Litvinov', 'Lobanov', 'Loginov', 'Lopatin', 'Losev', 'Lukin', 'Lukyanov', 'Lykov', 'Lvov', 'Lyubimov', 'Majorov', 'Makarov', 'Makeev', 'Maksimov', 'Malaxov', 'Malinin', 'Malyshev', 'Malcev', 'Markelov', 'Markin', 'Markov', 'Martynov', 'Maslennikov', 'Maslov', 'Matveev', 'Medvedev', 'Melnikov', 'Merkulov', 'Meshkov', 'Meshheryakov', 'Minaev', 'Minin', 'Mironov', 'Mitrofanov', 'Mixajlov', 'Mixeev', 'Moiseev', 'Molchanov', 'Morgunov', 'Morozov', 'Moskvin', 'Muravev', 'Muratov', 'Muxin', 'Nazarov', 'Naumov', 'Nekrasov', 'Nesterov', 'Nefedov', 'Nechaev', 'Nikitin', 'Nikiforov', 'Nikolaev', 'Nikolskij', 'Nikonov', 'Nikulin', 'Novikov', 'Noskov', 'Nosov', 'Ovsyannikov', 'Ovchinnikov', 'Odincov', 'Ozerov', 'Okulov', 'Olejnikov', 'Orexov', 'Orlov', 'Osipov', 'Ostrovskij', 'Pavlov', 'Pavlovskij', 'Panin', 'Pankov', 'Pankratov', 'Panov', 'Panteleev', 'Panfilov', 'Paramonov', 'Parfenov', 'Pastuxov', 'Paxomov', 'Petrov', 'Petrovskij', 'Petuxov', 'Pimenov', 'Pirogov', 'Platonov', 'Plotnikov', 'Pozdnyakov', 'Pokrovskij', 'Polikarpov', 'Polyakov', 'Ponomarev', 'Popov', 'Postnikov', 'Potapov', 'Prokofev', 'Proxorov', 'Pugachev', 'Rakov', 'Rogov', 'Rodin', 'Rodionov', 'Rozhkov', 'Rozanov', 'Romanov', 'Rubcov', 'Rudakov', 'Rudnev', 'Rumyancev', 'Rusakov', 'Rusanov', 'Rybakov', 'Ryzhov', 'Ryabinin', 'Ryabov', 'Savelev', 'Savin', 'Savickij', 'Sazonov', 'Salnikov', 'Samojlov', 'Samsonov', 'Safonov', 'Saxarov', 'Sveshnikov', 'Sviridov', 'Sevastyanov', 'Sedov', 'Seleznev', 'Selivanov', 'Semenov', 'Semin', 'Sergeev', 'Serebryakov', 'Serov', 'Sidorov', 'Sizov', 'Simonov', 'Sinicyn', 'Sitnikov', 'Skvorcov', 'Smirnov', 'Snegirev', 'Sobolev', 'Sokolov', 'Solovev', 'Somov', 'Sorokin', 'Sotnikov', 'Sofronov', 'Spiridonov', 'Starikov', 'Starostin', 'Stepanov', 'Stolyarov', 'Subbotin', 'Suvorov', 'Sudakov', 'Surkov', 'Suslov', 'Suxanov', 'Suxarev', 'Suxov', 'Sychev', 'Tarasov', 'Terentev', 'Terexov', 'Timofeev', 'Titov', 'Tixomirov', 'Tixonov', 'Tkachev', 'Tokarev', 'Tolkachev', 'Tretyakov', 'Trifonov', 'Troickij', 'Trofimov', 'Troshin', 'Tumanov', 'Uvarov', 'Ulyanov', 'Usov', 'Uspenskij', 'Ustinov', 'Utkin', 'Ushakov', 'Fadeev', 'Fedorov', 'Fedoseev', 'Fedosov', 'Fedotov', 'Fetisov', 'Filatov', 'Filimonov', 'Filippov', 'Firsov', 'Fokin', 'Fomin', 'Fomichev', 'Frolov', 'Xaritonov', 'Xomyakov', 'Xoxlov', 'Xromov', 'Xudyakov', 'Carev', 'Cvetkov', 'Chebotarev', 'Cherepanov', 'Cherkasov', 'Chernov', 'Chernyj', 'Chernyx', 'Chernyshev', 'Chernyaev', 'Chesnokov', 'Chizhov', 'Chistyakov', 'Chumakov', 'Shapovalov', 'Shaposhnikov', 'Sharov', 'Shvecov', 'Shevelev', 'Shevcov', 'Shestakov', 'Shilov', 'Shirokov', 'Shiryaev', 'Shishkin', 'Shmelev', 'Shubin', 'Shuvalov', 'Shulgin', 'Shheglov', 'Shherbakov', 'Shhukin', 'Yudin', 'Yakovlev', 'Yashin'
-- -- 					]) as name2
-- -- 			) as namesm,
-- -- 			(select array_agg(distinct concat(name2, ' ', name1))
-- -- 				from 
-- -- 					explode_array(array[
-- -- 						'Agata', 'Ada', 'Adel', 'Adriana', 'Alevtina', 'Aleksandra', 'Alina', 'Alisa', 'Alla', 'Albina', 'Anastasiya', 'Angelina', 'Anzhela', 'Anna', 'Antonina', 'Anfisa', 'Arina', 'Bogdana', 'Borislava', 'Valentina', 'Valeriya', 'Varvara', 'Vasilisa', 'Vera', 'Veronika', 'Viktoriya', 'Violetta', 'Galina', 'Darya', 'Diana', 'Eva', 'Evgeniya', 'Ekaterina', 'Elena', 'Elizaveta', 'Zhanna', 'Zinaida', 'Zoya', 'Ilona', 'Inga', 'Inessa', 'Inna', 'Irina', 'Karina', 'Kira', 'Klavdiya', 'Klara', 'Kristina', 'Kseniya', 'Larisa', 'Lidiya', 'Liliya', 'Lyubov', 'Lyudmila', 'Majya', 'Margarita', 'Marina', 'Mariya', 'Marta', 'Maryana', 'Miroslava', 'Nadezhda', 'Natalya', 'Nelli', 'Nina', 'Oksana', 'Olga', 'Polina', 'Raisa', 'Regina', 'Rimma', 'Roza', 'Roksana', 'Ruslana', 'Svetlana', 'Snezhana', 'Sofiya', 'Taisiya', 'Tamara', 'Tatyana', 'Ulyana', 'Xristina', 'Evelina', 'Eleonora', 'Elvira', 'Emma', 'Yuliana', 'Yuliya', 'Yana', 'Yaroslava'
-- -- 					]) as name1
-- -- 				cross join
-- -- 					explode_array(array[
-- -- 						'Kovalenko', 'Shevchenko', 'Bondarenko', 'Kravchenko', 'Tkachenko', 'Savchenko', 'Boyko', 'Lysenko', 'Marchenko', 'Sidorenko', 'Vlasenko', 'Mishchenko', 'Fomenko', 'Levchenko', 'Rudenko', 'Karpenko', 'Petrenko', 'Miroshnichenko', 'Kharchenko', 'Klimenko', 'Yurchenko', 'Romanenko', 'Gerasimenko', 'Vasilenko', 'dyachenko', 'Yeremenko', 'Ponomarenko', 'Gavrilenko', 'Denisenko', 'Pavlenko', 'Tarasenko', 'Kuzmenko', 'Alekseyenko', 'Nazarenko', 'Litvinenko', 'Tishchenko', 'Panchenko', 'Tereshchenko', 'Borisenko', 'Doroshenko', 'Korniyenko', 'Prikhodko', 'Didenko', 'Makarenko', 'Yakovenko', 'Velichko', 'Demchenko', 'Kostenko', 'Nesterenko', 'Radchenko', 'Sergiyenko', 'Zinchenko', 'Ivashchenko', 'Kondratenko', 'Taran', 'Shcherbina', 'Babenko', 'Vasilchenko', 'Ostapenko', 'Voytenko', 'Goncharenko', 'Maksimenko', 'Prokopenko', 'Chernyavskiy', 'Kirichenko', 'Martynenko', 'Chernenko', 'Ignatenko', 'Lutsenko', 'Oleynik', 'Parkhomenko', 'Fisenko', 'Simonenko', 'Golovko', 'Kolesnichenko', 'Mironenko', 'Yatsenko', 'Pilipenko', 'Serdyuk', 'Fedorenko', 'Gordiyenko', 'Ishchenko', 'Melnik', 'Ivanenko', 'Moskalenko', 'Ovcharenko', 'Berezhnoy', 'Bondar', 'Redko', 'Shulga', 'Dmitriyenko', 'Sereda', 'Dzyuba', 'Yermolenko', 'Zelenskiy', 'Koval', 'Onishchenko', 'Timoshenko', 'Bozhko', 'Galushko', 'Gritsenko', 'Yevtushenko', 'Stepanenko', 'Yefimenko', 'Kolomiyets', 'Nikitenko', 'Samoylenko', 'Sviridenko', 'Timchenko', 'Davydenko', 'Zakharchenko', 'Garmash', 'Glushchenko', 'Rybalko', 'Savenko', 'Babich', 'Gavrish', 'Matviyenko', 'Moroz', 'Artemenko', 'Kovtun', 'Kolesnik', 'Kulik', 'Kucherenko', 'Nikolenko', 'Omelchenko', 'Taranenko', 'Berezovskiy', 'Grigorenko', 'Gritsay', 'Zhuk', 'Ivanchenko', 'Levitskiy', 'Nikolayenko', 'Protsenko', 'Chumak', 'Grebenyuk', 'Grishchenko', 'Dobrovolskiy', 'Dolzhenko', 'Kalinichenko', 'Kovalchuk', 'Kostyuk', 'Osadchiy', 'Sayenko', 'Andryushchenko', 'Gorobets', 'Danilchenko', 'Dotsenko', 'Kramarenko', 'Krivenko', 'Kutsenko', 'Moiseyenko', 'Pisarenko', 'Sakhno', 'Shvets', 'Gerashchenko', 'Lyashenko', 'Mikhaylenko', 'Novak', 'Reznichenko', 'Romanchenko', 'Rybalchenko', 'Sokol', 'Cherednichenko', 'Antonenko', 'Derevyanko', 'Zozulya', 'Kosenko', 'Lavrinenko', 'Lyakh', 'Titarenko', 'Chumachenko', 'Yakimenko', 'Andriyenko', 'Vakulenko', 'Voloshchenko', 'Golub', 'Kulish', 'Lukyanenko', 'Osipenko', 'Fesenko', 'Shpak', 'Avramenko', 'Logvinenko', 'Lyashko', 'Mozgovoy', 'Piven', 'Ruban', 'Fedchenko', 'Khomenko', 'Shevchuk', 'Yaroshenko', 'Verbitskiy', 'Vishnevskiy', 'Davidenko', 'Derkach', 'Zaporozhets', 'Ivchenko', 'Korzh', 'Litovchenko', 'Boychenko', 'Gorbenko', 'Danilenko', 'Vorushilo', 'Kozlovskiy', 'Kulinich', 'Lebedinskiy', 'Lozovoy', 'Reshetnyak', 'Suprun', 'Abramenko', 'Bublik', 'Butenko', 'Butko', 'Vashchenko', 'Dubovik', 'Dudnik', 'Zubenko', 'Krivonos', 'Lazarenko', 'Leshchenko', 'Mikhaylichenko', 'Pasechnik', 'Petrovskiy', 'Statsenko', 'Storozhenko', 'Chayka', 'Chub', 'Shepel', 'Yarovoy', 'Gavrilko', 'Gontar', 'Guba', 'Demidenko', 'Zima', 'Korol', 'Lebed', 'Pavlovskiy', 'Pashchenko', 'Stetsenko', 'Teslenko', 'Trofimenko', 'Usenko', 'Fedorchenko', 'Buryak', 'Gorban', 'Grishko', 'Ilchenko', 'Korolenko', 'Kocherga', 'Litvin', 'Semenchenko', 'Sklyar', 'Troyan', 'Khizhnyak', 'Chirva', 'Shumeyko', 'Yanovskiy', 'Anishchenko', 'Beletskiy', 'Glushko', 'Grechko', 'Gurskiy', 'Dmitrenko', 'Donskoy', 'Yeroshenko', 'Zaychenko', 'Isayenko', 'Kamenskiy', 'Kuts', 'Linnik', 'Lugovoy', 'Manko', 'Mikhaylyuk', 'Musiyenko', 'Podolskiy', 'Popovich', 'Reva', 'Silchenko', 'Sirota', 'Tkach', 'Troitskiy', 'Shinkarenko', 'Belokon', 'Belous', 'Vovk', 'Volovik', 'Galenko', 'Galushka', 'Golub', 'Gonchar', 'Grinko', 'Drobyazko', 'Zubko', 'Kirilenko', 'Kiyashko', 'Kostyuchenko', 'Kukharenko', 'Lyubchenko', 'Malinovskiy', 'Minenko', 'Mirgorodskiy', 'Mikhaylovskiy', 'Nemchenko', 'Orel', 'Ostrovskiy', 'Okhrimenko', 'Panasenko', 'Perederiy', 'Pokrovskiy', 'Rud', 'Sobol', 'Stupak', 'Tretyak', 'Ustimenko', 'Chernyak', 'Sheremet', 'Aleshchenko', 'Belinskiy', 'Bondarchuk', 'But', 'Vdovenko', 'Volk', 'Grushko', 'Degtyarenko', 'Deynega', 'Zheleznyak', 'Kalita', 'Kobzar', 'Korotkiy', 'Kochura', 'Kravchuk', 'Lugovskoy', 'Nosenko', 'Pavlyuk', 'Polishchuk', 'Sushko', 'Tsokur', 'Shramko', 'Arkhipenko', 'Babeshko', 'Bakumenko', 'Gorbatenko', 'Gorskiy', 'Dudka', 'Zhurba', 'Zabara', 'Klochko', 'Kravets', 'Kucher', 'Kushnarenko', 'Loboda', 'Makhno', 'Novitskiy', 'Opryshko', 'Pelipenko', 'Pogrebnoy', 'Podoprigora', 'Poltavskiy', 'Potapenko', 'Priymak', 'Savchuk', 'Semak', 'Skiba', 'Skorik', 'Spivak', 'Tikhonenko', 'Shulzhenko', 'Shcherbak',
-- -- 						'Aleksandrova', 'Alekseeva', 'Andreeva', 'Andreeva', 'Anisimova', 'Antonova', 'Baranova', 'Belova', 'Belousova', 'Belyaeva', 'Blinova', 'Bobrova', 'Bogdanova', 'Bolshakova', 'Borisova', 'Vasileva', 'Veselova', 'Vinogradova', 'Volkova', 'Vorobeva', 'Gerasimova', 'Golubeva', 'Grigoreva', 'Gromova', 'Guseva', 'Davydova', 'Danilova', 'Denisova', 'Dmitrieva', 'Dorofeeva', 'Egorova', 'Ershova', 'Efimova', 'Zhukova', 'Zhuravleva', 'Zajceva', 'Zaxarova', 'Ivanova', 'Ilina', 'Kazakova', 'Kalinina', 'Kiseleva', 'Kovalyova', 'Kozlova', 'Kolesnikova', 'Komarova', 'Konovalova', 'Koroleva', 'Krylova', 'Kudryavceva', 'Kuznecova', 'Kuzmina', 'Kulikova', 'Lazareva', 'Lebedeva', 'Makarova', 'Maksimova', 'Markova', 'Matveeva', 'Medvedeva', 'Melnikova', 'Mironova', 'Mixajlova', 'Molchanova', 'Morozova', 'Nikitina', 'Nikiforova', 'Nikolaeva', 'Novikova', 'Orlova', 'Osipova', 'Pavlova', 'Panina', 'Petrova', 'Petuxova', 'Polyakova', 'Ponomareva', 'Popova', 'Pugacheva', 'Putina', 'Romanova', 'Romanova', 'Ryabova', 'Semenova', 'Sergeeva', 'Sidorova', 'Smirnova', 'Soboleva', 'Sokolova', 'Solovaeva', 'Sorokina', 'Stepanova', 'Suxanova', 'Tarasova', 'Timofeeva', 'Titova', 'Fedorova', 'Fedotova', 'Filippova', 'Fomina', 'Frolova', 'Cvetkova', 'Shestakova', 'Shherbakova', 'Yakovaleva'
-- -- 					]) as name2
-- -- 			) as namesw
-- -- 		) n

-- -- 	) q2;

-- -- -- select name, sex, count(*) 
-- -- --   from newPersonNames 
-- -- --  group by name, sex
-- -- -- having count(*) > 1;

-- -- update lt_party pt set name = n.name
-- --   from newPersonNames n
-- --  where pt.id = n.id;
        
-- -- update lt_party set 
-- --     legalName = name,
-- --     phone1 = null, phone2 = null,
-- --     email1 = null, email2 = null,
-- --     fax = null, webAddress = null;


-- -- update lt_passport psp set 
-- --     firstname = name1,
-- --     lastname = name2,
-- --     gender = sex
-- --   from lt_party p
-- --     inner join newPersonNames n on p.id = n.id
-- --  where p.id = psp.owner;
    
    
/*** Пассажиры ***/

-- update lt_product_passenger psg set
--     passengerName = (
--         select 
--             concat(
--                 upper(replace(name, ' ', '/')), ' ',
--                 case sex when 1
--                     then case when random() > .5 then 'MRS' else 'MISS' end 
--                     else 'MR' 
--                 end
--             ) 
--           from newPersonNames pt
--          where psg.passenger = pt.id
--      )
--  where passenger is not null;

-- create temp table newPersonNameArr on commit drop
-- as
-- select array_agg(name) as names from newPersonNames;

-- update lt_product_passenger psg set
--     passengerName = (
--         select 
--             concat(
--                 upper(replace(name, ' ', '/')), ' ',
--                 case when right(name, 1) = 'a' 
--                     then case when random() > .5 then 'MRS' else 'MISS' end 
--                     else 'MR' 
--                 end
--             ) 
--           from (select names[random() * array_length(names, 1) + length(psg.id) - 32] as name from newPersonNameArr) q
--      )
--  where passenger is null;

 
-- /*** Переименовывание организаций ***/

drop table if exists newOrganizationNames;

create temp table newOrganizationNames --on commit drop
as
select
    id,
    name
  from (
    select
        id,
        trim(replace(concat(name1, ' ', name2, ' ', name3), '  ', ' ')) as name
      from (
        select
            id,
            names[1 + random() * (array_length(names, 1) - 1)] as name1,
            names[1 + random() * 1.2 * array_length(names, 1)] as name2,
            names[1 + random() * 4 * array_length(names, 1)] as name3
          from 
            (select id from lt_party 
              where class = 'Organization' and not isairline
			) o
            cross join (
                select array_agg(name) as names 
                  from (
                    select distinct regexp_split_to_table(name, E'\\s+|/') as name
                      from lt_party
                     where class = 'Organization'
                       and name not similar to '%[а-яА-Я(0-9]%'
                  ) q
                 where length(name) >= 3
            ) q2
        ) q3
    ) q4
  where not exists(select null from lt_party where name = q4.name);

delete from newOrganizationNames n1
 where exists(select null from newOrganizationNames where id <> n1.id and name = n1.name);

--select * from newOrganizationNames

update lt_party pt set 
    name = n.name
  from newOrganizationNames n
 where pt.id = n.id;



/*** GDS agents ***/

-- -- create temp table newGdsOffices on commit drop
-- -- as
-- -- select officeCode, concat('OFF', round(random() * 8999) + 1000) as newCode
-- --   from (
-- --     select distinct officeCode from lt_gds_agent
-- -- ) q;

-- -- update lt_gds_agent a set 
-- --     officeCode = (select newCode from newGdsOffices where officeCode = a.officeCode),
-- --     code = concat('AG', round(random() * 8999) + 1000);

-- -- update lt_avia_document d set
-- --     bookerOffice = (select officeCode from lt_gds_agent where person = booker limit 1),
-- --     bookerCode = (select code from lt_gds_agent where person = booker limit 1);

-- -- update lt_avia_document d set
-- --     ticketerOffice = (select officeCode from lt_gds_agent where person = ticketer limit 1),
-- --     ticketerCode = (select code from lt_gds_agent where person = ticketer limit 1);


-- -- /*** Имена пользователей ***/

-- -- update lt_identity i 
-- --    set name = concat('admin', rowno)
-- --   from (
-- -- 	select u.id, row_number() over (order by active desc, p.name) as rowno
-- -- 	  from lt_user u
-- -- 		inner join lt_party p on u.person = p.id
-- -- 	 where u.isadministrator
-- --    ) q
-- --  where i.id = q.id;

-- -- update lt_identity i 
-- --    set name = concat('supervisor', rowno)
-- --   from (
-- -- 	select u.id, row_number() over (order by active desc, p.name) as rowno
-- -- 	  from lt_user u
-- -- 		inner join lt_party p on u.person = p.id
-- -- 	 where u.issupervisor and not u.isadministrator
-- --    ) q
-- --  where i.id = q.id;

-- -- update lt_identity i 
-- --    set name = concat('agent', rowno)
-- --   from (
-- -- 	select u.id, row_number() over (order by active desc, p.name) as rowno
-- -- 	  from lt_user u
-- -- 		inner join lt_party p on u.person = p.id
-- -- 	 where u.isagent and not u.issupervisor and not u.isadministrator
-- --    ) q
-- --  where i.id = q.id;


/*** Названия, номера, даты ***/

-- -- drop table if exists aviaDocumentNumbers;

-- -- create temp table aviaDocumentNumbers
-- -- as
-- -- select
-- -- 	id,
-- -- 	number_ as old,
-- -- 	(number_ / 1000000000) * 1000000000 + (number_ % 1000000000) / 100000 + (number_ % 100000) * 10000 as new
-- --   from lt_avia_document
-- -- ;

-- -- update lt_avia_document a
-- --    set number_ = n.new
-- --   from aviaDocumentNumbers n
-- --  where a.id = n.id;

-- -- update lt_product a
-- --    set name = replace(name, lpad(n.old::citext2, 10, '0'), lpad(n.new::citext2, 10, '0'))
-- --   from aviaDocumentNumbers n
-- --  where a.id = n.id;



drop table if exists orderIssueDateOffsets;

create temp table orderIssueDateOffsets
as
select id, round(random() * 30 - 15 + 3 * 365) * '1 day'::INTERVAL as offset
  from lt_order;

update lt_order o set 
	issuedate = issuedate + oo.offset
  from orderIssueDateOffsets oo
 where o.id = oo.id;

update lt_product p set 
	issuedate = issuedate + oo.offset
  from orderIssueDateOffsets oo
 where p.order_ = oo.id;

update lt_payment p set 
	date_ = date_ + oo.offset
  from orderIssueDateOffsets oo
 where p.order_ = oo.id;



update lt_system_configuration 
   set companydetails = replace(replace(replace(replace(companydetails, '1', '9'), '2', '8'), '3', '7'), '4', '6');


/*** Нестабильное... ***/

-- -- update lt_passport set
-- --     number_ = concat('XX', round(89999999 * random()) + 10000000);


--commit;
--rollback;
