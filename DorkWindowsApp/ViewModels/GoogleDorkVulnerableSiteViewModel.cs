
using AutoMapper;
using DorkBusiness.Google.Entities;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkVulnerableSiteViewModel : BaseViewModel
    {
        private int _id;
        private int _googleDorkId;
        private string _site = "";
        private string _keywords;
        private GoogleDorkViewModel _googleDork;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value) { return; }
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public int GoogleDorkId
        {
            get
            {
                return _googleDorkId;
            }
            set
            {
                if (_googleDorkId == value) { return; }
                _googleDorkId = value;
                _googleDork = null;
                NotifyPropertyChanged();
            }
        }

        public GoogleDorkViewModel GoogleDorkViewModel
        {
            get
            {
                if (_googleDork != null)
                {
                    return _googleDork;
                }
                _googleDork = Mapper.Map<GoogleDorkViewModel>(GoogleDork.GetGoogleDork(GoogleDorkId));
                return _googleDork;
            }
        }

        public string Site
        {
            get
            {
                return _site;
            }
            set
            {
                if (_site == value) { return; }
                _site = value;
                Update();
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
                if (_keywords == value) { return; }
                _keywords = value;
                Update();
                NotifyPropertyChanged();
            }
        }

        public string GoogleUrl
        {
            get
            {
                var url = GoogleDorkViewModel.GoogleUrl;
                var keywords = Keywords ?? string.Empty;
                var site = Site ?? string.Empty;
                return url.Replace("??keywords??", keywords).Replace("??site??", site);
            }
        }

        private void Update()
        {
            var vulnerableSite =  Mapper.Map<GoogleDorkVulnerableSite>(this);
            vulnerableSite.Update();
            // ReSharper disable once ExplicitCallerInfoArgument
            NotifyPropertyChanged("GoogleUrl");
        }

        public void Delete()
        {
            var vulnerableSite = Mapper.Map<GoogleDorkVulnerableSite>(this);
            vulnerableSite.Delete();
        }
    }
}
