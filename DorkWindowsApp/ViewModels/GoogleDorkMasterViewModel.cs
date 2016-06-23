using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private GoogleDorkVulnerableSiteViewModelCollection _googleDorkVulnerableSiteViewModels;
        private string _siteToSearch;
        private string _syncOutput;
        private string _keywords;
        private string _browserUrl;
        private string _searchTerm;
        private GoogleDorkSyncProgressViewModel _syncProgress;
        private readonly GoogleDorkMaster _googleDorkMaster;
        private readonly GoogleDorkSync _googleDorkSync;

        public DelegateCommand RepopulateGoogleDorkParentsCommand { get; private set; }
        public DelegateCommand<string> LaunchBrowserCommand { get; private set; }
        public AsyncDelegateCommand SyncCommand { get; private set; }
        public DelegateCommand<string> SelectGoogleDorkFromTreeCommand { get; private set; }
        public DelegateCommand SaveSiteVulnerabilitiesCommand { get; private set; }
        public DelegateCommand<GoogleDorkVulnerableSiteViewModel> DeleteSiteVulnerabilityCommand { get; private set; }

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

        public GoogleDorkVulnerableSiteViewModelCollection GoogleDorkVulnerableSiteViewModels
        {
            get
            {
                return _googleDorkVulnerableSiteViewModels;
            }
            set
            {
                if (_googleDorkVulnerableSiteViewModels == value) { return; }
                _googleDorkVulnerableSiteViewModels = value;
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

        public string SearchTerm
        {
            get
            {
                return _searchTerm;
            }
            set
            {
                if (_searchTerm == value) { return; }
                _searchTerm = value;
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
            _browserUrl = "http://www.google.com/";
            SelectGoogleDorkFromTreeCommand = new DelegateCommand<string>(SelectGoogleDorkFromTree, CanUpdateAllUrls);
            SyncCommand = new AsyncDelegateCommand(Sync, CanSync);
            RepopulateGoogleDorkParentsCommand = new DelegateCommand(RepopulateGoogleDorkParents, CanRepopulateGoogleDorkParents);
            SaveSiteVulnerabilitiesCommand = new DelegateCommand(SaveSiteVulnerabilities, CanSaveSiteVulnerabilities);
            DeleteSiteVulnerabilityCommand = new DelegateCommand<GoogleDorkVulnerableSiteViewModel>(DeleteSiteVulnerability, CanDeleteSiteVulnerability);
            LaunchBrowserCommand = new DelegateCommand<string>(LaunchBrowser, CanLaunchBrowser);
            _googleDorkMaster = new GoogleDorkMaster();
            _googleDorkVulnerableSiteViewModels = Mapper.Map<GoogleDorkVulnerableSiteViewModelCollection>(GoogleDorkVulnerableSite.GetGoogleDorkVulnerableSites());
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

            SyncOutput =
                (e.ProcessedItem.PercentageComplete >= 100)
                    ? string.Format("{0}{3}{3}{1}{3}{2}", _syncOutput, e.ProcessedItem.Summary, "Writing to database...", System.Environment.NewLine)
                    : string.Format("{0}{4}{4}{1}{4}{2}{4}{3}", _syncOutput, e.ProcessedItem.GoogleDorkParentName, e.ProcessedItem.Title, e.ProcessedItem.Summary, System.Environment.NewLine);
        }

        private void SelectGoogleDorkFromTree(string url)
        {
            BrowserUrl = url;

            foreach (var googleDorkParent in GoogleDorkParentViewModels)
            {
                foreach (var googleDork in googleDorkParent.GoogleDorks)
                {
                    googleDork.IsSelected = googleDork.GoogleUrl == url;
                }
            }
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

        // ReSharper disable MemberCanBeMadeStatic.Local
        private void LaunchBrowser(string url)
        {
            Process.Start(new ProcessStartInfo(url));
        }

        private void SaveSiteVulnerabilities()
        {

        }

        private void DeleteSiteVulnerability(GoogleDorkVulnerableSiteViewModel vulnerableSite)
        {
            GoogleDorkVulnerableSiteViewModels.Remove(vulnerableSite);
        }
        
        private bool CanUpdateAllUrls(string url)
        {
            return true;
        }

        private bool CanRepopulateGoogleDorkParents()
        {
            return true;
        }

        private bool CanSync()
        {
            return true;
        }

        private bool CanSaveSiteVulnerabilities()
        {
            return true;
        }

        private bool CanDeleteSiteVulnerability(GoogleDorkVulnerableSiteViewModel vulnerableSite)
        {
            return true;
        }

        private bool CanLaunchBrowser(string url)
        {
            return true;
        }
    }
}