using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;

namespace TRA_MobileAPIs.ViewModel
{
    public class EntityData
    {

        public Guid ID { get; set; }

        public Guid _guidId { get; set; }
        public IOrganizationService _service { get; set; }
        public string EntitytKey { get; set; }
        public string EntityValue { get; set; }
        public RetrieveAttributeRequest _retrieveAttrRequest { get; set; }
        public RetrieveAttributeResponse _retrieveAttrResponse { get; set; }
        public AttributeMetadata retrievedAttributeMetadata { get; set; }
        public string EntityAtrribute { get; set; }
        public string parentmasterentityname { get; set; }
        public string masterconditionkey { get; set; }

        public string ParentEntityType { get; set; }
  
    
             
     
    }
}