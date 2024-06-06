using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Models
{
    public class PhysicalItem : Item
    {
        public Dictionary<string, int> CopiesByLocation { get; set; }

        // Constructor to initialize CopiesByLocation
        public PhysicalItem()
        {
            CopiesByLocation = new Dictionary<string, int>();
        }

        // Method to get details of copies by location
        protected string GetLocationsDetails()
        {
            var locationsWithCopies = CopiesByLocation.Where(location => location.Value > 0);
            return string.Join("; ", locationsWithCopies.Select(location => $"{location.Key}: {location.Value} copies"));
        }

        // Override method to display item details
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Locations: {GetLocationsDetails()}";
        }

        // Method to loan an item from a specific location
        public bool LoanItem(string location)
        {
            if (CopiesByLocation.ContainsKey(location) && CopiesByLocation[location] > 0)
            {
                CopiesByLocation[location]--;
                return true;
            }
            return false;
        }

        // Method to return an item to a specific location
        public void ReturnItem(string location)
        {
            if (CopiesByLocation.ContainsKey(location))
            {
                CopiesByLocation[location]++;
            }
            else
            {
                CopiesByLocation[location] = 1;
            }
        }
    }
}
