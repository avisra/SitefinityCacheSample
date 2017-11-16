using Avisra.Samples.Hogwarts.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model;

namespace Avisra.Samples.Hogwarts.Data
{
    public class HouseRepository
    {
        private readonly DynamicModuleManager manager;

        public HouseRepository(DynamicModuleManager manager)
        {
            this.manager = manager;
        }

        public IEnumerable<House> GetHouses()
        {
            using (new ElevatedModeRegion(this.manager))
            {
                var keys = HouseCache.GetKeys();

                // if cache is empty, initialize the cache
                if (keys == null)
                {
                    var houses = LoadHouses();
                    keys = houses.Select(m => m.Id.ToString()).ToList();
                    HouseCache.AddKeys(keys);
                }

                List<House> houseModels = new List<House>();
                foreach (var key in keys)
                {
                    houseModels.Add(GetHouse(Guid.Parse(key)));
                }

                return houseModels;
            }
        }

        public House GetHouse(Guid id)
        {
            using (new ElevatedModeRegion(this.manager))
            {
                var house = HouseCache.Get(id.ToString());
                if (house == null)
                {
                    var houses = this.LoadHouses().ToList();
                    house = houses.FirstOrDefault(h => h.Id == id);
                    if (house != null)
                    {
                        house.Points = this.manager.GetChildItems(new List<Guid>() { house.Id }, HogwartsConstants.activityType).Select(a => a.GetValue<int>("Points")).Sum();
                        HouseCache.Add(house.Id.ToString(), house);
                    }
                    else
                    {
                        house = null;
                    }
                }
                return house;
            }
        }

        private IEnumerable<House> LoadHouses()
        {
            using (new ElevatedModeRegion(this.manager))
            {
                return this.manager.GetDataItems(HogwartsConstants.houseType).Select(h => new House(h) { LogoUrl = h.GetValue<Image>("Logo").MediaUrl });
            }
        }
    }
}
