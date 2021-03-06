﻿using System;
using ArtOfTest.WebAii.Core;
using Feather.Widgets.TestUI.Framework;
using Feather.Widgets.TestUI.Framework.Framework.Wrappers.Backend.Widgets;
using FeatherWidgets.TestUI.TestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.TestUI.Framework.Utilities;

namespace FeatherWidgets.TestUI.TestCases.Identity
{
    /// <summary>
    /// VerifyPagingOnFrontendPageForUsersListWidget_ test class.
    /// </summary>
    [TestClass]
    public class VerifyPagingOnFrontendPageForUsersListWidget_ : FeatherTestCase
    {
        /// <summary>
        /// UI test VerifyPagingOnFrontendPageForUsersListWidget
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.SitefinityTeam7),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.Identity),
        TestCategory(FeatherTestCategories.UsersList)]
        public void VerifyPagingOnFrontendPageForUsersListWidget()
        {
            RuntimeSettingsModificator.ExecuteWithClientTimeout(800000, () => BAT.Macros().NavigateTo().CustomPage("~/sitefinity/pages", true, null, new HtmlFindExpression("class=~sfMain")));
            BAT.Macros().NavigateTo().Pages(this.Culture);
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SwitchToListSettingsTab();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().VerifyCheckedRadioButtonOption(WidgetDesignerRadioButtonIds.UsePaging);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().ChangePagingOrLimitValue("2", "Paging");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().VerifyPageValue("2", "Paging");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().VerifyPageValue("20", "Limit");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SaveChanges();
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SwitchToListSettingsTab();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().VerifyCheckedRadioButtonOption(WidgetDesignerRadioButtonIds.UsePaging);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().VerifyPageValue("2", "Paging");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().VerifyPageValue("20", "Limit");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().PressCancelButton();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), true, this.Culture);
            BATFeather.Wrappers().Frontend().Identity().UsersListWrapper().VerifyUsersListOnHybridPage(new string[] { UserFirstLastName1, UserFirstLastName2 });
            Assert.IsFalse(BATFeather.Wrappers().Frontend().CommonWrapper().AreTitlesPresentOnThePageFrontend(new string[] { UserFirstLastName3, UserFirstLastName4, UserFirstLastName3 }));
            BATFeather.Wrappers().Frontend().CommonWrapper().NavigateToPageUsingPager("2", 3);
            BATFeather.Wrappers().Frontend().Identity().UsersListWrapper().VerifyUsersListOnHybridPage(new string[] { UserFirstLastName3, UserFirstLastName4 });
            Assert.IsFalse(BATFeather.Wrappers().Frontend().CommonWrapper().AreTitlesPresentOnThePageFrontend(new string[] { UserFirstLastName5, UserFirstLastName1, UserFirstLastName2 }));
            BATFeather.Wrappers().Frontend().CommonWrapper().NavigateToPageUsingPager("3", 3);
            BATFeather.Wrappers().Frontend().Identity().UsersListWrapper().VerifyUsersListOnHybridPage(new string[] { UserFirstLastName5 });
            Assert.IsFalse(BATFeather.Wrappers().Frontend().CommonWrapper().AreTitlesPresentOnThePageFrontend(new string[] { UserFirstLastName1, UserFirstLastName4, UserFirstLastName3, UserFirstLastName2 }));
            BATFeather.Wrappers().Frontend().CommonWrapper().NavigateToPageUsingPager("1", 3);
            BATFeather.Wrappers().Frontend().Identity().UsersListWrapper().VerifyUsersListOnHybridPage(new string[] { UserFirstLastName1, UserFirstLastName2 });
            Assert.IsFalse(BATFeather.Wrappers().Frontend().CommonWrapper().AreTitlesPresentOnThePageFrontend(new string[] { UserFirstLastName3, UserFirstLastName4, UserFirstLastName3 }));
            BAT.Macros().NavigateTo().Pages(this.Culture);
        }

        /// <summary>
        /// Performs Server Setup and prepare the system with needed data.
        /// </summary>
        protected override void ServerSetup()
        {
            BAT.Macros().User().EnsureAdminLoggedIn();
            BAT.Arrange(this.TestName).ExecuteSetUp();
        }

        /// <summary>
        /// Performs clean up and clears all data created by the test.
        /// </summary>
        protected override void ServerCleanup()
        {
            BAT.Arrange(this.TestName).ExecuteTearDown();
        }

        private const string PageName = "UsersListPage";
        private const string WidgetName = "Users list";
        private const string UserFirstLastName1 = "admin admin";
        private const string UserFirstLastName2 = "admin1 admin1";
        private const string UserFirstLastName3 = "fn ln";
        private const string UserFirstLastName4 = "fname lname";
        private const string UserFirstLastName5 = "fname1 lname1";
    }
}
