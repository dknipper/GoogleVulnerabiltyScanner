using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DorkBusiness.Google.Enumerations;
using DorkBusiness.Utilities;
using DorkDataAccess;
using HtmlAgilityPack;
using GoogleDork = DorkBusiness.Google.Entities.GoogleDork;
using GoogleDorkParent = DorkBusiness.Google.Entities.GoogleDorkParent;
using System.Linq.Dynamic;
using DorkBusiness.Google.Entities;

namespace DorkBusiness.Google.Utilities
{
    public class GoogleDorkUtilities
    {
        public delegate void GoogleDorkSyncProgressChangeHandler(object sender, GoogleDorkSyncProgressChangeEventArgs e);
        public event GoogleDorkSyncProgressChangeHandler OnGoogleDorkSyncProgressChange;

        public List<GoogleDorkParent> GetGoogleDorksFromGhdb()
        {
            var googleDorkTotal = GetGoogleDorkTotals();

            if (googleDorkTotal == 0)
            {
                return null;
            }

            var processedDorks = 0;

            var googleDorkParents = GetGoogleDorkParentsFromDatabase(GoogleDorkParentSort.Id);
            var client = new DorkWebClient();

            foreach (var googleDorkParent in googleDorkParents)
            {
                var url = string.Format("{0}ghdb/?function=summary&cat={1}", AppSettings.Config.GhdbHomePage, googleDorkParent.Id);
                var downloadString = client.DownloadString(url);

                var doc = new HtmlDocument();
                doc.LoadHtml(downloadString);

                if (doc.DocumentNode.SelectSingleNode("//td") == null)
                {
                    continue;
                }

                var table = doc.DocumentNode.SelectSingleNode("//table");

                if (table == null)
                {
                    continue;
                }
                
                var tableRows = table.SelectNodes("./tr");
                var firstRow = true;
                foreach (var tr in tableRows)
                {
                    if (firstRow)
                    {
                        firstRow = false;
                        continue;
                    }
                    ProcessDork(tr, googleDorkParent, client, googleDorkTotal, ref processedDorks);
                }
            }

            if (OnGoogleDorkSyncProgressChange == null)
            {
                return googleDorkParents;
            }

            const string allDone = "All Done!";
            var processedItem =
                new GoogleDorkSyncProgress
                {
                    Date = DateTime.Now,
                    GhdbUrl = allDone,
                    GoogleDorkParentName = allDone,
                    ProcessedNumber = processedDorks,
                    Summary = allDone,
                    Title = allDone,
                    PercentageComplete = 100
                };

            var args = new GoogleDorkSyncProgressChangeEventArgs(processedItem);
            OnGoogleDorkSyncProgressChange(this, args);

            return googleDorkParents;
        }

