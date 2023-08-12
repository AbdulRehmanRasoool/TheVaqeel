using System.Collections.Generic;
using System;
using System.Web;
using System.Web.Optimization;
using TheVaqeel.Website.Models;

namespace TheVaqeel.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Content/js/jquery-3.6.0.min.js",
                      "~/Content/js/bootstrap.min.js",
                      "~/Content/js/slick.min.js",
                      "~/Content/js/jquery.magnific-popup.min.js",
                      "~/Content/js/isotope.pkgd.min.js",
                      "~/Content/js/imagesloaded.pkgd.min.js",
                      "~/Content/js/jquery.nice-select.min.js",
                      "~/Content/js/jquery.counterup.min.js",
                      "~/Content/js/jquery.waypoints.js",
                      "~/Content/js/wow.min.js",
                      "~/Content/js/main.js"));

            bundles.Add(new Bundle("~/bundles/AdminScripts").Include(
                      "~/Content/js/jquery-3.6.0.min.js",
                      "~/Content/js/bootstrap.min.js",
                      "~/Content/js/jquery-ui.min.js",                   
                      "~/Content/js/adminlte.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/animate.css",
                      "~/Content/css/default.css",
                      "~/Content/css/slick.css",
                      "~/Content/css/magnific-popup.css",
                      "~/Content/css/nice-select.css",
                      "~/Content/fonts/fontawesome/css/all.min.css",
                      "~/Content/fonts/flaticon/flaticon.css",
                      "~/Content/css/style.css"));

            bundles.Add(new StyleBundle("~/Content/Admincss").Include(
                      "~/Content/css/adminlte.min.css",
                       "~/Content/fonts/fontawesome/css/all.min.css"));
        }
      
    }
}
