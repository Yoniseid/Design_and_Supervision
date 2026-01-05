using System.Web;
using System.Web.Mvc;

namespace Design_and_Supervion_Issue_Tracking
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
