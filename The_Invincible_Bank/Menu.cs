using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    static class Menu
    {
        public static void AdminMenu(Admin admin)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                if (DateTime.Now - Bank.lastRunTime >= Bank.interval)
                {
                    Bank.ProcessTransfers();
                    Bank.lastRunTime = DateTime.Now;
                }

                UI.DisplayMessage("1: Create new user\n2: Update currency value\n3: Handle locked accounts\n4: Log out");

                switch (Input.GetNumberFromUser(1, 4))
                {
                    case 1:
                        Console.Clear();
                        admin.CreateNewUser();
                        UI.WriteContinueMessage();
                        break;
                    case 2:
                        Console.Clear();
                        Currency.ShowCurrencies();
                        UI.DisplayMessage("Updating currency values...", ConsoleColor.DarkGray, ConsoleColor.DarkGray);
                        Thread.Sleep(2000);
                        UI.DisplayMessage("Currencies updated!", ConsoleColor.Green, ConsoleColor.Green);
                        Currency.UpdateCurrencyValue();
                        Currency.ShowCurrencies();
                        UI.WriteContinueMessage();
                        break;
                    case 3:
                        admin.HandleLockedAccounts();
                        break;
                    case 4:
                        Console.Clear();
                        exit = true;
                        break;

                }
            }
        }
        public static void CustomerMenu(Customer customer)
        {
            
            bool exist = false;

            while (!exist)
            {
                Console.Clear();

                var time = (DateTime.Now - Bank.lastRunTime);

                if (time >= Bank.interval)
                {
                    Bank.ProcessTransfers();
                    Bank.lastRunTime = DateTime.Now;
                }

                UI.DisplayMessage("1: Show Accounts\n2: Create new account\n3: Transfer money \n4: Deposit money\n5: Show account history\n6: Borrow money\n7: Log out");

                switch (Input.GetNumberFromUser(1, 8))
                {
                    case 1:
                        Console.Clear();
                        customer.ShowAccounts();
                        UI.WriteContinueMessage();
                        break;
                    case 2:
                        Console.Clear();
                        customer.CreateBankAccount(Input.GetString(), Input.GetCurrency());
                        UI.DisplayMessage("Account was created!", ConsoleColor.Green, ConsoleColor.Green);
                        UI.WriteContinueMessage();
                        break;
                    case 3:
                        Console.Clear();
                        customer.TransferMoney();
                        UI.WriteContinueMessage();
                        break;
                    case 4:
                        Console.Clear();
                        customer.DepositMoney();
                        UI.WriteContinueMessage();
                        break;
                    case 5:
                        Console.Clear();
                        customer.ShowAccountHistory();
                        UI.WriteContinueMessage();
                        break;
                    case 6:
                        Console.Clear();
                        customer.BorrowMoney();
                        UI.WriteContinueMessage();
                        break;
                    case 7:
                        Console.Clear();
                        exist = true;
                        break;
                }
            }
        }
    }

}
