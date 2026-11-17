using System;
using System.Linq;
using System.Text;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses
{
    /// <summary>
    /// Code builder to generate a code.
    /// </summary>
    public class CodeBuilder
    {
        /// <summary>
        /// To generate the code.
        /// </summary>
        /// <returns>Returns a string of generated code.</returns>
        public static string GenerateCode()
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));
            string id = builder.ToString();
            string dateTime = DateTime.Now.ToString("yyMMss");

            return id + dateTime;
        }
    }
}