using CommunityToolkit.Mvvm.ComponentModel;
using MauiSample.Common.Helper;
using MauiSample.Common.ThrottleDebounce;
using Nito.AsyncEx;
using System.Collections.ObjectModel;

namespace Editorjs.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        public MainPageViewModel()
        {
            PropertyChanged+=UserProfilePageViewModel_PropertyChanged;
            Init();

        }

        private async void Init()
        {

            this.Theme="Light";


        }


        private void UserProfilePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Theme))
            {
                if (!string.IsNullOrEmpty(Theme))
                {
                    if (Theme=="Dark")
                    {
                        Application.Current.UserAppTheme = AppTheme.Dark;
                    }
                    else if (Theme=="Light")
                    {
                        Application.Current.UserAppTheme = AppTheme.Light;
                    }
                    else
                    {
                        Application.Current.UserAppTheme = AppTheme.Unspecified;
                    }

                }
            }

          
        }

        private string _theme;


        public string Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                OnPropertyChanged();

            }
        }



    }

}