        private void ProcessDork(HtmlNode tr, GoogleDorkParent googleDorkParent, DorkWebClient client, int googleDorkTotal, ref int processedDorks)
        {
            var dork =
                new GoogleDork
                    {
                        GoogleDorkParentId = googleDorkParent.Id
                    };

            var currentIndex = 0;

            var tableRowCells = tr.SelectNodes("./td");
            if (tableRowCells == null)
            {
                return;
            }
            
            foreach (var td in tableRowCells)
            {
                switch (currentIndex)
                {
                    case ((int)GoogleDorkColumnIndex.Date):
                        {
                            DateTime tempDate;
                            tempDate = DateTime.TryParse(td.InnerText, out tempDate) ? tempDate : new DateTime(2000, 1, 1);
                            dork.DiscoveryDate = tempDate;
                            break;
                        }
                    case ((int)GoogleDorkColumnIndex.Summary):
                        {
                            dork.Summary = HttpUtility.HtmlDecode(td.InnerText);
                            break;
                        }
                    case ((int)GoogleDorkColumnIndex.Link):
                        {
                            var link = td.SelectSingleNode("./a");

                            if ((link != null) && (link.Attributes != null))
                            {
                                var linkAttribute = link.Attributes.FirstOrDefault(x => x.Name.ToLower() == "href");
                                if (linkAttribute != null)
                                {
                                    dork.GhdbUrl = HttpUtility.HtmlDecode(string.Format("{0}{1}", AppSettings.Config.GhdbHomePage, linkAttribute.Value.TrimStart('/')));
                                }
                            }
                            break;
                        }
                }
                currentIndex++;
            }

            var ghdbPage = client.DownloadString(dork.GhdbUrl);

            var ghdb = new HtmlDocument();
            ghdb.LoadHtml(ghdbPage);

            var h2 = ghdb.DocumentNode.SelectSingleNode("//td");

            if ((h2 != null) && (h2.SelectSingleNode("./a") != null))
            {
                var link = h2.SelectSingleNode("./a");

                if ((link != null) && (link.Attributes != null))
                {
                    var linkAttribute = link.Attributes.FirstOrDefault(x => x.Name.ToLower() == "href");
                    if (linkAttribute != null)
                    {
                        dork.GoogleUrl = HttpUtility.HtmlDecode(linkAttribute.Value);
                    }
                }
            }

            if (string.IsNullOrEmpty(dork.GoogleUrl))
            {
                return;
            } 
            
            dork.GoogleUrl = dork.GoogleUrl.Replace("q=", "q=??keywords?? ");
            if ((!dork.GoogleUrl.Contains("site:") && dork.GoogleUrl.Contains("q=")) == false)
            {
                return;
            }
            
            dork.GoogleUrl = dork.GoogleUrl.Replace("q=", "q=(");
            var indexOfq = dork.GoogleUrl.IndexOf("q=(", StringComparison.OrdinalIgnoreCase);

            if (indexOfq == -1)
            {
                return;
            }

            var indexOfamp = dork.GoogleUrl.IndexOf("&", indexOfq, StringComparison.OrdinalIgnoreCase);

            dork.GoogleUrl =
                (indexOfamp != -1)
                    ? dork.GoogleUrl.Insert(indexOfamp, ") site:??site??")
                    : string.Format("{0}) site:??site??", dork.GoogleUrl);

            var dorkUri = new Uri(dork.GoogleUrl);
            var dorkParams = HttpUtility.ParseQueryString(dorkUri.Query);

            var badParameters =
                new List<string>
                    {
                        "hl","lr","cr","gl","gr","gcs","gpc","gm","btnG","rls","source"
                    };

            var goodParameters =
                new Dictionary<string,string>
                    {
                        {"nfpr", "1"},{"pws", "0"},{"complete", "0"},{"safe", "off"}
                    };

            dorkParams = QueryString.RemoveParameters(dorkParams, badParameters);
            dorkParams = QueryString.AddParameters(dorkParams, goodParameters);

            dork.GoogleUrl = string.Format("{0}://{1}{2}?{3}", dorkUri.Scheme, dorkUri.Authority, dorkUri.AbsolutePath, HttpUtility.UrlDecode(dorkParams.ToString()));
            var dorkExists = googleDorkParent.GoogleDorks.Any(x => x.GoogleUrl == dork.GoogleUrl);

            if (!dorkExists)
            {
                googleDorkParent.GoogleDorks.Add(dork);
            }

            if (OnGoogleDorkSyncProgressChange == null)
            {
                return;
            }

            processedDorks++;

            var processedItem =
                new GoogleDorkSyncProgress
                    {
                        Date = dork.DiscoveryDate,
                        GhdbUrl = dork.GhdbUrl,
                        GoogleDorkParentName = googleDorkParent.Name,
                        ProcessedNumber = processedDorks,
                        Summary = dork.Summary,
                        Title = dork.GoogleUrl,
                        PercentageComplete = ((processedDorks/(double)googleDorkTotal)*100.00)
                    };

            var args = new GoogleDorkSyncProgressChangeEventArgs(processedItem);
            OnGoogleDorkSyncProgressChange(this, args);
        }

