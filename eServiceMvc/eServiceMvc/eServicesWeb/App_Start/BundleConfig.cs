namespace Uma.Eservices.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// Configuration of Bundles - minimized and grouped CSS and JavaScript files
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class BundleConfig
    {
        /// <summary>
        /// Registers the bundles with Wen Optimization framework.
        /// </summary>
        /// <param name="bundles">The bundles collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            if (bundles == null)
            {
                return;
            }

            bundles.UseCdn = true;

            var jQuery = new ScriptBundle("~/bundles/jquery", "//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js")
                .Include("~/Scripts/jquery-{version}.js");
            jQuery.CdnFallbackExpression = "window.jquery";
            bundles.Add(jQuery);

            var bootstrap = new ScriptBundle("~/bundles/bootstrap", "//netdna.bootstrapcdn.com/bootstrap/3.0.2/js/bootstrap.min.js")
                .Include("~/Scripts/bootstrap.js");
            bootstrap.CdnFallbackExpression = "$.fn.modal";
            bundles.Add(bootstrap);

            // Scripts that should present on all pages!
            var readScripts = new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery.pnotify.js",
                "~/Scripts/moment-with-langs.js",
                "~/Scripts/ui.js");
            bundles.Add(readScripts);

            // Scripts that should be added ONLY to Forms (Edit) pages.
            bundles.Add(new ScriptBundle("~/bundles/formjs").Include(
                        "~/Scripts/bootstrap-select*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/bootstrap.validate.js",
                        "~/Scripts/bootstrap-maxlength.js",
                        "~/Scripts/inputmask.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/locales/bootstrap-datepicker*",
                        "~/Scripts/blueimp/jquery.ui.widget.js",
                        "~/Scripts/blueimp/tmpl.min.js",
                        "~/Scripts/blueimp/load-image.min.js",
                        "~/Scripts/blueimp/canvas-to-blob.min.js",
                        "~/Scripts/blueimp/jquery.iframe-transport.js",
                        "~/Scripts/blueimp/jquery.fileupload.js",
                        "~/Scripts/blueimp/jquery.fileupload-process.js",
                        "~/Scripts/blueimp/jquery.fileupload-image.js",
                        "~/Scripts/blueimp/jquery.fileupload-validate.js",
                        "~/Scripts/blueimp/jquery.fileupload-ui.js",
                        "~/Scripts/blueimp/jquery.fileupload-validate.js",
                        "~/Scripts/jquery.livequery.min.js",
                        "~/Scripts/moment-with-langs.js",
                        "~/Scripts/jquery.pnotify.js",
                        "~/Scripts/typeahead.bundle.js",
                        "~/Scripts/DynamicControls.js",
                        "~/Scripts/forms-scripts.js",
                        "~/Scripts/Expander.js",
                        "~/Scripts/jquery.serializeObject.js"));

            bundles.Add(new ScriptBundle("~/bundles/account").Include("~/Scripts/account.js"));

            var fontLoader = new ScriptBundle("~/bundles/fontLoader", "//ajax.googleapis.com/ajax/libs/webfont/1.5.2/webfont.js")
                .Include("~/Scripts/webfont.min.js");
            fontLoader.CdnFallbackExpression = "window.WebFont";
            bundles.Add(fontLoader);

            var fontAwesome = new StyleBundle("~/static/fa", "//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css")
                .Include("~/Static/style/fontAwesome/font-awesome.css");
            bundles.Add(fontAwesome);

            bundles.Add(new StyleBundle("~/static/style/css").Include("~/Static/style/styles.css",
                                                                "~/Static/style/bootstrap-select/bootstrap-select.css",
                                                                "~/Static/style/jquery.pnotify.default.css",
                                                                "~/Static/style/jquery.fileupload.css",
                                                                "~/Static/style/jquery.fileupload-ui.css",
                                                                "~/Static/style/jquery.pnotify.default.css"));

            bundles.Add(new StyleBundle("~/static/style/pdfStyle").Include("~/Static/style/pdfLayout/PdfStyle.css"));
        }
    }
}