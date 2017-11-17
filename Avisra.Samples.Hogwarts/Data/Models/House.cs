using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;

namespace Avisra.Samples.Hogwarts.Data.Models
{
    public class House
    {
        public House() { }
        public House(DynamicContent house)
        {
            this.Id = house.OriginalContentId;
            this.Title = house.GetString("Title");
            this.LogoUrl = house.GetRelatedItems<Image>("Logo")?.FirstOrDefault()?.MediaUrl;

            this.CacheDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string LogoUrl { get; set; }
        public int Points { get; set; }
        public DateTime CacheDate { get; set; }
    }
}
