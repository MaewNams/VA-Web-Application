using System.Web;
using System.Web.Optimization;

namespace VA
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
               "~/Scripts/semantic.js",
                 "~/Scripts/semantic.js",
               "~/Scripts/myscript.js",
               "~/Scripts/jquery.datetimepicker.full.js",
               "~/Scripts/main.js",
               "~/Scripts/masonry.js",
               "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/semantic.css",
                      "~/Content/raptor.css",
                      "~/Content/jquery.datetimepicker.css",
                      "~/Content/site.css"));
        }
    }
}
