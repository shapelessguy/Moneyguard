using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyguard
{
    class MyUser
    {
        public MyUser()
        {

        }
        public MyUser(string email, string pass, string gen, string name, string nic)
        {
            Email = email;
            Password = pass;
            Gender = gen;
            Fullname = name;
            NICno = nic;
        }

        public static string error;
        public static bool isEqual(MyUser user1, MyUser user2)
        {
            error = "";
            if (user1 == null || user2 == null) { error += "account doesn't exist"; return false; }
            if (user1.Email != user2.Email) { if (error != "") error += ", "; error += "usernames doesn't match"; }
            if (user1.Password != user2.Password) { if (error != "") error += ", "; error += "passwords doesn't match"; }
            //if (user1.Gender != user2.Gender) { if (error != "") error += ", "; error += "genders doesn't match"; }
            //if (user1.Fullname != user2.Fullname) { if (error != "") error += ", "; error += "names doesn't match"; }
            //if (user1.NICno != user2.NICno) { if (error != "") error += ", "; error += "phone numbers doesn't match"; }
            if (error == "") return true; else return false;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Fullname { get; set; }
        public string NICno { get; set; }
    }
}
