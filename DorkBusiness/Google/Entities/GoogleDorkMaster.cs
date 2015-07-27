using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using DorkBusiness.Google.Enumerations;
using DorkDataAccess;

namespace DorkBusiness.Google.Entities
{
    public class GoogleDorkMaster
    {
        private List<FullGoogleDork> _fullGoogleDorks;
        private List<FullGoogleDork> FullGoogleDorks
        {
            get
            {
                if (_fullGoogleDorks != null)
                {
                    return _fullGoogleDorks;
                }
                using (var context = new DorkDatabaseContext())
                {
                    _fullGoogleDorks = context.FullGoogleDorks.OrderByDescending(x => x.GoogleDorkParentId).ThenByDescending(x => x.DiscoveryDate).ToList();
                }
                return _fullGoogleDorks;
            }
        }

        public List<GoogleDorkParent> GetGoogleDorksForSite(string site)
        {
            var googleDorkParents = new List<GoogleDorkParent>();

            if (string.IsNullOrEmpty(site))
            {
                return googleDorkParents;
            }

            var currentId = -1;
            var googleDorkParent = new GoogleDorkParent();
            foreach (var fullGoogleDork in FullGoogleDorks)
            {
                var newId = fullGoogleDork.GoogleDorkParentId;

                if (currentId != newId)
                {
                    currentId = newId;
                    googleDorkParent =
                        new GoogleDorkParent
                        {
                            Id = newId,
                            Name = fullGoogleDork.GoogleDorkParentName,
                            GoogleDorks = new List<GoogleDork>()
                        };

                    googleDorkParents.Add(googleDorkParent);
                }

                googleDorkParent.GoogleDorks.Add(
                    new GoogleDork
                    {
                        DiscoveryDate = fullGoogleDork.DiscoveryDate ?? default(DateTime),
                        GhdbUrl = fullGoogleDork.GhdbUrl,
                        GoogleDorkParentId = fullGoogleDork.GoogleDorkParentId,
                        Summary = fullGoogleDork.Summary,
                        GoogleUrl = fullGoogleDork.GoogleUrl.Replace(" ??site??", site)
                    });
            }

            return googleDorkParents;
        }

        public List<GoogleDorkParent> SearchGoogleDorks(string site, string keywords)
        {
            using (var context = new DorkDatabaseContext())
            {
                var googleDorkParentIds = context.GoogleDorkParents.Select(x => x.Id).ToList();
                return SearchGoogleDorks(site, keywords, googleDorkParentIds);
            }
        }

        public List<FullGoogleDork> GetFullGoogleDorks(List<int> googleDorkParentIds)
        {
            googleDorkParentIds = googleDorkParentIds ?? new List<int>();
            googleDorkParentIds.Add(-1);
            return FullGoogleDorks.Where(x => googleDorkParentIds.Contains(x.GoogleDorkParentId)).ToList();
        }

        public List<GoogleDorkParent> SearchGoogleDorks(string site, string keywords, List<int> googleDorkParentIds)
        {
            var googleDorkParents = new List<GoogleDorkParent>();
            var fullGoogleDorks = GetFullGoogleDorks(googleDorkParentIds);

            var currentId = -1;
            var googleDorkParent = new GoogleDorkParent();

            foreach (var fullGoogleDork in fullGoogleDorks)
            {
                var newId = fullGoogleDork.GoogleDorkParentId;
                if (currentId != newId)
                {
                    currentId = newId;
                    googleDorkParent =
                        new GoogleDorkParent
                            {
                                Id = newId,
                                Name = fullGoogleDork.GoogleDorkParentName,
                                GoogleDorks = new List<GoogleDork>()
                            };

                    googleDorkParents.Add(googleDorkParent);
                }

                googleDorkParent.GoogleDorks.Add(
                    new GoogleDork
                        {
                            DiscoveryDate = fullGoogleDork.DiscoveryDate ?? default(DateTime),
                            GhdbUrl = fullGoogleDork.GhdbUrl,
                            GoogleDorkParentId = fullGoogleDork.GoogleDorkParentId,
                            Summary = fullGoogleDork.Summary,
                            GoogleUrl = GenerateGoogleUrl(fullGoogleDork, keywords, site)
                        });
            }
            
            return googleDorkParents;
        }

        public List<GoogleDorkParent> GetGoogleDorkParents(GoogleDorkParentSort googleDorkParentSort)
        {
            var googleDorkParents = new List<GoogleDorkParent>();

            using (var context = new DorkDatabaseContext())
            {
                var googleDorksFromDb = context.GoogleDorkParents.OrderBy(googleDorkParentSort.ToString());
                foreach (var googleDorkFromDb in googleDorksFromDb)
                {
                    googleDorkParents.Add(
                        new GoogleDorkParent
                        {
                            Id = googleDorkFromDb.Id,
                            Name = googleDorkFromDb.Name,
                            GoogleDorks = new List<GoogleDork>()
                        });
                }
            }

            return googleDorkParents;
        }

        private static string GenerateGoogleUrl(FullGoogleDork fullGoogleDork, string keywords, string site)
        {
            var googleUrl = (!string.IsNullOrEmpty(site)) ? fullGoogleDork.GoogleUrl.Replace("??site??", site) : fullGoogleDork.GoogleUrl.Replace("site:??site??", string.Empty);
            return (!string.IsNullOrEmpty(keywords)) ? googleUrl.Replace("??keywords??", keywords) : googleUrl.Replace("??keywords?? ", string.Empty);
        }
    }
}