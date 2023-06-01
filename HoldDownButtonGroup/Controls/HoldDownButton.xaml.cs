using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

using System.Diagnostics;


namespace HoldDownButtonGroup.Controls;

public partial class HoldDownButton : ContentView
{

    public HoldDownButton()
    {
        InitializeComponent();

    }

    private void TouchContentView_OnTouchActionInvoked(object sender, TouchActionEventArgs e)
    {
        switch (e.Type)
        {
            case TouchActionType.Entered:
                break;
            case TouchActionType.Pressed:
                this.UpdateProgressWithAnimate(100);

                break;
            case TouchActionType.Moved:
                break;
            case TouchActionType.Released:
                this.UpdateProgressWithAnimate(0);

                break;
            case TouchActionType.Exited:
                break;
            case TouchActionType.Cancelled:
                break;
            default:
                break;
        }
    }

    private void UpdateProgressWithAnimate(double progressTarget = 100, Action<double, bool> finished = null)
    {
        Content.AbortAnimation("ReshapeAnimations");
        var scaleAnimation = new Animation();

        double progressOrigin = this.MainCircleProgressBar.Progress;

        var animateAction = (double r) =>
        {
            this.MainCircleProgressBar.Progress = r;
        };

        var scaleUpAnimation0 = new Animation(animateAction, progressOrigin, progressTarget);
        scaleAnimation.Add(0, 1, scaleUpAnimation0);
        var calcLenght = (double)(Math.Abs((progressOrigin - progressTarget) / 100)) * 5 * 1000;

        scaleAnimation.Commit(this, "ReshapeAnimations", 16, (uint)calcLenght, finished: finished);

    }

}