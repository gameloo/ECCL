using ECCL.src.Attributes;
using ECCL.src.Components.Other;

namespace ECCL.src.ActiveComponents.Components
{
    [ActiveComponent]
    [DCSource]
    public class DCVoltageSource : IVoltageSource
    {
        private readonly Pin[] pins;
        public Pin this[int index]
        {
            get
            {
                return pins[index];
            }
        }

        public double Voltage { get; set; }

        public int PinCount { get { return pins.Length; } }

        public DCVoltageSource()
        {
            pins = new Pin[] { new Pin(this), new Pin(this) };
        }
    }
}
