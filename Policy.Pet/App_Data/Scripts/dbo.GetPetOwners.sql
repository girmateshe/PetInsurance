/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'GetPetOwners' ) 
BEGIN           
    DROP proc GetPetOwners
END   
*/

CREATE PROCEDURE [dbo].[GetPetOwners]
AS
	select Id
	, Name
	, PolicyNumber
	, PolicyDate
	, CountryID
	, Email
	, Archived
	, dbo.getCountryIsoCode(CountryID) as 'CountryIsoCode' 
	from PetOwner
	where Archived = 0 or Archived is null
	order by policyDate desc