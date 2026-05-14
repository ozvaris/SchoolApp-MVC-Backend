# SQL Script Calistirma Ornekleri

Bu klasordeki scriptleri su sirayla calistir:

1. `1_create_database.sql`
2. `2_create_course_table.sql`
3. `3_seed_course.sql`
4. `4_create_student_table.sql`
5. `5_seed_student.sql`

## 1) Host makineden calistirma (onerilen)

Proje kokunde:

```bash
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '$MSSQL_SA_PASSWORD' < db/1_create_database.sql
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '$MSSQL_SA_PASSWORD' < db/2_create_course_table.sql
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '$MSSQL_SA_PASSWORD' < db/3_seed_course.sql
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '$MSSQL_SA_PASSWORD' < db/4_create_student_table.sql
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '$MSSQL_SA_PASSWORD' < db/5_seed_student.sql
```

## 2) Container icinden calistirma (Attach shell)

Container shell icinde scriptleri tek tek calistirmak istersen:

```bash
/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '<SA_PASSWORD>'
```

Sonra sqlcmd ekraninda script icerigini yapistirip `GO` ile calistirabilirsin.

## 3) Dogrulama sorgusu

```bash
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '$MSSQL_SA_PASSWORD' -d SchoolAppDb -Q "SELECT CourseID, CourseName, CourseCode, CourseCredit FROM dbo.Course;"
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P '$MSSQL_SA_PASSWORD' -d SchoolAppDb -Q "SELECT StudentID, StudentName, StudentSurname, StudentEmail FROM dbo.Student;"
```
