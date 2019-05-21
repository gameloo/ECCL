using ECCL.src.Components;

namespace ECCL.src.ActiveComponents
{
    public interface IVoltageSource: IComponentBase
    {
        double Voltage { get; set; }
    }
}
