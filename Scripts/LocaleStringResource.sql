USE [IdentityServer4.Admin]
GO

/****** Object:  Table [dbo].[LocaleStringResource]    Script Date: 2020/12/11 14:13:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LocaleStringResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResourceName] [nvarchar](200) NOT NULL,
	[ResourceValue] [nvarchar](max) NOT NULL,
	[LanguageId] [int] NOT NULL,
 CONSTRAINT [PK_LocaleStringResource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[LocaleStringResource]  WITH CHECK ADD  CONSTRAINT [FK_LocaleStringResource_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LocaleStringResource] CHECK CONSTRAINT [FK_LocaleStringResource_Language]
GO


