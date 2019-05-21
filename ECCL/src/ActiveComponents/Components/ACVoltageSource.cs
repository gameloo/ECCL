using ECCL.src.Attributes;
using ECCL.src.Components.Other;
using System;

namespace ECCL.src.ActiveComponents.Components
{
    [ActiveComponent]
    [ACSource]
    public class ACVoltageSource : IVoltageSource
    {
        public Pin this[int index] => throw new NotImplementedException();

        public double Voltage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PinCount => throw new NotImplementedException();
    }
}
