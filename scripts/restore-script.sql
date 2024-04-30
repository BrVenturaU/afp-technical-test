CREATE DATABASE AfpCompanyDb
GO

USE AfpCompanyDb
GO

CREATE TABLE [Company] (
  [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
  [Name] NVARCHAR(100) NOT NULL,
  [OfficialName] NVARCHAR(100) NOT NULL,
  [CreatedAt] DATETIME2,
  [UpdatedAt] DATETIME2,
  [DeletedAt] DATETIME2
)
GO

CREATE TABLE [Departments] (
  [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
  [Name] NVARCHAR(100) NOT NULL,
  [EmployeesCount] INT DEFAULT (0),
  [Level] nvarchar(255) NOT NULL CHECK ([Level] IN ('ESTRATEGICO', 'TACTICO', 'OPERATIVO')),
  [CompanyId] BIGINT NOT NULL
)
GO

ALTER TABLE [Departments] ADD FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id])
GO

CREATE OR ALTER PROCEDURE SPGET_COMPANY_BY_ID(
	@CompanyId INT
)
AS
BEGIN
	SELECT * FROM Company
	WHERE Id = @CompanyId
END
GO

CREATE OR ALTER PROCEDURE SPGET_DEPARTMENTS_BY_COMPANY(
	@CompanyId INT
)
AS
BEGIN
	SELECT * FROM Departments 
	WHERE CompanyId = @CompanyId
END
GO

INSERT INTO Company (Name, OfficialName) VALUES ('Fliptune', 'Kaymbo');
INSERT INTO Company (Name, OfficialName) VALUES ('Feedfish', 'Edgepulse');
INSERT INTO Company (Name, OfficialName) VALUES ('Devpulse', 'Divanoodle');
INSERT INTO Company (Name, OfficialName) VALUES ('Skyvu', 'Edgewire');
INSERT INTO Company (Name, OfficialName) VALUES ('Thoughtbeat', 'Brainbox');
INSERT INTO Company (Name, OfficialName) VALUES ('Youtags', 'Yakidoo');
INSERT INTO Company (Name, OfficialName) VALUES ('Eare', 'Realcube');
INSERT INTO Company (Name, OfficialName) VALUES ('Twinder', 'Linkbuzz');
INSERT INTO Company (Name, OfficialName) VALUES ('Bubblebox', 'Flashset');
INSERT INTO Company (Name, OfficialName) VALUES ('Riffpath', 'Gabvine');
GO

INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Support', 97, 5, 'ESTRATEGICO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Legal', 97, 7, 'ESTRATEGICO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Marketing', 25, 6, 'ESTRATEGICO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Engineering', 15, 4, 'ESTRATEGICO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Training', 56, 5, 'TACTICO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Research and Development', 50, 3, 'TACTICO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Human Resources', 55, 6, 'TACTICO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Legal', 63, 7, 'OPERATIVO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Sales', 25, 2, 'OPERATIVO');
INSERT INTO Departments (Name, EmployeesCount, CompanyId, Level) VALUES ('Engineering', 73, 9, 'OPERATIVO');
GO