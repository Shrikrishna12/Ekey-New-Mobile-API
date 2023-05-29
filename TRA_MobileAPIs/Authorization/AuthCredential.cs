
using TRA_MobileAPIs.ConfigSettings;

namespace TRA_MobileAPIs.Authorization
{
    public class AuthCredential
    {
        public static bool Login(string Username,string Password)
        {

            ConfigData _serviceConfiguration = ConfigEncrypt.GetCrmCredentials();

            if (_serviceConfiguration != null)
            {
                string username = Encryption.Auth_Decrypt(_serviceConfiguration.Auth_UserName);
                string password = Encryption.Auth_Decrypt(_serviceConfiguration.Auth_Password);


                if (username == Username && password == Password)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false; 
        }
    }
}