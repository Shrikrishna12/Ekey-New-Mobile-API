using System;


namespace TRA_MobileAPIs.ViewModel
{
    public class LoginVM
    {

        public string email { get; set; }
        public string currentPass { get; set; }
        public string newPassword { get; set; }

       

    }


    
    public class accountInfo
    {

        public string email { get; set; }
        public string currentPass { get; set; }
        public Guid contactid { get; set; }

    }



    public class accountLabel
    {

        public const string emailId = "emailaddress1";
        public const string Currentpassword = "tra_password";
        public const string newPassword = "tra_newPassword";
        public const string _ContactID = "contactid";
        public const string IsSocialToken = "isSocialToken";
        public const string socialToken = "tra_socialtoken";
        public const string tra_idnumber = "tra_idnumber";
    }


    public class LoginVMs
    {

        public const string email = "emailaddress1";
        public const string password = "tra_password";
        public const string _ContactID = "contactid";
        public const string _resetPass = "tra_resetpassword";
      
    }

    public class loginData
    {
        public string _email { get; set; }
        public string _password { get; set; }
        public Guid ContactId { get; set; }    
        public string socialtoken { get; set; }

        public  string tra_idnumber { get; set; }


}
    public class compaireLoginData
    {
        public string _email1 { get; set; }
        public string _password1 { get; set; }
        public Guid ContId { get; set; }
    public string tra_idnumber { get; set; }

}
}