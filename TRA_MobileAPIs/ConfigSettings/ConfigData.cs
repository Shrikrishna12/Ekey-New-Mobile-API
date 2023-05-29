using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.Text;

namespace TRA_MobileAPIs.ConfigSettings
{
    public class ConfigData
    {

        public string D65Url { get; set; }
        public string client_key { get; set; }
        public string secret_key { get; set; }

        public string ContactEntity { get; set; }


        public string Dev_ServerName { get; set; }
        public string Dev_Organization { get; set; }
        public string Dev_UserName { get; set; }
        public string Dev_Password { get; set; }

        public string Auth_UserName { get; set; }
        public string Auth_Password { get; set; }

       public string caseEntity { get; set; }
        public string converstionEnity { get; set; }
        public string notify { get; set; }
        public string annotation { get; set; }
        public string fcmApiKey { get; set; }
        public string fcmSenderId { get; set; }

        public string files { get; set; }
    }

    public static class ConfigEncrypt
    {

        public static ConfigData GetCrmCredentials()
        {

            ConfigData configurations = new ConfigData();

            configurations.D65Url = ConfigurationManager.AppSettings["D65URL"];
            configurations.notify = ConfigurationManager.AppSettings["notify"];
            configurations.client_key = ConfigurationManager.AppSettings["client_key"];
            configurations.secret_key = ConfigurationManager.AppSettings["secret_key"];
            configurations.ContactEntity = ConfigurationManager.AppSettings["contact"];
            configurations.caseEntity = ConfigurationManager.AppSettings["case"];
            configurations.Dev_Organization = ConfigurationManager.AppSettings["Dev_Organization"];
            configurations.Dev_Password = ConfigurationManager.AppSettings["Dev_Password"];
            configurations.Dev_ServerName = ConfigurationManager.AppSettings["Dev_ServerName"];
            configurations.Dev_UserName = ConfigurationManager.AppSettings["Dev_UserName"];
            configurations.Auth_UserName = ConfigurationManager.AppSettings["Auth_UserName"];
            configurations.Auth_Password = ConfigurationManager.AppSettings["Auth_Password"];
            configurations.converstionEnity = ConfigurationManager.AppSettings["conversation"];
            configurations.annotation = ConfigurationManager.AppSettings["Notes"];
            configurations.fcmApiKey = ConfigurationManager.AppSettings["api_Key"];
            configurations.fcmSenderId = ConfigurationManager.AppSettings["senderId"];
            configurations.files= ConfigurationManager.AppSettings["files"];

            return configurations;
        }



    }

    public class OrganizationService
    {

        public IOrganizationService service { get; set; }
        private static OrganizationService instance = null;

        public static OrganizationService GetInstance()
        {
            if (instance == null)
            {
                instance = new OrganizationService();

            }
            return instance;
        }
        public OrganizationService()
        {
            ConfigData configData = ConfigEncrypt.GetCrmCredentials();
            this.service = this.GetCRMService(configData);

        }

        public IOrganizationService GetCRMService(ConfigData _serviceConfiguration)
        {

            ClientCredentials Credentials = new ClientCredentials();
            // Encryption.Encrypt("#2019&APITRA");
            Credentials.Windows.ClientCredential.Domain = System.Configuration.ConfigurationManager.AppSettings["Dev_Domain"]; //"sourceedge";
            //Credentials.Windows.ClientCredential.UserName = System.Configuration.ConfigurationManager.AppSettings["Dev_UserName"]; //"crm.install";
            //Credentials.Windows.ClientCredential.Password = System.Configuration.ConfigurationManager.AppSettings["Dev_Password"];  //"pass@word1";
            Credentials.Windows.ClientCredential.UserName = Encryption.Decrypt(_serviceConfiguration.Dev_UserName); //"crm.install";
            Credentials.Windows.ClientCredential.Password = Encryption.Decrypt(_serviceConfiguration.Dev_Password);  //"pass@word1";

            //Credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

            //This URL needs to be updated to match the servername and Organization for the environment. 

            Uri OrganizationUri = new Uri("https://" + _serviceConfiguration.Dev_ServerName + "/" + _serviceConfiguration.Dev_Organization + "/" + "XRMServices/2011/Organization.svc");



            Uri HomeRealmUri = null;

            //OrganizationServiceProxy serviceProxy;   

            // commented on 10.09
          //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback =
          delegate (object s, X509Certificate certificate,
               X509Chain chain, SslPolicyErrors sslPolicyErrors)
          { return true; };

            OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, HomeRealmUri, Credentials, null);
            IOrganizationService service = (IOrganizationService)serviceProxy;
            return service;
        }

    }
    public static class Encryption
    {
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "Tr@Bahra!n1$";
            //string EncryptionKey = "TRA@Mobile2019";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            //string EncryptionKey = "TRA@Mobile2019";
            string EncryptionKey = "Tr@Bahra!n1$";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        public static string Auth_Encrypt(string clearText)
        {
            // string EncryptionKey = "Tr@Bahra!n1$";
            string EncryptionKey = "TRA@Mobile2019";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Auth_Decrypt(string cipherText)
        {
            string EncryptionKey = "TRA@Mobile2019";
            //  string EncryptionKey = "Tr@Bahra!n1$";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        //Encryption Password
        public static string Encrypt_Password(string clearText)
        {
            string password = "AEH12";
            byte[] bytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[13]
                {
          (byte) 73,
          (byte) 118,
          (byte) 97,
          (byte) 110,
          (byte) 32,
          (byte) 77,
          (byte) 101,
          (byte) 100,
          (byte) 118,
          (byte) 101,
          (byte) 100,
          (byte) 101,
          (byte) 118
                });
                aes.Key = rfc2898DeriveBytes.GetBytes(32);
                aes.IV = rfc2898DeriveBytes.GetBytes(16);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.Close();
                    }
                    clearText = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return clearText;
        }


        public static string Decrypt_Pass(string cipherText)
        {
            string EncryptionKey = "AEH12";
            byte[] clearBytes = Encoding.Unicode.GetBytes(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[13] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    cipherText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return cipherText;
        }
    }

  //  DYCRYPT


}