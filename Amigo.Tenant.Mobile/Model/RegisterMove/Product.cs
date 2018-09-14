using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Model.RegisterMove
{
    public class Product : BindableObject
    {
        public Product(string name,string description,string brand)
        {
            Name = name;
            Description = description;
            Brand = brand;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        private string _brand;
        public string Brand
        {
            get { return _brand; }
            set
            {
                _brand = value;
                OnPropertyChanged();
            }
        }
    }
}
