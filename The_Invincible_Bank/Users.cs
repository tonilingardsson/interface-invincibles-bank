using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Users
    {
        private int SecurityNumber { get; set; }
        private string Password { get; set; }

        public Users(int securityNumber, string password)
        {
            SecurityNumber = securityNumber;
            Password = password;
        }

        public bool LogIn(int securityNumber, string password)
        {
            //Code here! :D
            return true;
        }
        public Users CreateNewUser(int securityNumber, string password)
        {
            return new Users(securityNumber, password);
        }
    }
}
