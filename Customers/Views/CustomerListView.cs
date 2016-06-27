using System;
using System.Collections.Generic;
using System.Linq;

using iFactr.Core;
using iFactr.Core.Utilities;
using iFactr.UI;
using iFactr.UI.Controls;
using Customers.ViewModels;
using Customers.Controllers;
using Customers.Models;
using iFactr.Utilities.Serialization;

namespace Customers.Views
{
    class CustomerListView : ListView<CustomerViewModel>
    {
        CustomerViewModel filteredModel = new CustomerViewModel();

        /// <summary>
        /// Called when the view is ready to be rendered.
        /// </summary>
        protected override void OnRender()
        {
            Title = "Customer List";

            filteredModel = Model.Clone();

            var mySearchBox = new SearchBox();
            mySearchBox.SearchPerformed += (o, e) =>
            {
                string query = e.SearchText.ToUpperInvariant();  // case insenstive
                // filter model data based on query
                filteredModel.Customers = Model.Customers.Where(c => c.Name.ToUpperInvariant().StartsWith(query)).ToList();
                filteredModel.Regions = Model.Regions.Where(r => r.Code.ToString().StartsWith(query)).ToList();
                Sections[0].ItemCount = filteredModel.TotalCustomers;
                Sections[1].ItemCount = filteredModel.TotalRegions;
                ReloadSections();
            };
            SearchBox = mySearchBox;

            // Set the item count to the desired number of cells in the list.
            Sections[0].ItemCount = Model.TotalCustomers;
            Sections[0].Header = new SectionHeader("Customers")
            {
                Font = Font.PreferredHeaderFont,
                BackgroundColor = Color.Green,
                ForegroundColor = Color.White,
            };

            Sections[1].ItemCount = Model.TotalRegions;
            Sections[1].CellRequested += (index, recycledCell) =>
            {
                ContentCell cell = new ContentCell();
             
                cell.TextLabel.Text = filteredModel.Regions[index].Code.ToString();
                cell.SubtextLabel.Text = filteredModel.Regions[index].AccountRepresentative;
                cell.ValueLabel.Text = filteredModel.Regions[index].CustomerCount.ToString();
                return cell;
            };
            Sections[1].Header = new SectionHeader("Regions")
            {
                Font = Font.PreferredHeaderFont,
                BackgroundColor = Color.Green,
                ForegroundColor = Color.White,
            };
            SeparatorColor = Color.Gray;

            var addAction = new MenuButton("Add");
            Menu = new Menu(addAction);
            addAction.Clicked += (o, e) =>
            {
                iApp.Navigate(new Link(CustomerDetailController.Uri));
            };

        }

        private void SaveAction_Clicked(object sender, EventArgs e)

        {
            iApp.Log.Debug("Serialize to XML file");
            var ser = new SerializerXml<CustomerViewModel>();

            iApp.Log.Debug("ApplicationPath is: " + Device.ApplicationPath);
            iApp.Log.Debug("DataPath is: " + Device.DataPath);
            iApp.Log.Debug("Platform is: " + Device.Platform);
            iApp.Log.Debug("SeparatorChar is: " + Device.DirectorySeparatorChar);
            iApp.Log.Debug("SessionDataPath" + Device.SessionDataPath);


            System.Text.StringBuilder mysb = new System.Text.StringBuilder();
            var dataFile = iApp.Factory.DataPath.AppendPath("appendedFile");
            var xmlDataFile = iApp.Factory.DataPath.AppendPath("customerdata.xml");
            foreach (var customer in Model.Customers)
            {

                mysb.AppendLine(customer.Name);
                mysb.AppendLine(customer.EmailAddress);
                mysb.AppendLine(customer.Phone);
                mysb.AppendLine(customer.Score.ToString());

            }

            ser.SerializeObjectToFile(Model, xmlDataFile);
            iApp.Factory.File.Save(dataFile, mysb.ToString());


        }

