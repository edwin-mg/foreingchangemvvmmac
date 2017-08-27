
namespace foreingchangemvvmmac.ViewModels
{
    using System;
    using Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.ComponentModel;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Xamarin.Forms;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        bool _IsRunning;
        bool _IsEnabled;
        string _Result;
        ObservableCollection<Rate> _rates;
        #endregion


        #region Properties

        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
			get
			{
				return _rates;
			}
			set
			{
				if (_rates != value)
				{
					_rates = value;
					PropertyChanged?.Invoke(
						this, new PropertyChangedEventArgs(nameof(Rates)));
				}
			}
        }

        public Rate SourceRate
        {
            get;
            set;
        }

        public Rate TargetRate
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get
            {
                return _IsRunning;
            }
            set
            {
                if (_IsRunning != value)
                {
                    _IsRunning = value;
                    PropertyChanged?.Invoke(
                        this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
			get
			{
				return _IsEnabled;
			}
			set
			{
				if (_IsEnabled != value)
				{
					_IsEnabled = value;
					PropertyChanged?.Invoke(
						this, new PropertyChangedEventArgs(nameof(IsEnabled)));
				}
			}
        }

        public string Result
        {
			get
			{
				return _Result;
			}
			set
			{
				if (_Result != value)
				{
					_Result = value;
					PropertyChanged?.Invoke(
						this, new PropertyChangedEventArgs(nameof(Result)));
				}
			}

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

        async void Convert()
        {
            if (string.IsNullOrEmpty(Amount))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must must enter a value in amount",
                    "Acept");
                return;
            }

            decimal amount = 0;

            if (!decimal.TryParse(Amount, out amount))
            {
				await Application.Current.MainPage.DisplayAlert(
				   "Error",
				   "You must must enter a numeric value in amount",
				   "Acept");
				return;
            }

            if (SourceRate == null)
			{
				await Application.Current.MainPage.DisplayAlert(
				   "Error",
				   "You must select a source rate",
				   "Acept");
				return;
			}

			if (TargetRate == null)
			{
				await Application.Current.MainPage.DisplayAlert(
				   "Error",
				   "You must select a target rate",
				   "Acept");
				return;
			}

            var amountConverted = amount / 
                                   (decimal)SourceRate.TaxRate * 
                                   (decimal)TargetRate.TaxRate;
            Result = string.Format("{0} {1:C2} = {2} {3:C2}", 
                                   SourceRate.Code, 
                                   amount, 
                                   TargetRate.Code, 
                                   amountConverted);
        }

        #endregion


        #region Constructors
        public MainViewModel()
        {
            LoadRates();
        }
        #endregion


        #region Methods

            async void LoadRates()
        {
            IsRunning = true;
            Result = "Loading rate...";

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new 
                    Uri("http://apiexchangerates.azurewebsites.net");
                var controller = "/api/Rates";
                var responce = await client.GetAsync(controller);
                var result = await responce.Content.ReadAsStringAsync();

                if (!responce.IsSuccessStatusCode)
                {
                    IsRunning = false;
                    Result = result;
                }

                var rates = JsonConvert.DeserializeObject<List<Rate>>(result);
                Rates = new ObservableCollection<Rate>(rates);
                IsRunning = false;
                IsEnabled = true;
                result = "Ready to convert...";
            }
            catch (Exception ex)
            {
                IsRunning = false;
				Result = ex.Message;
            }
        }

        #endregion

    }
}
