using Customers.ViewModels;
using iFactr.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Models
{
    public class Customer : INotifyPropertyChanged
    {

        // The "Database" assigns GUID when first saving
        public void Save()
        {
            var custDB = (CustomerViewModel)iApp.Session["DB"];
            if (CustomerID.IsNullOrEmptyOrWhiteSpace())  // it's a new customer and needs a GUID for "unique" CustomerID
            {
                this.CustomerID = Guid.NewGuid().ToString();
                custDB.Customers.Add(this);
            }
            else
            {
                // it's an update (the customer is already in the Database)
                iApp.Log.Debug("Nothing to do here.  The Model has already been updated");
            }
        }

        string _customerID;
        public string CustomerID // unique ID

        {
            get { return _customerID; }
            set
            {
                if (_customerID != value)
                {
                    _customerID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CustomerID"));
                }
            }
        }
        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Phone"));
                }
            }
        }
        int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                if (_score != value)
                {
                    _score = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }
        string _email;
        public string EmailAddress
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmailAddress"));
                }
            }
        }

        RegionCodes _region;
        public RegionCodes RegionCode
        {
            get { return _region; }
            set
            {
                if (_region != value)
                {
                    _region = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RegionCode"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Customer()
        {
            CustomerID = Name = Phone = EmailAddress = string.Empty;
            Score = 0;
        }

        public Customer Clone()
        {
            return (Customer) this.MemberwiseClone();
        }

    }
}
