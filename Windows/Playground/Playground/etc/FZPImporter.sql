--__________________________________________________________________________________________________________________________________________________________________________________
-- Firma:			Funk, Zander und Partner GmbH
-- Inhalt:			Script für FZP Importer
-- Erstellung:		10.05.2012, JR
-- letzte Änderung: 29.05.2012, JR
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Felder
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Felder in KHKAdressen
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKAdressen]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKAdressen] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'Adresse'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'Adresse'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('Adresse','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Felder in KHKKontokorrent
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKKontokorrent]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKKontokorrent] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'Kontokorrent'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'Kontokorrent'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('Kontokorrent','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportKontokorrentID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportKontokorrentID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKKontokorrent]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKKontokorrent] ADD [USER_FZPImportKontokorrentID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'Kontokorrent'
	       AND Field LIKE 'USER_FZPImportKontokorrentID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'Kontokorrent'
	       AND Field LIKE 'USER_FZPImportKontokorrentID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('Kontokorrent','USER_FZPImportKontokorrentID','FZPImportKontokorrentID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Felder in KHKAnsprechpartner
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKAnsprechpartner]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKAnsprechpartner] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'Ansprechpartner'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'Ansprechpartner'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('Ansprechpartner','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportAnsprechpartnerID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportAnsprechpartnerID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKAnsprechpartner]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKAnsprechpartner] ADD [USER_FZPImportAnsprechpartnerID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'Ansprechpartner'
	       AND Field LIKE 'USER_FZPImportAnsprechpartnerID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'Ansprechpartner'
	       AND Field LIKE 'USER_FZPImportAnsprechpartnerID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('Ansprechpartner','USER_FZPImportAnsprechpartnerID','FZPImportAnsprechpartnerID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Felder in KHKBankverbindungenD
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKBankverbindungenD]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKBankverbindungenD] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportBankverbindungID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportBankverbindungID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKBankverbindungenD]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKBankverbindungenD] ADD [USER_FZPImportBankverbindungID] [varchar](50) NULL
END
GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKBuchungserfassung]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKBuchungserfassung] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'Buchung'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'Buchung'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('Buchung','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportdatum
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportdatum') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKBuchungserfassung]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKBuchungserfassung] ADD [USER_FZPImportdatum] [datetime] NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'Buchung'
	       AND Field LIKE 'USER_FZPImportdatum')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'Buchung'
	       AND Field LIKE 'USER_FZPImportdatum'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('Buchung','USER_FZPImportdatum','FZPImportdatum',8,0,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKVKBelege]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKVKBelege] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO


IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKArchivVKBelege]')))
BEGIN
ALTER TABLE dbo.KHKArchivVKBelege ADD [USER_FZPImportID] [varchar](50) NULL
End
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'VKBeleg'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'VKBeleg'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('VKBeleg','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKEKBelege]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKEKBelege] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO


IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKArchivEKBelege]')))
BEGIN
ALTER TABLE dbo.KHKArchivEKBelege ADD [USER_FZPImportID] [varchar](50) NULL
End
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'EKBeleg'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'EKBeleg'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('EKBeleg','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO



--__________________________________________________________________________________________________________________________________________________________________________________
--  User-Feld USER_FZPImportID
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKVKBelegePositionen]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKVKBelegePositionen] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO


IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKPJBelegePositionen]')))
BEGIN
ALTER TABLE [dbo].[KHKPJBelegePositionen] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKArchivVKPos]')))
BEGIN
ALTER TABLE [dbo].[KHKArchivVKPos] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKArchivPJPos]')))
BEGIN
ALTER TABLE [dbo].[KHKArchivPJPos] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'VKBelegPosition'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'VKBelegPosition'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('VKBelegPosition','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO

IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKEKBelegePositionen]')))
	       
BEGIN
ALTER TABLE [dbo].[KHKEKBelegePositionen] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF NOT EXISTS (SELECT * FROM dbo.syscolumns 
	       WHERE (name LIKE 'USER_FZPImportID') 
	       AND (id = OBJECT_ID(N'[dbo].[KHKArchivEKPos]')))
