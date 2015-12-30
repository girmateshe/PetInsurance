/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'GetPets' ) 
BEGIN           
    DROP proc GetPets
END   
*/

CREATE PROCEDURE [dbo].[GetPets]
	@policyId int = null
AS
	select Id, 
		   PetOwnerId, 
		   Name, 
		   DateOfBirth, 
		   EnrollmentDate,
		   BreadId, 
		   dbo.getBreedName(BreadId) as 'BreedName'
	from Pet
	where PetOwnerId = @policyId or @policyId is null