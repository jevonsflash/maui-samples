using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui.Controls.Compatibility;
using StickyTab;
using Region = MauiSample.Common.Common.Region;
namespace MauiSample.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorizontalPanContainer : ContentView
    {

        public event EventHandler<PitGrid> OnfinishedChoise;
        public event EventHandler<EventArgs> OnTapped;

        private bool isInPitPre = false;
        private bool isSwitch = false;

        private bool IsRuningTranslateToTask;

        public HorizontalPanContainer()
        {
            InitializeComponent();
            this.PropertyChanged += PanContainer_PropertyChanged1;
        }

        private IList<PitGrid> _pitLayout;

        public IList<PitGrid> PitLayout
        {
            get { return _pitLayout; }
            set { _pitLayout = value; }
        }

        private PitGrid _currentView;

        public PitGrid CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; }
        }



        public static readonly BindableProperty PositionXProperty =
 BindableProperty.Create("PositionX", typeof(double), typeof(HorizontalPanContainer), default(double), propertyChanged: (bindable, oldValue, newValue) =>
 {
     var obj = (HorizontalPanContainer)bindable;
     //obj.Content.TranslationX = obj.PositionX;
     obj.Content.TranslateTo(obj.PositionX, obj.PositionY, 0);

 });

        public double PositionX
        {
            get { return (double)GetValue(PositionXProperty); }
            set
            {
                SetValue(PositionXProperty, value);
                OnPropertyChanged();

            }
        }



        public static readonly BindableProperty PositionYProperty =
BindableProperty.Create("PositionY", typeof(double), typeof(HorizontalPanContainer), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (HorizontalPanContainer)bindable;
    obj.Content.TranslateTo(obj.PositionX, obj.PositionY, 0);
    //obj.Content.TranslationY = obj.PositionY;

});

        public double PositionY
        {
            get { return (double)GetValue(PositionYProperty); }
            set
            {
                SetValue(PositionYProperty, value);
                OnPropertyChanged();

            }
        }



        private async void PanContainer_PropertyChanged1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Content))
            {
                //await Content.TranslateTo(PositionX, PositionY, 0);
            }


        }

        private async void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var isInPit = false;

            switch (e.StatusType)
            {
                case GestureStatus.Started:

                    WeakReferenceMessenger.Default.Send<HorizontalPanActionArgs, string>(new HorizontalPanActionArgs(HorizontalPanType.Start, this.CurrentView), TokenHelper.PanAction);

                    break;
                case GestureStatus.Running:
                    var translationX =
                         PositionX + e.TotalX;
                    var translationY = PositionY;
                    PitGrid lastView = null;
                    if (PitLayout != null)
                    {
                        foreach (var item in PitLayout)
                        {
                            var pitRegion = new Region(item.X, item.X + item.Width, item.Y, item.Y + item.Height, item.PitName);
                            var isXin = (e.TotalX>0 && translationX >= pitRegion.StartX - Content.Width / 2 && pitRegion.StartX>this.Width/2)||
                              (e.TotalX<0 && translationX <= pitRegion.EndX - Content.Width / 2&&pitRegion.EndX<this.Width/2);
                            if (isXin)
                            {
                                isInPit = true;
                                if (this.CurrentView == item)
                                {
                                    isSwitch = false;
                                }
                                else
                                {
                                    if (this.CurrentView != null)
                                    {
                                        isSwitch = true;
                                        lastView = this.CurrentView;

                                    }

                                    this.CurrentView = item;

                                }

                            }
                        }

                    }
                    if (isInPit)
                    {

                        if (isSwitch)
                        {
                            WeakReferenceMessenger.Default.Send<HorizontalPanActionArgs, string>(new HorizontalPanActionArgs(HorizontalPanType.Out, lastView), TokenHelper.PanAction);
                            WeakReferenceMessenger.Default.Send<HorizontalPanActionArgs, string>(new HorizontalPanActionArgs(HorizontalPanType.In, this.CurrentView), TokenHelper.PanAction);
                            isSwitch = false;
                        }
                        if (!isInPitPre)
                        {
                            WeakReferenceMessenger.Default.Send<HorizontalPanActionArgs, string>(new HorizontalPanActionArgs(HorizontalPanType.In, this.CurrentView), TokenHelper.PanAction);
                            isInPitPre = true;


                        }
                    }
                    else
                    {


                        if (isInPitPre)
                        {
                            WeakReferenceMessenger.Default.Send<HorizontalPanActionArgs, string>(new HorizontalPanActionArgs(HorizontalPanType.Out, this.CurrentView), TokenHelper.PanAction);
                            isInPitPre = false;
                        }
                        this.CurrentView = null;

                    }
                    Content.TranslationX = translationX;
                    Content.TranslationY = translationY;
                    break;

                case GestureStatus.Completed:
                    double destinationX=0;
                    var view = this.CurrentView;

                    if (isInPitPre)
                    {
                        var pitRegion = new Region(view.X, view.X + view.Width, view.Y, view.Y + view.Height, view.PitName);

                        var prefix = pitRegion.StartX>this.Width/2 ? 1 : -1;
                      
                    }
                    else
                    {
                        destinationX=PositionX;

                    }


                    Content.AbortAnimation("ReshapeAnimations");

                    var parentAnimation = new Animation(v => Content.TranslationX = v, Content.TranslationX, destinationX);
                    parentAnimation.Commit(this, "RestoreAnimation", 16, finished: (d, b) =>
                    {
                        if (isInPitPre)
                        {

                            Content.TranslationX = PositionX;

                            if (view.IsEnable)
                            {
                                OnfinishedChoise?.Invoke(this, view);
                            }
                            // WeakReferenceMessenger.Default.Send("true");
                        }
                    });


                    WeakReferenceMessenger.Default.Send<HorizontalPanActionArgs, string>(new HorizontalPanActionArgs(HorizontalPanType.Over, this.CurrentView), TokenHelper.PanAction);

                    break;
            }

        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {

            this.OnTapped?.Invoke(this, EventArgs.Empty);
        }
    }

    public class HorizontalPanActionArgs
    {
        public HorizontalPanActionArgs(HorizontalPanType type, PitGrid pit = null)
        {
            PanType = type;
            CurrentPit = pit;
        }
        public HorizontalPanType PanType { get; set; }
        public PitGrid CurrentPit { get; set; }

    }

    public enum HorizontalPanType
    {
        Out, In, Over, Start
    }

}