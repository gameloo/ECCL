using ECCL.src.Components.Other;
using ECCL.src.Components.Passive;

namespace ECCL.src.Components.Passive
{
    public class Resistor : IPassiveComponentBase
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

        public double Resistance { get; set; }

        public Resistor()
        {
            pins = new Pin[2] { new Pin(this), new Pin(this) };
        }

        public double GetResistance()
        {
            return Resistance;
        }

        public double GetResistance(double AngularFrequency)
        {
            return Resistance;
        }
    }
}
