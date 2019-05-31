using ECCL.src.Attributes;
using ECCL.src.Components.Other;

namespace ECCL.src.Components.Passive
{
    [HaveCapacity]
    public class Capacitor : IPassiveComponentBase
    {
        private readonly Pin[] pins;

        public Pin this[int index]
        {
            get
            {
                return pins[index];
            }
        }

        public int PinCount { get { return pins.Length; } }
        public double Capacitance { get; set; }

        public Capacitor()
        {
            pins = new Pin[2] { new Pin(this), new Pin(this) };
        }

        public double GetResistance()
        {
            return 9999999;
        }

        public double GetResistance(double AngularFrequency)
        {
            return 1/(AngularFrequency*Capacitance);
        }
    }
}
