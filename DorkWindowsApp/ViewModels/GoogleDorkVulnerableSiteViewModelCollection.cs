using System.Collections.ObjectModel;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkVulnerableSiteViewModelCollection : ObservableCollection<GoogleDorkVulnerableSiteViewModel>
    {
        public new bool Remove(GoogleDorkVulnerableSiteViewModel item)
        {
            item.Delete();
            base.Remove(item);
            return false;
        }
    }
}
