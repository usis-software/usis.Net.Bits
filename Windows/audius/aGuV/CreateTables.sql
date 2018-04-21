-- delete existing tables

IF OBJECT_ID('FK_audiusGuVSachkonten_audiusGuVPositionen', 'F') IS NOT NULL
	ALTER TABLE [dbo].[audiusGuVSachkonten]
	DROP CONSTRAINT [FK_audiusGuVSachkonten_audiusGuVPositionen]
IF OBJECT_ID('dbo.audiusGuVSachkonten', 'U') IS NOT NULL
	DROP TABLE [dbo].[audiusGuVSachkonten]

IF OBJECT_ID('FK_audiusGuVPositionen_audiusGuVPositionen', 'F') IS NOT NULL
	ALTER TABLE [dbo].[audiusGuVPositionen] DROP CONSTRAINT [FK_audiusGuVPositionen_audiusGuVPositionen]
IF OBJECT_ID('dbo.audiusGuVPositionen', 'U') IS NOT NULL
	DROP TABLE [dbo].[audiusGuVPositionen]

-- create GuV Positionen

CREATE TABLE [dbo].[audiusGuVPositionen]
(
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Position] [int] NULL,
	CONSTRAINT [PK_audiusGuVPositionen] PRIMARY KEY ([Id] ASC)
)
ALTER TABLE [dbo].[audiusGuVPositionen]
	ADD CONSTRAINT [FK_audiusGuVPositionen_audiusGuVPositionen]
	FOREIGN KEY([Position])
	REFERENCES [dbo].[audiusGuVPositionen] ([Id])

-- create GuV Sachkonten

CREATE TABLE [dbo].[audiusGuVSachkonten]
(
	[Id] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[SaKtoVon] [varchar](20) NOT NULL,
	[SaKtoBis] [varchar](20) NOT NULL,
	CONSTRAINT [PK_audiusGuVSachkonten] PRIMARY KEY ([Id] ASC)
)
ALTER TABLE [dbo].[audiusGuVSachkonten]
	ADD CONSTRAINT [FK_audiusGuVSachkonten_audiusGuVPositionen]
	FOREIGN KEY([Position])
	REFERENCES [dbo].[audiusGuVPositionen] ([Id])
