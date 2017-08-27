
namespace foreingchangemvvmmac.ViewModels
{
    using System;
    using Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    public class MainViewModel
    {
        
        #region Properties

        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }

        public Rate SourceRate
        {
            get;
            set;
       }

        public Rate TragetRate
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public string Result
        {
            get;
            set;
        }

        #endregion


        #region Comandos

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }

        }

        void Convert()
        {
            throw new NotImplementedException();
        }

        #endregion

        public MainViewModel()
        {
            
        }
    }
}
