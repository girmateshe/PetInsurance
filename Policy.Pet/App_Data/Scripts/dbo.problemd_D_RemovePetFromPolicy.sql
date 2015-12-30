/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'RemovePetFromPolicy' ) 
BEGIN           
    DROP proc RemovePetFromPolicy
END   
*/

CREATE PROCEDURE [dbo].[RemovePetFromPolicy]
@ID int
AS

	Delete [dbo].[Pet]
	where ID = @ID

	select    Id
			, PetOwnerId
			, Name
			, DateOfBirth
			, BreadId
			, dbo.getBreedName(BreadId) as 'BreedName'
	from Pet
	where ID = @ID