USE [DATABASE1]
GO
/****** Object: Table [dbo].[BankAccounts] Script Date: 18.06.2020 23:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BankAccounts](
[AccountID] [int] IDENTITY(1,1) NOT NULL,
[CustomerID] [int] NOT NULL,
[AccountNo] varchar NOT NULL,
[Balance] [decimal](18, 2) NOT NULL,
CONSTRAINT [PK_BankAccounts] PRIMARY KEY CLUSTERED
(
[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object: Table [dbo].[CreditCards] Script Date: 18.06.2020 23:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CreditCards](
[CardID] [int] IDENTITY(1,1) NOT NULL,
[CardNo] varchar NOT NULL,
[ExpiredDate] [date] NOT NULL,
[AccountID] [int] NOT NULL,
[CustomerID] [int] NOT NULL,
[CVV] [int] NOT NULL,
[CardHolder] varchar NOT NULL,
[PIN] [int] NOT NULL,
[CardType] varchar NOT NULL,
[Restricted] [bit] NOT NULL,
[FailedLogins] [int] NOT NULL,
CONSTRAINT [PK_CreditCards] PRIMARY KEY CLUSTERED
(
[CardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object: Table [dbo].[Customers] Script Date: 18.06.2020 23:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customers](
[CustomerID] [int] IDENTITY(1,1) NOT NULL,
[Name] varchar NOT NULL,
[Surname] varchar NOT NULL,
[PhoneNo] [bigint] NOT NULL,
[Address] varchar NOT NULL,
[PersonalID] [bigint] NOT NULL,
CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED
(
[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object: Table [dbo].[Transactions] Script Date: 18.06.2020 23:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Transactions](
[TransactionID] [int] NOT NULL,
[CustomerID] [int] NOT NULL,
[Operation] varchar NOT NULL,
[Amount] [decimal](18, 0) NOT NULL,
CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED
(
[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[BankAccounts] WITH CHECK ADD CONSTRAINT [FK_BankAccounts_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[BankAccounts] CHECK CONSTRAINT [FK_BankAccounts_Customers]
GO
ALTER TABLE [dbo].[CreditCards] WITH CHECK ADD CONSTRAINT [FK_CreditCards_BankAccounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[BankAccounts] ([AccountID])
GO
ALTER TABLE [dbo].[CreditCards] CHECK CONSTRAINT [FK_CreditCards_BankAccounts]
GO
ALTER TABLE [dbo].[CreditCards] WITH CHECK ADD CONSTRAINT [FK_CreditCards_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[CreditCards] CHECK CONSTRAINT [FK_CreditCards_Customers]
GO
ALTER TABLE [dbo].[Transactions] WITH CHECK ADD CONSTRAINT [FK_Transaction_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transaction_Customers]
GO