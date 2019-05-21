using ECCL.src.Components;

namespace ECCL.src.ActiveComponents
{
    public interface ICurrentSource: IComponentBase
    {
        double Current { get; set; }
    }
}
