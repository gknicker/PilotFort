use PilotFort
go

/*
drop table dbo.FlightTime
drop table dbo.DutyPeriod
drop table dbo.ScheduleEntry
drop table dbo.ScheduleTemplateEntry
drop table dbo.ScheduleTemplateUser
drop table dbo.ScheduleTemplate
drop table dbo.AccountAircraft
drop table dbo.Aircraft
drop table dbo.AircraftModel
drop table dbo.AccountUser
drop table dbo.Account
drop table dbo.AppError
drop table dbo.AppUser
drop table dbo.AspNetUserClaims
drop table dbo.AspNetUserLogins
drop table dbo.AspNetUserRoles
drop table dbo.AspNetRoles
drop table dbo.AspNetUsers
drop table dbo.__MigrationHistory
*/

if object_id('dbo.__MigrationHistory', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.__MigrationHistory(
		MigrationId nvarchar(150) not null,
		ContextKey nvarchar(300) not null,
		Model varbinary(max) not null,
		ProductVersion nvarchar(32) not null,
		constraint PK___MigrationHistory primary key (MigrationId, ContextKey),
	)
	insert dbo.__MigrationHistory select '201704012103392_InitialCreate', 'PilotFort.Models.ApplicationDbContext', 0x1F8B0800000000000400DD5C6D6FE33612FE7EC0FD07419F7A87D4CACBED622FB05BA44EDC0B6EF38275B6E8B7052DD10EB112A54A549AE0D05FD60FFD49FD0B37942859E28B5E6CC5768A051611397C66381C92C3E1D07FFEFEC7F8FBE7C0B79E709C90904EEC93D1B16D61EA861EA1AB899DB2E5B71FECEFBFFBFBDFC6575EF06CFD54D09D713A68499389FDC85874EE3889FB8803948C02E2C661122ED9C80D030779A1737A7CFC6FE7E4C4C100610396658D3FA5949100671FF0390DA98B239622FF26F4B09F8872A89967A8D62D0A701221174FEC7BE2876C16C66C94D3DAD6854F10C831C7FED2B610A521430CA43CFF9CE0398B43BA9A475080FC87970803DD12F90916D29FAFC9BB76E4F89477C459372CA0DC346161D013F0E44C68C6919B6FA45FBBD41CE8EE0A74CC5E78AF33FD4DEC6B0F67459F421F1420333C9FFA31279ED837258B8B24BAC56C54341CE590B318E07E0DE3AFA32AE291D5B9DD516949A7A363FEEFC89AA63E4B633CA1386531F28FACFB74E113F7BFF8E521FC8AE9E4EC64B13CFBF0EE3DF2CEDEFF0B9FBDABF614FA0A74B50228BA8FC308C7201B5E96FDB72DA7DECE911B96CD2A6D72AD802DC1A4B0AD1BF4FC11D3157B84E972FAC1B666E4197B458930AECF94C01C82462C4EE1F336F57DB4F07159EF34F2E4FF37703D7DF77E10AEB7E889ACB2A197F8C3C489615E7DC27E569B3C92289F5EB5F1FE22C8667118F0EFBA7DE5B55FE6611ABBBC33A191E401C52BCCEAD28D9DB5F17632690E35BC5917A8876FDA5C52D5BCB5A4BC439BCC8482C5AE674321EFEBF2ED6C7117510483979916D74893C1C95BD5486A0B965050ACCDE6A4ABD950E8CE5F7915BC0A10F10758063B7001FF6349E20097BDFC2104A343B4B7CCF728496015F0FE8392C706D1E1CF01449F63378DC138E70C05D1AB73BB7F0C29BE4D8305B7F9DDF11A6C681E7E0D67C865617C4579ABADF13E86EED7306557D4BB440C7F666E01C83F1F48D01D6010712E5C1727C90C8C197BD310DCEB02F09AB2B3D3DE707C79DAB71B32F51109F47E88B4907E2948D7BE889E42F1470C643A9FA449D48FE18AD06EA216A46651738A565105595F513958374905A559D08CA055CE9C6A302F2F1BA1E1DDBC0CF6F0FDBCED366FD35A5051E31C5648FC23A6388665CCBB478CE198AE47A0CBBAB10F67211B3ECEF4D5F7A68CD34FC84F8766B5D16CC81681E16743067BF8B32113138A9F88C7BD920E879F8218E03BD1EBCF55ED734E926CD7D3A1D6CD5D33DFCD1A609A2E174912BA249B059AB097085AD4E5071FCE6A8F60E4BD91A320D0313074C2B73C2881BED9B251DDD14BEC6386AD0B370F0B4E51E2224F552374C8EB2158B1A36A045B4743EAC2FD53E109968E63DE08F1435002339550A64E0B425D1221BF554B52CB8E5B18EF7BC943AEB9C411A69C61AB26BA30D7073FB800251F6950DA3434762A16D76C8806AFD534E66D2EEC7ADC9598C44E6CB2C57736D8A5F0DF5EC5309B35B603E36C564917018C81BC7D18A838AB743500F9E07268062A9D980C062A5CAA9D18685D637B30D0BA4ADE9C81E647D4AEE32F9D570FCD3CEB07E5DD6FEB8DEADA836DD6F47160A699FB9ED086410B1CABE679B9E095F899690E6720A7389F25C2D5954D8483CF31AB876CD6FEAED60F759A4164236A025C1B5A0BA8B80254809409D543B82296D7289DF0227AC01671B74658B1F64BB0151B50B1AB57A11542F385A96C9C9D4E1F65CF4A6B508CBCD361A182A3310879F1AA77BC83524C715955315D7CE13EDE70A56362301A14D4E2B91A94547466702D15A6D9AE259D43D6C725DB4A4B92FB64D052D199C1B5246CB45D491AA7A0875BB0958AEA5BF84093AD887494BB4D593776F2FC285130760C8954E31B144584AE2A8955A2C49AE75955D36FE7FD138E821CC371134DDE51296DC98985315A61A9165883A4331227EC1231B4403CCE33F502854CBBB71A96FF826575FB5407B1D8070A6AFEB7089FC917F7B59D56754504C20CFA17707F260BA26B465FDFDCE2696EC847B1266E3F0DFD34A066F7CADC3ABFBDABB6CF4B5484B123C9AFB84F8AAE1427B7AEF84EC3A24E894186A8F45D361F26338449D985E75955B7C91B35A314C1A92A8A2960B5B7613339313D864AF60EFB8F542BC2EBCC299192520510453D312A590D0A58A5AE3B6A3DF1A48A59AFE98E2865975421A5AA1E525673486A42562B36C23368544FD19D839A355245576BBB236BF247AAD09AEA0DB03532CB75DD5135292655604D7577EC75BE89BC841EF0AE653CB36CB86DE587DAEDF62D03C6EBAC87C36C7B95BBFB2A50A5B82796B89D57C044F941DA92F164B7A12DE5918CED6CC980615E756A77DEF545A7F1A2DE8C59BBC8AE2DEC4D17F966BC7E16FBAA76A11CEB6492927B79BC938E716371A46A7F34A39CB17212DB2AD4089BFA4BC27030E204A3F92FFED427982FE105C10DA264891396276FD8A7C727A7D2CB9BC37905E32489E76B8EA4A6A730F531DB411E167D42B1FB8862352B628B97226B5025E07C4D3DFC3CB1FF97B53ACF6217FCAFACF8C8BA4E3E53F24B0A150F718AADDFD42CCF6132E79B8F5707FACEA1BB56AF7FFE92373DB2EE629831E7D6B1A4CB4D46B8FEFAA1973479D32DA4D9F84DC4DB9D50B547075A5469426CFEC66041D820EF0B0A29BF09D0F33FFA8AA67D43B015A2E69DC0507883A8D0F40E60132CE31B000F3E59F606A05F67F56F023611CDF81E80D0FE60F26B80EECB50D1728F5B8DE644B48B2529D3736B36F556A995FBDE9B94A4EBAD26BA9A58DD036E8BE4E90D2CE38DE51D0FB63B6AD28A07C3DEA769BF7A2EF1A1A40FAF133BF69B35BCCB44E1860BA1BF547EF00164B4693274F69F05BC6B5B3345710F3C95B25FAEEF81199BC8DBDA7F46EFAE8DCD14E63D7063EB95B77B60B6B6AFFD73CF96D6790BDD7B16AE9A5064B88DD1C582DBB26CF3C0399CF017211841EE51E68F23F5695D4D29A92D0CD72466A6E67C3299B1327114BE0A4533DB7E7D151B7E6367054D335B431666136FB1FE37F21634CDBC0DB98DFBC80FD66617EA72B65BD6B1A6F4A7B7940F5CEB494BFA799BCFDA78B5FE96D27F07514A6DF618EE88DF4EB6EF202A1972EAF4C8EE55AF7B61EFACFC9222ECDF0959AD21F8EF2A52ECD676CD92E69A2EC362F396242A48A408CD0D66C8832DF5226664895C06D53CC69CBDEECEE276FCA66381BD6B7A97B22865D0651C2CFC5AC08B3B014DFCB314E6BACCE3BB28FBA19221BA0062121E9BBFA33FA4C4F74AB9679A989001827B1722A2CBC792F1C8EEEAA544BA0D694720A1BED2297AC041E403587247E7E8096F221B98DF47BC42EECB3A026802691F88BADAC79704AD6214240263DD1E3EC186BDE0F9BBFF034B6358CF50540000, '6.1.3-40302'
end
go

if object_id('dbo.AspNetUsers', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AspNetUsers(
		Id nvarchar(128) not null constraint PK_AspNetUsers primary key,
		Email nvarchar(256) null,
		EmailConfirmed bit not null,
		PasswordHash nvarchar(max) null,
		SecurityStamp nvarchar(max) null,
		PhoneNumber nvarchar(max) null,
		PhoneNumberConfirmed bit not null,
		TwoFactorEnabled bit not null,
		LockoutEndDateUtc datetime2 null,
		LockoutEnabled bit not null,
		AccessFailedCount int not null,
		UserName nvarchar(256) not null,
	)
	insert dbo.AspNetUsers select 'ede5c239-c223-4dd3-95f4-d9a6da7c8b03', 'nick.knickerbocker@gmail.com', 0, 'ADtRCX1TWh2IVO+i/JdlUU/HywzgGh86Rec3QdbYCzFbX7vbdiayQwZ97Q01QPthHg==', '395cdf8c-8fc1-4dac-88c5-bc45abb7f38d', null, 0, 0, null, 1, 0, 'nick.knickerbocker@gmail.com'
end
go

if object_id('dbo.AspNetRoles', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AspNetRoles(
		Id nvarchar(128) not null constraint PK_AspNetRoles primary key,
		Name nvarchar(256) not null,
	)
end
go

if object_id('dbo.AspNetUserRoles', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AspNetUserRoles(
		UserId nvarchar(128) not null constraint FK_AspNetUserRoles_AspNetUsers references dbo.AspNetUsers,
		RoleId nvarchar(128) not null constraint FK_AspNetUserRoles_AspNetRoles references dbo.AspNetRoles,
		constraint PK_AspNetUserRoles primary key (UserId, RoleId),
	)
end
go

if object_id('dbo.AspNetUserLogins', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AspNetUserLogins(
		LoginProvider nvarchar(128) not null constraint PK_AspNetUserLogins primary key,
		ProviderKey nvarchar(128) not null,
		UserId nvarchar(128) not null constraint FK_AspNetUserLogins_AspNetUsers references dbo.AspNetUsers,
	)
end
go

if object_id('dbo.AspNetUserClaims', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AspNetUserClaims(
		Id int identity not null constraint PK_AspNetUserClaims primary key,
		UserId nvarchar(128) not null constraint FK_AspNetUserClaims_AspNetUsers references dbo.AspNetUsers,
		ClaimType nvarchar(max) null,
		ClaimValue nvarchar(max) null,
	)
end
go

if object_id('dbo.AppUser', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AppUser(
		AppUserId uniqueidentifier not null constraint PK_AppUser primary key constraint DF_AppUser_Id default newsequentialid(),
		AspNetUserId nvarchar(128) not null constraint FK_AppUser_AspNetUsers references dbo.AspNetUsers,
		FirstName nvarchar(50) not null,
		LastName nvarchar(100) not null,
		IsSiteAdmin bit not null constraint DF_AppUser_IsSiteAdmin default 0,
		CreatedDateUtc datetime2 not null constraint DF_AppUser_CreatedDateUtc default sysutcdatetime(),
		CreatedByUserId nvarchar(128) not null constraint FK_AppError_CreatedBy references dbo.AspNetUsers,
	)
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
	@Email nvarchar(256)
as
begin
	declare @AspNetUserId nvarchar(128) = (select Id from dbo.AspNetUsers where Email = @Email)
	insert dbo.AppUser (AspNetUserId, FirstName, LastName, CreatedByUserId)
		select @AspNetUserId, '', '', @AspNetUserId
end
go

if object_id('dbo.AppError', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AppError(
		AppErrorId uniqueidentifier not null constraint PK_AppError primary key constraint DF_AppError_Id default newsequentialid(),
		ErrorClass varchar(255) not null,
		ErrorMessage varchar(1024) not null,
		StackTrace varchar(max) not null,
		OccurredUtc datetime2 not null constraint DF_AppError_OccurredUtc default sysutcdatetime(),
		AppUserId uniqueidentifier constraint FK_AppError_AppUser references dbo.AppUser,
	)
end
go

if object_id('dbo.AppError_Insert', 'P') is not null
	drop procedure dbo.AppError_Insert
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.AppError_Insert
	@AppErrorId uniqueidentifier
	, @ErrorClass varchar(255)
	, @ErrorMessage varchar(1024)
	, @StackTrace varchar(max)
	, @AppUserId nvarchar(128) = null
as
begin
	insert dbo.AppError (AppErrorId, ErrorClass, ErrorMessage, StackTrace, AppUserId)
		select @AppErrorId, @ErrorClass, @ErrorMessage, @StackTrace, @AppUserId
end
go

if object_id('dbo.Account', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.Account(
		AccountId uniqueidentifier not null constraint PK_Account primary key constraint DF_Account_Id default newsequentialid(),
		AccountName nvarchar(128) not null constraint UK_Account unique,
		CreatedDateUtc datetime2 not null constraint DF_Account_CreatedDateUtc default sysutcdatetime(),
		CreatedByUserId uniqueidentifier not null constraint FK_Account_AppUser references dbo.AppUser,
	)
end
go

if object_id('dbo.AccountUser', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AccountUser(
		AccountId uniqueidentifier not null constraint FK_AccountUser_Account references dbo.Account,
		AppUserId uniqueidentifier not null constraint FK_AccountUser_AppUser references dbo.AppUser,
		IsAccountAdmin bit not null constraint DF_AccountUser_IsAccountAdmin default 0,
		ConfirmedByUserId uniqueidentifier constraint FK_AccountUser_ConfirmedBy references dbo.AppUser,
		constraint PK_AccountUser primary key (AccountId, AppUserId),
	)
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
	join (select top 1 a.AccountId, a.AppUserId, a.IsAccountAdmin
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
	declare @AppUserId uniqueidentifier = (select top 1 u.AppUserId from dbo.AppUser u
		join dbo.AspNetUsers s on u.AspNetUserId = s.Id where s.UserName = @UserName)
	exec dbo.AppUser_Get @AppUserId
end
go

if object_id('dbo.Account_FindByUser', 'P') is not null
	drop procedure dbo.Account_FindByUser
go
set ansi_nulls on
go
set quoted_identifier on
go
create procedure dbo.Account_FindByUser
	@AppUserId uniqueidentifier
as
begin
	select a.*
	from dbo.Account a
	join dbo.AccountUser u on a.AccountId = u.AccountId
	where u.AppUserId = @AppUserId
end
go

if object_id('dbo.AircraftModel', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AircraftModel(
		AircraftModelId char(7) not null constraint PK_AircraftModel primary key,
		Manufacturer nvarchar(30) not null,
		Model nvarchar(50) not null,
		AircraftType tinyint not null,
		EngineType tinyint not null,
		Category tinyint not null,
		Certified bit not null,
		Engines tinyint not null,
		Seats smallint not null,
		WeightClass tinyint not null,
		SpeedMph smallint,
	)
end
go

if object_id('dbo.Aircraft', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.Aircraft(
		AircraftId nvarchar(10) not null constraint PK_Aircraft primary key,
		SerialNumber nvarchar(30) not null,
		AircraftModelId char(7) not null constraint FK_Aircraft_AircraftModel references dbo.AircraftModel,
		EngineModelId char(5) not null,
		YearManufactured decimal(4,0) not null,
		RegistrantType tinyint not null,
		RegistrantName nvarchar(50) not null,
		RegistrantStreet nvarchar(33) not null,
		RegistrantStreet2 nvarchar(33) not null,
		RegistrantCity nvarchar(18) not null,
		RegistrantState char(2) not null,
		RegistrantZipCode nvarchar(10) not null,
		RegistrantRegion char(1) not null,
		RegistrantCounty char(3) not null,
		RegistrantCountry char(2) not null,
		LastActionDate date not null,
		CertIssueDate date,
		Certification nvarchar(10),
		AirWorthyDate date,
		ExpirationDate date,
		UniqueId char(8),
		KitManufacturer nvarchar(30),
		KitModel nvarchar(20),
	)
end
go

if object_id('dbo.AccountAircraft', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.AccountAircraft(
		AccountId uniqueidentifier not null constraint FK_AccountAircraft_AccountId references dbo.Account,
		AircraftId nvarchar(10) not null constraint FK_AccountAircraft_AircraftId references dbo.Aircraft,
		CreatedDateUtc datetime2 not null constraint DF_AccountAircraft_CreatedDateUtc default sysutcdatetime(),
		CreatedByUserId uniqueidentifier not null constraint FK_AccountAircraft_AppUser references dbo.AppUser,
	)
end
go

if object_id('dbo.ScheduleTemplate', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.ScheduleTemplate(
		ScheduleTemplateId uniqueidentifier not null constraint PK_ScheduleTemplate primary key constraint DF_ScheduleTemplate_Id default newsequentialid(),
		BeginDate date not null,
		EndDate date not null,
		CreatedDateUtc datetime2 not null constraint DF_ScheduleTemplate_CreatedDateUtc default sysutcdatetime(),
		CreatedByUserId uniqueidentifier not null constraint FK_ScheduleTemplate_AppUser references dbo.AppUser,
	)
end
go

if object_id('dbo.ScheduleTemplateUser', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.ScheduleTemplateUser(
		ScheduleTemplateId uniqueidentifier not null constraint FK_ScheduleTemplateUser_ScheduleTemplate references dbo.ScheduleTemplate,
		AppUserId uniqueidentifier not null constraint FK_ScheduleTemplateUser_AppUser references dbo.AppUser,
		constraint PK_ScheduleTemplateUser primary key (ScheduleTemplateId, AppUserId),
	)
end
go

if object_id('dbo.ScheduleTemplateEntry', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.ScheduleTemplateEntry(
		ScheduleTemplateEntryId uniqueidentifier not null constraint PK_ScheduleTemplateEntry primary key constraint DF_ScheduleTemplateEntry_Id default newsequentialid(),
		ScheduleTemplateId uniqueidentifier not null constraint FK_ScheduleTemplateEntry_ScheduleTemplate references dbo.ScheduleTemplate,
		BeginTime time(0) not null,
		EndTime time(0) not null,
		Frequency varchar(10) not null constraint CK_ScheduleTemplateEntry_Frequency check (Frequency in ('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday')) 
	)
end
go

if object_id('dbo.ScheduleEntry', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.ScheduleEntry(
		ScheduleEntryId uniqueidentifier not null constraint PK_ScheduleEntry primary key constraint DF_ScheduleEntry_Id default newsequentialid(),
		ScheduleTemplateEntryId uniqueidentifier constraint FK_ScheduleEntry_ScheduleTemplateEntry references dbo.ScheduleTemplateEntry,
		AppUserId uniqueidentifier not null constraint FK_ScheduleEntry_AppUser references dbo.AppUser,
		BeginDateUtc smalldatetime not null,
		EndDateUtc smalldatetime not null,
	)
end
go

if object_id('dbo.DutyPeriod', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.DutyPeriod(
		DutyPeriodId uniqueidentifier not null constraint PK_DutyPeriod primary key constraint DF_DutyPeriod_Id default newsequentialid(),
		AppUserId uniqueidentifier not null constraint FK_DutyPeriod_AppUser references dbo.AppUser,
		ScheduleEntryId uniqueidentifier constraint FK_DutyPeriod_ScheduleEntry references dbo.ScheduleEntry,
		BeginDateUtc smalldatetime not null,
		EndDateUtc smalldatetime not null,
		EndConfirmed bit not null constraint DF_DutyPeriod_EndConfirmed default 0,
	)
end
go

if object_id('dbo.FlightTime', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	create table dbo.FlightTime(
		FlightTimeId uniqueidentifier not null constraint PK_FlightTime primary key constraint DF_FlightTime_Id default newsequentialid(),
		AppUserId uniqueidentifier not null constraint FK_FlightTime_AppUser references dbo.AppUser,
		AircraftId nvarchar(10) not null constraint FK_FlightTime_Aircraft references dbo.Aircraft,
		FlightBeginUtc datetime2,
		FlightEndUtc datetime2,
		HobbsBegin decimal(10,2),
		HobbsEnd decimal(10,2),
		HobbsHours decimal(5,2),
		IsNightLanding bit not null constraint DF_FlightTime_NightLanding default 0,
		CreatedDateUtc datetime2 not null constraint DF_FlightTime_CreatedDateUtc default sysutcdatetime(),
		CreatedByUserId uniqueidentifier not null constraint FK_FlightTime_CreatedBy references dbo.AppUser,
	)
end
go

if not exists (select 1 from sys.schemas where name = 'faa')
	exec('create schema faa')
go

if object_id('faa.ACFTREF', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	set ansi_padding on
	create table faa.ACFTREF(
		[CODE] varchar(50),
		[MFR] varchar(50),
		[MODEL] varchar(50),
		[TYPE-ACFT] varchar(50),
		[TYPE-ENG] varchar(50),
		[AC-CAT] varchar(50),
		[BUILD-CERT-IND] varchar(50),
		[NO-ENG] varchar(50),
		[NO-SEATS] varchar(50),
		[AC-WEIGHT] varchar(50),
		[SPEED] varchar(50)
	)
	set ansi_padding off
end
go

if object_id('faa.[MASTER]', 'U') is null
begin
	set ansi_nulls on
	set quoted_identifier on
	set ansi_padding on
	create table faa.[MASTER](
		[N-NUMBER] varchar(50),
		[SERIAL NUMBER] varchar(50),
		[MFR MDL CODE] varchar(50),
		[ENG MFR MDL] varchar(50),
		[YEAR MFR] varchar(50),
		[TYPE REGISTRANT] varchar(50),
		[NAME] varchar(50),
		[STREET] varchar(50),
		[STREET2] varchar(50),
		[CITY] varchar(50),
		[STATE] varchar(50),
		[ZIP CODE] varchar(50),
		[REGION] varchar(50),
		[COUNTY] varchar(50),
		[COUNTRY] varchar(50),
		[LAST ACTION DATE] varchar(50),
		[CERT ISSUE DATE] varchar(50),
		[CERTIFICATION] varchar(50),
		[TYPE AIRCRAFT] varchar(50),
		[TYPE ENGINE] varchar(50),
		[STATUS CODE] varchar(50),
		[MODE S CODE] varchar(50),
		[FRACT OWNER] varchar(50),
		[AIR WORTH DATE] varchar(50),
		[OTHER NAMES(1)] varchar(50),
		[OTHER NAMES(2)] varchar(50),
		[OTHER NAMES(3)] varchar(50),
		[OTHER NAMES(4)] varchar(50),
		[OTHER NAMES(5)] varchar(50),
		[EXPIRATION DATE] varchar(50),
		[UNIQUE ID] varchar(50),
		[KIT MFR] varchar(50),
		[ KIT MODEL] varchar(50),
		[MODE S CODE HEX] varchar(50)
	)
	set ansi_padding off
end
go

insert into dbo.AircraftModel
	(AircraftModelId
	,Manufacturer
	,Model
	,AircraftType
	,EngineType
	,Category
	,Certified
	,Engines
	,Seats
	,WeightClass
	,SpeedMph)
select
	 [CODE]
	,[MFR]
	,r.[MODEL]
	,[TYPE-ACFT]
	,[TYPE-ENG]
	,[AC-CAT]
	,[BUILD-CERT-IND]
	,[NO-ENG]
	,[NO-SEATS]
	,[AC-WEIGHT]
	,[SPEED]
from faa.ACFTREF r
left join dbo.AircraftModel m on r.CODE = m.AircraftModelId
where m.AircraftModelId is null
go

update faa.[MASTER]
set [YEAR MFR] = '0'
where [YEAR MFR] = ''

update faa.[MASTER]
set [TYPE REGISTRANT] = '0'
where [TYPE REGISTRANT] = ''

insert into dbo.Aircraft
	(AircraftId
	,SerialNumber
	,AircraftModelId
	,EngineModelId
	,YearManufactured
	,RegistrantType
	,RegistrantName
	,RegistrantStreet
	,RegistrantStreet2
	,RegistrantCity
	,RegistrantState
	,RegistrantZipCode
	,RegistrantRegion
	,RegistrantCounty
	,RegistrantCountry
	,LastActionDate
	,CertIssueDate
	,Certification
	,AirWorthyDate
	,ExpirationDate
	,UniqueId
	,KitManufacturer
	,KitModel)
select
	 [N-NUMBER]
	,[SERIAL NUMBER]
	,[MFR MDL CODE]
	,[ENG MFR MDL]
	,[YEAR MFR]
	,[TYPE REGISTRANT]
	,[NAME]
	,[STREET]
	,[STREET2]
	,[CITY]
	,[STATE]
	,[ZIP CODE]
	,[REGION]
	,[COUNTY]
	,[COUNTRY]
	,[LAST ACTION DATE]
	,[CERT ISSUE DATE]
	,m.[CERTIFICATION]
	,[AIR WORTH DATE]
	,[EXPIRATION DATE]
	,[UNIQUE ID]
	,[KIT MFR]
	,[ KIT MODEL]
from faa.[MASTER] m
left join dbo.Aircraft a on m.[N-NUMBER] = a.AircraftId
where a.AircraftId is null
go

select count(*) from faa.ACFTREF
select count(*) from dbo.AircraftModel
select count(*) from faa.[MASTER]
select count(*) from dbo.Aircraft
