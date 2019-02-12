using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SectionalCropper.Models
{
    internal class Frame
    {
        internal enum RectangleVariables
        {
            Left,
            Top,
            Width,
            Height
        }

        internal static List<Frame> Frames;
        internal int Index;
        internal bool IsKey;
        internal string ImageSource;
        internal Rect Rectangle => IsKey ? _rectangle : GetRectangle();
        internal double Top = 0.5;
        internal double Width = 0.1;
        internal double Height = 0.1;
        internal Thickness RectangleMargin => new Thickness(Rectangle.Left * ImageControlInfo.Width, Rectangle.Top * ImageControlInfo.Height, ImageControlInfo.Width - (Rectangle.Left * ImageControlInfo.Width), ImageControlInfo.Height - (Rectangle.Top * ImageControlInfo.Height));
        protected Thickness[] _margins;
        private Rect _rectangle;
        
        private Frame()
        {
            Index = Frames.Count;
        }

        internal static void Add(string framePath)
        {
            if (Frames == null)
            {
                Frames = new List<Frame>();
            }
            var frame = new Frame();
            frame.ImageSource = framePath;
            Frames.Add(frame);
        }
        
        internal void SetRectangle(RectangleVariables variable, double value)
        {
            var left = variable == RectangleVariables.Left ? value : _rectangle.Left;
            var top = variable == RectangleVariables.Top ? value : _rectangle.Top;
            var width = variable == RectangleVariables.Width ? value : _rectangle.Width;
            var height = variable == RectangleVariables.Height ? value : _rectangle.Height;
            _rectangle = new Rect(left, top, width, height);

        }

        private Rect GetRectangle()
        {
            var frameMin = Frames.LastOrDefault(x => x.IsKey && x.Index < Index);
            var frameMax = Frames.FirstOrDefault(x => x.IsKey && x.Index > Index);
            if (frameMin == null && frameMax == null) return new Rect(0.5, 0.5, 0.1, 0.1);
            if (frameMax == null) return frameMin.Rectangle;
            if (frameMin == null) return frameMax.Rectangle;
            var left = new List<WeightedItem> {
                new WeightedItem{Value = frameMin.Rectangle.Left , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax.Rectangle.Left , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            var top = new List<WeightedItem> {
                new WeightedItem{Value = frameMin.Rectangle.Top , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax.Rectangle.Top , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            var right = new List<WeightedItem> {
                new WeightedItem{Value = frameMin.Rectangle.Width , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax.Rectangle.Width , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            var bottom = new List<WeightedItem> {
                new WeightedItem{Value = frameMin.Rectangle.Height , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax.Rectangle.Height , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            return new Rect(left, top, right, bottom);
        }

        public class WeightedItem
        {
            public double Value;
            public double Weight;
        }
        
    }
}
