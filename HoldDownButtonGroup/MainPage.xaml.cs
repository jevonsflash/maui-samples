using HoldDownButtonGroup.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Handlers;

using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using Path = Microsoft.Maui.Controls.Shapes.Path;


namespace HoldDownButtonGroup;

public partial class MainPage : ContentPage
{
    private IDispatcherTimer _dispatcherTimer;
    private bool _isInProgress;

    public MainPage()
    {
        InitializeComponent();

    }


    private void TouchContentView_OnTouchActionInvoked(object sender, TouchActionEventArgs e)
    {
        var layout = ((Microsoft.Maui.Controls.Layout)(sender as TouchContentView).Content).Children;
        var bubbles = layout[0] as Bubbles;
        var circleProgressBar = layout[1] as CircleProgressBar;


        switch (e.Type)
        {
            case TouchActionType.Entered:
                break;
            case TouchActionType.Pressed:
                _dispatcherTimer =Dispatcher.CreateTimer();
                _dispatcherTimer.Interval=new TimeSpan(0, 0, 1);

                _dispatcherTimer.Tick+=   async (o, e) =>
                {
                    _isInProgress=true;
                    this.UpdateProgressWithAnimate(ProgressBar1, 100, 2*1000, (d, b) =>
                    {
                        if (circleProgressBar.Progress==100)
                        {
                            this.bubbles1.StartAnimation();
                            StartCelebrationAnimate(this.Icon1);
                        }
                    });
                    this.UpdateProgressWithAnimate(ProgressBar2, 100, 2*1000, (d, b) =>
                    {
                        if (circleProgressBar.Progress==100)
                        {
                            this.bubbles2.StartAnimation();
                            StartCelebrationAnimate(this.Icon2);
                        }
                    });
                    this.UpdateProgressWithAnimate(ProgressBar3, 100, 2*1000, (d, b) =>
                    {
                        if (circleProgressBar.Progress==100)
                        {
                            this.bubbles3.StartAnimation();
                            StartCelebrationAnimate(this.Icon3);
                        }
                    });

                };

                _dispatcherTimer.Start();


                break;
            case TouchActionType.Moved:
                break;
            case TouchActionType.Released:

                if (!_isInProgress)
                {
                    var brandColor = (Color)this.Resources["BrandColor"];
                    var disabledColor = (Color)this.Resources["DisabledColor"];

                    if (circleProgressBar.Progress==100)
                    {
                        this.UpdateProgressWithAnimate(ProgressBar1, 0, 1000);
                        this.UpdateProgressWithAnimate(ProgressBar2, 0, 1000);
                        this.UpdateProgressWithAnimate(ProgressBar3, 0, 1000);
                        (ProgressBar1.LabelContent as Path).Fill=disabledColor;
                        (ProgressBar2.LabelContent as Path).Fill=disabledColor;
                        (ProgressBar3.LabelContent as Path).Fill=disabledColor;


                    }
                    else
                    {
                        if (((circleProgressBar.LabelContent as Path).Fill  as SolidColorBrush).Color==disabledColor)
                        {
                            StartCelebrationAnimate(circleProgressBar.LabelContent as Path);

                        }
                        else
                        {
                            (circleProgressBar.LabelContent as Path).Fill=disabledColor;
                        }
                    }


                }
                if (_dispatcherTimer!=null)
                {
                    if (_dispatcherTimer.IsRunning)
                    {
                        _dispatcherTimer.Stop();
                        _isInProgress=false;

                    }
                    _dispatcherTimer=null;

                    if (circleProgressBar.Progress==100)
                    {
                        return;
                    }

                    this.UpdateProgressWithAnimate(ProgressBar1, 0, 1000);
                    this.UpdateProgressWithAnimate(ProgressBar2, 0, 1000);
                    this.UpdateProgressWithAnimate(ProgressBar3, 0, 1000);
                }



                break;
            case TouchActionType.Exited:
                break;
            case TouchActionType.Cancelled:
                break;
            default:
                break;
        }
    }

    private void UpdateProgressWithAnimate(CircleProgressBar progressElement, double progressTarget = 100, double totalLenght = 5*1000, Action<double, bool> finished = null)
    {
        Content.AbortAnimation("ReshapeAnimations");
        var scaleAnimation = new Animation();

        double progressOrigin = progressElement.Progress;

        var animateAction = (double r) =>
        {
            progressElement.Progress = r;
        };

        var scaleUpAnimation0 = new Animation(animateAction, progressOrigin, progressTarget);

        scaleAnimation.Add(0, 1, scaleUpAnimation0);
        var calcLenght = (double)(Math.Abs((progressOrigin - progressTarget) / 100)) * totalLenght;

        scaleAnimation.Commit(progressElement, "ReshapeAnimations", 16, (uint)calcLenght, finished: finished);

    }

    private void StartCelebrationAnimate(Path progressElement, Action<double, bool> finished = null)
    {
        var toColor = (Color)this.Resources["BrandColor"];
        var fromColor = (Color)this.Resources["DisabledColor"];

        var scaleAnimation = new Animation();

        var scaleUpAnimation0 = new Animation(v => progressElement.Scale=v, 0, 1.5);
        var scaleUpAnimation1 = new Animation(v => progressElement.Fill=GetColor(v, fromColor, toColor), 0, 1);
        var scaleUpAnimation2 = new Animation(v => progressElement.Scale=v, 1.5, 1);

        scaleAnimation.Add(0, 0.5, scaleUpAnimation0);
        scaleAnimation.Add(0, 1, scaleUpAnimation1);
        scaleAnimation.Add(0.5, 1, scaleUpAnimation2);

        scaleAnimation.Commit(progressElement, "CelebrationAnimate", 16, 400, finished: finished);

    }


    private Color GetColor(double t, Color fromColor, Color toColor)
    {
        return Color.FromRgba(fromColor.Red + t * (toColor.Red - fromColor.Red),
                           fromColor.Green + t * (toColor.Green - fromColor.Green),
                           fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                           fromColor.Alpha + t * (toColor.Alpha - fromColor.Alpha));
    }


}