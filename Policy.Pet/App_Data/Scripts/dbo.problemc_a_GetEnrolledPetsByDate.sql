/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'GetPetsCountByEnrollmentDate' ) 
BEGIN           
    DROP proc GetPetsCountByEnrollmentDate
END   
*/
CREATE PROCEDURE [dbo].[GetPetsCountByEnrollmentDate]
	@enrollmentDate datetime
AS

	select count(*)
	from Pet
	where DateDiff(dd, EnrollmentDate, @enrollmentDate) = 0
