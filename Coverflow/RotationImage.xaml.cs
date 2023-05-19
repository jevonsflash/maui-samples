using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Coverflow;

public partial class RotationImage : ContentView
{

    public string Source { get; set; }
    public SKBitmap bitmap { get; private set; }


    public static readonly BindableProperty RotateYProperty =
BindableProperty.Create("RotateY", typeof(double), typeof(RotationImage), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (RotationImage)bindable;
    if (obj.canvasView != null)
    {
        obj.canvasView.InvalidateSurface();
    }
});

    public double RotateY
    {
        get { return (double)GetValue(RotateYProperty); }
        set
        {
            SetValue(RotateYProperty, value);
            OnPropertyChanged();

        }
    }

    public float Depth { get; set; } = 300;
    public RotationImage()
    {
        InitializeComponent();
        Loaded+=RotationImage_Loaded;


    }

    private void RotationImage_Loaded(object sender, EventArgs e)
    {
        Init();
    }

    public async void Init()
    {
        using (Stream stream = await FileSystem.OpenAppPackageFileAsync(Source))
        {
            if (stream!=null)
            {
                var bitmap = SKBitmap.Decode(stream);
                bitmap= bitmap.Resize(new SKImageInfo(200, 200), SKFilterQuality.Medium);
                this.bitmap=bitmap;
            }

        }
    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        SKImageInfo info = args.Info;
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear();

        float xCenter = info.Width / 2;
        float yCenter = info.Height / 2;

        SKMatrix matrix = SKMatrix.CreateTranslation(-xCenter, -yCenter);

        SKMatrix44 matrix44 = SKMatrix44.CreateIdentity();
        matrix44.PostConcat(SKMatrix44.CreateRotationDegrees(1, 0, 0, (float)0));
        matrix44.PostConcat(SKMatrix44.CreateRotationDegrees(0, 1, 0, (float)RotateY));
        matrix44.PostConcat(SKMatrix44.CreateRotationDegrees(0, 0, 1, (float)0));

        SKMatrix44 perspectiveMatrix = SKMatrix44.CreateIdentity();
        perspectiveMatrix[3, 2] = -1 / (float)Depth;
        matrix44.PostConcat(perspectiveMatrix);

        matrix= matrix.PostConcat(matrix44.Matrix);

        matrix= matrix.PostConcat(
            SKMatrix.CreateTranslation(xCenter, yCenter));

        canvas.SetMatrix(matrix);
        float xBitmap = xCenter - bitmap.Width / 2;
        float yBitmap = yCenter - bitmap.Height / 2;
        canvas.DrawBitmap(bitmap, xBitmap, yBitmap);
    }


}