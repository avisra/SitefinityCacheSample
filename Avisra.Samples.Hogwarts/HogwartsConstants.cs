using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace Avisra.Samples.Hogwarts
{
    public class HogwartsConstants
    {
        public static readonly Type houseType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Howgarts.House");
        public static readonly Type activityType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Howgarts.Activity");
        public static readonly string cacheInstanceName = "Houses";
        public static readonly string cacheKeysInstanceName = "HouseKeys";
    }
}
