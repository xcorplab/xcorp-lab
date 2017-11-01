--tabela para tratar nomes com diferentes idiomas
declare @dDataIngles table(
	DateKey int NOT NULL,
	InglesDiaNomeSemana nvarchar(10) NULL,
	InglesMesNome nvarchar(10) NULL
)

declare @start datetime,
@end datetime
set @start = '2015-01-01'
set @end = '2100-01-01'
;with calendar(id, date, isweekday, y, s, q, m, d, dw, dy, monthname, dayname, w, fq, fy, fs) as
(
	select
		convert(int, convert(varchar, @start, 112)),
		@start ,
		case when datepart(dw,@start) in (1,7) then 0 else 1 end,
		year(@start),
		case when datepart(mm,@start) in(1,2,3,4,5,6) then 1 else 2 end,
		datepart(qq,@start),
		datepart(mm,@start),
		datepart(dd,@start),
		datepart(dw,@start),
		datepart(dy,@start),
		datename(month,@start),
		datename(dw,@start),
		datepart(wk,@start),
		case when datepart(qq,@start) in(1,2) then datepart(qq,@start) + 2 else datepart(qq,@start) - 2 end,
		case when datepart(mm,@start) in(1,2,3,4,5,6) then year(@start) else year(@start) + 1 end,
		case when datepart(mm,@start) in(1,2,3,4,5,6) then 2 else 1 end
	union all
	select
		convert(int, convert(varchar, date + 1, 112)),
		date + 1,
		case when datepart(dw,date + 1) in (1,7) then 0 else 1 end,
		year(date + 1),
		case when datepart(mm,date + 1) in(1,2,3,4,5,6) then 1 else 2 end,
		datepart(qq,date + 1),
		datepart(mm,date + 1),
		datepart(dd,date + 1),
		datepart(dw,date + 1),
		datepart(dy,date + 1),
		datename(month,date + 1),
		datename(dw,date + 1),
		datepart(wk,date + 1),
		case when datepart(qq,date + 1) in(1,2) then datepart(qq, date + 1) + 2 else datepart(qq, date + 1) - 2 end,
		case when datepart(mm,date + 1) in(1,2,3,4,5,6) then year(date + 1) else year(date + 1) + 1 end,
		case when datepart(mm,date + 1) in(1,2,3,4,5,6) then 2 else 1 end
	
	from calendar where date + 1< @end
)
--select * from calendar option(maxrecursion 32767)
--insert into printlaser_dw.dbo.DimData
--select
--	id as DateKey,
--	date as FullDateAlternateKey,
--	isweekday as DiaUtil,
--	dw as DiaNumeroSemana,
--	null as InglesDiaNomeSemana,
--	dayname as PortuguesDiaNomeSemana,
--	d as DiaNumeroMes,
--	dy as DiaNumeroAno,
--	w as SemanaNumeroAno,
--	null as InglesMesNome,
--	monthname as PortuguesMesNome,
--	m as MesNumeroAno,
--	q as CalendarioTrimestre,
--	y as CalendarioAno,
--	s as CalendarioSemestre,
--	fq as FiscalTrimestre,
--	fy as FiscalAno,
--	fs as FiscalSemestre

--from calendar option(maxrecursion 32767)

--inserir valor em ingles
insert into @dDataIngles
select
	id as DateKey,
	dayname as InglesDiaNomeSemana,
	monthname as InglesMesNome
from calendar option(maxrecursion 32767)
--select * from @dDataIngles

update PRINTLASER_DW.dbo.DimData
set DimData.InglesDiaNomeSemana = DataIngles.InglesDiaNomeSemana, DimData.InglesMesNome = DataIngles.InglesMesNome
from @dDataIngles as DataIngles
where 
	DimData.DateKey = DimData.DateKey




--SELECT DATENAME(year, '12:10:30.123')
--    ,DATENAME(month, '12:10:30.123')
--    ,DATENAME(day, '12:10:30.123')
--    ,DATENAME(dayofyear, '12:10:30.123')
--    ,DATENAME(weekday, '12:10:30.123');

--SELECT CONVERT(int, CONVERT(varchar, GETDATE(), 112))

--SELECT DATENAME(hour, '2007-06-01')
--    ,DATENAME(minute, '2007-06-01')
--    ,DATENAME(second, '2007-06-01');
