USE [SchoolAppDb];
GO

IF OBJECT_ID(N'dbo.Course', N'U') IS NULL
BEGIN
    THROW 50001, 'Table dbo.Course does not exist. Run 2_create_course_table.sql first.', 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Course WHERE CourseCode = N'MATH101')
BEGIN
    INSERT INTO dbo.Course (CourseName, CourseCode, CourseCredit)
    VALUES (N'Calculus I', N'MATH101', 4);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Course WHERE CourseCode = N'CS101')
BEGIN
    INSERT INTO dbo.Course (CourseName, CourseCode, CourseCredit)
    VALUES (N'Introduction to Programming', N'CS101', 5);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Course WHERE CourseCode = N'PHY101')
BEGIN
    INSERT INTO dbo.Course (CourseName, CourseCode, CourseCredit)
    VALUES (N'Physics I', N'PHY101', 4);
END
GO
