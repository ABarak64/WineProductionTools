using System.Web;
using System.Web.Optimization;

namespace WineProdTools
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxlogin").Include(
                "~/Scripts/app/ajaxlogin.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/app.js",
                "~/Scripts/app/services/tanks.js",
                "~/Scripts/app/services/accounts.js",
                "~/Scripts/app/services/notes.js",
                "~/Scripts/app/controllers/addtank.js",
                "~/Scripts/app/controllers/editcontents.js",
                "~/Scripts/app/controllers/deletetank.js",
                "~/Scripts/app/controllers/addnote.js",
                "~/Scripts/app/controllers/editaccount.js",
                "~/Scripts/app/controllers/dashboard.js",
                "~/Scripts/app/controllers/tanks.js",
                "~/Scripts/app/controllers/tankdashboard.js",
                "~/Scripts/app/controllers/transfers.js",
                "~/Scripts/app/controllers/tanktransfer.js",
                "~/Scripts/app/controllers/emptytank.js",
                "~/Scripts/app/controllers/filltank.js",
                "~/Scripts/hamster.js",
                "~/Scripts/app/directives/tankCanvas.js",
                "~/Scripts/app/directives/angular-mousewheel.js",
                "~/Scripts/app/directives/ng-infinite-scroll.js",
                "~/Scripts/app/directives/btnRadio.js",
                "~/Scripts/app/directives/alert.js",
                "~/Scripts/app/directives/modelStateErrorDisplay.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/Site.css"));
        }
    }
}