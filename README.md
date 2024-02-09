Expense Tracker
Expense Tracker is a comprehensive web application designed to streamline financial management tasks. It offers robust features for expense categorization, transaction management, and insightful data visualization. With a strong focus on identity security, users can trust their privacy is protected against unauthorized access.

Project Components
Areas:

Identity: Handles user authentication and access control.
Pages: Manages user accounts, including login, registration, and password reset.
Controllers:

Category, Dashboard, Home, Report, and Transaction controllers oversee various aspects of the application's functionality.
Migrations: Database migration files facilitate schema updates.

Models:

ApplicationDbContext, Category, and Transaction models maintain data integrity.
Views: Organized folders contain shared and area-specific views.

wwwroot: Houses static files like stylesheets and scripts.

Other Files: Includes project configuration files, such as .gitignore and appsettings.json.

Technologies Utilized
Backend Development: ASP.NET Core with Entity Framework Core for data operations.
Frontend Frameworks: Razor Pages and MVC for user interface development.
Database: SQL Server database for data storage.
Additional Tools: Automapper for object mapping, Syncfusion UI components for enhanced user experience, and Identity for user management.
Usage
To deploy the Expense Tracker application:

Clone the repository.
Configure the database connection string in appsettings.json.
Run migrations to establish the database schema.
Build and launch the application.
Contributing
Contributions to the Expense Tracker project are encouraged! Feel free to report bugs, suggest features, or submit pull requests for improvements to enhance the user experience and functionality.
