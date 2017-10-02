using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EDKMO.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundle/jquery")
            .Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.inputmask/inputmask.js",
                "~/Scripts/jquery.inputmask/jquery.inputmask.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/btsp")
          .Include(

              "~/Scripts/nprogress.js",
              "~/Scripts/holder.min.js",
              //"~/Scripts/popper.js",
              "~/Scripts/umd/popper.js",
              "~/Scripts/bootstrap.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                   "~/Content/nprogress.css",
                   "~/Content/font-awesome.css",
                   "~/Content/Site.css"
                   ));
        }
    }
}