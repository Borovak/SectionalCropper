using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SectionalCropper.Models
{
    internal class Frame
    {
        internal static List<Frame> Frames;
        internal int Index;
        internal bool IsKey;
        internal string ImageSource;
        internal Thickness Pointer0Margin { get => IsKey ? _margins[0] : GetMargin(0); set { _margins[0] = value; IsKey = true; } }
        internal Thickness Pointer1Margin { get => IsKey ? _margins[1] : GetMargin(1); set { _margins[1] = value; IsKey = true; } }
        protected Thickness[] _margins;
        
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

        private Thickness GetMargin(int index)
        {
            var frameMin = Frames.LastOrDefault(x => x.IsKey && x.Index < Index);
            var frameMax = Frames.FirstOrDefault(x => x.IsKey && x.Index > Index);
            if (frameMin == null && frameMax == null) return new Thickness(0.0);
            if (frameMax == null) return frameMin._margins[index];
            if (frameMin == null) return frameMax._margins[index];
            var left = new List<WeightedItem> {
                new WeightedItem{Value = frameMin._margins[index].Left , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax._margins[index].Left , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            var top = new List<WeightedItem> {
                new WeightedItem{Value = frameMin._margins[index].Top , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax._margins[index].Top , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            var right = new List<WeightedItem> {
                new WeightedItem{Value = frameMin._margins[index].Right , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax._margins[index].Right , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            var bottom = new List<WeightedItem> {
                new WeightedItem{Value = frameMin._margins[index].Bottom , Weight = Index - frameMin.Index },
                new WeightedItem{Value = frameMax._margins[index].Bottom , Weight = frameMax.Index - Index }
            }.WeightedAverage(x => x.Value, x => x.Weight);
            return new Thickness(left, top, right, bottom);
        }

        public class WeightedItem
        {
            public double Value;
            public double Weight;
        }
        
    }
}
