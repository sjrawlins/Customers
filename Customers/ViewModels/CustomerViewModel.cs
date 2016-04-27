using Customers.Models;
using System.Collections.Generic;

namespace Customers.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            Customers = null;
            Regions = null;
        }

        public CustomerViewModel Clone()
        {
            return (CustomerViewModel) MemberwiseClone();
        } 
        public List<Customer> Customers { get; set; }
        public int TotalCustomers { get { return Customers.Count; } }

        public List<Region> Regions { get; set; }

        public int TotalRegions { get { return Regions.Count; } }
    }
}