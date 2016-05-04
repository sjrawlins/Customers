using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iFactr.Core;
using iFactr.Core.Utilities;
using MonoCross.Navigation;
using Customers.Models;
using Customers.ViewModels;
using iFactr.UI;

namespace Customers.Controllers
{
    class CustomerDetailController : MXController<Customer>
    {
        public override string Load(string uri, Dictionary<string, string> parameters)
        {
            var action = parameters.GetValueOrDefault("action");
            if (action == "SAVE")
            {
                Model.Save();
                iApp.Navigate(new Link(CustomerListController.Uri));  // re-renders the ListView, adding the new Customer
            }
            else if (action == "UPDATE")
            {
                var custID = parameters.GetValueOrDefault("customerID");
                if (custID.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("customerID missing on UPDATE");
                }
                var customerViewModel = iApp.Session.GetValueOrDefault("DB") as CustomerViewModel;
                if (customerViewModel == null)
                {
                    throw new Exception("DB missing");
                }
                var custObject = customerViewModel.Customers.Where(c => c.CustomerID == custID).First();
                Model = custObject.Clone();  // make a copy so that updates are not instant
                
            }
            else if (action == "DELETE")
            {
                iApp.Log.Debug("TODO: Remove the Customer");
            }
            else
            {
                Model = new Customer();
            }

            // return ViewPerpective based on the state of the Model
            return ViewPerspective.Default;

        }

        public const string Uri = "CustomerDetailControllerUri";
    }
}