using System.Web;
using System.Web.Optimization;

namespace Design_and_Supervion_Issue_Tracking
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
           

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.css",
                      "~/Content/Site.css"));
            // jquery datataables js files

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                         "~/Scripts/DataTables/jquery.dataTables.js",
                          "~/Scripts/DataTables/dataTables.bootstrap.js",
                         "~/Scripts/DataTables/dataTables.responsive.js"
                          

                          ));
                                 

            // jquery datatables css file
            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                       "~/Content/DataTables/css/dataTables.bootstrap4.css",
                       "~/Content/DataTables/css/responsive.dataTables.css"
                      ));
        }
    }
}
