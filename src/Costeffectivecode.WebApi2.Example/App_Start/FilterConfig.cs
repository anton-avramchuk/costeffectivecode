using System.Web;
using System.Web.Mvc;

namespace Costeffectivecode.WebApi2.Example
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
