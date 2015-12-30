/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'InsertPetOwner' ) 
BEGIN           
    DROP procedure InsertPetOwner
END   
*/

CREATE PROCEDURE [dbo].[InsertPetOwner]
    @Name nvarchar(200), 
    @IsoCode char(3),
	@Email nvarchar(256)  
AS

declare @countryId int
declare @policyNumber char(13)
declare @policyDate datetime

select @countryId = Id from dbo.Country where IsoCode = @IsoCode

begin tran

	select @policyDate = getdate()
	exec dbo.generatePolicyNumber @IsoCode, @policyNumber out

	INSERT INTO PetOwner(Name, PolicyNumber, PolicyDate, CountryId, Email)
	VALUES (@Name, @policyNumber, @policyDate , @CountryId, @Email);

commit tran

select CAST(@@identity AS int ) 'Id', 
		@Name 'Name', 
		@policyNumber 'PolicyNumber', 
		@policyDate 'PolicyDate', 
		@IsoCode 'CountryIsoCode', 
		@Email 'Email'

