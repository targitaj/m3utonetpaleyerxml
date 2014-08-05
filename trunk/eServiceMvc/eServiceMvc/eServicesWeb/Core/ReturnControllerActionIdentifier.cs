namespace Uma.Eservices.Web.Core
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Class to hold Controller/Action/Id for return or redirection actions
    /// </summary>
    [Serializable]
    [DebuggerDisplay("ReturnPath:{ControllerName,nq}/{ActionName,nq}")]
    public class ReturnControllerActionIdentifier
    {
        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the action in <see cref="ControllerName"/>
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the entity id for default route Cntr/Act/Id
        /// Can be null.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the bookmark or tag that allows further to scroll page or pre-select element (like: tab) on it.
        /// </summary>
        public string BookmarkTag { get; set; }
    }
}