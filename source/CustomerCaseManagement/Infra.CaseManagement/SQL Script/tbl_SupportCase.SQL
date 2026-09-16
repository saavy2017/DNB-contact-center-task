USE [CustomerCaseManagement]
GO

/****** Object:  Table [dbo].[SupportCase]    Script Date: 16-09-2026 09:05:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SupportCase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceNumber] [int] NOT NULL,
	[CustomerName] [varchar](100) NOT NULL,
	[CustomerEmail] [varchar](250) NOT NULL,
	[Subject] [varchar](250) NOT NULL,
	[Description] [varchar](500) NULL,
	[Priority] [varchar](7) NOT NULL,
	[Status] [varchar](10) NOT NULL,
	[CreatedOn] [date] NOT NULL,
	[ModifiedOn] [date] NULL,
 CONSTRAINT [ref_unique] UNIQUE NONCLUSTERED 
(
	[ReferenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


