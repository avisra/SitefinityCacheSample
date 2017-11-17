using System;
using System.Collections.Generic;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.Events;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Avisra.Samples.Hogwarts.Startup), "Application_Start")]
namespace Avisra.Samples.Hogwarts
{
    public class Startup
    {
        public static void Application_Start()
        {
            Bootstrapper.Initialized += new EventHandler<ExecutedEventArgs>(Bootstrapper_Initialized);
        }

        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                if (Bootstrapper.IsDataInitialized)
                {
                    ObjectFactory.Container.RegisterType<ICacheManager, CacheManager>();

                    // Sitefinity has out of the box dependencies for each dynamic content item. But, it does notify the parent that one of the children have changed.
                    // So we are adding our own notifications so when a child item is added/updated/deleted, we notify the parent item so we can purge it from cache
                    // and recalculate the scoreboard.

                    // If this were a custom module/content type, we could completely control the notifications from the models!

                    EventHub.Subscribe<IDynamicContentCreatedEvent>(evt => IDynamicContentCreatedEvent(evt));
                    EventHub.Subscribe<IDynamicContentUpdatedEvent>(evt => IDynamicContentUpdatedEvent(evt));
                    EventHub.Subscribe<IDynamicContentDeletingEvent>(evt => IDynamicContentDeletingEvent(evt));
                }
            }
        }

        private static void IDynamicContentCreatedEvent(IDynamicContentCreatedEvent eventInfo)
        {
            if (eventInfo.Item.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live)
            {
                if (eventInfo.Item.GetType() == HogwartsConstants.activityType)
                {
                    // A new activity was created. Purge the cache for whichever house it belongs to
                    CacheDependency.Notify(new List<CacheDependencyKey>() { new CacheDependencyKey() { Key = eventInfo.Item.SystemParentId.ToString(), Type = HogwartsConstants.houseType } });
                }
                else if (eventInfo.Item.GetType() == HogwartsConstants.houseType)
                {
                    // House has been created. Purge the HouseKeys cache so the new house appears in the list
                    // ... Yes. I know that Hogwarts will only ever have 4 houses...
                    CacheDependency.Notify(new List<CacheDependencyKey>() { new CacheDependencyKey() { Key = HogwartsConstants.cacheKeysInstanceName, Type = HogwartsConstants.houseType } });
                }
            }
        }

        private static void IDynamicContentUpdatedEvent(IDynamicContentUpdatedEvent eventInfo)
        {
            if (eventInfo.Item.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live)
            {
                if (eventInfo.Item.GetType() == HogwartsConstants.activityType)
                {
                    // An activity was modified. Purge the cache for whichever house it belongs to
                    CacheDependency.Notify(new List<CacheDependencyKey>() { new CacheDependencyKey() { Key = eventInfo.Item.SystemParentId.ToString(), Type = HogwartsConstants.houseType } });
                }
                else if (eventInfo.Item.GetType() == HogwartsConstants.houseType)
                {
                    // House has been edited. Purge it's cache (so new title or logo loads in)
                    CacheDependency.Notify(new List<CacheDependencyKey>() { new CacheDependencyKey() { Key = eventInfo.Item.Id.ToString(), Type = HogwartsConstants.houseType } });
                }
            }
        }

        private static void IDynamicContentDeletingEvent(IDynamicContentDeletingEvent eventInfo)
        {
            // Notice we are using IDynamicContentDeletingEvent instead of IDynamicContentDeletedEvent
            // The only difference is that Deleting occurs before the transation is submitted to the database. But, it does occur after permission checks. The only reason this would fail
            // would be due to do a bug or database connection. We have to do this because IDynamicContentDeletedEvent occurs AFTER it has been deleted from the database. And Sitefinity
            // doesn't have access to the SystemParentId at that point.

            // Here we look at the Master record to verify the record is being deleted

            if (eventInfo.Item.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live)
            {
                if (eventInfo.Item.GetType() == HogwartsConstants.activityType)
                {
                    // An activity was deleted. Purge the cache for whichever house it belongs to
                    CacheDependency.Notify(new List<CacheDependencyKey>() { new CacheDependencyKey() { Key = eventInfo.Item.SystemParentId.ToString(), Type = HogwartsConstants.houseType } });
                }
                else if (eventInfo.Item.GetType() == HogwartsConstants.houseType)
                {
                    // House has been deleted. Purge the HouseKeys cache so the house disappears from the list
                    // ... Yes. I know that Hogwarts will only ever have 4 houses...
                    CacheDependency.Notify(new List<CacheDependencyKey>() { new CacheDependencyKey() { Key = HogwartsConstants.cacheKeysInstanceName, Type = HogwartsConstants.houseType } });
                }
            }
        }
    }
}
