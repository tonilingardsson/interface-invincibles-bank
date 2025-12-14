# interface-invincibles-bank
# The Invincible Bank

A console-based banking application built in C# as a learning project to practice object-oriented programming and agile development methods.

## About the Project

This application simulates a banking system where users can log in as either customers or administrators. Customers can manage their accounts, transfer money with automatic currency conversion, deposit and withdraw funds, take loans, and view transaction history. Administrators can create new users, unlock locked accounts, and update exchange rates.

## Technologies Used

- C# / .NET
- Console Application
- File I/O for transaction logging

## Project Structure

The application is divided into several classes, each with specific responsibilities:

### Entry Point

**Program.cs**  
The starting point of the application. Creates a Bank instance and calls its `Run()` method to start the program.

### Core Classes

**Bank.cs**  
The main controller class that manages the entire banking system. It handles user authentication, coordinates transfers between accounts, and manages loan processing.

Key features:
- Stores all user accounts (both customers and admins) in a list
- Manages locked accounts (customers who fail to log in 3 times)
- Processes transfers on a 40-second interval using a queue system
- Validates account ownership before allowing transactions
- Handles the main program loop and user login

Important methods:
- `Run()` - Main program loop
- `UserLogIn()` - Handles user authentication with password masking
- `Transfer()` - Creates transfer objects and adds them to the queue
- `ProcessTransfers()` - Executes all queued transfers every 40 seconds
- `Borrow()` - Allows customers to take loans up to 5x their total account balance
- `GetBankAccountByNumber()` - Finds an account by its 4-digit number

**User.cs**  
The base class for all users in the system. Contains basic authentication properties and methods.

Properties:
- `SecurityNumber` - A 4-digit identifier for the user
- `Password` - User's password (protected)

Methods:
- `LogIn()` - Verifies security number and password

**Customer.cs**  
Inherits from User. Represents a customer who can own multiple bank accounts. Each customer automatically gets one default SEK account when created.

Main capabilities:
- `ShowAccounts()` - Displays all accounts owned by the customer
- `TransferMoney()` - Transfer between own accounts or to other customers
- `DepositMoney()` - Add money to an account with 5% interest calculation
- `WithdrawMoney()` - Remove money from an account (logs failed attempts)
- `BorrowMoney()` - Take a loan (max 5x total balance, 7% interest)
- `CreateBankAccount()` - Create a new account with chosen currency
- `ShowAccountHistory()` - View transaction history from account file

**Admin.cs**  
Inherits from User. Represents an administrator with special system privileges.

Admin functions:
- `CreateNewUser()` - Create new customer accounts
- `HandleLockedAccounts()` - Unlock accounts that were locked after 3 failed login attempts

**BankAccount.cs**  
Represents an individual bank account with its own balance, currency, and transaction file.

Properties:
- `Name` - Account name chosen by user
- `Sum` - Current balance
- `CurrencyType` - Currency (SEK, USD, EUR, or GBP)
- `AccountNumber` - Unique 4-digit identifier
- `FilePath` - Location of the account's transaction history file

Methods:
- `Deposit()` - Add money to the account
- `Withdraw()` - Remove money from the account
- `WriteToFile()` - Log transactions to the account's text file
- `CreateAccountFile()` - Creates a new transaction log file when account is created

**Transfer.cs**  
Represents a money transfer between two accounts. Handles currency conversion automatically.

