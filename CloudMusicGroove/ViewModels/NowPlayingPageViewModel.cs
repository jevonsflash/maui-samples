using CommunityToolkit.Mvvm.Input;
using MatoMusic.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.ViewModels
{

    public class NowPlayingPageViewModel : MusicRelatedViewModel
    {

        public NowPlayingPageViewModel()
        {
            this.SwitchPannelCommand = new RelayCommand(SwitchPannelAction, () => true);
            IsLrcPanel = false;
        }

        private void SwitchPannelAction()
        {
            IsLrcPanel = !IsLrcPanel;
        }

        private bool _isLrcPanel;

        public bool IsLrcPanel
        {
            get { return _isLrcPanel; }
            set
            {
                if (_isLrcPanel != value)
                {
                    _isLrcPanel = value;
                    OnPropertyChanged();


                }
            }
        }

        public RelayCommand SwitchPannelCommand { get; set; }

    }
}
