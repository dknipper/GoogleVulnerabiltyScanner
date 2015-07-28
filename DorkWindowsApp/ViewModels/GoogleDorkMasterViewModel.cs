using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using DorkBusiness.Google.Entities;
using DorkWindowsApp.Commands;
using Microsoft.Practices.Prism.Commands;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkMasterViewModel : BaseViewModel
    {
        private ObservableCollection<GoogleDorkParentViewModel> _googleDorkParentViewModels;
        private ObservableCollection<string> _googleDorkParentValues;
        private ObservableCollection<int> _googleDorkParentIds;
        private string _siteToSearch;
        private string _syncOutput;
        private string _keywords;
        private string _browserUrl;
        private string _navigationBarUrl;
        private GoogleDorkSyncProgressViewModel _syncProgress;
        private readonly GoogleDorkMaster _googleDorkMaster;
        private readonly GoogleDorkSync _googleDorkSync;

        public DelegateCommand RepopulateGoogleDorkParentsCommand { get; private set; }
        public AsyncDelegateCommand SyncCommand { get; private set; }
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

        public string SyncOutput
        {
            get
            {
                return _syncOutput;
            }
            set
            {
                if (_syncOutput == value) { return; }
                _syncOutput = value;
                NotifyPropertyChanged();
            }
        }

        public GoogleDorkSyncProgressViewModel SyncProgress
        {
            get
            {
                return _syncProgress;
            }
            set
            {
                if (_syncProgress == value) { return; }
                _syncProgress = value;
                NotifyPropertyChanged();
            }
        }

        public GoogleDorkMasterViewModel()
        {
            _browserUrl = _navigationBarUrl = "http://www.google.com/";
            UpdateAllUrlsCommand = new DelegateCommand<string>(UpdateAllUrls, CanUpdateAllUrls);
            SyncCommand = new AsyncDelegateCommand(Sync, CanSync);
            RepopulateGoogleDorkParentsCommand = new DelegateCommand(RepopulateGoogleDorkParents, CanRepopulateGoogleDorkParents);
            _googleDorkMaster = new GoogleDorkMaster();
            _googleDorkParentViewModels = Mapper.Map<ObservableCollection<GoogleDorkParentViewModel>>(_googleDorkMaster.SearchGoogleDorks(_siteToSearch, _keywords));
            _googleDorkSync = new GoogleDorkSync();
            _googleDorkSync.OnGoogleDorkSyncProgressChange += GoogleDorkSyncProgressChange;
            _syncProgress = new GoogleDorkSyncProgressViewModel();
        }

        public void GoogleDorkSyncProgressChange(object sender, GoogleDorkSyncProgressChangeEventArgs e)
        {
            SyncProgress.Title = e.ProcessedItem.Title;
            SyncProgress.PercentageComplete = e.ProcessedItem.PercentageComplete;
            SyncProgress.Summary = e.ProcessedItem.Summary;

            if (e.ProcessedItem.PercentageComplete >= 100)
            {
                
            }

            SyncOutput =
                (e.ProcessedItem.PercentageComplete >= 100)
                    ? string.Format("{0}{3}{3}{1}{3}{2}", _syncOutput, e.ProcessedItem.Summary, "Writing to database...", System.Environment.NewLine)
                    : string.Format("{0}{4}{4}{1}{4}{2}{4}{3}", _syncOutput, e.ProcessedItem.GoogleDorkParentName, e.ProcessedItem.Title, e.ProcessedItem.Summary, System.Environment.NewLine);
        }

        private void UpdateAllUrls(string url)
        {
            NavigationBarUrl = url;
            BrowserUrl = url;
        }

        private void Sync()
        {
            _googleDorkSync.SyncGoogleDorkParents();
            _googleDorkSync.SyncGoogleDorks();           
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

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private bool CanUpdateAllUrls(string url)
        {
            return true;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private bool CanRepopulateGoogleDorkParents()
        {
            return true;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private bool CanSync()
        {
            return true;
        }
    }
}