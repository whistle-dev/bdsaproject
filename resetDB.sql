DELETE FROM [bdsaproject].[dbo].[Commits] WHERE Sha IS NOT NULL;
DELETE FROM [bdsaproject].[dbo].[Repos] WHERE Path IS NOT NULL;
