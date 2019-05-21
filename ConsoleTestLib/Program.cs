using ECCL.src;
using ECCL.src.ActiveComponents.Components;
using ECCL.src.Analysis;
using ECCL.src.Components;
using ECCL.src.Components.Other;
using ECCL.src.Components.Passive;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleTestLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Circuit circuit = new Circuit();
            var listElements = new List<IComponentBase>();
            var R1 = new Resistor() { Resistance = 25 };
            var R2 = new Resistor() { Resistance = 22 };
            var R3 = new Resistor() { Resistance = 42 };
            var R4 = new Resistor() { Resistance = 35 };
            var R5 = new Resistor() { Resistance = 51 };
            var R6 = new Resistor() { Resistance = 10 };
            var R7 = new Resistor() { Resistance = 47 };

            var E1 = new DCVoltageSource() { Voltage = 50 };
            var E2 = new DCVoltageSource() { Voltage = 100 };

            var N1 = new Node();
            var N2 = new Node();
            var N3 = new Node();
            var N4 = new Node();

            E1[1].Connect(R1[0]);
            N1.Connect(E1[0]);
            N2.Connect(R1[1]);

            N1.Connect(R6[0]);
            N2.Connect(R6[1]);

            N1.Connect(R7[0]);
            N4.Connect(R7[1]);

            N2.Connect(R4[0]);
            N3.Connect(R4[1]);

            N2.Connect(R3[1]);
            N4.Connect(R3[0]);

            N4.Connect(R5[0]);
            N3.Connect(R5[1]);

            E2[1].Connect(R2[0]);
            N4.Connect(E2[0]);
            N3.Connect(R2[1]);

            listElements.AddRange(new IComponentBase[] { R1, R2, R3, R4, R5, R6, R7, E1, E2, N1, N2, N3, N4 });
            circuit.Components = listElements;

            NodeAnalyst nodeAnalyst = new NodeAnalyst(circuit);
            nodeAnalyst.PropertyChanged += Dotnth;

            nodeAnalyst.StartAnalysis();
            Console.WriteLine("Started");
            Thread.Sleep(3000);
            nodeAnalyst.PauseAnalysis();
            Console.WriteLine("Pause");
            Thread.Sleep(3000);
            nodeAnalyst.PauseAnalysis();
            Console.WriteLine("Started");
            Thread.Sleep(3000);
            nodeAnalyst.StopAnalysis();
            Console.WriteLine("Stoped");
            Console.ReadKey();
        }

        public static void Dotnth(object sender, EventArgs e)
        {
            var a = (sender as NodeAnalyst).VoltageAndCurrentComponents;
            foreach (var i in a)
            {
                Console.WriteLine($"R = {(i.Key as Resistor).Resistance}  I = {i.Value.I}A U = {i.Value.U}V");
            }
        }

    }
}
