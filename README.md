RFC Generator Web Application
A complete ASP.NET MVC web application that generates Mexican RFC (Registro Federal de Contribuyentes) codes according to official SAT specifications.
Features
Core Functionality

RFC Generation: Automatically generates RFC codes based on user's personal information following official Mexican tax authority rules
CRUD Operations: Complete Create, Read, Update, Delete functionality for RFC records
Database Management: Full database integration with SQL Server and stored procedures
Search Functionality: Advanced search across names, dates, and RFC codes with partial matching
Duplicate Detection: Visual highlighting of duplicate RFC entries in red text

RFC Generation Rules Implementation
The application implements all official RFC generation rules including:

Basic Structure: First 4 letters from surnames and name + 6-digit birth date (YYMMDD)
Special Character Handling: Automatic replacement of Ñ characters with "X"
Compound Names: Proper handling of names like "MARIA LUISA" or "JOSE CARLOS"
Compound Surnames: Uses only the first word of multi-word surnames
Missing Data: Assigns "X" for missing apellido materno or missing vowels
Inappropriate Words: Filters and replaces offensive 4-letter combinations with "X"
Complete Exception Handling: Implements all edge cases per SAT specifications

User Interface

Clean Bootstrap Design: Responsive web interface using Bootstrap framework
Multi-page Workflow: Separate views for generation, editing, deletion, and database browsing
Form Validation: Client and server-side validation for required fields
Success/Error Messages: User feedback for all operations using TempData
Confirmation Dialogs: Safe deletion with confirmation screens

Technical Architecture

3-Layer Architecture: Presentation (MVC), Business Logic, Data Access separation
Entity Framework: Structured data models with Ent_Rfc entity
Stored Procedures: Database operations through SQL Server stored procedures
Exception Handling: Comprehensive error handling throughout the application
MVC Pattern: Clean separation of concerns following ASP.NET MVC best practices

Technology Stack

Framework: ASP.NET MVC 5
Language: C#
Database: SQL Server
Frontend: HTML5, CSS3, Bootstrap
Data Access: ADO.NET with SqlConnection
Architecture: 3-Layer (Presentation, Business, Data)
---

*Note: This application is for educational purposes and generates RFC codes according to publicly available specifications. For official RFC registration, consult the Mexican Tax Authority (SAT).*
