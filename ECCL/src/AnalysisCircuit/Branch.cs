using ECCL.src.ActiveComponents;
using ECCL.src.Components;
using ECCL.src.Components.Other;
using ECCL.src.Components.Passive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCL.src.Analysis
{
    public enum Direction
    {
        Node_1_to_Node_2,
        Node_2_to_Node_1,
        /// <summary>
        /// От
        /// </summary>
        From,
        /// <summary>
        /// К
        /// </summary>
        Towards,
        None,
        Uncknow
    }

    internal class Branch
    {
        public static bool DCmode = true;

        public Node Node_1 { get; private set; }
        public Node Node_2 { get; private set; }

        public List<IComponentBase> AllComponentsInBrunch { get; private set; }
        public List<ICurrentSource> CurrentSources { get; private set; }
        public List<IVoltageSource> VoltageSources { get; private set; }
        // Проводимость ветви
        public Complex Y
        {
            get
            {
                var result = new Complex(0, 0);
                double summ = 0;
                foreach (var i in AllComponentsInBrunch)
                {
                    if (i is Resistor)
                    {
                        summ += (i as Resistor).Resistance;
                    }
                    else if (i is IVoltageSource)
                    {
                        summ += 0.0001;
                    }
                    else if (i is ICurrentSource)
                    {
                        summ += 9999999;
                    }
                    else if (i is Capacitor)
                    {
                        summ += 9999999;
                    }
                    else if (i is Inductor)
                    {
                        summ += 0.0001;
                    }
                }
                return result + 1.0 / summ;
            }
        }
        public Direction DirectionCurrent { get; private set; } = Direction.Node_1_to_Node_2;

        public Branch(Node node)
        {
            AllComponentsInBrunch = new List<IComponentBase>();
            CurrentSources = new List<ICurrentSource>();
            VoltageSources = new List<IVoltageSource>();
            Node_1 = node;
        }

        public void Add(IComponentBase component)
        {
            if (component is Node)
            {
                Node_2 = component as Node;
                FindDirection();
                return;
            }
            AllComponentsInBrunch.Add(component);
            if (component is ICurrentSource)
                CurrentSources.Add(component as ICurrentSource);
            if (component is IVoltageSource)
                VoltageSources.Add(component as IVoltageSource);
        }

        private void FindDirection()
        {
            if (CurrentSources.Count != 0 || VoltageSources.Count != 0)
            {
                IComponentBase component;
                if (CurrentSources.Count != 0) component = CurrentSources[0];
                else component = VoltageSources[0];
                do
                {
                    if (component[1].Component is Node)
                    {
                        if (Node_2.Equals(component[1].Component)) DirectionCurrent = Direction.Node_1_to_Node_2;
                        else DirectionCurrent = Direction.Node_2_to_Node_1;
                        return;
                    }
                    else
                    {
                        component = component[1].ConnectedComponent;
                    }

                } while (true);
            }
        }

        public Direction GetDirectionCurrent(Node node)
        {
            if (node.Equals(Node_1))
            {
                if (DirectionCurrent == Direction.Node_1_to_Node_2) return Direction.From;
                else return Direction.Towards;
            }
            else if (node.Equals(Node_2))
            {
                if (DirectionCurrent == Direction.Node_2_to_Node_1) return Direction.From;
                else return Direction.Towards;
            }
            else return Direction.Uncknow;
        }

        public Dictionary<IComponentBase, PropertyIU> GetIUParameterComponents(Dictionary<Node,Complex> nodalVoltages)
        {
            Complex I = 0;

            if (DirectionCurrent == Direction.Node_1_to_Node_2)
            {
                I = (nodalVoltages[Node_1] - nodalVoltages[Node_2] + VoltageSources.Sum(i=> i.Voltage)) * Y;
            }
            else
            {
                I = (nodalVoltages[Node_2] - nodalVoltages[Node_1] + VoltageSources.Sum(i => i.Voltage)) * Y;
            }

            var tempI = I.Magnitude;
            var retuenedD = new Dictionary<IComponentBase, PropertyIU>();

            foreach(var i in AllComponentsInBrunch)
            {
                if (i is Resistor)
                {
                    retuenedD.Add(i, new PropertyIU { I = I.Magnitude, U = I.Magnitude * (i as Resistor).Resistance });
                }
            }

            return retuenedD;
        }
    }
}
