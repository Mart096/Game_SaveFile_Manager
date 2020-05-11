using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Deployment.Application;

namespace Games_SaveFiles_Manager
{
    /// <summary>
    /// Logika interakcji dla klasy AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            Get_App_Version();
        }

        public void Get_App_Version()
        {
            string version = null;

            try
            {
                version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch (InvalidDeploymentException)
            {
                MessageBox.Show("Failed to read application's version!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                version = "unknown";
            }
            finally
            {
                version_label.Content = version;
            }
        }
    }
}
