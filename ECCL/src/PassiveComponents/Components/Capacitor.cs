using ECCL.src.Components.Passive;
using ECCL.src.Attributes;
using ECCL.src.Components.Other;
using System;

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
            throw new NotImplementedException();
        }

        public double GetResistance(double AngularFrequency)
        {
            throw new NotImplementedException();
        }
    }
}
