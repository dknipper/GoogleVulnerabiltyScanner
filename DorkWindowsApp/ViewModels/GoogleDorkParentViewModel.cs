using System.Collections.ObjectModel;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkParentViewModel : BaseViewModel
    {
        private string _name;
        private int _id;
        private bool _isSelected;
        private bool _isExpanded;
        private ObservableCollection<GoogleDorkViewModel> _googleDorks;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value){return;}
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value){return;}
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<GoogleDorkViewModel> GoogleDorks
        {
            get
            {
                return _googleDorks;
            }
            set
            {
                if (_googleDorks == value){return;}
                _googleDorks = value;
                NotifyPropertyChanged();
            }
        }
        
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value == _isSelected){return;}
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }
        
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                if (value == _isExpanded){return;}
                _isExpanded = value;
                NotifyPropertyChanged();
            }
        }
    }
}