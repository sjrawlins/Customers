using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iFactr.Core;
using iFactr.Core.Utilities;
using iFactr.UI;
using MonoCross.Navigation;

using Customers.Controllers;
using Customers.Views;
using Customers.ViewModels;
using Customers.Models;

namespace Customers
{
    public class MyApp : iApp
    {
        // Add code to initialize your application here.  For more information, see http://support.ifactr.com/
        public override void OnAppLoad()
        {
            // Set the application title
            Title = "Customers";
            iApp.Log.LoggingLevel = iFactr.Core.Utilities.Logging.LoggingLevel.Debug;

            // Add navigation mappings

            var myC = new CustomerDetailController();
            NavigationMap.Add(CustomerDetailController.Uri + "/{action}", myC);   // SAVE
            NavigationMap.Add(CustomerDetailController.Uri + "/{action}/{customerID}", myC);  // UPDATE or DELETE
            NavigationMap.Add(CustomerDetailController.Uri, myC);   // for creating a new customer ("Add" action off the list menu)

            NavigationMap.Add(CustomerListController.Uri, new CustomerListController());

            // Add Views to ViewMap
            MXContainer.AddView<Customer>(typeof(CustomerDetailView));
            MXContainer.AddView<CustomerViewModel>(typeof(CustomerListView));

            // Set default navigation URI
            NavigateOnLoad = CustomerListController.Uri;

        }
    }
}
