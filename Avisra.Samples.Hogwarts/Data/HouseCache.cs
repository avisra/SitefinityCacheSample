using Avisra.Samples.Hogwarts.Data.Models;
using System.Collections.Generic;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Services;

namespace Avisra.Samples.Hogwarts.Data
{
    public static class HouseCache
    {
        public static void Add(string key, House value)
        {
            var cacheDependency = new CompoundCacheDependency();

            // Subscribe to whenever the system receives messages to purge cache for this house
            cacheDependency.CacheDependencies.Add(new DataItemCacheDependency(HogwartsConstants.houseType, value.Id));

            // You can add additional dependencies here if needed

            lock (housesSync)
            {
                ICacheManager housesCache = SystemManager.GetCacheManager(HogwartsConstants.cacheInstanceName);
                // Add house domain model to cache with all dependencies
                housesCache.Add(key, value, CacheItemPriority.Normal, null, new ICacheItemExpiration[] { cacheDependency });
            }
        }

        public static House Get(string key)
        {
            lock (housesSync)
            {
                ICacheManager housesCache = SystemManager.GetCacheManager(HogwartsConstants.cacheInstanceName);
                if (housesCache.Contains(key))
                    return housesCache.GetData(key) as House;
            }

            return null;
        }

        #region Collection

        public static void AddKeys(IEnumerable<string> houseIds)
        {
            lock (housesSync)
            {
                ICacheManager housesCache = SystemManager.GetCacheManager(HogwartsConstants.cacheInstanceName);
                housesCache.Add("house_ids", houseIds, CacheItemPriority.Normal, null, new DataItemCacheDependency(HogwartsConstants.houseType, HogwartsConstants.cacheKeysInstanceName));
            }
        }

        public static IEnumerable<string> GetKeys()
        {
            lock (housesSync)
            {
                ICacheManager housesCache = SystemManager.GetCacheManager(HogwartsConstants.cacheInstanceName);
                return housesCache.GetData("house_ids") as IEnumerable<string>;
            }
        }

        public static IEnumerable<House> All()
        {
            lock (housesSync)
            {
                ICacheManager housesCache = SystemManager.GetCacheManager(HogwartsConstants.cacheInstanceName);
                List<House> allHouses = new List<House>();
                var keys = housesCache.GetData("house_ids") as IEnumerable<string>;
                foreach (var key in keys)
                {
                    if (housesCache.Contains(key))
                        allHouses.Add(housesCache.GetData(key) as House);
                }
                return allHouses;
            }
        }

        #endregion

        private static readonly object housesSync = new object();
    }
}
