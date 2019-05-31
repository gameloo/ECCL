using ECCL.src.Attributes;
using ECCL.src.Components.Other;

namespace ECCL.src.Components.Passive
{
    [HaveInduction]
    public class Inductor : IPassiveComponentBase
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

        public double Inductance { get; set; }

        public Inductor()
        {
            pins = new Pin[2] { new Pin(this), new Pin(this) };
        }

        public double GetResistance()
        {
            return 0.001;
        }

        public double GetResistance(double AngularFrequency)
        {
            return AngularFrequency * Inductance;
        }
    }
}
