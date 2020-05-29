drop table if exists  shutdowns;
create table logouts (username text , hour_min text);
insert into logouts values ('saray','0920');
select hour_min from logouts;
insert into logouts values ('papa','1020');

delete from logouts where username like 'papa';
update logouts set hour_min='1745';



select app,username,sum(coalesce(minutes,0))::integer from (
	                                        select app,username,
	                                        extract(epoch from (coalesce(end_time, now()) - start_time))/60 as minutes 
                                            from daily_apps 
	                                        union all
	                                        select app,username,
	                                        extract(epoch from (coalesce(end_time, now()) - start_time))/ 60 as minutes 
                                            from hist_apps where start_time > now()
                                        )t
                                        group by app,username order by 3 desc limit 10;


										select * from logouts;