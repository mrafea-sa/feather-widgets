﻿using System;
using System.Linq;
using FeatherWidgets.TestUtilities.CommonOperations;
using MbUnit.Framework;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Frontend.DynamicContent.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.DynamicContent.Mvc.Models;
using Telerik.Sitefinity.Frontend.TestUtilities;
using Telerik.Sitefinity.Lifecycle;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.TestUtilities.CommonOperations;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.WebTestRunner.Server.Attributes;

namespace FeatherWidgets.TestIntegration.DynamicWidgets
{
    /// <summary>
    /// This test fixture contains tests related to the behavior of dynamic widgets on hierarchical modules.
    /// </summary>
    [TestFixture]
    public class DynamicWidgetsHierarchicalContentTests
    {
        #region Setup and TearDown
        
        [FixtureSetUp]
        public void Setup()
        {
            ServerOperationsFeather.DynamicModules().EnsureModuleIsImported(DynamicWidgetsHierarchicalContentTests.ModuleName, DynamicWidgetsHierarchicalContentTests.ModuleResource);
            
            this.CreateItems();
        }
        
        [FixtureTearDown]
        public void Teardown()
        {
            ServerOperations.ModuleBuilder().DeleteAllModules(string.Empty, DynamicWidgetsHierarchicalContentTests.TransactionName);
        }
        
        #endregion
        
        #region Tests
        
        [Test]
        [Category(TestCategories.DynamicWidgets)]
        [Author(FeatherTeams.SitefinityTeam6)]
        [Description("Verifies that the ChildItems method of the ItemViewModelExtensions works properly.")]
        [Multilingual(MultilingualExecutionMode.MultiAndMonoLingual)]
        public void ItemViewModelExtensions_ChildItems_ReturnsChildren()
        {
            var dynamicContentModel = new DynamicContentModel();
            var rootContentType = TypeResolutionService.ResolveType(DynamicWidgetsHierarchicalContentTests.RootContentTypeName);
            
            dynamicContentModel.ContentType = rootContentType;
            
            var manager = DynamicModuleManager.GetManager();
            var viewModel = dynamicContentModel.CreateDetailsViewModel(manager.GetDataItems(rootContentType).First());
            
            var result = viewModel.Item.ChildItems("Child1Types");
            
            Assert.IsNotNull(result, "ChildItems returned null when there should have been child items enumerable.");
            Assert.AreEqual(2, result.Count(), "ChildItems did not return the expected number of child items.");
        }
        
        [Test]
        [Category(TestCategories.DynamicWidgets)]
        [Author(FeatherTeams.SitefinityTeam6)]
        [Description("Verifies that the ParentItem method of the ItemViewModelExtensions works properly.")]
        [Multilingual(MultilingualExecutionMode.MultiAndMonoLingual)]
        public void ItemViewModelExtensions_ParentItem_ReturnsParentItem()
        {
            var dynamicContentModel = new DynamicContentModel();
            var child1Type = TypeResolutionService.ResolveType(DynamicWidgetsHierarchicalContentTests.Child1TypeName);
            var rootContentType = TypeResolutionService.ResolveType(DynamicWidgetsHierarchicalContentTests.RootContentTypeName);
            
            dynamicContentModel.ContentType = child1Type;
            
            var manager = DynamicModuleManager.GetManager();
            var liveItem = manager.GetDataItems(child1Type).Where(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live).First();
            var masterItem = manager.Lifecycle.GetMaster(liveItem);
            
            var viewModel = dynamicContentModel.CreateDetailsViewModel(masterItem);
            
            var result = viewModel.Item.ParentItem();
            var lifecycleDataItem = (ILifecycleDataItem)result.DataItem;
            
            Assert.IsNotNull(result, "ParentItem returned null when there should have been a parent item.");
            Assert.IsAssignableFrom(rootContentType, result.DataItem.GetType(), "The ParentItem result did not contain a DataItem of the expected type.");
            
            Assert.IsNotNull(lifecycleDataItem, "DataItem is not a lifecycle data item.");
            Assert.AreEqual(lifecycleDataItem.Status, Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master);
        }
        
        [Test]
        [Category(TestCategories.DynamicWidgets)]
        [Author(FeatherTeams.SitefinityTeam6)]
        [Description("Verifies that the LiveParentItem method of the ItemViewModelExtensions works properly.")]
        [Multilingual(MultilingualExecutionMode.MultiAndMonoLingual)]
        public void ItemViewModelExtensions_LiveParentItem_ReturnsLiveParentItem()
        {
            var dynamicContentModel = new DynamicContentModel();
            var child1Type = TypeResolutionService.ResolveType(DynamicWidgetsHierarchicalContentTests.Child1TypeName);
            var rootContentType = TypeResolutionService.ResolveType(DynamicWidgetsHierarchicalContentTests.RootContentTypeName);
            
            dynamicContentModel.ContentType = child1Type;
            
            var manager = DynamicModuleManager.GetManager();
            var liveItem = manager.Lifecycle.GetLive(manager.GetDataItems(child1Type).Where(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master).First());
            var viewModel = dynamicContentModel.CreateDetailsViewModel(liveItem);
            
            var result = viewModel.Item.ParentItem();
            var lifecycleDataItem = (ILifecycleDataItem)result.DataItem;
            
            Assert.IsNotNull(result, "LiveParentItem returned null when there should have been a parent item.");
            Assert.IsAssignableFrom(rootContentType, result.DataItem.GetType(), "The LiveParentItem result did not contain a DataItem of the expected type.");
            
            Assert.IsNotNull(lifecycleDataItem, "DataItem is not a lifecycle data item.");
            Assert.AreEqual(lifecycleDataItem.Status, Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live);
        }
        
        #endregion
        
        #region Private methods
        
        private void CreateItems()
        {
            var child1Type = TypeResolutionService.ResolveType(DynamicWidgetsHierarchicalContentTests.Child1TypeName);
            var rootContentType = TypeResolutionService.ResolveType(DynamicWidgetsHierarchicalContentTests.RootContentTypeName);
            
            var dynamicModuleManager = DynamicModuleManager.GetManager();
            
            var child1 = dynamicModuleManager.CreateDataItem(child1Type);
            child1.SetValue("Title", "Child1 title");
            child1.UrlName = "Child1-title";
            
            var child2 = dynamicModuleManager.CreateDataItem(child1Type);
            child2.SetValue("Title", "Child2 title");
            child2.UrlName = "Child2-title";
            
            var rootContentItem = dynamicModuleManager.CreateDataItem(rootContentType);
            
            rootContentItem.SetValue("Title", "Some Title");
            rootContentItem.UrlName = "SomeUrlName";
            
            rootContentItem.AddChildItem(child1);
            rootContentItem.AddChildItem(child2);
            
            dynamicModuleManager.Lifecycle.Publish(child1);
            dynamicModuleManager.Lifecycle.Publish(child2);
            dynamicModuleManager.Lifecycle.Publish(rootContentItem); 
            
            dynamicModuleManager.SaveChanges();
        }
        
        #endregion
        
        #region Fields and constants
        
        private const string ModuleName = "Hierarchical Module";
        private const string ModuleResource = "FeatherWidgets.TestUtilities.Data.DynamicModules.HierarchicalModule.zip";
        private const string TransactionName = "Module Installations";
        private const string Child1TypeName = "Telerik.Sitefinity.DynamicTypes.Model.HierarchicalModule.Child1Type";
        private const string RootContentTypeName = "Telerik.Sitefinity.DynamicTypes.Model.HierarchicalModule.RootContentType";
    
        #endregion
    }
}