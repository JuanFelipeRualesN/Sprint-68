using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
    public class ResponseAddressModel : ResponseModel
    {
        public string SuggestedAddressLine1 { get; set; }
        public string SuggestedAddressLine2 { get; set; }
        public string SuggestedCity { get; set; }
        public string SuggestedState { get; set; }
        public string SuggesstedCpuntry { get; set; }
        public string SuggestedZipCode { get; set; }
        public string SuggestedMessage { get; set; }
        public bool SuggestedValidAddress { get; set; }
    }
}
