CREATE VIEW [dbo].[vw_audiusGuV]
AS
SELECT
	LTRIM(STR(Quelle.Mandant)) + ' - ' + dbo.KHKMandanten.Wert AS Mandant,
	CONVERT([int], LEFT(Quelle.Periode, 4)) AS Jahr,
	CONVERT([int], RIGHT(Quelle.Periode, 3)) AS Monat,
	GuVSum.Name AS Summe,
	GuVPos.Name AS Position,
	dbo.KHKGruppen.Bezeichnung AS Verdichtungskostenart,
	Quelle.Kto + ' - ' + dbo.KHKSachkonten.Bezeichnung AS Sachkonto,
	SUM(ISNULL(Quelle.HabenEw, 0) - ISNULL(Quelle.SollEw, 0)) AS Saldo
	FROM dbo.KHKBuchungsjournal AS Quelle
	INNER JOIN dbo.audiusGuVSachkonten AS GuVSaKto ON Quelle.Kto BETWEEN GuVSaKto.SaKtoVon AND GuVSaKto.SaKtoBis
	INNER JOIN dbo.audiusGuVPositionen AS GuVPos ON GuVSaKto.Position = GuVPos.Id
	INNER JOIN dbo.audiusGuVPositionen AS GuVSum ON GuVPos.Position = GuVSum.Id
	INNER JOIN dbo.KHKMandanten ON Quelle.Mandant = dbo.KHKMandanten.Mandant AND dbo.KHKMandanten.Eigenschaft = 1 AND dbo.KHKMandanten.Jahr = 0 AND dbo.KHKMandanten.PartnerId = '*'
	INNER JOIN dbo.KHKSachkonten ON Quelle.Mandant = dbo.KHKSachkonten.Mandant AND Quelle.Kto = dbo.KHKSachkonten.SaKto
	LEFT OUTER JOIN dbo.KHKGruppen ON Quelle.Mandant = dbo.KHKGruppen.Mandant AND dbo.KHKSachkonten.Verdichtungskostenart = dbo.KHKGruppen.Gruppe AND dbo.KHKGruppen.Typ = 10002
	GROUP BY
		Quelle.Mandant,
		dbo.KHKMandanten.Wert,
		Quelle.Periode,
		GuVSum.Name,
		GuVPos.Name,
		dbo.KHKGruppen.Bezeichnung,
		Quelle.Kto,
		dbo.KHKSachkonten.Bezeichnung
