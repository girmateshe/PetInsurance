/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'DeletePetOwner' ) 
BEGIN           
    DROP proc DeletePetOwner
END   
*/

CREATE PROCEDURE [dbo].[DeletePetOwner]
@ID nvarchar(40)
AS

	Update [dbo].[PetOwner]
	SET Archived = 1
	where ID = @ID

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