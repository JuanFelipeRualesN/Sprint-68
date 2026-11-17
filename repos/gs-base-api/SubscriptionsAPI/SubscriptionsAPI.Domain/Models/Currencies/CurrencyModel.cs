namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Currencies
{
    public class CurrencyModel
    {
        private string _id;
        private string _isoCode;
        private string _symbol;
        private string _name;
        private string _decimalPlaces;
        private string _available;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string IsoCode { get { return _isoCode; } set { _isoCode = value; } }
        public string Symbol { get { return _symbol; } set { _symbol = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string DecimalPlaces { get { return _decimalPlaces; } set { _decimalPlaces = value; } }
        public string Available { get { return _available; } set { _available = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}