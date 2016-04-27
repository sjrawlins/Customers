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
            Name = Phone = EmailAddress = string.Empty;
            Score = 0;
        }

        public Customer(Customer other)
        {
            Name = other.Name;
            EmailAddress = other.EmailAddress;
            Phone = other.Phone;
            Score = other.Score;
            RegionCode = other.RegionCode;
        }

        public void Copy(Customer other)
        {
            other.Name = Name;
            other.EmailAddress = EmailAddress;
            other.Phone = Phone;
            other.Score = Score;
            other.RegionCode = RegionCode;
        }
    }
}
