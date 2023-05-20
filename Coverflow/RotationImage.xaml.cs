using Microsoft.Maui.Graphics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using static Microsoft.Maui.Controls.Internals.GIFBitmap;
using Microsoft.Maui;
using System.Drawing;
using System.Diagnostics;

namespace Coverflow;

public partial class RotationImage : ContentView
{

    public SKBitmap bitmap { get; private set; }


    public static readonly BindableProperty SourceProperty =
BindableProperty.Create("Source", typeof(string), typeof(RotationImage), default(string), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (RotationImage)bindable;
    if (obj.canvasView != null)
    {
        obj.canvasView.InvalidateSurface();
    }
});


    public string Source
    {
        get { return (string)GetValue(SourceProperty); }
        set
        {
            SetValue(SourceProperty, value);
            OnPropertyChanged();

        }
    }


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
        get
        {
            var val = (double)GetValue(RotateYProperty);
            Debug.WriteLine("--Reading the value of RotateY: "+val+"--");
            return val;
        }
        set
        {
            SetValue(RotateYProperty, value);
            OnPropertyChanged();

        }
    }

    public static readonly BindableProperty SkewYProperty =
BindableProperty.Create("SkewY", typeof(double), typeof(RotationImage), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (RotationImage)bindable;
    if (obj.canvasView != null)
    {
        obj.canvasView.InvalidateSurface();
    }
});


    public double SkewY
    {
        get { return (double)GetValue(SkewYProperty); }
        set
        {
            SetValue(SkewYProperty, value);
            OnPropertyChanged();

        }
    }

    public static readonly BindableProperty TransYProperty =
BindableProperty.Create("TransY", typeof(double), typeof(RotationImage), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (RotationImage)bindable;
    if (obj.canvasView != null)
    {
        obj.canvasView.InvalidateSurface();
    }
});


    public double TransY
    {
        get { return (double)GetValue(TransYProperty); }
        set
        {
            SetValue(TransYProperty, value);
            OnPropertyChanged();

        }
    }

    public double ImageHeight { get; set; }
    public double ImageWidth { get; set; }

    public float Depth { get; set; } = 800;
    public RotationImage()
    {
        InitializeComponent();
        this.SizeChanged+=RotationImage_SizeChanged;
    }

    private async void RotationImage_SizeChanged(object sender, EventArgs e)
    {
        await InitBitmap();
    }

    private async Task InitBitmap()
    {
        using (Stream stream = await FileSystem.OpenAppPackageFileAsync(Source))
        {
            if (stream!=null)
            {
                var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
                var pixcelHeight = mainDisplayInfo.Density*ImageHeight;
                var pixcelWidth = mainDisplayInfo.Density*ImageWidth;

                var bitmap = SKBitmap.Decode(stream);
                bitmap= bitmap.Resize(new SKImageInfo((int)pixcelHeight,
                    (int)pixcelWidth),
                    SKFilterQuality.Medium);
                this.bitmap=bitmap;
            }

        }
    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        if (this.bitmap==null)
        {
            return;
        }
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
        matrix.SkewY =  (float)Math.Tan(Math.PI * (float)this.SkewY / 180);
        matrix.TransY = (float)this.TransY;

        canvas.SetMatrix(matrix);
        float xBitmap = xCenter - bitmap.Width / 2;
        //float yBitmap = yCenter - bitmap.Height / 2;
        float yBitmap = yCenter-bitmap.Height;
        canvas.DrawBitmap(bitmap, xBitmap, yBitmap);

        using (SKPaint paint = new SKPaint())
        {


            paint.Color = SKColors.Black.WithAlpha((byte)(255 * 0.8));
            canvas.Scale(1, -1, 0, yCenter);
            canvas.DrawBitmap(bitmap, xBitmap, yBitmap, paint);
            SKRect rect = SKRect.Create(xBitmap, yBitmap, bitmap.Width, bitmap.Height);
            canvas.DrawRect(rect, paint);

        }
    }


}