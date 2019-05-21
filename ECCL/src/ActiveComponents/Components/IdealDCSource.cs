using ECCL.src.Attributes;
using ECCL.src.Components.Other;

namespace ECCL.src.ActiveComponents.Components
{
    [ActiveComponent]
    [DCSource]
    public class IdealDCSource : ICurrentSource
    {
        private readonly Pin[] pins;
        public Pin this[int index]
        {
            get
            {
                return pins[index];
            }
        }

        public double Current { get; set; }
        public int PinCount { get { return pins.Length; } }
    }
}
