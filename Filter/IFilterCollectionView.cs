namespace Sprint.Filter
{
    using System.Collections.Generic;

    public interface IFilterCollectionView
    {
        /// <summary>
        /// Filters for viewmodel.
        /// </summary>
        IDictionary<string, IFilterView> Filters { get; }

        /// <summary>
        /// The name of the action method.
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Name of the controller that contains the action method.
        /// </summary>
        string Controller { get; set; }

        /// <summary>
        /// An object containing the parameters for a route.        
        /// </summary>        
        object RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the DOM-member, which must be updated with the reply from the server.
        /// </summary>        
        /// <returns>
        /// DOM-element ID, which should be updated.
        /// </returns>
        string UpdateTargetId { get; set; }

        /// <summary>
        /// Current filter options.
        /// </summary>
        IFilterOptions FilterOptions { get; }
    }
}
