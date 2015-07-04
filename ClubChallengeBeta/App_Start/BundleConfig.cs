using System.Web;
using System.Web.Optimization;

namespace ClubChallengeBeta
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            //blueimp.github.io/Gallery/js/jquery.blueimp-gallery.min.js
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.min.js",
                      "~/Scripts/blueimp-gallery.min.js",
                      "~/Scripts/bootstrap-image-gallery.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/blueimp-gallery.min.css",
                      "~/Content/bootstrap-image-gallery.min.css",
                      "~/Content/bootstrap-social.css",
                      "~/Content/font-awesome.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/site.css"));
        }
    }
}
