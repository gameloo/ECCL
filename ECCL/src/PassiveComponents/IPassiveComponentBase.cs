using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCL.src.Components.Passive
{
    public interface IPassiveComponentBase: IComponentBase
    {
        double GetResistance();
        double GetResistance(double AngularFrequency);
    }
}
