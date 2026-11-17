using System.Collections.Generic;

namespace AddressAPI.AddressAPI.Domain.Models.Address
{
    public class ResponseAddressModel
    {
        private string _countryIsoCode2;
        private string _name;
        private string _addressExtraIdentifier;

        public string countryIsoCode2 { get { return _countryIsoCode2; } set { _countryIsoCode2 = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public string addressExtraIdentifier { get { return _addressExtraIdentifier; } set { _addressExtraIdentifier = value; } }

    }
}
