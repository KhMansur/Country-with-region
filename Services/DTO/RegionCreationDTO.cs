using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class RegionCreationDTO
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int CountryId { get; set; }
    }
}
