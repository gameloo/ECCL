using ECCL.src.Components.Other;
using System;

namespace ECCL.src.Exceptions
{
    public class PinAlreadyConnectedExcepton: Exception
    {
        public Pin AlreadyUsedPin { get; private set; }

        public PinAlreadyConnectedExcepton(Pin pin, string optionalmsg = "msg") : base(optionalmsg)
        {
            AlreadyUsedPin = pin;
        }
    }
}