BEGIN
ALTER TABLE [dbo].[KHKArchivEKPos] ADD [USER_FZPImportID] [varchar](50) NULL
END
GO

IF EXISTS (SELECT * FROM USysClassFields
	       WHERE Class LIKE 'EKBelegPosition'
	       AND Field LIKE 'USER_FZPImportID')
BEGIN
DELETE FROM USysClassFields
	       WHERE Class LIKE 'EKBelegPosition'
	       AND Field LIKE 'USER_FZPImportID'
END
GO

INSERT INTO USysClassFields (
	Class,
	Field,
	Description,
	Type,
	[Size],
	Locked,
	Readonly,
	ComboBoxMode,
	ComboBoxSource,
	ComboBoxLimitToList,
	LayoutTab,
	LayoutLeft,
	LayoutTop,
	LayoutWidthLabel,
	LayoutWidthField,
	LayoutHeightField
) VALUES ('EKBelegPosition','USER_FZPImportID','FZPImportID',10,50,0,-1,0,'',0,0,0,0,0,0,0)

GO


--__________________________________________________________________________________________________________________________________________________________________________________
--  IMPORT TABELLEN
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
--__________________________________________________________________________________________________________________________________________________________________________________
--  IMPORT BUCHUNG
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.sysobjects 
	       WHERE id = object_id(N'[dbo].[FZPImporterBuchungsimport]') 
	       AND OBJECTPROPERTY(id, N'IsUserTable') = 1)

BEGIN

--ALTER TABLE [dbo].[FZPImporterBuchungsimport] DROP  CONSTRAINT [DF_FZPImporterBuchungsimport_IstImportiert]   


CREATE TABLE [dbo].[FZPImporterBuchungsimport](
	[ImportID] [int] IDENTITY(1,1) NOT NULL,
	[BuchungsID] [varchar](50) NOT NULL,
	[Buchungsdatum] [datetime] NOT NULL,
	[Belegdatum] [datetime] NOT NULL,
	[Faelligkeitsdatum] [money] NULL,
	[Belegnummer] [varchar](20) NOT NULL,
	[Buchungstext] [varchar](255) NOT NULL,
	[Konto_Soll] [varchar](20) NOT NULL,
	[Konto_Haben] [varchar](20) NOT NULL,
	[WKz] [varchar](3) NULL,
	[Buchungsbetrag] [money] NOT NULL,
	[Steuerbetrag] [money] NULL,
	[Adresse] [int] NULL,
	[Kostenstelle] [varchar](20) NULL,
	[Kostentraeger] [varchar](20) NULL,
	[SDIV_Betrag] [money] NULL,
	[SDIV_Buchungstext] [varchar](255) NULL,
	[SDIV_Konto] [varchar](20) NULL,
	[SDIV_Kostenstelle] [varchar](20) NULL,
	[SDIV_Kostentraeger] [varchar](20) NULL,
	[IstImportiert] [smallint] NOT NULL,
	[Mandant] [smallint] NULL,
	[Steuercode] [smallint] NULL,
	[Zahlungskondition] [varchar](10) NULL,
	[OPNummer] [varchar](20) NULL,
	[OPDatum] [datetime] NULL,
	[Referenznummer] [varchar](27) NULL,
	[PreNotification] [smallint] NULL,
	[MandatsNummer] [varchar](50) NULL,
	[Buchungsperiode] [int] NULL,
	[Buchungssitzung] [varchar](50) NULL,
	[Fehlertext] [varchar](max) NULL,
	[Kennung] [varchar](50) NULL,
 CONSTRAINT [PK_FZPImporterBuchungsimport] PRIMARY KEY CLUSTERED 
(
	[ImportID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FZPImporterBuchungsimport] ADD  CONSTRAINT [DF_FZPImporterBuchungsimport_IstImportiert]  DEFAULT ((0)) FOR [IstImportiert]
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Mandant')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Mandant] smallint NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Steuercode')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Steuercode] smallint NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Steuerbetrag')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Steuerbetrag] money NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Referenznummer')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Referenznummer] varchar(27) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Zahlungskondition')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Zahlungskondition] varchar(10) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'OPNummer')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [OPNummer] Varchar(20) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'OPDatum')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [OPDatum] datetime NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'PreNotification')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [PreNotification] [dbo].[KHKBoolean] NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'MandatsNummer')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [MandatsNummer] [varchar](50) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Buchungssitzung')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Buchungssitzung] [varchar](50) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Buchungsperiode')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Buchungsperiode] int NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Fehlertext')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Fehlertext] [varchar](max) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBuchungsimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Kennung')
BEGIN
ALTER TABLE FZPImporterBuchungsimport ADD [Kennung] [varchar](50) NULL
END
GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  IMPORT ADRESSE
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯

