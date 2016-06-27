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
            var customerViewModel = iApp.Session.GetValueOrDefault("DB") as CustomerViewModel;
            if (customerViewModel == null)
            {
                throw new Exception("DB missing");
            }
            if (action == "SAVE")
            {
                Model.Save();
                iApp.Navigate(new Link(CustomerListController.Uri));  // re-renders the ListView, adding the new Customer
            }
            else if (action == "UPDATE" || action == "DELETE")
            {
                var custID = parameters.GetValueOrDefault("customerID");
                if (custID.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("customerID missing on " + action);
                }

                var custObject = customerViewModel.Customers.Where(c => c.CustomerID == custID).First();
                if (action == "DELETE")
                {
                    custObject.Delete();
                    iApp.Navigate(CustomerListController.Uri);

                }
                else  // UPDATE, meaning: the customer already exists and the user has tapped that customer cell for details, so copy for potential update
                {
                    Model = custObject.Clone();  // make a copy so that updates are not instant
                }     
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