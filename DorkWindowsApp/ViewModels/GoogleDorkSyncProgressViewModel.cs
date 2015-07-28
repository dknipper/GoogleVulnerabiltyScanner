using System;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkSyncProgressViewModel : BaseViewModel
    {
        private int _processedNumber;
        private double _percentageComplete;
        private string _googleDorkParentName;
        private DateTime _date;
        private string _title;
        private string _summary;
        private string _ghdbUrl;

        public double MinimumPercentComplete
        {
            get { return 0.0d; }
        }

        public double MaximumPercentComplete
        {
            get { return 100.0d; }
        }

        public int ProcessedNumber
        {
            get
            {
                return _processedNumber;
            }
            set
            {
                if (_processedNumber == value) { return; }
                _processedNumber = value;
                NotifyPropertyChanged();
            }
        }

        public double PercentageComplete
        {
            get
            {
                return _percentageComplete;
            }
            set
            {
                if (_percentageComplete.Equals(value)) { return; }
                _percentageComplete = value;
                NotifyPropertyChanged();
            }
        }

        public string GoogleDorkParentName
        {
            get
            {
                return _googleDorkParentName;
            }
            set
            {
                if (_googleDorkParentName == value) { return; }
                _googleDorkParentName = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date == value) { return; }
                _date = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title == value) { return; }
                _title = value;
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
    }
}
