using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class PlanResponseModel : PlanModel
    {
        public PlanResponseModel()
        {
            Messages = new List<string>();
            PlanAvailableIsos = new List<PlanAvailableIsoModel>();
            PlanTaxes = new List<TaxPlanModel>();
            PlanDescriptionByLang = new List<PlanLangModel>();
        }
        public bool PlanAvailableForIso { get; set; }
        public List<PlanLangModel> PlanDescriptionByLang { get; set; }
        public ProductResponseModel ProductDetails { get; set; }
        public bool TransactionValid { get; set; }
        public List<string> Messages { get; set; }
        public string ClientPrice { get; set; } // To show the amount as the client would read it. 
        public int TrialDays { get; set; }
        public List<PlanAvailableIsoModel> PlanAvailableIsos { get; set; }
        public List<TaxPlanModel> PlanTaxes { get; set; }
    }
}
