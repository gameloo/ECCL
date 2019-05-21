using ECCL.src.Components;
using ECCL.src.Components.Other;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra.Complex.Solvers;
using MathNet.Numerics.LinearAlgebra.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCL.src.Analysis
{
    public static class NodeAnalysis
    {

        public static void Test(Circuit circuit)
        {

            var branches = GetBranches(circuit);
            var nodes = circuit.Components.FindAll(i => i is Node).Cast<Node>().ToList();
            var nv = GetNodalVoltages(nodes, branches);
        }




        static Dictionary<Node, Complex> GetNodalVoltages(List<Node> nodes, List<Branch> branches)
        {
            var baseNode = nodes[0];
            nodes.Remove(baseNode);

            // Матрица соединений для графа схемы
            var matrixA = new int[nodes.Count, branches.Count];
            // Матрица с активными компонентами в ветвях схемы
            var matrixJplusYE = new Complex[branches.Count];
            // Правая часть СЛАУ
            var matrixRight = new Complex[nodes.Count];
            // Левая часть СЛАУ
            var matrixLeft = new Complex[nodes.Count,nodes.Count];

            // Заполнение матрицы соединений A
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < branches.Count; j++)
                {
                    switch (branches[j].GetDirectionCurrent(nodes[i]))
                    {
                        case Direction.From: { matrixA[i, j] = 1; break; }
                        case Direction.Towards: { matrixA[i, j] = -1; break; }
                        case Direction.Uncknow: { matrixA[i, j] = 0; break; }
                    }
                }
            }

            // Заполнение матрицы matrixJplusYE значениямя токов в ветвях
            for (int i = 0; i < matrixJplusYE.Length; i++)
            {
                Complex summCurrent = 0;
                branches[i]?.CurrentSources?.ForEach((q) => { summCurrent += q.Current; });
                Complex summVoltage = 0;
                branches[i]?.VoltageSources?.ForEach((q) => { summVoltage += q.Voltage; });
                matrixJplusYE[i] = summCurrent + summVoltage * branches[i].Y;
            }

            // Заполнение правой части СЛАУ (токи)
            for (int i = 0; i < matrixRight.Length; i++)
            {
                matrixRight[i] = 0;
                for (int j = 0; j < matrixJplusYE.Length; j++)
                {
                    matrixRight[i] += matrixA[i,j] * matrixJplusYE[j];
                }
                matrixRight[i] *= -1;
            }

            // Заполнение левой части СЛАУ (проводимости)
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (i == j)
                    {
                        var br = branches.Where(nod => nod.Node_1.Equals(nodes[i]) || nod.Node_2.Equals(nodes[i])).ToList();
                        matrixLeft[i,j] = 0;
                        br.ForEach(b => matrixLeft[i, j] += b.Y);
                    }
                    else
                    {
                        var br = branches.Where(nod => nod.Node_1.Equals(nodes[i]) && nod.Node_2.Equals(nodes[j]) || nod.Node_2.Equals(nodes[i]) && nod.Node_1.Equals(nodes[j])).ToList();
                        matrixLeft[i, j] = 0;
                        br.ForEach(b => matrixLeft[i, j] -= b.Y);
                    }
                }
            }

            var result =  CalculateSystemOfLinearEquations(matrixLeft, matrixRight);

            var nodalVoltages = new Dictionary<Node, Complex>();
            for (int i = 0; i < nodes.Count; i++)
            {
                nodalVoltages.Add(nodes[i], result[i]);
            }
            nodalVoltages.Add(baseNode, new Complex(0, 0));

            return nodalVoltages;
        }

        public static Complex[] CalculateSystemOfLinearEquations(Complex[,] matrixSystem, Complex[] matrixRightSide)
        {
            var matrixA = DenseMatrix.OfArray(matrixSystem);
            var vectorB = new DenseVector(matrixRightSide);
            var iterationCountStopCriterion = new IterationCountStopCriterion<Complex>(1000);
            var residualStopCriterion = new ResidualStopCriterion<Complex>(1e-10);
            var monitor = new Iterator<Complex>(iterationCountStopCriterion, residualStopCriterion);
            var solver = new BiCgStab();
            return matrixA.SolveIterative(vectorB, solver, monitor).ToArray();
        }


        internal static List<Branch> GetBranches(Circuit circuit)
        {
            var AllComponents = circuit.Components;
            var firstNode = circuit.Components.First(i => i is Node);
            var ListBranches = new List<Branch>();
            var ListPassedNodes = new List<Node>();
            var ListPassedComponents = new List<IComponentBase>();
            DFS(firstNode as Node, ListPassedNodes, ListPassedComponents, ListBranches);
            if (ListPassedNodes.Count + ListPassedComponents.Count != circuit.Components.Count) throw new Exception();
            return ListBranches;
        }

        private static void DFS(Node node, List<Node> passedNodes, List<IComponentBase> passedComponents, List<Branch> branches)
        {
            passedNodes.Add(node);

            foreach (Pin pin in node)
            {
                if (!passedComponents.Any(i => i.Equals(pin.ConnectedComponent)))
                {
                    var branch = new Branch(node);
                    branches.Add(branch);
                    var nextNode = FillBranches(branch, node, pin.ConnectedComponent, passedComponents);
                    if (!passedNodes.Any(i => i.Equals(nextNode)))
                    {
                        DFS(nextNode, passedNodes, passedComponents, branches);
                    }
                }
            }
        }

        private static Node FillBranches(Branch branch, IComponentBase previousComponent, IComponentBase component, List<IComponentBase> passedComponents)
        {
            branch.Add(component);
            passedComponents.Add(component);

            Pin pin = (component[0].ConnectedComponent.Equals(previousComponent)) ? component[1] : component[0];

            if (pin.ConnectedComponent is Node)
            {
                branch.Add(pin.ConnectedComponent);
                return pin.ConnectedComponent as Node;
            }
            else
            {
                return FillBranches(branch, component, pin.ConnectedComponent, passedComponents);
            }

            throw new InvalidOperationException(); // Схема не замкнута?
        }

    }
}
