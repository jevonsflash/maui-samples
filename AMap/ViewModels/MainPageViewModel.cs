using CommunityToolkit.Mvvm.ComponentModel;
using MauiSample.Common.Helper;
using MauiSample.Common.ThrottleDebounce;
using Nito.AsyncEx;
using System.Collections.ObjectModel;
using Amap;
using AMap.Location;
using Poi = AMap.Location.Poi;

namespace AMap.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {


        private readonly AmapHttpRequestClient amapHttpRequestClient;
        public event EventHandler<FinishedChooiseEvenArgs> OnFinishedChooise;
        private static AsyncLock asyncLock = new AsyncLock();
        public static RateLimitedAction throttledAction = Debouncer.Debounce(null, TimeSpan.FromMilliseconds(1500), leading: false, trailing: true);
        public MainPageViewModel()
        {
            Search = new Command(SearchAction);
            Done = new Command(DoneAction);
            Remove = new Command(RemoveAction);
            this.amapHttpRequestClient=new AmapHttpRequestClient();
            PropertyChanged+=MainPageViewModel_PropertyChanged;
        }

        private void RemoveAction(object obj)
        {
            this.Address=null;
            this.CurrentLocation=null;
            OnFinishedChooise?.Invoke(this, new FinishedChooiseEvenArgs(Address, CurrentLocation));
        }

        private void DoneAction(object obj)
        {
            OnFinishedChooise?.Invoke(this, new FinishedChooiseEvenArgs(Address, CurrentLocation));

        }

        private void SearchAction(object obj)
        {
            Init();
        }


        private async void MainPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(SearchKeywords))
            {
                if (string.IsNullOrEmpty(SearchKeywords))
                {
                    Init();
                }
            }

            else if (e.PropertyName == nameof(CurrentLocation))
            {
                if (CurrentLocation!=null)
                {

                    // 使用防抖
                    using (await asyncLock.LockAsync())
                    {

                        var amapLocation = new Location.Location()
                        {
                            Latitude=CurrentLocation.Latitude,
                            Longitude=CurrentLocation.Longitude
                        };
                        var amapInverseHttpRequestParamter = new AmapInverseHttpRequestParamter()
                        {
                            Locations= new Location.Location[] { amapLocation }
                        };
                        ReGeocodeLocation reGeocodeLocation = null;
                        try
                        {
                            reGeocodeLocation = await amapHttpRequestClient.InverseAsync(amapInverseHttpRequestParamter);
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine(ex.ToString());
                        }

                        throttledAction.Update(() =>
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                CurrentLocation=amapLocation;
                                if (reGeocodeLocation!=null)
                                {
                                    Address = reGeocodeLocation.Address;
                                    Pois=new ObservableCollection<Poi>(reGeocodeLocation.Pois);

                                }
                            });
                        });
                        throttledAction.Invoke();
                    }
                }
            }


        }

        public async void Init()
        {
            var location = await GeoLocationHelper.GetNativePosition();
            if (location==null)
            {
                return;
            }
            var amapLocation = new Location.Location()
            {
                Latitude=location.Latitude,
                Longitude=location.Longitude
            };
            CurrentLocation=amapLocation;

        }

        private Location.Location _currentLocation;

        public Location.Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {

                if (_currentLocation != value)
                {
                    if (value!=null &&_currentLocation!=null&&Location.Location.CalcDistance(value, _currentLocation)<100)
                    {
                        return;
                    }

                    _currentLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Poi> _pois;

        public ObservableCollection<Poi> Pois
        {
            get { return _pois; }
            set
            {
                _pois = value;
                OnPropertyChanged();
            }
        }

        private Poi _selectedPoi;

        public Poi SelectedPoi
        {
            get { return _selectedPoi; }
            set
            {
                _selectedPoi = value;
                OnPropertyChanged();

            }
        }


        private string _searchKeywords;

        public string SearchKeywords
        {
            get { return _searchKeywords; }
            set
            {
                _searchKeywords = value;
                OnPropertyChanged();

            }
        }

        public Command Search { get; set; }
        public Command Done { get; set; }
        public Command Remove { get; set; }

    }

}
