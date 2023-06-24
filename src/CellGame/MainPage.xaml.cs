namespace CellGame;

public partial class MainPage : ContentPage
{
    //int count = 0;
    private readonly World _world;
    private readonly Timer _timer;
    Color _originalBackgroundColor;

    public MainPage()
	{
		InitializeComponent();

        _world = new World(80, 60);
        ((GraphicsDrawable)DrawingBoard.Drawable).World = _world;
        _world.InitializeRandom();
		_timer = new Timer(OnTimerTick, null, 0, 10);

        _originalBackgroundColor = this.BackgroundColor;

        this.Title = DeviceInfo.Current.Name;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"Model: {DeviceInfo.Current.Model}");
        sb.AppendLine($"Manufacturer: {DeviceInfo.Current.Manufacturer}");
        sb.AppendLine($"Name: {DeviceInfo.Current.Name}");
        sb.AppendLine($"OS Version: {DeviceInfo.Current.VersionString}");
        sb.AppendLine($"Idiom: {DeviceInfo.Current.Idiom}");
        sb.AppendLine($"Platform: {DeviceInfo.Current.Platform}");

        bool isVirtual = DeviceInfo.Current.DeviceType switch
        {
            DeviceType.Physical => false,
            DeviceType.Virtual => true,
            _ => false
        };

        sb.AppendLine($"Virtual device? {isVirtual}");

        sb.AppendLine();

        sb.AppendLine($"Pixel width: {DeviceDisplay.Current.MainDisplayInfo.Width} / Pixel Height: {DeviceDisplay.Current.MainDisplayInfo.Height}");
        sb.AppendLine($"Density: {DeviceDisplay.Current.MainDisplayInfo.Density}");
        sb.AppendLine($"Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}");
        sb.AppendLine($"Rotation: {DeviceDisplay.Current.MainDisplayInfo.Rotation}");
        sb.AppendLine($"Refresh Rate: {DeviceDisplay.Current.MainDisplayInfo.RefreshRate}");

        var deviceInfo = sb.ToString();
    }

    private bool _running;
    private void OnTimerTick(object state)
    {
        if (_running)
            OneStep();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        _world.Clear();
        DrawingBoard.Invalidate();
    }
    private void OnGenerateRandomClicked(object sender, EventArgs e)
    {
        _world.InitializeRandom();
        DrawingBoard.Invalidate();
    }
    private void OnGenerateRandomBulletClicked(object sender, EventArgs e)
    {
        _world.GenerateRandomBullet();
        DrawingBoard.Invalidate();
    }
    private void OnStartClicked(object sender, EventArgs e)
    {
		_running = !_running;
        
        StepBtn.IsEnabled = !_running;
        ClearBtn.IsEnabled = !_running;
        GenerateRandomBtn.IsEnabled = !_running;
        InfiniteSwitch.IsEnabled = !_running;

        StartBtn.Text = _running ? "Stop" : "Start";
        SemanticScreenReader.Announce(StartBtn.Text);
    }
    private void OnStepClicked(object sender, EventArgs e)
    {
        OneStep();
    }
    private void OnInfiniteToggled(object sender, ToggledEventArgs e)
    {
        _world.Infinite = e.Value;
    }
    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        if(e.Value)
            this.BackgroundColor = Colors.Black;
        else
            this.BackgroundColor = _originalBackgroundColor;

        ((GraphicsDrawable)DrawingBoard.Drawable).DarkMode = e.Value;
        DrawingBoard.Invalidate();
    }
    private void OnGlowingToggled(object sender, ToggledEventArgs e)
    {
        ((GraphicsDrawable)DrawingBoard.Drawable).Glowing = e.Value;
    }

    private void OneStep()
    {
        Dispatcher.DispatchAsync(() =>
        {
            _world.GenerateNext();
            DisplayLabel.Text = _world.Generation.ToString();
            DrawingBoard.Invalidate();
        });
    }
}
