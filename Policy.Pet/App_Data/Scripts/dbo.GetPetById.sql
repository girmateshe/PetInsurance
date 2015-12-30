/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'GetPetById' ) 
BEGIN           
    DROP proc GetPetById
END   
*/

CREATE PROCEDURE [dbo].[GetPetById]
	@ID int
AS
	select Id, 
		   PetOwnerId, 
		   Name, 
		   DateOfBirth, 
		   BreadId, 
		   dbo.getBreedName(BreadId) as 'BreedName'
	from Pet
	where Id = @id