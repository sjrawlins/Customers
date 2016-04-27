using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iFactr.Core;
using iFactr.Core.Utilities;
using Customers.ViewModels;
using MonoCross.Navigation;
using Customers.Models;

namespace Customers.Controllers
{
    class CustomerListController : MXController<CustomerViewModel>
    {
        public override string Load(string uri, Dictionary<string, string> parameters)
        {
            //  Populate the Controller's model

            iApp.Log.Info("Controller starts.  Coming from uri:" + uri);

            var theDataPath =
            iApp.Factory.DataPath;

            Model = iApp.Session.GetValueOrDefault("DB") as CustomerViewModel;
            if (Model == null)
            {
                Model = new CustomerViewModel();
                // initialize the data for demo
                Model.Customers = new List<Customer> {
                  new Customer() { Name = "Apple", Phone = "(982) 229-5630", EmailAddress = "tim@apple.com", RegionCode = RegionCodes.West, Score = 52, },
                  new Customer() { Name = "Google", Phone = "(982) 665-8800", EmailAddress = "jon@google.com", RegionCode = RegionCodes.East, Score = 17, },
                  new Customer() { Name = "Microsoft", Phone = "(789) 444-6000", EmailAddress = "sally@microsoft.com", RegionCode = RegionCodes.South, Score = 82, },
                  new Customer() { Name = "Xamarin", Phone = "(988) 789-0123", EmailAddress = "nat@xamarin.com", RegionCode = RegionCodes.North, Score = 72, },
                  new Customer() { Name = "Zebra", Phone = "(651) 456-7890", EmailAddress = "anders@zebra.com", RegionCode = RegionCodes.Central, Score = 99, },
                };

                Model.Regions = new List<Region> {
                  new Region() { Code = RegionCodes.North, AccountRepresentative = "Jeremy", },
                  new Region() { Code = RegionCodes.East, AccountRepresentative = "Nicolle", },
                  new Region() { Code = RegionCodes.South, AccountRepresentative = "James", },
                  new Region() { Code = RegionCodes.West, AccountRepresentative = "Chuck", },
                  new Region() { Code = RegionCodes.Central, AccountRepresentative = "Tim", },
                };
                iApp.Session["DB"] = Model;
            }

            iApp.Log.Debug("Controller about to return");

            // return ViewPerpective based on the state of the Model
            return ViewPerspective.Default;

        }

        public const string Uri = "CustomerListControllerUri";
    }
}