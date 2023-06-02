using MauiSample.Common.Controls;

namespace HoldDownButtonGroup.Controls;

public partial class Bubbles : ContentView
{
    private const int bubbleSize = 20;
    private const int bubbleCnt =60;
    private Size targetSize = new Size(80, 80);

    private static readonly Random rnd = new Random();

    public PitGrid CurrentPitView { get; set; }

    public Bubbles()
    {
        InitializeComponent();

    }



    private Animation InitAnimation(Bubble element, Size targetSize, bool isOnTop = true)
    {
        var offsetAnimation = new Animation();

        if (targetSize == default)
        {
            targetSize = element.DesiredSize;

        }
        var easing = Easing.Linear;

        var originX = PitContentLayout.Width / 2;
        var originY = PitContentLayout.Height / 2;

        var targetX = rnd.Next(-(int)targetSize.Width, (int)targetSize.Width) + (int)targetSize.Width / 2 + originX;
        var targetY = isOnTop ? rnd.Next(-(int)(targetSize.Height * 1.5), 0) + (int)targetSize.Height / 2 + originY :
             rnd.Next(0, (int)(targetSize.Height * 1.5)) + (int)targetSize.Height / 2 + originY
            ;


        var offsetX = targetX - originX;
        var offsetY = targetY - originY;


        var offsetAnimation1 = new Animation(v => element.TranslationX = v, originX - targetSize.Width / 2, targetX - targetSize.Width / 2, easing);
        var offsetAnimation2 = new Animation(v => element.TranslationY = v, originY - targetSize.Height / 2, targetY - targetSize.Height / 2, easing);

        offsetAnimation.Add(0.2, 0.8, offsetAnimation1);
        offsetAnimation.Add(0.2, 0.8, offsetAnimation2);
        offsetAnimation.Add(0, 1, element.BoxAnimation);

        offsetAnimation.Finished = () =>
        {
            foreach (var item in this.PitContentLayout.Children)
            {
                if (item is Bubble)
                {
                    Init(item as Bubble);
                }
            }
        };

        return offsetAnimation;
    }



    private void ContentView_SizeChanged(object sender, EventArgs e)
    {

        foreach (var item in this.PitContentLayout.Children)
        {
            if (item is Bubble)
            {
                Init(item as Bubble);
            }
        }
    }

    public void StartAnimation()
    {
        Content.AbortAnimation("ReshapeAnimations");
        var offsetAnimationGroup = new Animation();

        foreach (var item in this.PitContentLayout.Children)
        {
            if (item is Bubble)
            {
                var isOntop = this.PitContentLayout.Children.IndexOf(item) > this.PitContentLayout.Children.Count / 2;
                var currentAnimation = InitAnimation(item as Bubble, targetSize, isOntop);
                offsetAnimationGroup.Add(0, 1, currentAnimation);


            }
        }
        offsetAnimationGroup.Commit(this, "ReshapeAnimations", 16, 400);

    }

    private void Init(VisualElement element)
    {
        var TargetSize = element.DesiredSize;
        var originX = (PitContentLayout.Width - TargetSize.Width) / 2;
        var originY = (PitContentLayout.Height - TargetSize.Height) / 2;

        element.TranslationX = originX;
        element.TranslationY = originY;
        element.Rotation = 0;
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        for (int i = 0; i < bubbleCnt; i++)
        {
            var currentBox = new Bubble();
            currentBox.FillColor = i % 2 == 0 ? SolidColorBrush.Red : SolidColorBrush.Transparent;
            currentBox.BorderColor = SolidColorBrush.Red;
            currentBox.HeightRequest = bubbleSize;
            currentBox.WidthRequest = bubbleSize;
            currentBox.HorizontalOptions = LayoutOptions.Start;
            currentBox.VerticalOptions = LayoutOptions.Start;
            this.PitContentLayout.Add(currentBox);
        }
    }
}

