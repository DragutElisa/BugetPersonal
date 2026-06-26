using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugetPersonal.Models
{
    public class CheltuieliCategory
    {
        public string Name { get; set; }
        public string IconSource { get; set; }

        public static List<CheltuieliCategory> GetCategories()
        {
            return new List<CheltuieliCategory>
            {
                new CheltuieliCategory { Name = "Facturi", IconSource = "bills.png" },
                new CheltuieliCategory { Name = "Alimente", IconSource = "groceries.png" },
                new CheltuieliCategory { Name = "Haine", IconSource = "clothes.png" },
                new CheltuieliCategory { Name = "Educație", IconSource = "education.png" },
                new CheltuieliCategory { Name = "Gym", IconSource = "gym.png" },
                 new CheltuieliCategory { Name = "Sanatate", IconSource = "healthcare.png" },
                 new CheltuieliCategory { Name = "Altele", IconSource = "more.png" }
            };
        }
    }
}