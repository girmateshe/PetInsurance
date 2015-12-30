/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'AddPetToPolicy' ) 
BEGIN           
    DROP proc AddPetToPolicy
END   
*/

CREATE PROCEDURE dbo.AddPetToPolicy
    @PetOwnerID int,
	@Name nvarchar(200),
	@DateOfBirth datetime,
	@BreedName nvarchar(200)
AS

declare @BreedId int

select @BreedId = b.Id
from Breed b
where Name = @BreedName 

INSERT INTO Pet(PetOwnerID, Name, DateOfBirth, EnrollmentDate, BreadId)
VALUES (@PetOwnerID, @Name, @DateOfBirth, GETDATE(), @BreedId);

select CAST(@@identity AS int ) 'Id', 
		@Name 'Name', 
		@PetOwnerID 'PetOwnerId', 
		@BreedName 'BreedName', 
		@BreedId 'BreedId'
