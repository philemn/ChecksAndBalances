using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ChecksAndBalances.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bool optimizationsEnabled = true;
            
#if DEBUG
            optimizationsEnabled = false;
#endif

            BundleTable.EnableOptimizations = optimizationsEnabled;

            bundles.Add(new Bundle("~/scripts/packed.js", new JsMinify())
                .Include(
                    "~/Scripts/jquery-ui.custom.min.js",
                    "~/Scripts/jquery.easing.js",
                    "~/Scripts/jquery.flexslider-min.js",
                    "~/Scripts/camera.min.js",
                    "~/Scripts/jquery.cslider.js",
                    "~/Scripts/jquery.touchSwipe.min.js",
                    "~/Scripts/jquery.caroufredsel.js",
                    "~/Scripts/jquery.isotope.min.js",
                    "~/Scripts/jquery.tipsy.js",
                    "~/Scripts/jquery.zflickrfeed.js",
                    "~/Scripts/jquery.fitvid.js",
                    "~/Scripts/jquery.fancybox.js",
                    "~/Scripts/jquery.timeago.js",
                    "~/Scripts/jquery.tweetable.js",
                    "~/Scripts/jquery.hoverIntent.js",
                    "~/Scripts/jquery.superfish.js",
                    "~/Scripts/jquery.countdown.min.js",
                    "~/Scripts/jquery.easy-pie-chart.js",
                    "~/Scripts/jquery.gmap.min.js",
                    "~/Scripts/omnibiz-custom.js"));

            bundles.Add(new Bundle("~/content/packed.css", new CssMinify())
                .Include(
                    "~/Content/style.css",
                    "~/Content/plugins.css",
                    "~/Content/colors/blue.css",
                    "~/Content/responsive.css"));
        }
    }
}