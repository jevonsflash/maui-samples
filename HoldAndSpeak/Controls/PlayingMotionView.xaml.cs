using System;
using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;


namespace HoldAndSpeak.Controls
{
    public partial class PlayingMotionView : ContentView
    {
        Random r = new Random();
        private bool isOpen = false;
        private double originHeight = 10;

        public PlayingMotionView()
        {
            InitializeComponent();
        }

        public void Init()
        {
            viewbox1down();
            viewbox2down();
            viewbox3down();
            viewbox4down();
        }

        private bool viewbox1down()
        {

            double origin = originHeight;
            double range = r.Next(10, 25);
            uint duration = 1000;
            Action<double> currentAction = v => this.BoxView01.HeightRequest = v;

            Animation scaleUpAnimation = new Animation();
            Animation scaleUpAnimation1;
            Animation scaleUpAnimation2;

            scaleUpAnimation1 = new Animation(currentAction, origin, range, Easing.CubicInOut);
            scaleUpAnimation2 = new Animation(currentAction, range, origin, Easing.CubicInOut);
            scaleUpAnimation.Add(0, 0.5, scaleUpAnimation1);
            scaleUpAnimation.Add(0.5, 1, scaleUpAnimation2);
            scaleUpAnimation.Commit(this, "RestoreAnimation1", 16, duration, repeat: () => true);
            return isOpen;
        }
        private bool viewbox2down()
        {
            double origin = originHeight;
            double range = r.Next(10, 25);
            uint duration = 800;
            Action<double> currentAction = v => this.BoxView02.HeightRequest = v;

            Animation scaleUpAnimation = new Animation();
            Animation scaleUpAnimation1;
            Animation scaleUpAnimation2;

            scaleUpAnimation1 = new Animation(currentAction, origin, range, Easing.CubicInOut);
            scaleUpAnimation2 = new Animation(currentAction, range, origin, Easing.CubicInOut);
            scaleUpAnimation.Add(0, 0.5, scaleUpAnimation1);
            scaleUpAnimation.Add(0.5, 1, scaleUpAnimation2);
            scaleUpAnimation.Commit(this, "RestoreAnimation2", 16, duration, repeat: () => true);
            return isOpen;
        }
        private bool viewbox3down()
        {
            double origin = originHeight;
            double range = r.Next(10, 25);
            uint duration = 1200;
            Action<double> currentAction = v => this.BoxView03.HeightRequest = v;

            Animation scaleUpAnimation = new Animation();
            Animation scaleUpAnimation1;
            Animation scaleUpAnimation2;

            scaleUpAnimation1 = new Animation(currentAction, origin, range, Easing.CubicInOut);
            scaleUpAnimation2 = new Animation(currentAction, range, origin, Easing.CubicInOut);
            scaleUpAnimation.Add(0, 0.5, scaleUpAnimation1);
            scaleUpAnimation.Add(0.5, 1, scaleUpAnimation2);
            scaleUpAnimation.Commit(this, "RestoreAnimation3", 16, duration, repeat: () => true);
            return isOpen;
        }
        private bool viewbox4down()
        {
            double origin = originHeight;
            double range = r.Next(10, 25);
            uint duration = 900;
            Action<double> currentAction = v => this.BoxView04.HeightRequest = v;

            Animation scaleUpAnimation = new Animation();
            Animation scaleUpAnimation1;
            Animation scaleUpAnimation2;

            scaleUpAnimation1 = new Animation(currentAction, origin, range, Easing.CubicInOut);
            scaleUpAnimation2 = new Animation(currentAction, range, origin, Easing.CubicInOut);
            scaleUpAnimation.Add(0, 0.5, scaleUpAnimation1);
            scaleUpAnimation.Add(0.5, 1, scaleUpAnimation2);
            scaleUpAnimation.Commit(this, "RestoreAnimation4", 16, duration, repeat: () => true);
            return isOpen;
        }


    }
}
