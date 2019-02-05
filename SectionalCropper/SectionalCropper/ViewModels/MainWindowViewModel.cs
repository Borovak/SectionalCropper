using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SectionalCropper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand CommandLoad { get; }
        public RelayCommand CommandRemoveKey { get; }
        public RelayCommand CommandFirst { get; }
        public RelayCommand CommandPrevious { get; }
        public RelayCommand CommandNext { get; }
        public RelayCommand CommandLast { get; }
        public string ImageSource => _currentFrame?.ImageSource;
        public Thickness RectangleMargin => new Thickness(Pointer0Margin.Left, Pointer0Margin.Top, Pointer1Margin.Right, Pointer1Margin.Bottom);
        public Thickness Pointer0Margin => _currentFrame?.Pointer0Margin ?? new Thickness(0.0);
        public Thickness Pointer1Margin => _currentFrame?.Pointer1Margin ?? new Thickness(0.0);
        public SolidColorBrush PointerColor => Brushes.Transparent;
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
            CommandRemoveKey = new RelayCommand(() =>
            {
                if (_currentFrame == null) return;
                _currentFrame.IsKey = false;
                RaisePropertyChanged(nameof(Pointer0Margin));
                RaisePropertyChanged(nameof(Pointer1Margin));
                RaisePropertyChanged(nameof(RectangleMargin));
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
            RaisePropertyChanged(nameof(Pointer0Margin));
            RaisePropertyChanged(nameof(Pointer1Margin));
        }
    }
}
