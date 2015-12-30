/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'GetPetOwnerById' ) 
BEGIN           
    DROP proc GetPetOwnerById
END   
*/

CREATE PROCEDURE [dbo].[GetPetOwnerById]
	@ID int
AS

	select  Id, 
			Name, 
			PolicyNumber, 
			PolicyDate, 
			CountryID, 
			Email, 
			Archived, 
			dbo.getCountryIsoCode(CountryID) as 'CountryIsoCode' 
	From [dbo].[PetOwner]
	where ID = @ID