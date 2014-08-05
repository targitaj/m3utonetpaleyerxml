namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaJavascriptHelpers
    {
        /// <summary>
        /// Creates a JavaScript that uses Typeahead.js and Bloodhound engines to create auto suggestions for 
        /// string field (also rendered previosly by <see cref="UmaTextBoxFor{TModel,TProperty}(System.Web.Mvc.HtmlHelper{TModel},System.Linq.Expressions.Expression{System.Func{TModel,TProperty}},string,System.Nullable{bool},object)"/>)
        /// Shlould supply action method shich supplies string values for suggestions as user types in its verses.
        /// </summary>
        /// <typeparam name="TModel">The type of the view model.</typeparam>
        /// <param name="htmlHelper">The HTML helper itself.</param>
        /// <param name="expression">The expression indicating which property (string!) in model to use.</param>
        /// <param name="ajaxMethodNameForSuggestions">The ajax method name for suggestions. Should return JSON format list</param>
        /// <exception cref="ArgumentNullException">
        /// Parameter <paramref name="expression"/> - Html helper requires model property expression or parameter <paramref name="htmlHelper"/> is null
        /// </exception>
        public static MvcHtmlString UmaTextBoxSuggestionsScriptFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, string>> expression,
            string ajaxMethodNameForSuggestions)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Html helper requires model property expression");
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper", "htmlHelper should not be null");
            }
            MvcHtmlString fieldID = htmlHelper.IdFor(expression);
            TagBuilder scriptBuilder = new TagBuilder("script");
            scriptBuilder.MergeAttribute("type", "text/javascript");

            // use InnerHtml because it doesn't encode characters
            scriptBuilder.InnerHtml = @"
var suggestions = new Bloodhound({
  datumTokenizer: function(d) { return Bloodhound.tokenizers.whitespace(d.name); },
  queryTokenizer: Bloodhound.tokenizers.whitespace,
  limit: 10,
  prefetch: {
    url: '" + ajaxMethodNameForSuggestions + @"',
    filter: function(list) {
      return $.map(list, function(suggestion) { return { name: suggestion }; });
    }
  }
});
suggestions.initialize();
$('#" + fieldID + @"').typeahead(null, {
  name: 'suggestions',
  displayKey: 'name',
  source: suggestions.ttAdapter()
});
";
            return new MvcHtmlString(scriptBuilder.ToString());
        }
    }
}