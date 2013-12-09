IF (OBJECT_ID('dbo.Vacancies', 'U') IS NULL)
	CREATE TABLE dbo.Vacancies
	(
		Id				int				NOT NULL	IDENTITY(1,1) PRIMARY KEY,
		SourceName		nvarchar(30)	NOT NULL,
		SourceInnerId	nvarchar(20)	NOT NULL,
		DateAdded		date			NOT NULL,
	)

IF (OBJECT_ID('dbo.VacancyDetails', 'U') IS NULL)
	CREATE TABLE dbo.VacancyDetails
	(
		Id				int				NOT NULL	IDENTITY(1,1),
		InsertDatetime	datetime		NOT NULL,
		VacancyId		int				NULL,

		VacancyTitle	nvarchar(50)	NULL,
		CompanyName		nvarchar(50)	NULL,
		City			nvarchar(30)	NULL,

		SalaryMin		money			NULL,
		SalaryMax		money			NULL,
		SalaryCurrency	nvarchar(25)	NULL,

		Education		nvarchar(100)	NULL,
		ExperienceMin	float			NULL,
		ExperienceMax	float			NULL,

		EmploymentType	nvarchar(100)	NULL,
		Skills			nvarchar(max)	NULL,
		VacancyText		nvarchar(max)	NULL,

		ContactPerson	nvarchar(50)	NULL,
		ContactCompany	nvarchar(50)	NULL,
		ContactPhone	nvarchar(30)	NULL,
		ContactEmail	nvarchar(30)	NULL,
		ContactSite		nvarchar(30)	NULL,

		DateUpdated		date			NULL,
		NumberOfViews	int				NULL,
		SourceLink		nvarchar(200)	NOT NULL,

		CONSTRAINT PK_Vacancies PRIMARY KEY CLUSTERED (Id),
		CONSTRAINT FK_Vacancies_VacancyDetails FOREIGN KEY (VacancyId) REFERENCES Vacancies(Id)
	)

IF (OBJECT_ID('dbo.LastUpdateDatetime', 'U') IS NULL)
	CREATE TABLE dbo.LastUpdateDatetime
	(
		DatetimeMark	datetime		NOT NULL
	)

IF (OBJECT_ID('dbo.SetLastUpdateDatetime', 'P') IS NOT NULL)
	DROP PROCEDURE dbo.SetLastUpdateDatetime
GO

CREATE PROCEDURE dbo.SetLastUpdateDatetime
(
	@DatetimeToSave	datetime
)
AS
BEGIN

	IF (EXISTS (SELECT * FROM dbo.LastUpdateDatetime))
		DELETE FROM dbo.LastUpdateDatetime

	INSERT dbo.LastUpdateDatetime (DatetimeMark)
	VALUES (@DatetimeToSave)

END
GO

IF (OBJECT_ID('dbo.InsertVacancyDetails', 'P') IS NOT NULL)
	DROP PROCEDURE dbo.InsertVacancyDetails
GO

CREATE PROCEDURE dbo.InsertVacancyDetails
(
		@VacancyTitle	nvarchar(50)	= NULL,
		@CompanyName	nvarchar(50)	= NULL,
		@City			nvarchar(30)	= NULL,

		@SalaryMin		money			= NULL,
		@SalaryMax		money			= NULL,
		@SalaryCurrency	nvarchar(25)	= NULL,

		@Education		nvarchar(100)	= NULL,
		@ExperienceMin	float			= NULL,
		@ExperienceMax	float			= NULL,
		@EmploymentType	nvarchar(100)	= NULL,
		@Skills			nvarchar(max)	= NULL,
		@VacancyText	nvarchar(max)	= NULL,

		@ContactPerson	nvarchar(50)	= NULL,
		@ContactCompany	nvarchar(50)	= NULL,
		@ContactPhone	nvarchar(30)	= NULL,
		@ContactEmail	nvarchar(30)	= NULL,
		@ContactSite	nvarchar(30)	= NULL,

		@DateAdded		date,
		@DateUpdated	date			= NULL,
		@NumberOfViews	int				= NULL,
		@SourceName		nvarchar(30),
		@SourceInnerId	nvarchar(20),
		@SourceLink		nvarchar(200)
)
AS
BEGIN

	DECLARE @vacancyId int = (SELECT TOP 1 Id
							  FROM dbo.Vacancies AS v
							  WHERE v.SourceName = @SourceName
								AND v.SourceInnerId = @SourceInnerId
								AND v.DateAdded = @DateAdded)

	IF (@vacancyId IS NULL)
	BEGIN
		INSERT INTO dbo.Vacancies (SourceName,  SourceInnerId,  DateAdded)
		VALUES					  (@SourceName, @SourceInnerId, @DateAdded)

		SELECT @vacancyId = MAX(Id)
		FROM dbo.Vacancies
	END

	INSERT dbo.VacancyDetails (VacancyId,  InsertDatetime,  VacancyTitle,  CompanyName,  City,  SalaryMin,  SalaryMax,  SalaryCurrency,  Education,  ExperienceMin,  ExperienceMax,  EmploymentType,  Skills,  VacancyText,  ContactPerson,  ContactCompany,  ContactPhone,  ContactEmail,  ContactSite,  DateUpdated,  NumberOfViews,  SourceLink)
	VALUES                    (@vacancyId, GETDATE(),       @VacancyTitle, @CompanyName, @City, @SalaryMin, @SalaryMax, @SalaryCurrency, @Education, @ExperienceMin, @ExperienceMax, @EmploymentType, @Skills, @VacancyText, @ContactPerson, @ContactCompany, @ContactPhone, @ContactEmail, @ContactSite, @DateUpdated, @NumberOfViews, @SourceLink)

END
GO