using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals
{
    public interface IResponse
    {
        List<string> Messages { get; set; }
        bool Valid { get; set; }
    }
}
