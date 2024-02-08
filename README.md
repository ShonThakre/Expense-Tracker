# Expense Tracker

Expense Tracker is a web application designed to help users manage their finances effectively.
It offers features for expense categorization, transaction management, and financial data visualization. With robust identity security measures,
it ensures user privacy and protection against unauthorized access.

## Project Structure

The project directory includes the following components:

### Areas

- **Identity**: Contains pages and controllers for user authentication and access control.
- **Pages**: Includes various pages for managing user accounts, such as login, registration, password reset, and confirmation.

### Controllers

- **CategoryController.cs**: Manages categories related to expenses.
- **DashboardController.cs**: Controls the main dashboard functionality.
- **HomeController.cs**: Handles the home page and related actions.
- **ReportController.cs**: Deals with generating reports.
- **TransactionController.cs**: Manages transactions.

### Migrations

Contains database migration files for updating the database schema.

### Models

- **ApplicatonDbContext.cs**: Represents the application's database context.
- **Category.cs**: Defines the category model.
- **Transaction.cs**: Represents transaction data.

### Views

Contains folders for various areas and shared views used across the application.

### wwwroot

Includes static files such as stylesheets, scripts, and images.

### Other Files

- **Expense Tracker.csproj**: The project file.
- **Program.cs**: Contains the application's entry point.
- **appsettings.json**: Configuration file for the application.
- **.gitignore**: Specifies intentionally untracked files to ignore in version control.
- **Expense Tracker.sln**: The solution file.

## Technologies Used

- ASP.NET Core for backend development.
- ASP.NET Core Identity for user management.
- Entity Framework Core for database operations.
- Razor Pages and MVC for the frontend.
- C# programming language.
- SQL Server Database
- Automapper
- Syncfusion UI
- Identity

## Usage

To use the Expense Tracker application:

1. Clone the repository.
2. Configure the database connection string in the `appsettings.json` file.
3. Run migrations to create the database schema.
4. Build and run the application.

## Contributing

Contributions to the Expense Tracker project are welcome! Feel free to open issues for bug fixes, feature requests, or improvements, and submit pull requests to contribute code changes.