        public List<GoogleDorkParent> GetGoogleDorksFromDatabase(string site)
        {
            var googleDorkParents = new List<GoogleDorkParent>();

            if (string.IsNullOrEmpty(site))
            {
                return googleDorkParents;
            }

            using (var context = new DorkDatabaseContext())
            {
                var fullGoogleDorks = context.FullGoogleDorks.OrderByDescending(x => x.GoogleDorkParentId).ThenByDescending(x => x.DiscoveryDate);

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
                            GoogleUrl = fullGoogleDork.GoogleUrl.Replace(" ??site??", site)
                        });
                }
            }

            return googleDorkParents;
        }

        public void SyncGoogleDorks()
        {
            var googleDorkParents = GetGoogleDorksFromGhdb();

            using (var context = new DorkDatabaseContext())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM GoogleDork");

                // ReSharper disable LoopCanBePartlyConvertedToQuery
                foreach (var googleDorkParent in googleDorkParents)
                {
                    foreach (var googleDork in googleDorkParent.GoogleDorks)
                    {
                        var newGoogleDorkDbRecord =
                            new DorkDataAccess.GoogleDork
                                {
                                    DiscoveryDate = googleDork.DiscoveryDate,
                                    GhdbUrl = googleDork.GhdbUrl,
                                    GoogleDorkParentId = googleDorkParent.Id,
                                    GoogleUrl = googleDork.GoogleUrl,
                                    Summary = googleDork.Summary
                                };

                        context.GoogleDorks.Add(newGoogleDorkDbRecord);
                    }
                }
                context.SaveChanges();
            }
        }

        public List<GoogleDorkParent> GetGoogleDorkParentsFromGhdb()
        {
            var client = new DorkWebClient();
            var rtrn = new List<GoogleDorkParent>();

            var downloadString = client.DownloadString(string.Format("{0}ghdb/", AppSettings.Config.GhdbHomePage));

            var maindoc = new HtmlDocument();
            maindoc.LoadHtml(downloadString);

            foreach (var node in maindoc.DocumentNode.SelectNodes("//body//a").Where(x => x.Attributes.Contains("href")))
            {
                var ghdbLinkUri = string.Format("{0}{1}", AppSettings.Config.GhdbHomePage, node.Attributes["href"].Value.TrimStart('/'));
                if (!Uri.IsWellFormedUriString(ghdbLinkUri, UriKind.Absolute))
                {
                    continue;
                }

                var aUri = new Uri(ghdbLinkUri, UriKind.Absolute);
                var queryString = HttpUtility.ParseQueryString(aUri.Query);
                var cat = queryString["cat"];
                if (string.IsNullOrEmpty(cat))
                {
                    continue;
                }

                int index;
                Int32.TryParse(cat, out index);

                if (index == 0)
                {
                    continue;
                }

                rtrn.Add(
                    new GoogleDorkParent
                        {
                            Id = index,
                            Name = HttpUtility.HtmlDecode(node.InnerText)
                        });
            }

            return rtrn;
        }

        public List<GoogleDorkParent> GetGoogleDorkParentsFromDatabase(GoogleDorkParentSort googleDorkParentSort)
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

        public void SyncGoogleDorkParents()
        {
            var googleDorkParents = GetGoogleDorkParentsFromGhdb();

            using (var context = new DorkDatabaseContext())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM GoogleDorkParent");

                // ReSharper disable once LoopCanBePartlyConvertedToQuery
                foreach (var googleDorkParent in googleDorkParents)
                {
                    var newGoogleDorkDbRecord =
                        new DorkDataAccess.GoogleDorkParent
                            {
                                Id = googleDorkParent.Id,
                                Name = googleDorkParent.Name
                            };

                    context.GoogleDorkParents.Add(newGoogleDorkDbRecord);
                }
                context.SaveChanges();
            }
        }

        public List<GoogleDorkParent> SearchGoogleDorks(string site, string keywords, List<int> googleDorkParentIds)
        {
            googleDorkParentIds = googleDorkParentIds ?? new List<int>();
            googleDorkParentIds.Add(-1);

            var googleDorkParents = new List<GoogleDorkParent>();

            using (var context = new DorkDatabaseContext())
            {
                var fullGoogleDorks =
                    from t in context.FullGoogleDorks
                    where googleDorkParentIds.Contains(t.GoogleDorkParentId)
                    orderby t.GoogleDorkParentId descending, t.DiscoveryDate descending
                    select t;

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

                    var googleUrl = (!string.IsNullOrEmpty(site)) ? fullGoogleDork.GoogleUrl.Replace("??site??", site) : fullGoogleDork.GoogleUrl.Replace("site:??site??", string.Empty);
                    googleUrl = (!string.IsNullOrEmpty(keywords)) ? googleUrl.Replace("??keywords??", keywords) : googleUrl.Replace("??keywords?? ", string.Empty);

                    googleDorkParent.GoogleDorks.Add(
                        new GoogleDork
                            {
                                DiscoveryDate = fullGoogleDork.DiscoveryDate ?? default(DateTime),
                                GhdbUrl = fullGoogleDork.GhdbUrl,
                                GoogleDorkParentId = fullGoogleDork.GoogleDorkParentId,
                                Summary = fullGoogleDork.Summary,
                                GoogleUrl = googleUrl
                            });
                }
            }
            return googleDorkParents;
        }

        public int GetGoogleDorkTotals()
        {
            var client = new DorkWebClient();
            var rtrn = 0;
            var downloadString = client.DownloadString(string.Format("{0}ghdb/", AppSettings.Config.GhdbHomePage));

            var matchCollection = Regex.Matches(downloadString, @"\([^\d]*(\d+)[^\d]* entries\)");
            foreach (Match match in matchCollection)
            {
                var matchString = new string(match.ToString().Where(char.IsDigit).ToArray());
                int tryMe;
                int.TryParse(matchString, out tryMe);
                rtrn += tryMe;
            }
            return rtrn;
        }
    }
}