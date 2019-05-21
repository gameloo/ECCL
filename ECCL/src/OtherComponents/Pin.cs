using ECCL.src.Components;
using ECCL.src.Exceptions;

namespace ECCL.src.Components.Other
{
    public class Pin
    {
        public bool IsConnected { get; private set; } = false;
        public readonly IComponentBase Component;
        public IComponentBase ConnectedComponent
        {
            get
            {
                return ConnectedPin.Component;
            }
        }

        private Pin ConnectedPin { get; set; }

        public Pin (IComponentBase component)
        {
            this.Component = component;
        }

        private static bool _connectStart = true;
        public void Connect(Pin pin)
        {
            if (pin.IsConnected) throw new PinAlreadyConnectedExcepton(pin);
            if (_connectStart)
            {
                _connectStart = false;
                pin.Connect(this);
            }
            ConnectedPin = pin;
            IsConnected = true;
            _connectStart = true;
        }

        private static bool _disconnectStart = true;
        public void Disconnect()
        {
            if (_disconnectStart)
            {
                _disconnectStart = false;
                ConnectedPin?.Disconnect();
            }
            ConnectedPin = null;
            IsConnected = false;
            _disconnectStart = true;
        }
    }
}
