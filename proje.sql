USE [AppProject]
GO
/****** Object:  StoredProcedure [dbo].[dashboard]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[dashboard]

AS
BEGIN
	
	SET NOCOUNT ON;

	select 
	(select COUNT(*) from Login) as ToplamUye ,
	(select COUNT(*) from Passenger) as SatilanBiletSayisi
END

GO
/****** Object:  StoredProcedure [dbo].[prdCityForPriceOperation]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prdCityForPriceOperation]
	@Id int =0,
	@ToCityId int=0,
	@FromCityId int=0,
	@Price float=0,
	@CompanyId int =0,
	@OperationType varchar(20)=''
AS
BEGIN

	SET NOCOUNT ON;

	

	IF @OperationType ='INS'
	  BEGIN
	    IF (select count(*) from CityAndPrice where ToCityId=@ToCityId and FromCityId=@FromCityId and CompanyId=@CompanyId) <=0
		  BEGIN
		     insert into CityAndPrice(ToCityId,FromCityId,Price,CompanyId)values(@ToCityId,@FromCityId,@Price,@CompanyId)
		     select SCOPE_IDENTITY()
		  END
	   
	  END
	ELSE IF @OperationType ='UPD'
	  BEGIN
	    update CityAndPrice set 
		ToCityId =@ToCityId ,
		FromCityId =@FromCityId,
		Price=@Price,
		CompanyId =@CompanyId
		where Id=@Id
	  END
     ELSE IF @OperationType='DEL'
	   BEGIN
	   delete from CityAndPrice where Id=@Id
	   END
     ELSE IF @OperationType ='SELECT'
	  BEGIN
	   select cp.Price,cp.Id,cp.FromCityId,cp.ToCityId,fromCity.Name as FromCityName,toCity.Name as ToCityName,cp.CompanyId,c.Name as Company from CityAndPrice (nolock) as cp
	   inner join City as fromCity on cp.FromCityId = fromCity.Id
	   inner join City as toCity on cp.ToCityId = toCity.Id
	   inner join Company as c on c.Id =cp.CompanyId
	  END
	  ELSE IF @OperationType ='GETCOMPANYFORCİTY'
	  BEGIN
	    select cp.FromCityId,cp.ToCityId,fromCity.Name as FromCityName,toCity.Name as ToCityName from CityAndPrice (nolock) as cp
	   inner join City as fromCity on cp.FromCityId = fromCity.Id
	   inner join City as toCity on cp.ToCityId = toCity.Id
	   inner join Company as c on c.Id =cp.CompanyId where cp.CompanyId=@CompanyId
	  END
	   ELSE IF @OperationType ='GETPRICE'
	  BEGIN
	   select * from CityAndPrice where ToCityId=@ToCityId and FromCityId=@FromCityId
	  END
	  ELSE IF @OperationType ='GETBYID'
	  BEGIN
	   select * from CityAndPrice (nolock) where Id=@Id
	  END


END

GO
/****** Object:  StoredProcedure [dbo].[prdCityOperation]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prdCityOperation]
    @Id int=0,
	@Name varchar(50)='',
	@OperationType varchar(25)=''
AS
BEGIN

	SET NOCOUNT ON;

	IF @OperationType ='INS'
	  BEGIN
	    insert into City(Name)values(@Name)
		select SCOPE_IDENTITY()
	  END
	ELSE IF @OperationType ='UPD'
	  BEGIN
	    update City set Name =@Name where Id=@Id
		select @Id
	  END
     ELSE IF @OperationType='DEL'
	   BEGIN
	   delete from City where Id=@Id
	   END
     ELSE IF @OperationType ='SELECT'
	  BEGIN
	   select * from City (nolock)
	  END
	  ELSE IF @OperationType ='GETBYID'
	  BEGIN
	   select * from City (nolock) where Id=@Id
	  END
END

GO
/****** Object:  StoredProcedure [dbo].[prdCompanyOperation]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prdCompanyOperation]
    @Id int=0,
	@Name varchar(50)='',
	@Description varchar(500)='',
	@OperationType varchar(25)=''
AS
BEGIN

	SET NOCOUNT ON;

	IF @OperationType ='INS'
	  BEGIN
	    insert into Company(Name,Description)values(@Name,@Description)
		select SCOPE_IDENTITY()
	  END
	ELSE IF @OperationType ='UPD'
	  BEGIN
	    update Company set Name =@Name ,Description =@Description where Id=@Id
		select @Id
	  END
     ELSE IF @OperationType='DEL'
	   BEGIN
	   delete from Company where Id=@Id
	   END
     ELSE IF @OperationType ='SELECT'
	  BEGIN
	   select * from Company (nolock)
	  END
	  ELSE IF @OperationType ='GETBYID'
	  BEGIN
	   select * from Company (nolock) where Id=@Id
	  END
END

GO
/****** Object:  StoredProcedure [dbo].[prdLogin]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prdLogin]
	@Id int =0,
	@UserName varchar(50)='',
	@Password varchar(50)='',
	@NameSurname varchar(70)='',
	@IsAdmin bit =0,
	@OperationType varchar(20)=''
AS
BEGIN

	SET NOCOUNT ON;

	

	IF @OperationType ='INS'
	  BEGIN
	    insert into Login(UserName,Password,NameSurname,IsAdmin)values(@UserName,@Password,@NameSurname,@IsAdmin)
		select SCOPE_IDENTITY()
	  END
	ELSE IF @OperationType ='UPD'
	  BEGIN
	    update Login set 
		UserName =@UserName ,
		Password =@Password,
		NameSurname=@NameSurname,
		IsAdmin=@IsAdmin
		where Id=@Id
	  END
     ELSE IF @OperationType='DEL'
	   BEGIN
	   delete from Login where Id=@Id
	   END
     ELSE IF @OperationType ='SELECT'
	  BEGIN
	   select * from Login (nolock)
	  END
	   ELSE IF @OperationType ='LOGIN'
	  BEGIN
	  select * from Login (nolock) where UserName =@UserName and Password =@Password
	  END


END

GO
/****** Object:  StoredProcedure [dbo].[prdPassengerAndTicket]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prdPassengerAndTicket]
	@Id int =0,
	
	@ToCityId int=0,
	@FromCityId int=0,
	@UserId int =0,
	@TotalCount int =0,
	@CreatedDate datetime ,
	@CompanyId int =0,
	@OperationType varchar(20)=''
AS
BEGIN

	SET NOCOUNT ON;

	

	IF @OperationType ='INS'
	  BEGIN
	    insert into PassengerAndTicket(ToCityId,FromCityId,UserId,TotalCount,CreatedDate,CompanyId)
		values(@ToCityId,@FromCityId,@UserId,@TotalCount,@CreatedDate,@CompanyId)
		select SCOPE_IDENTITY()
	  END
	ELSE IF @OperationType ='UPD'
	  BEGIN
	    update PassengerAndTicket set 
		
		ToCityId =@ToCityId ,
		FromCityId =@FromCityId,
		@UserId=@UserId,
		TotalCount=@TotalCount,
		CreatedDate=@CreatedDate,
		CompanyId=@CompanyId
		where Id=@Id
	  END
     ELSE IF @OperationType='DEL'
	   BEGIN
	   delete from PassengerAndTicket where Id=@Id
	   END
     ELSE IF @OperationType ='SELECT'
	  BEGIN
	 select fromCity.Name as FromCity,toCity.Name as ToCity ,l.NameSurname as Passenger,pt.CreatedDate,pt.Id,pt.TotalCount ,c.Name as Company
	  from PassengerAndTicket as pt
	  inner join City as fromCity on pt.FromCityId = fromCity.Id
	   inner join City as toCity on pt.ToCityId = toCity.Id
	   inner join Login as l on pt.UserId = l.Id
	   inner join Company as c on pt.CompanyId = c.Id
	  END
	 


END

GO
/****** Object:  StoredProcedure [dbo].[prdPassengerOperation]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prdPassengerOperation]
    @Id int=0,
	@Name varchar(50)='',
	@Surname varchar(50)='',
	@Age int=0,
	@BirthDate date=null,
	@Tc varchar(11)='',
	@IsTc bit=0,
	@PassportNo varchar(20)='',
	@PassengerAndTicketId int =0,
	@OperationType varchar(25)=''
AS
BEGIN

	SET NOCOUNT ON;

	IF @OperationType ='INS'
	  BEGIN
	    insert into Passenger(Name,Surname,Age,BirthDate,Tc,IsTc,PassportNo,PassengerAndTicketId)
		values(@Name,@Surname,@Age,@BirthDate,@Tc,@IsTc,@PassportNo,@PassengerAndTicketId)
	  END
	ELSE IF @OperationType ='UPD'
	  BEGIN
	    update Passenger set 
		Name =@Name ,
		Surname =@Surname,
		Age=@Age,
		BirthDate=@BirthDate,
		Tc=@Tc,
		IsTc=@IsTc,
		PassportNo=@PassportNo,
		PassengerAndTicketId=@PassengerAndTicketId
		where Id=@Id
	  END
     ELSE IF @OperationType='DEL'
	   BEGIN
	   delete from Passenger where Id=@Id
	   END
     ELSE IF @OperationType ='SELECT'
	  BEGIN
	   select * from Passenger (nolock)
	  END
	  ELSE IF @OperationType ='GETTICKETID'
	 BEGIN
	   select * from Passenger where PassengerAndTicketId = @PassengerAndTicketId
	 END
END

GO
/****** Object:  StoredProcedure [dbo].[ticketOperation]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ticketOperation]
	@TicketId int=0,
	@OperationType varchar(30)=''
AS
BEGIN

	SET NOCOUNT ON;
	 
	 IF @OperationType='PassengerAndTicket'
	   BEGIN
	    select *from PassengerAndTicket
	   END
	ELSE
	 BEGIN
	   select * from Passenger where PassengerAndTicketId = @TicketId
	 END
  
END

GO
/****** Object:  Table [dbo].[City]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[City](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CityAndPrice]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CityAndPrice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ToCityId] [int] NOT NULL,
	[FromCityId] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[CompanyId] [int] NOT NULL,
 CONSTRAINT [PK_CityAndPrice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Login]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Login](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[NameSurname] [varchar](70) NOT NULL,
	[IsAdmin] [bit] NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Passenger]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Passenger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[Age] [int] NOT NULL,
	[BirthDate] [date] NOT NULL,
	[Tc] [varchar](11) NOT NULL,
	[IsTc] [bit] NOT NULL,
	[PassportNo] [varchar](20) NOT NULL,
	[PassengerAndTicketId] [int] NOT NULL,
 CONSTRAINT [PK_Passenger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PassengerAndTicket]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PassengerAndTicket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FromCityId] [int] NOT NULL,
	[ToCityId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[TotalCount] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CompanyId] [int] NOT NULL,
 CONSTRAINT [PK_PassengerAndTicket] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PassengerTypes]    Script Date: 30.05.2018 21:56:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PassengerTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PassengerTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[City] ON 

INSERT [dbo].[City] ([Id], [Name]) VALUES (1, N'istanbul')
INSERT [dbo].[City] ([Id], [Name]) VALUES (2, N'ankara')
INSERT [dbo].[City] ([Id], [Name]) VALUES (3, N'izmir')
INSERT [dbo].[City] ([Id], [Name]) VALUES (4, N'adana')
INSERT [dbo].[City] ([Id], [Name]) VALUES (5, N'kars')
INSERT [dbo].[City] ([Id], [Name]) VALUES (6, N'artvin')
INSERT [dbo].[City] ([Id], [Name]) VALUES (7, N'erzurum')
INSERT [dbo].[City] ([Id], [Name]) VALUES (8, N'izmit')
INSERT [dbo].[City] ([Id], [Name]) VALUES (9, N'sakarya')
INSERT [dbo].[City] ([Id], [Name]) VALUES (10, N'rize')
INSERT [dbo].[City] ([Id], [Name]) VALUES (11, N'antalya')
INSERT [dbo].[City] ([Id], [Name]) VALUES (13, N'hakkari')
SET IDENTITY_INSERT [dbo].[City] OFF
SET IDENTITY_INSERT [dbo].[CityAndPrice] ON 

INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (1, 1, 2, 120, 1)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (2, 3, 4, 200, 1)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (4, 5, 6, 120, 1)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (5, 6, 8, 1200, 1)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (6, 8, 9, 12, 2)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (7, 7, 12, 23, 2)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (8, 10, 13, 256, 2)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (9, 7, 9, 500, 1)
INSERT [dbo].[CityAndPrice] ([Id], [ToCityId], [FromCityId], [Price], [CompanyId]) VALUES (10, 1, 4, 444, 1)
SET IDENTITY_INSERT [dbo].[CityAndPrice] OFF
SET IDENTITY_INSERT [dbo].[Company] ON 

INSERT [dbo].[Company] ([Id], [Name], [Description]) VALUES (1, N'Metro turizm', N'Metro firması')
INSERT [dbo].[Company] ([Id], [Name], [Description]) VALUES (2, N'Kamil koç', N'')
SET IDENTITY_INSERT [dbo].[Company] OFF
SET IDENTITY_INSERT [dbo].[Login] ON 

INSERT [dbo].[Login] ([Id], [UserName], [Password], [NameSurname], [IsAdmin]) VALUES (1, N'caner', N'1', N'caner inalı', 1)
INSERT [dbo].[Login] ([Id], [UserName], [Password], [NameSurname], [IsAdmin]) VALUES (2, N'osman', N'1', N'osman', 0)
INSERT [dbo].[Login] ([Id], [UserName], [Password], [NameSurname], [IsAdmin]) VALUES (3, N'sercan', N'1', N'sercan gül', 0)
SET IDENTITY_INSERT [dbo].[Login] OFF
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[CityAndPrice] ADD  CONSTRAINT [DF_CityAndPrice_ToCityId]  DEFAULT ((0)) FOR [ToCityId]
GO
ALTER TABLE [dbo].[CityAndPrice] ADD  CONSTRAINT [DF_CityAndPrice_FromCityId]  DEFAULT ((0)) FOR [FromCityId]
GO
ALTER TABLE [dbo].[CityAndPrice] ADD  CONSTRAINT [DF_CityAndPrice_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[CityAndPrice] ADD  CONSTRAINT [DF_CityAndPrice_CompanyId]  DEFAULT ((0)) FOR [CompanyId]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_Description]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [dbo].[Login] ADD  CONSTRAINT [DF_Login_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[Passenger] ADD  CONSTRAINT [DF_Passenger_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[Passenger] ADD  CONSTRAINT [DF_Passenger_Surname]  DEFAULT ('') FOR [Surname]
GO
ALTER TABLE [dbo].[Passenger] ADD  CONSTRAINT [DF_Passenger_Age]  DEFAULT ((0)) FOR [Age]
GO
ALTER TABLE [dbo].[Passenger] ADD  CONSTRAINT [DF_Passenger_BirthDate]  DEFAULT (getdate()) FOR [BirthDate]
GO
ALTER TABLE [dbo].[Passenger] ADD  CONSTRAINT [DF_Passenger_IsTc]  DEFAULT ((0)) FOR [IsTc]
GO
ALTER TABLE [dbo].[Passenger] ADD  CONSTRAINT [DF_Passenger_PassengerAndTicketId]  DEFAULT ((0)) FOR [PassengerAndTicketId]
GO
ALTER TABLE [dbo].[PassengerAndTicket] ADD  CONSTRAINT [DF_PassengerAndTicket_TotalCount]  DEFAULT ((1)) FOR [TotalCount]
GO
ALTER TABLE [dbo].[PassengerAndTicket] ADD  CONSTRAINT [DF_PassengerAndTicket_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PassengerAndTicket] ADD  CONSTRAINT [DF_PassengerAndTicket_CompanyId]  DEFAULT ((0)) FOR [CompanyId]
GO
ALTER TABLE [dbo].[PassengerTypes] ADD  CONSTRAINT [DF_PassengerTypes_Name]  DEFAULT ('') FOR [Name]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Yolcunun nereye gideceği' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PassengerAndTicket', @level2type=N'COLUMN',@level2name=N'FromCityId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'yolcunun hareket yeri' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PassengerAndTicket', @level2type=N'COLUMN',@level2name=N'ToCityId'
GO
