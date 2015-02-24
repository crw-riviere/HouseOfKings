using System.Web.Optimization;

namespace HouseOfKings.Web
{
    public class BundleConfig
    {
        private const string JQueryCDN = "//code.jquery.com/jquery-2.1.3.min.js";
        private const string BootstrapCDN = "//maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js";
        private const string SignalrCDN = "//ajax.aspnetcdn.com/ajax/signalr/jquery.signalr-2.2.0.min.js";

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery", JQueryCDN).Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr", SignalrCDN).Include(
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", BootstrapCDN).Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}