Properties:
- `FromAccount` - Account sending money
- `ToAccount` - Account receiving money
- `FromAmount` - Amount withdrawn from sender (in sender's currency)
- `ToAmount` - Amount deposited to receiver (converted to receiver's currency)
- `Timestamp` - When the transfer was created

The Transfer class automatically converts currencies when created and stores both the original and converted amounts.

**Currency.cs**  
A static class that manages exchange rates and currency conversions. Uses SEK as the base currency.

Supported currencies:
- SEK (Swedish Krona) - base currency, always 1.0
- USD (US Dollar)
- EUR (Euro)
- GBP (British Pound)

Methods:
- `Convert()` - Converts an amount from one currency to another
- `UpdateCurrencyValue()` - Randomly adjusts exchange rates by ±5%
- `ShowCurrencies()` - Displays current exchange rates

### UI and Input Classes

**UI.cs**  
Handles all visual output to the console. Creates bordered message boxes for a cleaner look.

Key methods:
- `DisplayLoggo()` - Shows the bank's ASCII art logo
- `DisplayMessage()` - Shows messages in a bordered box with custom colors
- `DisplayFile()` - Reads and displays a file's contents
- `HidePassword()` - Masks password input with asterisks
- `WriteContinueMessage()` - Prompts user to press any key

**Input.cs**  
A static class that validates and retrieves user input.

Methods:
- `GetNumberFromUser()` - Gets an integer within a specified range
- `GetDecimalFromUser()` - Gets a positive decimal number
- `GetAccountNumberFromUser()` - Gets a valid 4-digit account number
- `GetString()` - Gets a non-empty string from user
- `GetCurrency()` - Lets user choose from 4 available currencies

**Menu.cs**  
Displays menu options and routes user choices to the appropriate methods.

- `AdminMenu()` - Shows admin options (create user, update currencies, unlock accounts)
- `CustomerMenu()` - Shows customer options (show accounts, create account, transfer, deposit, withdraw, history, borrow)

Both menus check the 40-second transfer processing interval before displaying options.

## How It Works

### Program Flow

1. Application starts in `Program.cs` and creates a Bank instance
2. Bank displays the logo and prompts user to log in or exit
3. User enters their 4-digit security number
4. User enters password (hidden with asterisks)
5. If login succeeds, user sees either Admin or Customer menu
6. User can perform various banking operations
7. Transfers are queued and processed every 40 seconds
8. User logs out and returns to login screen

### Transfer Queue System

Instead of executing transfers immediately, the system uses a queue:
- When a customer initiates a transfer, it's added to `ListOfTransfers`
- Every 40 seconds (tracked by `lastRunTime` and `interval`), `ProcessTransfers()` runs
- All queued transfers are executed at once
- This simulates real-world banking where transactions aren't instant

### Security Features

- Passwords are masked during login (shown as asterisks)
- After 3 failed login attempts, customer accounts are locked
- Only admins can unlock accounts
- Users can only transfer from accounts they own

### Currency System

All currencies are stored relative to SEK:
- When creating an account, the starting balance is converted to the chosen currency
- When transferring between different currencies, amounts are automatically converted
- Admins can update exchange rates, which fluctuate by ±5%

### Transaction Logging

Each bank account has its own text file that logs:
- Account details (number, name, currency)
- All deposits and withdrawals
- All transfers (sent and received)
- Loan transactions
- Failed withdrawal attempts

## Class Relationships

- **Bank** manages collections of **User** objects (both **Customer** and **Admin**)
- Each **Customer** has a list of **BankAccount** objects
- **Transfer** objects connect two **BankAccount** instances
- **Currency** is used by **BankAccount** and **Transfer** for conversions
- **Menu** displays options and calls methods in **Customer** or **Admin**
- **Input** validates user input for all interactive classes
- **UI** handles all console output throughout the application

## Test Accounts

The Bank constructor creates these test accounts:

**Admin:**
- Security Number: `1111`
- Password: `1111`

**Customer 1:**
- Security Number: `2222`
- Password: `2222`
- Accounts: Bank Account (SEK), CSN Life (SEK), Dev Life (EUR)

**Customer 2:**
- Security Number: `3333`
- Password: `3333`
- Accounts: Bank Account (SEK), Dada Leif (SEK), Vacation savings (EUR)

## Running the Application

1. Open the project in Visual Studio or your preferred C# IDE
2. Build and run the application
3. Use one of the test accounts above to log in
4. Follow the on-screen menu options

## Features

- User authentication with password masking
- Multiple account management per customer
- Money transfers between accounts with currency conversion
- Automatic transfer processing every 40 seconds
- Deposit and withdrawal with transaction logging
- Loan system (borrow up to 5x total balance)
- Interest calculation on deposits
- Account locking after failed login attempts
- Admin functions for user management and currency updates
- File-based transaction history for each account

## Learning Goals

This project was created to practice:
- Object-oriented programming (inheritance, encapsulation, polymorphism)
- Working with lists and collections
- File I/O operations
- Input validation
- Agile development methodology
- Class design and separation of concerns

