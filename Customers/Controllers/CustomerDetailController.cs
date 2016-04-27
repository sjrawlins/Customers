using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iFactr.Core;
using iFactr.Core.Utilities;
using MonoCross.Navigation;
using Customers.Models;
using Customers.ViewModels;

namespace Customers.Controllers
{
    class CustomerDetailController : MXController<Customer>
    {
        public override string Load(string uri, Dictionary<string, string> parameters)
        {
            var custIndex = 0;
            string viewPerspective = ViewPerspective.Default;

            var index = parameters.GetValueOrDefault("index");
            if (index.IsNullOrEmptyOrWhiteSpace())
            {
                Model = null;  // forces the Detail View to create it
                viewPerspective = "Create";
            }
            else
            {
                bool res = int.TryParse(index, out custIndex);
                if (!res) custIndex = 0;
                var customerViewModel = iApp.Session.GetValueOrDefault("DB") as CustomerViewModel;
                if (customerViewModel == null)
                {
                    throw new System.Exception("Internal error");
                }
                Model = customerViewModel.Customers[custIndex];
            }

            // return ViewPerpective based on the state of the Model
            return viewPerspective;

        }

        public const string Uri = "CustomerDetailControllerUri";
    }
}