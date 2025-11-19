using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Users
    {
        public Users CreateNewUser(int securityNumber, string password)
        {
            return new Users(securityNumber, password);
        }
    }
}
