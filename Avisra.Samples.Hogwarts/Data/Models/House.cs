using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace Avisra.Samples.Hogwarts.Data.Models
{
    public class House
    {
        public House() { }
        public House(DynamicContent house)
        {
            this.Id = house.OriginalContentId;
            this.Title = house.GetString("Title");
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string LogoUrl { get; set; }
        public int Points { get; set; }
    }
}
