namespace Uma.Eservices.Web.Components
{
    using System.Web.Mvc;

    /// <summary>
    /// Wrapper class to define Font Awesome icon. 
    /// Information here: http://fontawesome.io/examples/
    /// Add rotation, flip, spin attributes if you require them (and default to "not used")
    /// </summary>
    public class FontAwesomeIcon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontAwesomeIcon"/> class.
        /// It makes defaults of normal size, left aligned icon.
        /// </summary>
        /// <param name="iconName">Name of the icon.</param>
        public FontAwesomeIcon(string iconName)
        {
            this.IconName = iconName;
            this.Size = IconSize.Normal;
            this.Justification = IconJustification.Left;
        }

        /// <summary>
        /// Gets or sets the full name of the icon, like  like "fa-check-square-o". Refer to http://fontawesome.io/icons/ for names.
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        /// The justification of an icon within element.
        /// </summary>
        public IconJustification Justification { get; set; }

        /// <summary>
        /// The size of an Icon.
        /// </summary>
        public IconSize Size { get; set; }

        /// <summary>
        /// Generate html for image.
        /// </summary>
        public string Html
        {
            get
            {
                var iconTagBuilder = new TagBuilder("i");
                switch (this.Size)
                {
                    case IconSize.Larger:
                        iconTagBuilder.AddCssClass("fa-lg");
                        break;
                    case IconSize.TwiceSize:
                        iconTagBuilder.AddCssClass("fa-2x");
                        break;
                    case IconSize.TrippleSize:
                        iconTagBuilder.AddCssClass("fa-3x");
                        break;
                }

                iconTagBuilder.AddCssClass(this.IconName);
                iconTagBuilder.AddCssClass("fa");

                return iconTagBuilder.ToString(TagRenderMode.Normal);
            }
        }
    }

    /// <summary>
    /// Icon Justification enumeration
    /// </summary>
    public enum IconJustification
    {
        /// <summary>
        /// Puts the icon on the left side of control (default)
        /// </summary>
        Left,

        /// <summary>
        /// Puts the icon in the center of a control (may be default for some controls)
        /// </summary>
        Center,

        /// <summary>
        /// Puts the icon on right side of a control
        /// </summary>
        Right
    }

    /// <summary>
    /// Enumeration on icon size
    /// </summary>
    public enum IconSize
    {
        /// <summary>
        /// Normal size of an icon (default)
        /// </summary>
        Normal,

        /// <summary>
        /// Puts an fa-lg class, which makes icon 33% largen than normal
        /// Good for normal bootstrap buttons
        /// </summary>
        Larger,

        /// <summary>
        /// Makes icon twice in size
        /// </summary>
        TwiceSize,

        /// <summary>
        /// Makes icon tripple size
        /// </summary>
        TrippleSize
    }
}