IF NOT EXISTS (SELECT * FROM dbo.sysobjects 
	       WHERE id = object_id(N'[dbo].[FZPImporterAdressimport]') 
	       AND OBJECTPROPERTY(id, N'IsUserTable') = 1)

BEGIN

CREATE TABLE [dbo].[FZPImporterAdressimport](
	[ImportID] [int] IDENTITY(1,1) NOT NULL,
	[Adr_Kategorie] [smallint] NULL,
	[Adr_Matchcode] [varchar](50) NULL,
	[Adr_Anrede] [varchar](50) NULL,
	[Adr_Name1] [varchar](50) NULL,
	[Adr_Name2] [varchar](50) NULL,
	[Adr_LieferZusatz] [varchar](64) NULL,
	[Adr_LieferStrasse] [varchar](64) NULL,
	[Adr_LieferLand] [varchar](3) NULL,
	[Adr_LieferPLZ] [varchar](24) NULL,
	[Adr_LieferOrt] [varchar](40) NULL,
	[Adr_PostZusatz] [varchar](64) NULL,
	[Adr_PostStrasse] [varchar](64) NULL,
	[Adr_PostLand] [varchar](3) NULL,
	[Adr_PostPLZ] [varchar](24) NULL,
	[Adr_PostOrt] [varchar](40) NULL,
	[Adr_Ansprache] [varchar](50) NULL,
	[Adr_Telefon] [varchar](32) NULL,
	[Adr_Telefax] [varchar](32) NULL,
	[Adr_Mobilfunk] [varchar](32) NULL,
	[Adr_EMail] [varchar](255) NULL,
	[Adr_Homepage] [varchar](130) NULL,
	[Adr_Memo] [text] NULL,
	[Adr_Sprache] [varchar](3) NULL,
	[Adr_Erstkontakt] [datetime] NULL,
	[Adr_Gruppe] [varchar](10) NULL,
	[Adr_A1Besteuerung] [dbo].[KHKBoolean] NULL,
	[Adr_Auswertungskennzeichen] [varchar](10) NULL,
	[Adr_Referenz] [varchar](20) NULL,
	[Adr_Aktiv] [dbo].[KHKBoolean] NULL,
	[Adr_Abladestelle] [varchar](5) NULL,
	[Adr_Werkschluessel] [varchar](3) NULL,
	[Adr_USER_FZPImportID] [varchar](50) NULL,
	[KK_Kto] [varchar](20) NULL,
	[KK_KtoArt] [varchar](1) NULL,
	[KK_KtoTyp] [smallint] NULL,
	[KK_Matchcode] [varchar](50) NULL,
	[KK_WKz] [varchar](3) NULL,
	[KK_Sammelkto] [smallint] NULL,
	[KK_Bebuchbarkeit] [int] NULL,
	[KK_Besteuerung] [smallint] NULL,
	[KK_EULand] [varchar](2) NULL,
	[KK_EUUStID] [varchar](15) NULL,
	[KK_EUDreiecksgeschaeft] [dbo].[KHKBoolean] NULL,
	[KK_Zahlungskond] [varchar](10) NULL,
	[KK_Inkassoart] [varchar](10) NULL,
	[KK_Nettobedingung] [varchar](255) NULL,
	[KK_Skontobedingung1] [varchar](255) NULL,
	[KK_Skontobedingung2] [varchar](255) NULL,
	[KK_Skontoprozent1] [money] NULL,
	[KK_Skontoprozent2] [money] NULL,
	[KK_Kreditlimit] [money] NULL,
	[KK_Gruppe] [varchar](10) NULL,
	[KK_Vertreter] [varchar](10) NULL,
	[KK_Mahnwesen] [smallint] NULL,
	[KK_Zahlungsverkehr] [smallint] NULL,
	[KK_Zahlungsavis] [dbo].[KHKBoolean] NULL,
	[KK_LetzteMahnung] [datetime] NULL,
	[KK_Zahlungsmoral] [varchar](10) NULL,
	[KK_Liefermoral] [varchar](10) NULL,
	[KK_GegenKto] [varchar](20) NULL,
	[KK_Kostenstelle] [varchar](20) NULL,
	[KK_Kostentraeger] [varchar](20) NULL,
	[KK_Tour] [varchar](10) NULL,
	[KK_Rechnungsempfaenger] [varchar](20) NULL,
	[KK_Rechnungskreis] [varchar](10) NULL,
	[KK_LetzterUmsatz] [datetime] NULL,
	[KK_Formularvariante] [varchar](10) NULL,
	[KK_IstGesperrt] [dbo].[KHKBoolean] NULL,
	[KK_Auswertungskennzeichen] [varchar](10) NULL,
	[KK_Preisliste] [int] NULL,
	[KK_Rabattliste] [int] NULL,
	[KK_Erloescode] [smallint] NULL,
	[KK_Provisionsfaehig] [dbo].[KHKBoolean] NULL,
	[KK_Preiskennzeichen] [dbo].[KHKBoolean] NULL,
	[KK_Rabattsatz] [money] NULL,
	[KK_Referenznummer] [varchar](80) NULL,
	[KK_Versand] [varchar](10) NULL,
	[KK_Lieferbedingung] [varchar](10) NULL,
	[KK_Verkehrszweig] [varchar](10) NULL,
	[KK_Geschaeftsart] [varchar](10) NULL,
	[KK_Rabattgruppe] [varchar](10) NULL,
	[KK_ABCKlasse] [varchar](1) NULL,
	[KK_Teillieferungen] [smallint] NULL,
	[KK_DataNormLiefKuerzel] [varchar](5) NULL,
	[KK_Aktiv] [dbo].[KHKBoolean] NULL,
	[KK_KundenSteuerNr] [varchar](20) NULL,
	[KK_ZessionarID] [int] NULL,
	[KK_TransferSchemaID] [int] NULL,
	[KK_USER_FZPImportID] [varchar](50) NULL,
	[KK_USER_FZPImportKontokorrentID] [varchar](50) NULL,
	[AP_Ansprechpartner] [varchar](50) NULL,
	[AP_Gruppe] [varchar](10) NULL,
	[AP_Titel] [varchar](50) NULL,
	[AP_Vorname] [varchar](50) NULL,
	[AP_Nachname] [varchar](50) NULL,
	[AP_Position] [varchar](64) NULL,
	[AP_Abteilung] [varchar](50) NULL,
	[AP_Anrede] [varchar](50) NULL,
	[AP_Briefanrede] [varchar](64) NULL,
	[AP_ZuHaendenText] [varchar](50) NULL,
	[AP_Telefon] [varchar](32) NULL,
	[AP_Telefax] [varchar](32) NULL,
	[AP_Mobilfunk] [varchar](32) NULL,
	[AP_TelefonPrivat] [varchar](32) NULL,
	[AP_Autotelefon] [varchar](32) NULL,
	[AP_EMail] [varchar](255) NULL,
	[AP_Geburtsdatum] [datetime] NULL,
	[AP_Memo] [text] NULL,
	[AP_Transferadresse] [varchar](255) NULL,
	[AP_USER_FZPImportID] [varchar](50) NULL,
	[AP_USER_FZPImportAnsprechpartnerID] [varchar](50) NULL,
	[Bank_Adresse] [int] NULL,
	[Bank_Zahlung] [dbo].[KHKBoolean] NULL,
	[Bank_Lastschrift] [dbo].[KHKBoolean] NULL,
	[Bank_Attribut] [int] NULL,
	[Bank_Matchcode] [varchar](60) NULL,
	[Bank_Bemerkung] [varchar](50) NULL,
	[Bank_Konto] [varchar](35) NULL,
	[Bank_BLZ] [varchar](8) NULL,
	[Bank_IBAN] [varchar](34) NULL,
	[Bank_Institut] [varchar](40) NULL,
	[Bank_Lastschriftkennzeichen] [varchar](2) NULL,
	[Bank_Swift] [varchar](11) NULL,
	[Bank_Institut2] [varchar](40) NULL,
	[Bank_Strasse] [varchar](40) NULL,
	[Bank_Ort] [varchar](40) NULL,
	[Bank_Laenderschluessel] [varchar](3) NULL,
	[Bank_Zahlungsart] [varchar](2) NULL,
	[Bank_Weisungscode1] [varchar](2) NULL,
	[Bank_Weisungscode2] [varchar](2) NULL,
	[Bank_Weisungscode3] [varchar](2) NULL,
	[Bank_Weisungscode4] [varchar](2) NULL,
	[Bank_Weisungszusatz] [varchar](25) NULL,
	[Bank_Ordervermerk1] [varchar](35) NULL,
	[Bank_Ordervermerk2] [varchar](35) NULL,
	[Bank_Kostenverrechnung] [varchar](2) NULL,
	[Bank_BuBaMeldung] [varchar](1) NULL,
	[Bank_ZahlungStandard] [dbo].[KHKBoolean] NULL,
	[Bank_LastschriftStandard] [dbo].[KHKBoolean] NULL,
	[Bank_Entgeltregelung] [smallint] NULL,
	[Bank_Transaktionsart] [smallint] NULL,
	[Bank_Kontoinhaber] [varchar](27) NULL,
	[Bank_MatchcodeIban] [varchar](60) NULL,
	[Bank_USER_FZPImportID] [varchar](50) NULL,
	[Bank_USER_FZPImportBankverbindungID] [varchar](50) NULL,
	[Sepa_Mandatsnummer] [varchar] (50) NULL,
	[Sepa_MandatsTyp] [int] NULL,
	[Sepa_AktivierungsStatus] [int] NULL,
	[Sepa_LastschriftTyp] [int] NULL,
	[Sepa_Land] [int] NULL,
	[Sepa_SignierungsDatum] [Datetime] NULL,
	[Sepa_ErstelltAm] [Datetime] NULL,
	[Sepa_LetzteAusfuehrungAm] [Datetime] NULL,
	[IstImportiert] [dbo].[KHKBoolean] NOT NULL,
	
	 CONSTRAINT [PK_FZPImporterAdressimport] PRIMARY KEY CLUSTERED 
(
	[ImportID] ASC
)
) ON [PRIMARY]
	
	ALTER TABLE [dbo].[FZPImporterAdressimport] ADD  CONSTRAINT [DF_FZPImporterAdressimport_IstImportiert]  DEFAULT ((0)) FOR [IstImportiert]
