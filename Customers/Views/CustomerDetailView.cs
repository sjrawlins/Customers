using System;
using System.Collections.Generic;
using System.Linq;

using iFactr.Core;
using iFactr.Core.Utilities;
using iFactr.UI;
using iFactr.UI.Controls;
using Customers.Models;
using Customers.ViewModels;

namespace Customers.Views
{
    // Display a single customer and allow update.
    class CustomerDetailView : GridView<Customer>
    {
        Thickness margin = new Thickness(5, 10, 10, 5);
        
        protected override void OnRender()
        {
            Title = "Customer Detail";
            Customer tempCustomer;
            CustomerViewModel custDB = null;

            if (Model == null)
            {
                tempCustomer = new Customer();
                // get the list "Database" in case user adds a new customer, then return
                // custDB = iApp.Session.GetValueOrDefault("DB") as CustomerViewModel;
                custDB = (CustomerViewModel)iApp.Session["DB"];
                var myGlobals = iApp.Session;

                if (custDB == null)
                {
                    // throw new System.Exception("Internal error");
                    tempCustomer = new Customer();
                }
            }
            else {
                tempCustomer = new Customer(Model);  // make a copy of the Customer in case user CANCELs
            }


            Columns.Add(Column.AutoSized);
            Columns.Add(Column.AutoSized);

            AddChild(new Label("Name:") { Margin = margin, });
            var nameBox = new TextBox() { Margin = margin, Placeholder = "Enter Customer Name", };
            nameBox.SetBinding(new Binding("Text", "Name")
            {
                Mode = BindingMode.TwoWay,   // provides update back to the Mod
                Source = tempCustomer,
            });
            AddChild(nameBox);

            AddChild(new Label("Phone:") { Margin = margin, });
            var phoneBox = new TextBox() { Margin = margin, Placeholder = "(nnn) nnn-nnnn", };
            phoneBox.SetBinding(new Binding("Text", "Phone")
            {
                Mode = BindingMode.TwoWay,
                Source = tempCustomer,
            });
            AddChild(phoneBox);

            AddChild(new Label("Email address:")
            {
                Margin = margin,
            });
            var emailBox = new TextBox()
            {
                Margin = margin,
                Placeholder = "boss@acme.com",
            };
            emailBox.SetBinding(new Binding("Text", "EmailAddress")
            {
                Mode = BindingMode.TwoWay,
                Source = tempCustomer,
            });
            AddChild(emailBox);

            AddChild(new Label("Region:")
            {
                Margin = margin,
            });
            var regionSelector = new SelectList(Enum.GetNames(typeof(RegionCodes)))
            {
                //SelectedIndex = (int)tempCustomer.RegionCode,
                Margin = margin,
            };
            regionSelector.SetBinding(new Binding("SelectedIndex", "RegionCode")
            {
                Mode = BindingMode.TwoWay,
                Source = tempCustomer,
            });
            AddChild(regionSelector);

            AddChild(new Label("Score:")
            {
                Margin = margin,
            });
            var scoreSlider = new Slider()
            {
                Margin = new Thickness(0, 20),
                MaximumTrackColor = Color.Red,
                MinimumTrackColor = Color.Green,
                MaxValue = 100,
                MinValue = 0,
            };
            var scoreNumber = new Label()
            {
                Margin = margin,
                HorizontalAlignment = HorizontalAlignment.Right,
                ColumnSpan = 2,  // because there is no label for this label!
            };
            scoreNumber.SetBinding(new Binding("Text", "Value")  // TARGET is the Label, SOURCE is the Slider Value which is double
            {
                Source = scoreSlider,
                ValueConverter = new SliderValueRounder(),  // round to nearest integer
            });
            scoreSlider.SetBinding(new Binding("Value", "Score")
            {
                Mode = BindingMode.TwoWay,
                Source = tempCustomer,
            });
            AddChild(scoreSlider);
            AddChild(scoreNumber);

            var cancelButton = new Button("CANCEL");
            var saveButton = new Button("SAVE");

            cancelButton.Clicked += (o, e) =>
            {
                var myAlert = new Alert("CANCEL ALERT", "Are you sure you want to abandon?", AlertButtons.YesNo);
                myAlert.Dismissed += CancelAlert_Dismissed;
                myAlert.Show();
            };

            saveButton.Clicked += (o, e) =>
            {
                var myAlert = new Alert("SAVE ALERT", "Are you sure you want to keep this?", AlertButtons.YesNo);
                myAlert.Dismissed += (obj, args) =>
                {
                    if (args.Result == AlertResult.Yes)
                    {
                        if (Model == null)
                        {
                            // this is an "ADD"
                            custDB.Customers.Add(tempCustomer);  // add the new customer to the list
                                                                 // re-render the list of customers
                                   

                            PaneManager.Instance.FromNavContext(Pane.Master).Views.Last(c => c is CustomerListView).Render();
                        }
                        else
                        {
                            tempCustomer.Copy(Model);  // copy temp back to original customer
                        }

                        this.Stack.PopView();
                    }
                };
                myAlert.Show();
            };

            AddChild(cancelButton);
            AddChild(saveButton);

        }



        private void CancelAlert_Dismissed(object sender, AlertResultEventArgs args)
        {
            if (args.Result == AlertResult.Yes) this.Stack.PopView();
        }
    }
    class SliderValueRounder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter)
        {
            double dblSliderValue = (double)value;
            int intSliderValue = (int)Math.Round(dblSliderValue);
            return (object)intSliderValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter)
        {
            return value;  // no conversion in this direction
        }
    }
}
