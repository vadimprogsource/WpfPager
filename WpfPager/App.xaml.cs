using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Mvc;

namespace WpfPager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static ServiceHost m_host;



        public App()
        {
            FrontController.Location = "/WpfPager;component/view";
            FrontController.Culture = "en";
            FrontController.Desktop = new Window();

            Controller.PagerController.StartUp(@"ui.config.xml");

            var controller = new Controller.PagerController();

            m_host = new ServiceHost(typeof(Controller.PagerController), new Uri(Controller.PagerController.From.Uri));
            m_host.Open();
            controller.Main();

            (FrontController.Desktop as Window).Show();

        }
    }
}