END 

GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_Mandatsnummer')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_Mandatsnummer] [varchar] (50) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_MandatsTyp')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_MandatsTyp] int NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_Land')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_Land] int NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_AktivierungsStatus')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_AktivierungsStatus] int NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_LastschriftTyp')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_LastschriftTyp] int NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_Land')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_Land] int NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_SignierungsDatum')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_SignierungsDatum] datetime NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_ErstelltAm')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_ErstelltAm] datetime NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Sepa_LetzteAusfuehrungAm')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Sepa_LetzteAusfuehrungAm] datetime NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterAdressimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Mandant')
BEGIN
ALTER TABLE FZPImporterAdressimport ADD [Mandant] smallint NULL
END
GO

--__________________________________________________________________________________________________________________________________________________________________________________
--  IMPORT BELEG
--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
IF NOT EXISTS (SELECT * FROM dbo.sysobjects 
	       WHERE id = object_id(N'[dbo].[FZPImporterBelegimport]') 
	       AND OBJECTPROPERTY(id, N'IsUserTable') = 1)

BEGIN

CREATE TABLE [dbo].[FZPImporterBelegimport](
	[ImportID] [int] IDENTITY(1,1) NOT NULL,
	[Mandant] smallint NULL,
	[BelegID] [varchar](50) NOT NULL,
	[Belegmatchcode] [varchar](50) NULL,
	[Vorgangsmatchcode] [varchar](50) NULL,
	[A0Empfaenger] [varchar](20) NOT NULL,
	[A0Anrede] [varchar](50) NULL,
	[A0Name1] [varchar](50) NULL,
	[A0Name2] [varchar](50) NULL,
	[A0Zusatz] [varchar](64) NULL,
	[A0Matchcode] [varchar](50) NULL,
	[A0Strasse] [varchar](64) NULL,
	[A0Land] [varchar](3) NULL,
	[A0PLZ] [varchar](24) NULL,
	[A0Ort] [varchar](40) NULL,
	[A1Anrede] [varchar](50) NULL,
	[A1Name1] [varchar](50) NULL,
	[A1Name2] [varchar](50) NULL,
	[A1Zusatz] [varchar](64) NULL,
	[A1Matchcode] [varchar](50) NULL,
	[A1Strasse] [varchar](64) NULL,
	[A1Land] [varchar](3) NULL,
	[A1PLZ] [varchar](24) NULL,
	[A1Ort] [varchar](40) NULL,
	[Rechnungsempfaenger] [varchar](20) NULL,
	[Liefertermin] [datetime] NULL,
	[Belegdatum] [datetime] NULL,
	[Kopftext] [text] NULL,
	[Fusstext] [text] NULL,
	[Kostenstelle] [varchar](20) NULL,
	[Kostentraeger] [varchar](20) NULL,
	[abweichenderWareneingangscode] smallint NULL,
	[Formularvariante] [varchar](10) NULL,
	[Auswertungskennzeichen] [varchar](10) NULL,
	[Buchungskreis] smallint NULL,
	[Memo] [text] NULL,
	[BelegPosID] [varchar](50) NOT NULL,
	[Artikelnummer] [varchar](31) NOT NULL,
	[Bezeichnung1] [varchar](50) NULL,
	[Bezeichnung2] [varchar](50) NULL,
	[Menge] [money] NOT NULL,
	[Einzelpreis] [money] NOT NULL,
	[Dimensionstext] [text] NULL,
	[Langtext] [text] NULL,
	[Rabattbetrag] [money] NULL,
	[IstImportiert] [dbo].[KHKBoolean] NOT NULL,
 CONSTRAINT [PK_FZPImporterBelegimport] PRIMARY KEY CLUSTERED 
(
	[ImportID] ASC
)
) ON [PRIMARY]

ALTER TABLE [dbo].[FZPImporterBelegimport] ADD  CONSTRAINT [DF_FZPImporterBelegimport_IstImportiert]  DEFAULT ((0)) FOR [IstImportiert]

END

GO


IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBelegimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Bearbeiter')
BEGIN
ALTER TABLE FZPImporterBelegimport ADD [Bearbeiter] varchar(50) NULL
END
GO

IF NOT EXISTS (	SELECT * FROM dbo.syscolumns
				INNER JOIN dbo.sysobjects
				ON dbo.syscolumns.id = dbo.sysobjects.id
				WHERE dbo.sysobjects.id = object_id(N'[FZPImporterBelegimport]') 
				AND OBJECTPROPERTY(dbo.sysobjects.id, N'IsUserTable') = 1
				AND dbo.syscolumns.name = 'Belegart')
BEGIN
ALTER TABLE FZPImporterBelegimport ADD [Belegart] varchar(3) NULL
END
GO




