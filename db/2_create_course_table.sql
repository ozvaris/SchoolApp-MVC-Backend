USE [SchoolAppDb];
GO

IF OBJECT_ID(N'dbo.Course', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Course
    (
        CourseID     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        CourseName   NVARCHAR(200) NOT NULL,
        CourseCode   NVARCHAR(50)  NOT NULL,
        CourseCredit INT NOT NULL,
        CONSTRAINT UQ_Course_CourseCode UNIQUE (CourseCode),
        CONSTRAINT CK_Course_CourseCredit CHECK (CourseCredit > 0)
    );
END
GO
