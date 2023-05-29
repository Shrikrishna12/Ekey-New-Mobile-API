using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Globalization;

namespace TRA_MobileAPIs.ViewModel
{
    public class ConvertHelper
    {



        public static class Constants
        {
            public const string Boolean = "Boolean";
            public const string Status = "Status";
            public const string Date = "Date";

            public const string DateTime = "DateTime";
            public const string String = "String";
            public const string Decimal = "Decimal";
            public const string Picklist = "Picklist";

            public const string WholeNumber = "WholeNumber";
            public const string OptionSet = "OptionSet";
            public const string Lookup = "Lookup";
            public const string Integer = "Integer";
            public const string customer = "Customer";
            public const string Memo = "Memo";



        }



        //dbanswer =value to convert
        public Entity ConvertDatatype(EntityData _entityData, Entity targetentity)
        {
            var type = _entityData.retrievedAttributeMetadata.AttributeType.Value.ToString();

            #region Condition for Look Up


            if (type == Constants.Memo)
            {
                targetentity[_entityData.EntitytKey] = (_entityData.EntityAtrribute).ToString();
                return targetentity;
            }
            if (type == Constants.customer)//for lokup
            {
                if (_entityData != null && targetentity != null)
                {
                    Guid parentrecordid = _entityData.ID;//get guid123

                    targetentity[_entityData.EntitytKey] = new EntityReference(_entityData.parentmasterentityname, parentrecordid);//tra_nationality
                }

                return targetentity;

            }

            if (type == Constants.Lookup)//for lokup
            {
                if(_entityData!=null&& targetentity != null) 
                {
                   Guid parentrecordid = _entityData.ID;//get guid123
                   targetentity[_entityData.EntitytKey] = new EntityReference(_entityData.parentmasterentityname, parentrecordid);//tra_nationality
                }
                return targetentity;

            }
            if (type == Constants.Status)//for status
            {

                EnumAttributeMetadata data = (EnumAttributeMetadata)_entityData.retrievedAttributeMetadata;
                foreach (OptionMetadata picklist in data.OptionSet.Options)
                {
                    // Check int value.     
                    string optionsetText = picklist.Label.UserLocalizedLabel.Label;

                    if (optionsetText.ToLower() == _entityData.EntityAtrribute.ToLower())
                        targetentity[_entityData.EntitytKey] = new OptionSetValue(Convert.ToInt32(picklist.Value));

                }

                return targetentity;

            }

            #endregion

            #region Condition for yesno
            if (type == Constants.Boolean)//for yesno
            {
                targetentity[_entityData.EntitytKey] = Convert.ToBoolean(_entityData.EntityAtrribute);
                return targetentity;

            }

            if (type == Constants.DateTime)
            {

                DateTime date = DateTime.ParseExact(_entityData.EntityAtrribute, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                targetentity[_entityData.EntitytKey] = date;
                return targetentity;
            }
            #endregion

            #region Condition for date
            if (type == Constants.Date)
            {
                targetentity[_entityData.EntitytKey] = Convert.ToDateTime(_entityData.EntityAtrribute);
                return targetentity;

            }
            #endregion

            #region Condition for string
            if (type == Constants.String)
            {
                targetentity[_entityData.EntitytKey] = (_entityData.EntityAtrribute).ToString();
                return targetentity;
            }
            #endregion

            #region Condition for Decimal
            if (type == Constants.Decimal)
            {
                targetentity[_entityData.EntitytKey] = Convert.ToDecimal(_entityData.EntityAtrribute);
                return targetentity;
            }

            if (type == Constants.Integer)
            {

                int a;
                bool b = Int32.TryParse(_entityData.EntityAtrribute, out a);

                targetentity[_entityData.EntitytKey] =a;
                return targetentity;
            }
            #endregion

            #region Condition for Picklist
            if (type == Constants.Picklist)//for status
            {

                EnumAttributeMetadata data = (EnumAttributeMetadata)_entityData.retrievedAttributeMetadata;
                foreach (OptionMetadata picklist in data.OptionSet.Options)
                {
                    // Check int value.     
                    string optionsetText = picklist.Value.ToString();

                    if (optionsetText.ToLower() == _entityData.EntityAtrribute.ToLower())
                        targetentity[_entityData.EntitytKey] = new OptionSetValue(Convert.ToInt32(picklist.Value));

                }

                return targetentity;
            }




            #endregion
            return null;

        }


    }
}