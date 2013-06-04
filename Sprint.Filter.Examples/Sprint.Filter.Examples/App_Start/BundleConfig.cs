using System.Web;
using System.Web.Optimization;

namespace Sprint.Filter.Examples
{
    public class BundleConfig
    {
        // Дополнительные сведения о Bundling см. по адресу http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Scripts/jquery.modalbox/jquery.modalbox-1.5.css",
                "~/Scripts/jquery.jnotifier/jnotifier.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/sprint.grid").Include(
                "~/Content/sprint.grid.css",
                "~/Content/sprintGridWithFilter.css"));

            bundles.Add(new ScriptBundle("~/bundles/sprintGridWithFilter")
                .Include(
                "~/Scripts/sprintGridWithFilter.js",
                "~/Scripts/sprint.grid/sprint.grid.js"));

            bundles.Add(new ScriptBundle("~/bundles/saveloadfilter").Include("~/Scripts/saveloadfilter.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery.form.js",
                "~/Scripts/jquery.modalbox/jquery.modalbox-1.5.js",
                 "~/Scripts/jquery.jnotifier/jnotifier.jquery.js"
                ));
        }
    }
}