using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Helper;
using Microsoft.Maui.Controls.Compatibility;
using Region = MauiSample.Common.Common.Region;
namespace MauiSample.Common.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PanContainer : ContentView
    {

        public event EventHandler<PitGrid> OnfinishedChoise;
        public event EventHandler<EventArgs> OnTapped;

        private bool isInPitPre = false;
        private bool isSwitch = false;

        private bool IsRuningTranslateToTask;

        public PanContainer()
        {
            InitializeComponent();
            this.PanScale = 0.5;
            this.PanScaleAnimationLength = 100;
            this.Orientation=ScrollOrientation.Both;
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
            set
            {
                _currentView = value;
                OnPropertyChanged();

            }
        }

        private ScrollOrientation _orientation;

        public ScrollOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }


        private double _panScale;

        public double PanScale
        {
            get { return _panScale; }
            set { _panScale = value; }
        }

        private int _panScaleAnimationLength;

        public int PanScaleAnimationLength
        {
            get { return _panScaleAnimationLength; }
            set { _panScaleAnimationLength = value; }
        }

        public static readonly BindableProperty AutoAdsorptionProperty =
BindableProperty.Create("AutoAdsorption", typeof(bool), typeof(PanContainer), default(bool));

        public bool AutoAdsorption
        {
            get { return (bool)GetValue(AutoAdsorptionProperty); }
            set
            {
                SetValue(AutoAdsorptionProperty, value);
                OnPropertyChanged();

            }
        }


        public static readonly BindableProperty PositionXProperty =
 BindableProperty.Create("PositionX", typeof(double), typeof(PanContainer), default(double), propertyChanged: (bindable, oldValue, newValue) =>
 {
     var obj = (PanContainer)bindable;
     //obj.Content.TranslationX = obj.PositionX;
     if (obj.Content.TranslationX != obj.PositionX)
     {
         obj.Content.TranslateTo(obj.PositionX, obj.PositionY, 0);
     }

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
BindableProperty.Create("PositionY", typeof(double), typeof(PanContainer), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (PanContainer)bindable;
    //obj.Content.TranslationY = obj.PositionY;
    if (obj.Content.TranslationY != obj.PositionY)
    {
        obj.Content.TranslateTo(obj.PositionX, obj.PositionY, 0);
    }
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


        public static readonly BindableProperty SpringBackProperty =
BindableProperty.Create("SpringBack", typeof(bool), typeof(PanContainer), true);

        public bool SpringBack
        {
            get { return (bool)GetValue(SpringBackProperty); }
            set
            {
                SetValue(SpringBackProperty, value);
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
            var isAdsorbInPit = false;

            switch (e.StatusType)
            {
                case GestureStatus.Started:

                    Content.AbortAnimation("ReshapeAnimations");
                    var scaleAnimation = new Animation();
                    var scaleUpAnimation0 = new Animation(v => Content.Scale = v, Content.Scale, PanScale);
                    scaleAnimation.Add(0, 1, scaleUpAnimation0);

                    scaleAnimation.Commit(this, "ReshapeAnimations", 16, (uint)PanScaleAnimationLength);
                    WeakReferenceMessenger.Default.Send<PanActionArgs, string>(new PanActionArgs(PanType.Start, this.CurrentView), TokenHelper.PanAction);

                    break;
                case GestureStatus.Running:

                    double translationX;
                    double translationY;
                    translationX = Orientation == ScrollOrientation.Both || Orientation == ScrollOrientation.Horizontal
                        ? Math.Max(0 - Content.Width / 2, Math.Min(PositionX + e.TotalX, this.Width - Content.Width / 2))
                        : PositionX;
                    translationY = Orientation == ScrollOrientation.Both || Orientation == ScrollOrientation.Vertical
                        ? Math.Max(0 - Content.Height / 2, Math.Min(PositionY + e.TotalY, this.Height - Content.Height / 2))
                        : PositionY;
                    PitGrid lastView = null;
                    if (PitLayout != null)
                    {

                        foreach (var item in PitLayout)
                        {

                            var pitRegion = new Region(item.X, item.X + item.Width, item.Y, item.Y + item.Height, item.PitName);
                            var isXin = translationX >= pitRegion.StartX - Content.Width / 2 && translationX <= pitRegion.EndX - Content.Width / 2;
                            var isYin = translationY >= pitRegion.StartY - Content.Height / 2 && translationY <= pitRegion.EndY - Content.Height / 2;

                            if (Orientation switch
                            {
                                ScrollOrientation.Horizontal => isXin,
                                ScrollOrientation.Vertical => isYin,
                                ScrollOrientation.Both => isYin && isXin,
                            })
                            {
                                isInPit = true;
                                if (AutoAdsorption)
                                {
                                    isAdsorbInPit = true;
                                    translationX = (pitRegion.EndX + pitRegion.StartX - Content.Width) / 2;
                                    translationY = (pitRegion.EndY + pitRegion.StartY - Content.Height) / 2;
                                }
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
                            WeakReferenceMessenger.Default.Send<PanActionArgs, string>(new PanActionArgs(PanType.Out, lastView), TokenHelper.PanAction);
                            WeakReferenceMessenger.Default.Send<PanActionArgs, string>(new PanActionArgs(PanType.In, this.CurrentView), TokenHelper.PanAction);
                            isSwitch = false;
                        }
                        if (!isInPitPre)
                        {
                            WeakReferenceMessenger.Default.Send<PanActionArgs, string>(new PanActionArgs(PanType.In, this.CurrentView), TokenHelper.PanAction);
                            isInPitPre = true;


                        }
                    }
                    else
                    {


                        if (isInPitPre)
                        {
                            WeakReferenceMessenger.Default.Send<PanActionArgs, string>(new PanActionArgs(PanType.Out, this.CurrentView), TokenHelper.PanAction);
                            isInPitPre = false;
                        }
                        this.CurrentView = null;

                    }
                    if (AutoAdsorption)
                    {
                        if (isAdsorbInPit)
                        {
                            if (!IsRuningTranslateToTask)
                            {
                                IsRuningTranslateToTask = true;
                                await Content.TranslateTo(translationX, translationY, 200, Easing.CubicOut).ContinueWith(c => IsRuningTranslateToTask = false); ;
                            }

                            isAdsorbInPit = false;
                        }
                        else
                        {
                            Content.TranslationX = translationX;
                            Content.TranslationY = translationY;
                        }
                    }
                    else
                    {
                        Content.TranslationX = translationX;
                        Content.TranslationY = translationY;
                    }
                    break;

                case GestureStatus.Completed:

                    Content.AbortAnimation("ReshapeAnimations");
                    var parentAnimation = new Animation();
                    var mySpringOut = (double x) => (x - 1) * (x - 1) * ((5f + 1) * (x - 1) + 5) + 1;

                    var destX = PositionX;
                    var destY = PositionY;
                    if (Orientation == ScrollOrientation.Both || Orientation == ScrollOrientation.Horizontal)
                    {
                        if (CurrentView != null && !SpringBack)
                        {
                            destX = CurrentView.X + (CurrentView.Width - this.Content.Width) / 2;
                        }
                        var scaleUpAnimation1 = new Animation(v => Content.TranslationX = v, Content.TranslationX, destX, mySpringOut);
                        parentAnimation.Add(0, 1, scaleUpAnimation1);
                    }
                    if (Orientation == ScrollOrientation.Both || Orientation == ScrollOrientation.Vertical)
                    {
                        if (CurrentView != null && !SpringBack)
                        {
                            destY = CurrentView.Y + (CurrentView.Height - this.Content.Height) / 2;
                        }
                        var scaleUpAnimation2 = new Animation(v => Content.TranslationY = v, Content.TranslationY, destY, mySpringOut);
                        parentAnimation.Add(0, 1, scaleUpAnimation2);

                    }

                    var scaleUpAnimation5 = new Animation(v => Content.Scale = v, Content.Scale, 1.0);

                    parentAnimation.Add(0, 1, scaleUpAnimation5);

                    parentAnimation.Commit(this, "RestoreAnimation", 16, (uint)PanScaleAnimationLength, finished: (d, b) =>
                    {
                        if (Orientation == ScrollOrientation.Both || Orientation == ScrollOrientation.Horizontal)
                        {
                            this.PositionX = destX;
                        }
                        if (Orientation == ScrollOrientation.Both || Orientation == ScrollOrientation.Vertical)
                        {
                            this.PositionY = destY;
                        }
                    });

                    if (isInPitPre)
                    {
                        var view = this.CurrentView;
                        if (view.IsEnable)
                        {
                            OnfinishedChoise?.Invoke(this, view);
                        }
                        // WeakReferenceMessenger.Default.Send("true");

                    }
                    WeakReferenceMessenger.Default.Send<PanActionArgs, string>(new PanActionArgs(PanType.Over, this.CurrentView), TokenHelper.PanAction);

                    break;
            }

        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var scaleAnimation = new Animation();
            var scaleUpAnimation0 = new Animation(v => Content.Scale = v, 1.0, 0.9);
            var scaleUpAnimation1 = new Animation(v => Content.Scale = v, 0.9, 1.1);
            var scaleUpAnimation2 = new Animation(v => Content.Scale = v, 1.1, 1.0);
            scaleAnimation.Add(0, 0.3, scaleUpAnimation0);
            scaleAnimation.Add(0.3, 0.6, scaleUpAnimation1);
            scaleAnimation.Add(0.6, 1, scaleUpAnimation2);

            scaleAnimation.Commit(this, "ReshapeAnimations", 16, 400);

            this.OnTapped?.Invoke(this, EventArgs.Empty);
        }
    }

    public class PanActionArgs
    {
        public PanActionArgs(PanType type, PitGrid pit = null)
        {
            PanType = type;
            CurrentPit = pit;
        }
        public PanType PanType { get; set; }
        public PitGrid CurrentPit { get; set; }

    }

    public enum PanType
    {
        Out, In, Over, Start
    }

}