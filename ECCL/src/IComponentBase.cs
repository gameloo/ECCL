using ECCL.src.Components.Other;

namespace ECCL.src.Components
{
    public interface IComponentBase
    {
        int PinCount { get; }
        Pin this[int index] { get; }
    }
}
