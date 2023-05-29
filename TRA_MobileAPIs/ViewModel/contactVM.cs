using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRA_MobileAPIs.ViewModel
{
    public class contactVM
    {

        public const string Firstname = "firstname";
        public const string ContactID = "contactid";
        public const string fullnm = "fullname";
        public const string Lastname = "lastname";
        public const string Nationality = "tra_nationality";
        public const string nationId = "tra_nationalityid";
        public const string securityQuizz = "tra_securityquestion";
        public const string securityQuizzId = "tra_securityquestionid";
        public const string serviceProvider = "tra_serviceprovider";
        public const string consumerType = "tra_consumertype";
        public const string IDType = "tra_idtype";

        public const string IDNumber = "tra_idnumber";
        public const string Email = "emailaddress1";
        public const string MSISDN = "tra_msisdn";
        public const string ICCID = "tra_iccid";
        public const string spContact = "tra_spcontact";
        public const string IMSI = "tra_imsi";
        public const string mobile_Phone = "mobilephone";
        public const string preferLang = "tra_preferredlanguage";
        public const string DOB = "birthdate";

        public const string Password = "tra_password";
        public const string FBToken = "tra_facebooktoken";
        public const string googleToken = "tra_googletoken";
        public const string tweetToken = "tra_twittertoken";

        public const string IMEINo = "tra_imei";

        public const string Reg_id = "tra_registrationid";

        public const string source = "tra_source";
        public const string securityAns = "tra_securityanswer";
        public const string countrycode = "tra_countrycode";


    }
    public static class SecurityQuizzLookUp
    {
        public const string tra_securityquestion = "tra_securityquestion";
        public const string tra_name = "tra_name";
    }

    public static class serviceProviderLookup
    {
        public const string tra_serviceprovider = "tra_serviceprovider";
        public const string tra_name = "tra_name";
    }

    public class updateContact
    {
        public string contID { get; set; }
        public string _fname { get; set; }
        public string _lname { get; set; }
        public string countrycode { get; set; }
        public string mobphone { get; set; }
        public string email { get; set; }
    }

    public static class NationalityLookup
    {

        public const string tra_nationality = "tra_nationality";
        public const string tra_name = "tra_name";
    }

    public class retrieveContact
    {

        public string fbToken { get; set; }
        public string tweetToken { get; set; }
        public string googleToken { get; set; }
        public string emails { get; set; }
        public int? msisdn { get; set; }
        public int? imsi { get; set; }
        public string iccid { get; set; }


    }
    public class ContactAttribute
    {
        public Guid contactID { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string emailaddress1 { get; set; }
        public string tra_nationality { get; set; }
        public string tra_countrycode { get; set; }
        public string mobilephone { get; set; }
        public DateTime? birthdate { get; set; }
        public string tra_consumertype { get; set; }
        public string tra_idtype { get; set; }
        public string tra_idnumber { get; set; }
        public string tra_preferredlanguage { get; set; }
        public string tra_securityanswer { get; set; }
        public string tra_securityquestion { get; set; }
        public int? tra_msisdn { get; set; }
        public string tra_iccid { get; set; }
        public int? tra_imsi { get; set; }

        public string tra_registrationid { get; set; }

    }


    public class serviceProvideVM
    {

        public Guid srcvProvideId { get; set; }
        public string srvc { get; set; }
    }

    public class IsEmailExist
    {
        public bool IsExistEmail { get; set; }
        public string email { get; set; }

        public Guid contactId { get; set; }
    }
}