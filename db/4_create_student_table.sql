USE [SchoolAppDb];

IF OBJECT_ID(N'dbo.Student', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Student
    (
        StudentID      INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        StudentName    NVARCHAR(100) NOT NULL,
        StudentSurname NVARCHAR(100) NOT NULL CONSTRAINT DF_Student_Surname DEFAULT (N''),
        StudentEmail   NVARCHAR(200) NOT NULL CONSTRAINT DF_Student_Email DEFAULT (N'')
    );
END

