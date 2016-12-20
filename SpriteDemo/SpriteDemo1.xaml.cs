using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SpriteDemo
{
    /// <remarks>
    /// Kudos:
    ///     FPS: https://blogs.msdn.microsoft.com/shawnhar/2007/06/08/displaying-the-framerate/
    /// </remarks>
    public sealed partial class SpriteDemo1 : Page
    {
        double _theta;
        double _thetaWorking;

        float _canvasWidth, _canvasHeight, _canvasCentreX, _canvasCentreY;
        Rect _canvasRect;

        Vector2[] _position;
        int _maxSpriteCount = 10000;

        CanvasTextFormat _debugTextFormat;
        Color _debugTextColour;
        Color _drawColour, _spriteColour, _spriteColourRunningSlowly;

        CanvasTimingInformation _updateTiming;

        TimeSpan _fpsTime;
        int _fpsCounter;
        int _fps;
        TimeSpan _oneSecond = TimeSpan.FromSeconds(1);

        bool _spriteBatchSupported;
        CanvasBitmap _spriteBitmap;
        Vector2 _spriteOrigin;
        Rect _spriteRect, _spriteRectNormal, _spriteRectRunningSlowly;

        public SpriteDemoViewModel ViewModel { get; set; }

        public SpriteDemo1()
        {
            InitializeComponent();

            ViewModel = new SpriteDemoViewModel();
            ViewModel.DrawModesAvailable.Add(DrawModes.DrawEllipse);
            ViewModel.DrawMode = DrawModes.DrawEllipse;

            DataContext = this;
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _canvasWidth = (float)e.NewSize.Width;
            _canvasHeight = (float)e.NewSize.Height;

            _canvasRect = new Rect(0d, 0d, e.NewSize.Width, e.NewSize.Height);

            _canvasCentreX = _canvasWidth / 2f;
            _canvasCentreY = _canvasHeight / 2f;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (sender as ListView);
            var index = lv.Items.IndexOf(e.AddedItems[0]);
            var b = lv.Tag.ToString()[0];
            char c = (char)(b + index);
            SetSliderBinding
            (
                b == 'a' ? sldXCoordinateSet : sldYCoordinateSet,
                $"{c}Integer"
            );
        }

        void SetSliderBinding(Slider sld, string propertyPath)
        {
            Binding b = new Binding() { Mode = BindingMode.TwoWay, Source = ViewModel, Path = new PropertyPath(propertyPath) };
            sld.SetBinding(Slider.ValueProperty, b);
        }

        private void canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(LoadSpriteSheet(sender.Device).AsAsyncAction());

            lvXCoordinate.SelectedIndex = 0;
            lvYCoordinate.SelectedIndex = 0;
            sldSpriteCount.Maximum = _maxSpriteCount;

            var defaultGridLength = new GridLength(0);
            var debugRowHeight = AnalyticsInfo.VersionInfo.DeviceFamily.ToLower().Contains("mobile")
                ? defaultGridLength
                : new GridLength(20);

            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "ShowDebug")
                {
                    Application.Current.DebugSettings.EnableFrameRateCounter = ViewModel.ShowDebug;
                    rowDebug.Height = ViewModel.ShowDebug
                        ? debugRowHeight
                        : defaultGridLength;
                }
            };

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                ViewModel.ShowDebug = true;
#endif

            _debugTextFormat = new CanvasTextFormat() { HorizontalAlignment = CanvasHorizontalAlignment.Right, VerticalAlignment = CanvasVerticalAlignment.Bottom };
            _debugTextColour = Colors.Black;
            _spriteColour = Color.FromArgb(60, 0, 0, 255);
            _spriteColourRunningSlowly = Color.FromArgb(60, 255, 0, 0);

            _fps = 0;
            _fpsCounter = 0;
            _fpsTime = TimeSpan.Zero;

            ViewModel.DeltaThetaInteger = 50;
            _theta = 0;

            ViewModel.SpriteCount = 315;
            _position = new Vector2[_maxSpriteCount];

            _spriteBatchSupported = CanvasSpriteBatch.IsSupported(sender.Device);
            if (_spriteBatchSupported)
            {
                ViewModel.DrawModesAvailable.Add(DrawModes.SpriteBatch);

                _spriteRectNormal = new Rect(0, 0, 10, 10);
                _spriteRectRunningSlowly = new Rect(10, 0, 10, 10);
            }

            ViewModel.aInteger = 100;
            ViewModel.bInteger = 1;
            ViewModel.cInteger = 100;

            ViewModel.mInteger = -100;
            ViewModel.nInteger = 29;
            ViewModel.oInteger = 74;
        }

        async Task LoadSpriteSheet(CanvasDevice device)
        {
            _spriteOrigin = new Vector2(5, 5);
            _spriteBitmap = await CanvasBitmap.LoadAsync(device, @"Assets/SpriteSheet.png");
        }

        private void canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            _updateTiming = args.Timing;

            if (ViewModel.DrawMode == DrawModes.DrawEllipse)
                _drawColour = args.Timing.IsRunningSlowly
                    ? _spriteColourRunningSlowly
                    : _spriteColour;
            else
                _spriteRect = args.Timing.IsRunningSlowly
                    ? _spriteRectRunningSlowly
                    : _spriteRectNormal;

            _fpsTime += args.Timing.ElapsedTime;
            if (_fpsTime > _oneSecond)
            {
                _fpsTime -= _oneSecond;
                _fps = _fpsCounter;
                _fpsCounter = 0;
            }

            _theta = (_theta + ViewModel.DeltaTheta);

            _thetaWorking = _theta;
            for (int i = 0; i < ViewModel.SpriteCount; i++)
            {
                _position[i] = new Vector2
                (
                    ViewModel.a * (float)Math.Cos(ViewModel.b * _thetaWorking + ViewModel.c * i) + _canvasCentreX,
                    ViewModel.m * (float)Math.Sin(ViewModel.n * _thetaWorking + ViewModel.o * i) + _canvasCentreY
                );
                _thetaWorking += ViewModel.ThetaStep;
            }
        }

        private void canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            _fpsCounter++;

            if (ViewModel.DrawMode == DrawModes.DrawEllipse)
                for (int i = 0; i < ViewModel.SpriteCount; i++)
                    args.DrawingSession.FillCircle(_position[i].X, _position[i].Y, 5, _drawColour);
            else
                using (var sb = args.DrawingSession.CreateSpriteBatch(CanvasSpriteSortMode.None, CanvasImageInterpolation.Linear, CanvasSpriteOptions.None))
                {
                    for (int i = 0; i < ViewModel.SpriteCount; i++)
                        sb.DrawFromSpriteSheet(_spriteBitmap, new Vector2(_position[i].X, _position[i].Y), _spriteRect, Vector4.One, _spriteOrigin, 0, Vector2.One, CanvasSpriteFlip.None);
                }

            if (ViewModel.ShowDebug)
                args.DrawingSession.DrawText(GetDebugText(), _canvasRect, _debugTextColour, _debugTextFormat);
        }

        void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas.RemoveFromVisualTree();
            canvas = null;
        }

        string GetDebugText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"theta: {_theta:0.00}, delta: {ViewModel.DeltaTheta:0.00}");
            sb.AppendLine($"x: {_position[0].X:0.000}, y: {_position[0].Y:0.000}");
            sb.AppendLine(_updateTiming.IsRunningSlowly ? "Is Running Slowly" : "Not Running Slowly");
            sb.AppendLine($"FPS: { _fps }");
            return sb.ToString();
        }
    }
}
