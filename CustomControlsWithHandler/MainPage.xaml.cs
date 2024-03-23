using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using Lession2.TouchRecognizer;
using System.Collections.ObjectModel;

namespace Lession2;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        this.DebugMessages=new ObservableCollection<string>(new string[] { "===Debug Messages start===" });
        InitializeComponent();

    }



    private ObservableCollection<string> _debugMessages;

    public ObservableCollection<string> DebugMessages
    {
        get { return _debugMessages; }
        set { _debugMessages = value; }
    }


    private void TouchContentView_OnTouchActionInvoked(object sender, TouchActionEventArgs e)
    {
        var msg = e.Type + " is Invoked, position:" + e.Location;
        DebugMessages.Insert(0, msg);
        Debug.WriteLine(msg);

    }
}

