use PilotFort
go

if object_id('dbo.UK_AppUser_AspNetUsers', 'UQ') is null
	alter table dbo.AppUser add constraint UK_AppUser_AspNetUsers unique (AspNetUserId)
go

if col_length('dbo.AppUser', 'TimeZone') is null
	alter table dbo.AppUser add TimeZone varchar(50) not null constraint DF_AppUser_TimeZone default 'Eastern Standard Time'
go

if object_id('dbo.FlightTime', 'U') is not null
	drop table dbo.FlightTime
go

if object_id('dbo.FlightLog', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.FlightLog(
		FlightLogId uniqueidentifier not null constraint PK_FlightLog primary key constraint DF_FlightLog_Id default newsequentialid(),
		AppUserId uniqueidentifier not null constraint FK_FlightLog_AppUser references dbo.AppUser,
		AircraftId nvarchar(10) not null constraint FK_FlightLog_Aircraft references dbo.Aircraft,
		FlightBeginUtc datetime2,
		FlightEndUtc datetime2,
		HobbsBegin decimal(10,2),
		HobbsEnd decimal(10,2),
		HobbsHours decimal(5,2),
		NightLandings tinyint not null constraint DF_FlightLog_NightLandings default 0,
		CreatedDateUtc datetime2 not null constraint DF_FlightLog_CreatedDateUtc default sysutcdatetime(),
		CreatedByUserId uniqueidentifier not null constraint FK_FlightLog_CreatedBy references dbo.AppUser,
	)
end
go

declare @AccountName nvarchar(50) = 'SPECTACULAIR LLC'
declare @CreatedByUserId uniqueidentifier = (
	select top 1 AppUserId from dbo.AppUser
)
declare @SpectaculairAccountId uniqueidentifier = (
	select AccountId from dbo.Account where AccountName = @AccountName
)
if @SpectaculairAccountId is null
	insert dbo.Account (AccountName, CreatedByUserId)
	select @AccountName, @CreatedByUserId
	set @SpectaculairAccountId = (
		select AccountId from dbo.Account where AccountName = @AccountName
	)
insert dbo.AccountUser (AccountId, AppUserId)
select @SpectaculairAccountId, u.AppUserId
from dbo.AppUser u left join dbo.AccountUser a on u.AppUserId = a.AppUserId
where a.AppUserId is null
if not exists (select 1 from dbo.AccountAircraft where AccountId = @SpectaculairAccountId)
	insert dbo.AccountAircraft (AccountId, AircraftId, CreatedByUserId)
	select @SpectaculairAccountId, AircraftId, @CreatedByUserId
	from dbo.Aircraft
	where RegistrantName = @AccountName
go

if object_id('dbo.Aircraft_Find', 'P') is not null
	drop procedure dbo.Aircraft_Find
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.Aircraft_Find
	@AppUserId uniqueidentifier = null
as
begin
	select a.AircraftId, a.SerialNumber, a.AircraftModelId, a.YearManufactured,
		m.Manufacturer, m.Model
	from dbo.Aircraft a
		join dbo.AircraftModel m on a.AircraftModelId = m.AircraftModelId
		join dbo.AccountAircraft c on a.AircraftId = c.AircraftId
		join dbo.AccountUser u on c.AccountId = u.AccountId
	where (@AppUserId is null
		or @AppUserId = '00000000-0000-0000-0000-000000000000'
		or @AppUserId = u.AppUserId)
end
go

if object_id('dbo.Aircraft_Get', 'P') is not null
	drop procedure dbo.Aircraft_Get
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.Aircraft_Get
	@AircraftId uniqueidentifier
as
begin
	select a.AircraftId, a.SerialNumber, a.AircraftModelId, a.YearManufactured,
		m.Manufacturer, m.Model
	from dbo.Aircraft a
		join dbo.AircraftModel m on a.AircraftModelId = m.AircraftModelId
	where a.AircraftId = @AircraftId
end
go

if object_id('dbo.AppUser_Get', 'P') is not null
	drop procedure dbo.AppUser_Get
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.AppUser_Get
	@AppUserId uniqueidentifier
as
begin
	select *
	from dbo.AppUser u
	join dbo.AspNetUsers s on u.AspNetUserId = s.Id
	left join (select top 1 a.AccountId, a.AppUserId, a.IsAccountAdmin
		from dbo.AccountUser a where a.AppUserId = @AppUserId
		) x on u.AppUserId = x.AppUserId
	where u.AppUserId = @AppUserId
end
go

if object_id('dbo.AppUser_GetByUserName', 'P') is not null
	drop procedure dbo.AppUser_GetByUserName
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.AppUser_GetByUserName
	@UserName nvarchar(256)
as
begin
	declare @AspNetUserId nvarchar(128) = (
		select top 1 Id from dbo.AspNetUsers where UserName = @UserName
	)
	declare @AppUserId uniqueidentifier = (
		select top 1 AppUserId from dbo.AppUser where AspNetUserId = @AspNetUserId
	)
	if @AppUserId is null and @AspNetUserId is not null
	begin
		declare @table table (AppUserId uniqueidentifier)
		insert into @table (AppUserId)
		exec dbo.AppUser_Insert @UserName
		select @AppUserId = AppUserId from @table
	end
	exec dbo.AppUser_Get @AppUserId
end
go

if object_id('dbo.AppUser_Insert', 'P') is not null
	drop procedure dbo.AppUser_Insert
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.AppUser_Insert
	@UserName nvarchar(256)
as
begin
	declare @table table (Id uniqueidentifier)
	declare @AspNetUserId nvarchar(128) = (
		select Id from dbo.AspNetUsers where UserName = @UserName
	)
	insert dbo.AppUser (AspNetUserId, FirstName, LastName, CreatedByUserId)
	output inserted.AppUserId into @table
	values (@AspNetUserId, '', '', @AspNetUserId)
	select Id from @table
end
go

if object_id('dbo.FlightTime_Add', 'P') is not null
	drop procedure dbo.FlightTime_Add
go
if object_id('dbo.FlightLog_Add', 'P') is not null
	drop procedure dbo.FlightLog_Add
go

if object_id('dbo.FlightTime_Find', 'P') is not null
	drop procedure dbo.FlightTime_Find
go
if object_id('dbo.FlightLog_Find', 'P') is not null
	drop procedure dbo.FlightLog_Find
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.FlightLog_Find
	@AppUserId uniqueidentifier = null,
	@FlightDateUtc date = null
as
begin
	select l.*, m.Manufacturer, m.Model
	from dbo.FlightLog l
		join dbo.Aircraft a on l.AircraftId = a.AircraftId
		join dbo.AircraftModel m on a.AircraftModelId = m.AircraftModelId
	where (@AppUserId is null or l.AppUserId = @AppUserId)
		and (@FlightDateUtc is null
			or @FlightDateUtc = convert(date, l.FlightBeginUtc)
			or @FlightDateUtc = convert(date, l.FlightEndUtc))
	order by l.FlightBeginUtc desc
end
go

if object_id('dbo.FlightTime_Get', 'P') is not null
	drop procedure dbo.FlightTime_Get
go
if object_id('dbo.FlightLog_Get', 'P') is not null
	drop procedure dbo.FlightLog_Get
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.FlightLog_Get
	@FlightLogId uniqueidentifier
as
begin
	select l.*
	from dbo.FlightLog l
	where l.FlightLogId = @FlightLogId
end
go

if object_id('dbo.FlightLog_Insert', 'P') is not null
	drop procedure dbo.FlightLog_Insert
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.FlightLog_Insert
	@AppUserId uniqueidentifier,
	@AircraftId nvarchar(10),
	@FlightBeginUtc datetime2(7) = null,
	@FlightEndUtc datetime2(7) = null,
	@HobbsBegin decimal(10,2) = null,
	@HobbsEnd decimal(10,2) = null,
	@HobbsHours decimal(5,2) = null,
	@NightLandings tinyint,
	@CreatedByUserId uniqueidentifier
as
begin
	declare @table table (Id uniqueidentifier)
	insert dbo.FlightLog (AppUserId, AircraftId, FlightBeginUtc, FlightEndUtc,
		HobbsBegin, HobbsEnd, HobbsHours, NightLandings, CreatedByUserId)
	output inserted.FlightLogId into @table
	values (@AppUserId, @AircraftId, @FlightBeginUtc, @FlightEndUtc,
		@HobbsBegin, @HobbsEnd, @HobbsHours, @NightLandings, @CreatedByUserId)
	select Id from @table
end
go

if object_id('dbo.FlightTime_Update', 'P') is not null
	drop procedure dbo.FlightTime_Update
go
if object_id('dbo.FlightLog_Update', 'P') is not null
	drop procedure dbo.FlightLog_Update
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.FlightLog_Update
	@FlightLogId uniqueidentifier,
	@AppUserId uniqueidentifier,
	@AircraftId nvarchar(10),
	@FlightBeginUtc datetime2(7) = null,
	@FlightEndUtc datetime2(7) = null,
	@HobbsBegin decimal(10,2) = null,
	@HobbsEnd decimal(10,2) = null,
	@HobbsHours decimal(5,2) = null,
	@NightLandings tinyint
as
begin
	update dbo.FlightLog
	set AppUserId = @AppUserId,
		AircraftId = @AircraftId,
		FlightBeginUtc = @FlightBeginUtc,
		FlightEndUtc = @FlightEndUtc,
		HobbsBegin = @HobbsBegin,
		HobbsEnd = @HobbsEnd,
		HobbsHours = @HobbsHours,
		NightLandings = @NightLandings
	where FlightLogId = @FlightLogId
end
go
