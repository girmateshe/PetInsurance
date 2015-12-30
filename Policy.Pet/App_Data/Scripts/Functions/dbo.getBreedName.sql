/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'getBreedName' ) 
BEGIN           
    DROP function getBreedName
END   
*/

USE [InsurancePolicy]
GO

CREATE FUNCTION [dbo].[getBreedName] 
(
	@BreedID int
)
RETURNS nvarchar(max)
AS
BEGIN
	
	DECLARE @BreedName nvarchar(max)

    select @BreedName = (select Name from Breed where ID = @BreedID)

	RETURN isnull(@BreedName,'')

END 