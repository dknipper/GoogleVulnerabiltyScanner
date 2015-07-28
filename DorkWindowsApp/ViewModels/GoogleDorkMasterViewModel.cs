using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AutoMapper;
using DorkBusiness.Google.Entities;
using Microsoft.Practices.Prism.Commands;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkMasterViewModel : BaseViewModel
    {
        private ObservableCollection<GoogleDorkParentViewModel> _googleDorkParentViewModels;
        private ObservableCollection<string> _googleDorkParentValues;
        private ObservableCollection<int> _googleDorkParentIds;
        private string _siteToSearch;
        private string _keywords;
        private string _browserUrl;
        private string _navigationBarUrl;
        private readonly GoogleDorkMaster _googleDorkMaster;

        public DelegateCommand UpdateBrowserUrlCommand { get; private set; }
        public DelegateCommand RepopulateGoogleDorkParentsCommand { get; private set; }
        public DelegateCommand<string> UpdateAllUrlsCommand { get; private set; }

        public ObservableCollection<GoogleDorkParentViewModel> GoogleDorkParentViewModels
        {
            get
            {
                return _googleDorkParentViewModels;
            }
            set
            {
                if (_googleDorkParentViewModels == value){return;}
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
                if (_googleDorkParentValues == value){return;}
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
                if (_googleDorkParentIds == value){return;}
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
                if (_siteToSearch == value){return;}
                _siteToSearch = value;
                NotifyPropertyChanged();
            }
        }

        public string BrowserUrl
        {
            get
            {
                return _browserUrl;
            }
            set
            {
                if (_browserUrl == value){return;}
                _browserUrl = value;
                NotifyPropertyChanged();
            }
        }

        public string NavigationBarUrl
        {
            get
            {
                return _navigationBarUrl;
            }
            set
            {
                if (_navigationBarUrl == value){return;}
                _navigationBarUrl = value;
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
                if (_keywords == value){return;}
                _keywords = value;
                NotifyPropertyChanged();
            }
        }

        public GoogleDorkMasterViewModel()
        {
            _browserUrl = _navigationBarUrl = "http://www.google.com/";
            UpdateBrowserUrlCommand = new DelegateCommand(UpdateBrowserUrl, CanUpdateBrowserUrl);
            UpdateAllUrlsCommand = new DelegateCommand<string>(UpdateAllUrls, CanUpdateAllUrls);
            RepopulateGoogleDorkParentsCommand = new DelegateCommand(RepopulateGoogleDorkParents, CanRepopulateGoogleDorkParents);
            _googleDorkMaster = new GoogleDorkMaster();
            _googleDorkParentViewModels = Mapper.Map<ObservableCollection<GoogleDorkParentViewModel>>(_googleDorkMaster.SearchGoogleDorks(_siteToSearch, _keywords));
        }

        private void RepopulateGoogleDorkParents()
        {
            var searchResults = _googleDorkMaster.SearchGoogleDorks(SiteToSearch, Keywords, GoogleDorkParentViewModels.Select(x => x.Id).ToList());
            foreach (var googleDorkParent in GoogleDorkParentViewModels)
            {
                var parent = googleDorkParent;
                var gp = searchResults.FirstOrDefault(x => x.Id == parent.Id);
                if (gp != null)
                {
                    googleDorkParent.GoogleDorks = Mapper.Map<ObservableCollection<GoogleDorkViewModel>>(gp.GoogleDorks);
                }
            }
        }

        private void UpdateBrowserUrl()
        {
            BrowserUrl = NavigationBarUrl;
        }

        private void UpdateAllUrls(string url)
        {
            NavigationBarUrl = url;
            BrowserUrl = url;
        }

        private bool CanUpdateBrowserUrl()
        {
            return true;
        }

        private bool CanUpdateAllUrls(string url)
        {
            return true;
        }

        private bool CanRepopulateGoogleDorkParents()
        {
            return true;
        }
    }
}