using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressAPI.AddressAPI.Domain.Models.Address
{
    public class ResponseCountryFullDetailsModel : CountryModel
    {
        public List<CountryLangModel> CountryNameTrans { get; set; }
        public List<StatesRegionsProvidenceModel> Regions { get; set; }
    }
}
