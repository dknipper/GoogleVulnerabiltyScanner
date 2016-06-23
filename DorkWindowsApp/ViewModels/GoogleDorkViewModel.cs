using System;
using System.Collections.ObjectModel;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkViewModel : BaseViewModel
    {
        private int _googleDorkParentId;
        private string _googleUrl;
        private bool _visibleOnTree;
        private bool _isSelected;
        private string _summary;
        private string _ghdbUrl;
        private DateTime? _discoveryDate;
        private ObservableCollection<GoogleDorkVulnerableSiteViewModel> _googleDorkVulnerableSites;

        public ObservableCollection<GoogleDorkVulnerableSiteViewModel> GoogleDorkVulnerableSites
        {
            get
            {
                return _googleDorkVulnerableSites;
            }
            set
            {
                if (_googleDorkVulnerableSites == value) { return; }
                _googleDorkVulnerableSites = value;
                NotifyPropertyChanged();
            }
        }

        public int GoogleDorkParentId
        {
            get
            {
                return _googleDorkParentId;
            }
            set
            {
                if (_googleDorkParentId == value) { return; }
                _googleDorkParentId = value;
                NotifyPropertyChanged();
            }
        }

        public string GoogleUrl
        {
            get
            {
                return _googleUrl;
            }
            set
            {
                if (_googleUrl == value) { return; }
                _googleUrl = value;
                NotifyPropertyChanged();
            }
        }

        public bool VisibleOnTree
        {
            get
            {
                return _visibleOnTree;
            }
            set
            {
                if (_visibleOnTree == value) { return; }
                _visibleOnTree = value;
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
                if (value == _isSelected) { return; }
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }
        public string Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                if (_summary == value) { return; }
                _summary = value;
                NotifyPropertyChanged();
            }
        }

        public string GhdbUrl
        {
            get
            {
                return _ghdbUrl;
            }
            set
            {
                if (_ghdbUrl == value) { return; }
                _ghdbUrl = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime? DiscoveryDate
        {
            get
            {
                return _discoveryDate;
            }
            set
            {
                if (_discoveryDate == value) { return; }
                _discoveryDate = value;
                NotifyPropertyChanged();
            }
        }
    }
}