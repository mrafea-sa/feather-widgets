﻿using System;
using System.Linq;
using Telerik.Sitefinity.Frontend.Search.Mvc.Controllers;

namespace FeatherWidgets.TestUnit.DummyClasses.SearchResults
{
    /// <summary>
    /// This class creates dummy <see cref="Telerik.Sitefinity.Frontend.Search.Mvc.Controllers.SearchResultsController"/>
    /// </summary>
    public class DummySearchResultsController : SearchResultsController
    {
        /// <summary>
        /// Gets fake current page URL.
        /// </summary>
        /// <returns></returns>
        protected override string GetCurrentUrl()
        {
            return "http://tempuri.org";
        }

        protected override bool IsSearchModuleActivated()
        {
            return true;
        }

        protected override bool EnableFilterByViewPermissions()
        {
            return false;
        }
    }
}