using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class User
    {
        protected string Password { get; set; }

        protected int _securityNumber;
        public int SecurityNumber {get { return _securityNumber; } }

        public User(int securityNumber, string password)
        {
            Password = password;
            _securityNumber = securityNumber;
        }
        public bool LogIn(int securityNumber, string password)
        {
            if (securityNumber == SecurityNumber)
            {
                if (password == Password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
