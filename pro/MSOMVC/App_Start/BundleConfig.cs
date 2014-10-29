using System.Web;
using System.Web.Optimization;

namespace MSOMVC
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                    "~/res/lib/zebradialog/css/zebra_dialog.css",
                    "~/res/lib/bootstrap/css/bootstrap.css",
                    "~/res/lib/bootstrap/css/bootstrap-theme.css",
                    "~/res/lib/bootstrap/css/bootstrap-docs.css",
                    "~/res/lib/jPaginate/css/style.css",
                    "~/res/css/main.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/basejs").Include(
                    "~/res/js/jquery.js",
                    "~/res/js/util.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                    "~/res/lib/zebradialog/js/zebra_dialog.js",
                    "~/res/lib/bootstrap/js/bootstrap.js",
                    "~/res/lib/jPaginate/jquery.paginate.js",
                    "~/res/js/default.js"
                ));
        }
    }
}