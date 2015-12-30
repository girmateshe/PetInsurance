/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'FindPetsByOwnerID' ) 
BEGIN           
    DROP proc FindPetsByOwnerID
END   
*/

create PROCEDURE [dbo].[FindPetsByOwnerID]
	@OwnerID int
AS

	select    Id
			, PetOwnerId
			, Name
			, DateOfBirth
			, BreadId
			, dbo.getBreedName(BreadId) as 'BreedName'
	from Pet
	where Id = @OwnerID
