using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SectionalCropper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand CommandLoad { get; }
        public RelayCommand CommandKey { get; }
        public RelayCommand CommandFirst { get; }
        public RelayCommand CommandPrevious { get; }
        public RelayCommand CommandNext { get; }
        public RelayCommand CommandLast { get; }
        public string ImageSource => _currentFrame?.ImageSource;
        public Thickness RectangleMargin => _currentFrame?.RectangleMargin ?? new Thickness(0.0);
        public double Width { set => Models.ImageControlInfo.Width = value; }
        public double Height { set => Models.ImageControlInfo.Height = value; }
        public SolidColorBrush ButtonKeyBackground => _currentFrame != null && _currentFrame.IsKey ? Brushes.Green : Brushes.Gray;
        public double RectangleLeft
        {
            get => _currentFrame?.Rectangle.Left ?? 0.0;
            set
            {
                _currentFrame.SetRectangle(Models.Frame.RectangleVariables.Left, value);
                RaisePropertyChanged(nameof(RectangleMargin));
            }
        }
        public double RectangleTop
        {
            get => _currentFrame?.Rectangle.Top ?? 0.0;
            set
            {
                _currentFrame.SetRectangle(Models.Frame.RectangleVariables.Top, value);
                RaisePropertyChanged(nameof(RectangleMargin));
            }
        }
        public double RectangleWidth
        {
            get => _currentFrame?.Rectangle.Width ?? 0.0;
            set
            {
                _currentFrame.SetRectangle(Models.Frame.RectangleVariables.Width, value);
                RaisePropertyChanged(nameof(RectangleMargin));
            }
        }
        public double RectangleHeight
        {
            get => _currentFrame?.Rectangle.Height ?? 0.0;
            set
            {
                _currentFrame.SetRectangle(Models.Frame.RectangleVariables.Height, value);
                RaisePropertyChanged(nameof(RectangleMargin));
            }
        }
        public int CurrentIndex
        {
            get => Math.Min(Math.Max(0, _currentIndex), MaximumIndex);
            set
            {
                _currentIndex = Math.Min(Math.Max(0, value), MaximumIndex);
                OnFrameChange();
            }
        }
        public int MaximumIndex => Models.Frame.Frames?.Count - 1 ?? 0;
        private Models.Frame _currentFrame => Models.Frame.Frames[CurrentIndex];
        private int _currentIndex;
        private readonly Dispatcher _dispatcher;

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            CommandLoad = new RelayCommand(Load, true);
            CommandKey = new RelayCommand(() =>
            {
                if (_currentFrame == null) return;
                _currentFrame.IsKey = !_currentFrame.IsKey;
                RaisePropertyChanged(nameof(RectangleMargin));
                RaisePropertyChanged(nameof(ButtonKeyBackground));
            }, true);
            CommandFirst = new RelayCommand(() => CurrentIndex = 0, true);
            CommandPrevious = new RelayCommand(() => CurrentIndex--, true);
            CommandNext = new RelayCommand(() => CurrentIndex++, true);
            CommandLast = new RelayCommand(() => CurrentIndex = MaximumIndex, true);
        }

        private void Load()
        {
            var inputDirectory = AppContext.BaseDirectory + @"input\";
            if (!System.IO.Directory.Exists(inputDirectory))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(inputDirectory);
                }
                catch (Exception)
                {
                    return;
                }
            }
            Models.Frame.Frames?.Clear();
            var files = System.IO.Directory.GetFiles(inputDirectory);
            foreach (var file in files)
            {
                Models.Frame.Add(file);
            }
            CurrentIndex = 0;
        }

        private void OnFrameChange()
        {
            RaisePropertyChanged(nameof(ImageSource));
            RaisePropertyChanged(nameof(RectangleMargin));
            RaisePropertyChanged(nameof(RectangleLeft));
            RaisePropertyChanged(nameof(RectangleTop));
            RaisePropertyChanged(nameof(RectangleWidth));
            RaisePropertyChanged(nameof(RectangleHeight));
            RaisePropertyChanged(nameof(ButtonKeyBackground));
        }
    }
}
