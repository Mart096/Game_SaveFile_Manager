using System.Deployment.Application;

namespace Games_SaveFiles_Manager.ViewModels
{
    class AboutViewModel : ViewModelBase
    {
        #region Fields
        private string _versionString = "None";
        #endregion

        #region Properties
        public string VersionString
        {
            get { return _versionString; }
            set
            {
                if( _versionString != value)
                {
                    _versionString = value;
                    OnPropertyChange("VersionString");
                }
            }
        }
        #endregion

        #region Events
        #endregion

        #region Constructors

        public AboutViewModel()
        {
            Get_App_Version();
        }

        #endregion

        #region Methods

        public void Get_App_Version()
        {
            string version = null;

            try
            {
                version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch (InvalidDeploymentException)
            {
                //MessageBox.Show("Failed to read application's version!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                version = "1.0.0.1";
            }
            finally
            {
                VersionString = $"Version {version}";
            }
        }

        #endregion

    }
}
