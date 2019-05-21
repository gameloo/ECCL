using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ECCL.src.Components.Other
{
    public class Node : IComponentBase, IEnumerable
    {
        private readonly List<Pin> pins;
        public Pin this[int index]
        {
            get
            {
                return pins[index];
            }

        }

        public int PinCount { get { return pins.Count; } }

        public Node()
        {
            pins = new List<Pin>();
        }

        public void Connect(Pin pin)
        {
            var unconnectedPin = new Pin(this);
            pins.Add(unconnectedPin);
            pin.Connect(unconnectedPin);
        }

        public void Disconnect(IComponentBase component)
        {
            var soughtforPin = pins.First(pin => pin.ConnectedComponent.Equals(component));
            soughtforPin.Disconnect();
            pins.Remove(soughtforPin);
        }

        public IEnumerator GetEnumerator()
        {
            return pins.GetEnumerator();
        }
    }
}
