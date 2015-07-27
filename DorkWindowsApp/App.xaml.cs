
using System.Windows;
using AutoMapper;
using DorkBusiness.Google.Entities;
using DorkWindowsApp.ViewModels;

namespace DorkWindowsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Mapper.CreateMap<GoogleDorkViewModel, GoogleDork>();
            Mapper.CreateMap<GoogleDork, GoogleDorkViewModel>();

            Mapper.CreateMap<GoogleDorkParentViewModel, GoogleDorkParent>();
            Mapper.CreateMap<GoogleDorkParent, GoogleDorkParentViewModel>();
        }
    }
}
