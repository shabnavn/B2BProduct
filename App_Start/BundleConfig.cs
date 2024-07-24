using System.Web;
using System.Web.Optimization;

namespace b2b_solution
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/scripts").Include(
                      "~/Content/js/vendor/modernizr-3.6.0.min.js",
                        "~/Content/js/vendor/jquery-3.6.0.min.js",
                        "~/Content/js/vendor/jquery-migrate-3.3.0.min.js",
                        "~/Content/js/vendor/bootstrap.bundle.min.js",
                        "~/Content/js/plugins/slick.js",
                        "~/Content/js/plugins/jquery.syotimer.min.js",
                        "~/Content/js/plugins/waypoints.js",
                        "~/Content/js/plugins/wow.js",
                        "~/Content/js/plugins/perfect-scrollbar.js",
                        "~/Content/js/plugins/magnific-popup.js",
                        "~/Content/js/plugins/select2.min.js",
                        "~/Content/js/plugins/counterup.js",
                        "~/Content/js/plugins/jquery.countdown.min.js",
                        "~/Content/js/plugins/images-loaded.js",
                        "~/Content/js/plugins/isotope.js",
                        "~/Content/js/plugins/scrollup.js",
                        "~/Content/js/plugins/jquery.vticker-min.js",
                        "~/Content/js/plugins/jquery.theia.sticky.js",
                        "~/Content/js/plugins/jquery.elevatezoom.js",
                        "~/Content/js/main.js?v=4.0",
                        "~/Content/js/shop.js?v=4.0"
                      ));
            
            bundles.Add(new StyleBundle("~/Content/styles").Include(
                      "~/Content/css/plugins/animate.min.css",
                      "~/Content/css/main.css"));


            bundles.Add(new ScriptBundle("~/Kendo").
                Include("~/Kendo/js/kendo.all.min.js",
             "~/Kendo/js/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Kendo/styles").Include("~/Kendo/styles/kendo.common.min.css",
                "~/Kendo/styles/kendo.default.min.css"));


            bundles.Add(new StyleBundle("~/Content/Gridstyles").Include(
                      "~/Content/GridContent/plugins/custom/fullcalendar/fullcalendar.bundle.css",
                      "~/Content/GridContent/css/style.bundle.css"));


            bundles.Add(new Bundle("~/Content/Gridscripts").Include(
                     "~/Content/plugins/global/plugins.bundle.js",
                      "~/Content/js/scripts.bundle.js",
                      "~/Content/plugins/custom/fullcalendar/fullcalendar.bundle.js",
                      "~/Content/js/custom/widgets.js",
                      "~/Content/js/custom/apps/chat/chat.js",
                      "~/Content/js/custom/modals/create-app.js",
                      "~/Content/js/custom/modals/upgrade-plan.js",
                       "~/Content/waypoints.js",
                       "~/Content/track_all.js"
                     ));



            bundles.IgnoreList.Clear();

        }
    }
}
