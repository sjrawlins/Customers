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

            NavigationMap.Add("CustomerDetailControllerUri/{index}", new CustomerDetailController());

            var myC = new CustomerDetailController();

            NavigationMap.Add(CustomerDetailController.Uri, myC);
            NavigationMap.Add(CustomerListController.Uri, new CustomerListController());

            // Add Views to ViewMap
            MXContainer.AddView<Customer>(typeof(Views.CustomerDetailView));
            MXContainer.AddView<Customer>(typeof(Views.CustomerDetailView), "Create");
            //

            MXContainer.AddView<CustomerViewModel>(typeof(CustomerListView));

            //  Class demo only:
            // MXContainer.AddView<CustomerViewModel>(typeof(OtherListView), "Another");


            // Set default navigation URI
            NavigateOnLoad = CustomerListController.Uri;

        }
    }
}
