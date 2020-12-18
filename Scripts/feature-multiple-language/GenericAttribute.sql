USE [IdentityServer4.Admin]
GO

/****** Object:  Table [dbo].[GenericAttribute]    Script Date: 2020/12/11 14:13:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GenericAttribute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KeyGroup] [nvarchar](400) NOT NULL,
	[Key] [nvarchar](400) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[EntityId] [int] NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_GenericAttribute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


