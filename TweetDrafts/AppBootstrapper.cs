using Caliburn.Micro;
using System.Windows;
using TweetDrafts.ViewModels;

namespace TweetDrafts
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}

