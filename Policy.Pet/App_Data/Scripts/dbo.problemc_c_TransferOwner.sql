/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'TransferOwner' ) 
BEGIN           
    DROP proc TransferOwner
END   
*/

CREATE PROCEDURE [dbo].[TransferOwner]
	@fromOwnerId int,
	@toOwnerId int
AS

	DECLARE pet_cursor CURSOR FOR 
	select Id
	from Pet
	where PetOwnerId = @fromOwnerId

	begin tran

	declare @petId int

	OPEN pet_cursor

	FETCH NEXT FROM pet_cursor 
	INTO @petId

	WHILE @@FETCH_STATUS = 0
	BEGIN
			
		update Pet
		set PetOwnerId = @toOwnerId
		where Id = @petId

		IF (@@ROWCOUNT = 0)
		BEGIN
			 rollback tran
		END
				-- Get the next pet.
		FETCH NEXT FROM pet_cursor 
		INTO @petId
	END 

	CLOSE pet_cursor;
	DEALLOCATE pet_cursor;

	commit tran

	select  Id, 
			PetOwnerId, 
			Name, 
			DateOfBirth, 
			BreadId, 
			Archived, 
			dbo.getBreedName(BreadId) as 'BreedName'
	from Pet
	where PetOwnerId = @toOwnerId and Archived = 0