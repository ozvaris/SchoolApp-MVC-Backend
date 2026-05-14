USE [SchoolAppDb];


IF OBJECT_ID(N'dbo.Student', N'U') IS NULL
BEGIN
    THROW 50001, 'Table dbo.Student does not exist. Run 4_create_student_table.sql first.', 1;
END

SET IDENTITY_INSERT dbo.Student ON;

IF NOT EXISTS (SELECT 1 FROM dbo.Student WHERE StudentID = 1001)
BEGIN
    INSERT INTO dbo.Student (StudentID, StudentName, StudentSurname, StudentEmail)
    VALUES (1001, N'Ali', N'Yilmaz', N'ali.yilmaz@example.com');
END

IF NOT EXISTS (SELECT 1 FROM dbo.Student WHERE StudentID = 1002)
BEGIN
    INSERT INTO dbo.Student (StudentID, StudentName, StudentSurname, StudentEmail)
    VALUES (1002, N'Ayse', N'Kaya', N'ayse.kaya@example.com');
END

SET IDENTITY_INSERT dbo.Student OFF;

