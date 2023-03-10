using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourNoteUWP.Util
{
    internal class UserUtilities
    {
        public static string CheckLength(string email)
        {
            if (email.Length < 2)
                return "Password can not be less than 3 characters";

            return null;
        }
        public static string CheckNullOrEmpty(string email)
        {
            if (String.IsNullOrEmpty(email) == true)
                return "This field cant be empty";

            return null;
        }
        public static string IsPasswordCheck(string check)
        {
            string checkNullOrEmpty = CheckNullOrEmpty(check);

            if (checkNullOrEmpty != null)
                return checkNullOrEmpty;

            string checkLength = CheckLength(check);


            if (checkLength != null)
                return checkLength;

            return null;
        }

      

        public static string CheckValidEmail(string email)
        {
            if (Models.User.IsValidEmail(email) == false)
                return "This is not a valid email address";
            return null;

        }

  



    }
}