        /// <summary>
        /// Called when a cell is ready to be rendered on the screen.  Return the ICell instance
        /// that should be placed at the given index within the given section.
        /// </summary>
        /// <param name="section">The section that will contain the cell.</param>
        /// <param name="index">The index at which the cell will be placed in the section.</param>
        /// <param name="recycledCell">An already instantiated cell that is ready for reuse, or null if no cell has been recycled.</param>
        protected override ICell OnCellRequested(int section, int index, ICell recycledCell)
        {
            if (filteredModel.TotalCustomers <= 0) return null;

            var cell = recycledCell as CustomerGridCell;
            if (cell == null)
                cell = new CustomerGridCell();

            var curCustomer = filteredModel.Customers[index];

            // Binding is 1-way, from Source (the Model) to the Target (the onscreen control).
            // There is no direct update from the list, but if, on iPad/tablet the Detail-screen value changes because
            // of user update, we want to see this List update immediately and accomplish this through Data Binding.

            cell.CustomerNameLabel.SetBinding(new Binding("Text", "Name")
            {
                Source = curCustomer,
            });

            cell.CustomerIDLabel.SetBinding(new Binding("Text", "CustomerID")
            {
                Source = curCustomer,
            });

            cell.PhoneNumberLabel.SetBinding(new Binding("Text", "Phone")
            {
                Source = curCustomer,
            });

            cell.EmailAddressLabel.SetBinding(new Binding("Text", "EmailAddress")
            {
                Source = curCustomer,
            });

            cell.RegionLabel.SetBinding(new Binding("Text", "RegionCode")
            {
                Source = curCustomer,
            });

            //cell.ScoreLabel.ClearAllBindings();
            cell.ScoreLabel.SetBinding(new Binding("Text", "Score")
            {
                Source = curCustomer,
            });

            // Tapping Customer cell navigates to that Customer's Detail/Update screen
            cell.NavigationLink = new Link(Controllers.CustomerDetailController.Uri + "/UPDATE/" + curCustomer.CustomerID);

            return cell;
        }

        /// <summary>
        /// Called when a reuse identifier is needed for a cell or tile.  Return the identifier that should be used
        /// to determine which cells or tiles may be reused for the item at the given index within the given section.
        /// This is only called on platforms that support recycling.  See Remarks for details.
        /// </summary>
        /// <param name="section">The section that will contain the item.</param>
        /// <param name="index">The index at which the item will be placed in the section.</param>
        protected override int OnItemIdRequested(int section, int index)
        {

            int returnType = section;
            switch (section)
            {
                case 0:
                    returnType = CustomerGridCell.CellTypeID;
                    break;
                default:
                    break;
            }

            return returnType;
        }
    }
    class CustomerGridCell : GridCell
    {
        public const int CellTypeID = 42;
        Thickness margin = new Thickness(0, 5, 10, 0);

        public ILabel CustomerNameLabel;
        public ILabel CustomerIDLabel;
        public ILabel PhoneNumberLabel;
        public ILabel EmailAddressLabel;
        public ILabel RegionLabel;
        public ILabel ScoreLabel;
        public CustomerGridCell()
        {

            Columns.Add(Column.AutoSized);
            Columns.Add(Column.AutoSized);

            // Customer name label and value
            var firstLabel = new Label()
            {
                ID = "nameLabelID",
                Text = "Name:",
                Margin = margin,
            };
            AddChild(firstLabel);

            CustomerNameLabel = new Label()
            {
                ID = "nameValueID",
                Margin = margin,
            };
            AddChild(CustomerNameLabel);

            // add a row for Unique Customer ID
            Rows.Add(Row.AutoSized);

            var custIDLabel = new Label()
            {
                ID = "custIDLabelID",
                Text = "Customer ID:",
                Margin = margin,
            };
            AddChild(custIDLabel);

            CustomerIDLabel = new Label()
            {
                ID = "custIDValueID",
                Margin = margin,
            };
            AddChild(CustomerIDLabel);

            // add a row for the phone number label and value
            Rows.Add(Row.AutoSized);
            firstLabel = new Label()
            {
                ID = "phoneLabelID",
                Text = "Phone:",
                Margin = margin,
            };
            AddChild(firstLabel);

            PhoneNumberLabel = new Label()
            {
                ID = "phoneValueID",
                Margin = margin,
            };
            AddChild(PhoneNumberLabel);

            // add a row for the email address label and value
            Rows.Add(Row.AutoSized);
            firstLabel = new Label()
            {
                ID = "emailLabelID",
                Text = "Email:",
                Margin = margin,
            };
            AddChild(firstLabel);
            EmailAddressLabel = new Label()
            {
                ID = "emailValueID",
                Margin = margin,
            };
            AddChild(EmailAddressLabel);

            // add a row for Region
            Rows.Add(Row.AutoSized);
            firstLabel = new Label()
            {
                ID = "regionLabelID",
                Text = "Region:",
                Margin = margin,
            };
            AddChild(firstLabel);
            RegionLabel = new Label()
            {
                ID = "regionValueID",
                Margin = margin,
            };
            AddChild(RegionLabel);

            // add a row for Rating/Score
            Rows.Add(Row.AutoSized);
            firstLabel = new Label()
            {
                ID = "scoreLabelID",
                Text = "Score:",
                Margin = margin,
            };
            AddChild(firstLabel);
            ScoreLabel = new Label()
            {
                ID = "scoreValueID",
                Margin = margin,
            };
            AddChild(ScoreLabel);

        }

    }
}
