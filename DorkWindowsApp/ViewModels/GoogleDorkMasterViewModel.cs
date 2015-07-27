using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using DorkBusiness.Google.Entities;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkMasterViewModel : BaseViewModel
    {
        private ObservableCollection<GoogleDorkParentViewModel> _googleDorkParentViewModels;
        private ObservableCollection<string> _googleDorkParentValues;
        private ObservableCollection<int> _googleDorkParentIds;
        private string _siteToSearch;
        private string _keywords;

        private GoogleDorkMaster _googleDorkMaster;

        public ObservableCollection<GoogleDorkParentViewModel> GoogleDorkParentViewModels
        {
            get
            {
                return _googleDorkParentViewModels;
            }
            set
            {
                if (_googleDorkParentViewModels == value)
                {
                    return;
                }
                _googleDorkParentViewModels = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string> GoogleDorkParentValues
        {
            get
            {
                return _googleDorkParentValues;
            }
            set
            {
                if (_googleDorkParentValues == value)
                {
                    return;
                }
                _googleDorkParentValues = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<int> GoogleDorkParentIds
        {
            get
            {
                return _googleDorkParentIds;
            }
            set
            {
                if (_googleDorkParentIds == value)
                {
                    return;
                }
                _googleDorkParentIds = value;
                NotifyPropertyChanged();
            }
        }

        public string SiteToSearch
        {
            get
            {
                return _siteToSearch;
            }
            set
            {
                if (_siteToSearch == value)
                {
                    return;
                }
                _siteToSearch = value;
                RepopulateGoogleDorkParentds();
                NotifyPropertyChanged();
            }
        }

        public string Keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                if (_keywords == value)
                {
                    return;
                }
                _keywords = value;
                RepopulateGoogleDorkParentds();
                NotifyPropertyChanged();
            }
        }

        private void RepopulateGoogleDorkParentds()
        {
            var searchResults = _googleDorkMaster.SearchGoogleDorks(SiteToSearch, Keywords, GoogleDorkParentViewModels.Select(x => x.Id).ToList());
            foreach (var googleDorkParent in GoogleDorkParentViewModels)
            {
                var gp = searchResults.FirstOrDefault(x => x.Id == googleDorkParent.Id);
                if (gp != null)
                {
                    googleDorkParent.GoogleDorks = Mapper.Map<ObservableCollection<GoogleDorkViewModel>>(gp.GoogleDorks);
                }
            }
        }

        public GoogleDorkMasterViewModel()
        {
            _googleDorkMaster = new GoogleDorkMaster();
            _googleDorkParentViewModels = Mapper.Map<ObservableCollection<GoogleDorkParentViewModel>>(_googleDorkMaster.SearchGoogleDorks(_siteToSearch, _keywords));
        }
    }
}