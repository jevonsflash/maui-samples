using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
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
        var layout = ((Microsoft.Maui.Controls.Layout)(sender as TouchContentView).Content).Children;
        var bubbles = layout[0] as Bubbles;
        var circleProgressBar = layout[1] as CircleProgressBar;
        switch (e.Type)
        {
            case TouchActionType.Entered:
                break;
            case TouchActionType.Pressed:
                this.UpdateProgressWithAnimate(circleProgressBar, 100, 2*1000, (d, b) =>
                {
                    Debug.WriteLine("!!!!");
                });

                break;
            case TouchActionType.Moved:
                break;
            case TouchActionType.Released:
                if (circleProgressBar.Progress==100)
                {
                    return;
                }
                this.UpdateProgressWithAnimate(circleProgressBar, 0, 1000);

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

        scaleAnimation.Commit(this, "ReshapeAnimations", 16, (uint)calcLenght, finished: finished);

    }

}