using System;
using System.Collections.Generic;
using System.Linq;

using iFactr.Core;
using iFactr.Core.Utilities;
using iFactr.UI;
using iFactr.UI.Controls;
using Customers.Models;
using Customers.ViewModels;
using Customers.Controllers;

namespace Customers.Views
{
    // Display a single customer and allow update.
    class CustomerDetailView : GridView<Customer>
    {
        Thickness margin = new Thickness(5, 10, 10, 5);

        protected override void OnRender()
        {
            Title = "Customer Detail";

            if (Model == null)
            {
                throw new Exception("Model cannot be null");
            };

            Columns.Add(Column.AutoSized);
            Columns.Add(Column.AutoSized);

            AddChild(new Label("Name:") { Margin = margin, });
            var nameBox = new TextBox()
            {
                Margin = margin,
                Placeholder = "Enter Customer Name",
                SubmitKey = "customerName",
            };
            nameBox.SetBinding(new Binding("Text", "Name")
            {
                Mode = BindingMode.TwoWay,   // provides update back to the Mod
                Source = Model,
            });
            AddChild(nameBox);

            AddChild(new Label("ID:") { Margin = margin, });
            var customerID = new Label()
            {
                Margin = margin,
                Text = Model.CustomerID,
            };
            customerID.SetBinding(new Binding("Text", "CustomerID")
            {
                Mode = BindingMode.TwoWay,   // provides update back to the Mod
                Source = Model,
            });
            AddChild(customerID);

            AddChild(new Label("Phone:") { Margin = margin, });
            var phoneBox = new TextBox()
            {
                Margin = margin,
                Placeholder = "(nnn) nnn-nnnn",
                SubmitKey = "phoneNummber",
            };
            phoneBox.SetBinding(new Binding("Text", "Phone")
            {
                Mode = BindingMode.TwoWay,
                Source = Model,
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
                Source = Model,
            });
            AddChild(emailBox);

            AddChild(new Label("Region:")
            {
                Margin = margin,
            });
            var regionSelector = new SelectList(Enum.GetNames(typeof(RegionCodes)))
            {
                //SelectedIndex = (int)Model.RegionCode,
                Margin = margin,
            };
            regionSelector.SetBinding(new Binding("SelectedIndex", "RegionCode")
            {
                Mode = BindingMode.TwoWay,
                Source = Model,
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
                Source = Model,
            });
            AddChild(scoreSlider);
            AddChild(scoreNumber);

            var cancelButton = new Button("CANCEL");
            var deleteButton = new Button("DELETE");
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
                        Submit(new Link(CustomerDetailController.Uri + "/SAVE"));
                    }
                };
                myAlert.Show();
            };

            deleteButton.Clicked += (o, e) =>
            {
                var myAlert = new Alert("DELETE ALERT", "Are you sure you want to delete this customer?", AlertButtons.YesNo);
                myAlert.Dismissed += (obj, args) =>
                {
                    if (args.Result == AlertResult.Yes)
                    {
                        Submit(new Link(CustomerDetailController.Uri + "/DELETE/" + Model.CustomerID));
                    }
                };
                myAlert.Show();
            };

            AddChild(cancelButton);
            AddChild(saveButton);
            AddChild(deleteButton);

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
