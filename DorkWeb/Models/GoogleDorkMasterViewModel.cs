using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DorkWeb.DorkServiceReference;

namespace DorkWeb.Models
{
    public class GoogleDorkMasterViewModel
    {
        private List<GoogleDorkParentViewModel> _googleDorkParentViewModels;
        public List<GoogleDorkParentViewModel> GoogleDorkParentViewModels
        {
            get
            {
                _googleDorkParentViewModels = _googleDorkParentViewModels ?? new List<GoogleDorkParentViewModel>();
                return _googleDorkParentViewModels;
            }
            set
            {
                _googleDorkParentViewModels = value;
            }
        }

        private List<string> _googleDorkParentValues;
        public List<string> GoogleDorkParentValues
        {
            get
            {
                _googleDorkParentValues = _googleDorkParentValues ?? new List<string>();
                return _googleDorkParentValues;
            }
            set
            {
                _googleDorkParentValues = value;
            }
        }

        public List<int> GoogleDorkParentIds
        {
            get
            {
                return GoogleDorkParentValues.Select(parent => Convert.ToInt32(parent)).ToList();
            }
        }

        private MultiSelectList _googleDorkParentList;
        public MultiSelectList GoogleDorkParentList
        {
            get
            {
                if (_googleDorkParentList != null)
                {
                    return _googleDorkParentList;
                }

                var googleDorkParents = new List<GoogleDorkParentViewModel>();

                using (var proxy = new DorkServiceClient("BasicHttpBinding_IDorkService"))
                {
                    var googleParents = proxy.GetGoogleDorkParents(GoogleDorkParentSort.Name);
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var parent in googleParents)
                    {
                        googleDorkParents.Add(
                            new GoogleDorkParentViewModel
                            {
                                Id = parent.Id,
                                Name = parent.Name
                            });
                    }
                }
                _googleDorkParentList = new MultiSelectList(googleDorkParents, "ID", "Name", null);
                return _googleDorkParentList;
            }
            set
            {
                _googleDorkParentList = value;
            }
        }


        private string _siteToSearch;
        public string SiteToSearch
        {
            get
            {
                _siteToSearch = _siteToSearch ?? string.Empty;
                return _siteToSearch;
            }
            set
            {
                _siteToSearch = value;
            }
        }

        private string _keywords;
        public string Keywords
        {
            get
            {
                _keywords = _keywords ?? string.Empty;
                return _keywords;
            }
            set
            {
                _keywords = value;
            }
        }
    }
}