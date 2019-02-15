using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Media;
using SectionalCropper.Controllers;
using SectionalCropper.Models;

namespace SectionalCropper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand CommandLoad { get; }
        public RelayCommand CommandOutput { get; }
        public RelayCommand CommandKey { get; }
        public RelayCommand CommandFirst { get; }
        public RelayCommand CommandPrevious { get; }
        public RelayCommand CommandNext { get; }
        public RelayCommand CommandLast { get; }
        public string ImageSource => _currentFrame?.ImageSource;
        public SolidColorBrush ButtonKeyBackground => _currentFrame != null && _currentFrame.IsKey ? Brushes.Green : Brushes.Gray;
        public double RectangleLeft
        {
            get => _currentFrame?.Rectangle.Left ?? 0.0;
            set
            {
                _currentFrame?.SetRectangle(Models.Frame.RectangleVariables.Left, value);
                OnFrameChange();
            }
        }
        public double RectangleTop
        {
            get => _currentFrame?.Rectangle.Top ?? 0.0;
            set
            {
                _currentFrame.SetRectangle(Models.Frame.RectangleVariables.Top, value);
                OnFrameChange();
            }
        }
        public double RectangleWidth
        {
            get => _currentFrame?.Rectangle.Width ?? 0.0;
            set
            {
                _currentFrame.SetRectangle(Models.Frame.RectangleVariables.Width, value);
                OnFrameChange();
            }
        }
        public double RectangleHeight
        {
            get => _currentFrame?.Rectangle.Height ?? 0.0;
            set
            {
                _currentFrame.SetRectangle(Models.Frame.RectangleVariables.Height, value);
                OnFrameChange();
            }
        }
        public int CurrentIndex
        {
            get => Math.Min(Math.Max(0, Frame.CurrentIndex), MaximumIndex);
            set
            {
                Frame.CurrentIndex = Math.Min(Math.Max(0, value), MaximumIndex);
                OnFrameChange();
            }
        }
        public int MaximumIndex => Frame.Frames?.Count - 1 ?? 0;
        public int OutputWidth
        {
            get => OutputParameters.Width;
            set => OutputParameters.Width = value;
        }
        public int OutputHeight
        {
            get => OutputParameters.Height;
            set => OutputParameters.Height = value;
        }
        private Frame _currentFrame => CurrentIndex >= 0 && CurrentIndex < Frame.Frames.Count ? Frame.Frames[CurrentIndex] : null;

        public MainWindowViewModel()
        {
            CommandLoad = new RelayCommand(() => { LoadController.Load(); OnFrameChange(); }, true);
            CommandOutput = new RelayCommand(OutputController.Output, true);
            CommandKey = new RelayCommand(() =>
            {
                if (_currentFrame == null) return;
                _currentFrame.IsKey = !_currentFrame.IsKey;
                RaisePropertyChanged(nameof(ButtonKeyBackground));
            }, true);
            CommandFirst = new RelayCommand(() => CurrentIndex = 0, true);
            CommandPrevious = new RelayCommand(() => CurrentIndex--, true);
            CommandNext = new RelayCommand(() => CurrentIndex++, true);
            CommandLast = new RelayCommand(() => CurrentIndex = MaximumIndex, true);
        }

        private void OnFrameChange()
        {
            RaisePropertyChanged(nameof(ImageSource));
            RaisePropertyChanged(nameof(RectangleLeft));
            RaisePropertyChanged(nameof(RectangleTop));
            RaisePropertyChanged(nameof(RectangleWidth));
            RaisePropertyChanged(nameof(RectangleHeight));
            RaisePropertyChanged(nameof(ButtonKeyBackground));
        }
    }
}
