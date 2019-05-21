using ECCL.src;
using ECCL.src.ActiveComponents.Components;
using ECCL.src.Analysis;
using ECCL.src.Components;
using ECCL.src.Components.Other;
using ECCL.src.Components.Passive;
using System.Collections.Generic;

namespace ConsoleTestLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Circuit circuit = new Circuit();
            var listElements = new List<IComponentBase>();
            var R1 = new Resistor() { Resistance = 1 };
            var R2 = new Resistor() { Resistance = 2 };
            var R3 = new Resistor() { Resistance = 3 };
            var R4 = new Resistor() { Resistance = 4 };
            var R5 = new Resistor() { Resistance = 5 };
            var R6 = new Resistor() { Resistance = 6 };
            var R7 = new Resistor() { Resistance = 7 };

            var E1 = new DCVoltageSource() { Voltage = 1 };
            var E2 = new DCVoltageSource() { Voltage = 2 };
            var E3 = new DCVoltageSource() { Voltage = 3 };

            var N1 = new Node();
            var N2 = new Node();
            var N3 = new Node();

            R1[1].Connect(R2[0]);
            N2.Connect(R1[0]);
            N3.Connect(R2[1]);

            R3[1].Connect(E1[0]);
            N1.Connect(R3[0]);
            N2.Connect(E1[1]);

            R4[1].Connect(E2[0]);
            N2.Connect(R4[0]);
            N3.Connect(E2[1]);

            N2.Connect(E3[1]);
            N3.Connect(E3[0]);

            R5[1].Connect(R6[0]);
            N1.Connect(R5[0]);
            N3.Connect(R6[1]);

            N1.Connect(R7[0]);
            N3.Connect(R7[1]);

            listElements.AddRange(new IComponentBase[] { R1, R2, R3, R4, R5, R6, R7, E1, E2, E3, N1, N2, N3 });
            circuit.Components = listElements;
            NodeAnalysis.Test(circuit);
        }
    }
}
