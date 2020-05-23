drop table if exists  shutdowns;
create table logouts (username text , hour_min text);
insert into logouts values ('saray','0920');
select * from logouts;
insert into logouts values ('papa','1020');