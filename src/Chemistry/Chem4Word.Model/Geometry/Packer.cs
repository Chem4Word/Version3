// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chem4Word.Model.Geometry
{
    public class Packer
    {
        public Model Model { get; set; }

        public double Width { get; private set; }
        public double Height { get; private set; }

        private List<Node> _packed = new List<Node>();

        public Packer()
        {
            // Insert seed Node
            _packed.Add(new Node("", 0, 0, int.MaxValue, int.MaxValue));
        }

        public bool Pack(double padding)
        {
            if (Model != null)
            {
                var watch = Stopwatch.StartNew();

                List<Node> nodes = new List<Node>();

                foreach (var moleclue in Model.Molecules)
                {
                    nodes.Add(new Node(moleclue.Id, 0, 0, moleclue.BoundingBox.Width, moleclue.BoundingBox.Height));
                }

                //nodes.Sort((a, b) => b.Area.CompareTo(a.Area));
                nodes.Sort((a, b) => b.W.CompareTo(a.W));
                //nodes.Sort((a, b) => b.H.CompareTo(a.H));
                //nodes.Sort((a, b) => b.Perimeter.CompareTo(a.Perimeter));

                for (int i = 0; i < nodes.Count; ++i)
                {
                    Node node = nodes[i];

                    if (!Pack(node.Id, node.W + padding, node.H + padding, out node.X, out node.Y))
                    {
                        Debug.WriteLine("Packing failed!");
                    }

                    nodes[i] = node;
                }

                int packTime = (int)watch.ElapsedMilliseconds;
                Debug.WriteLine($"Packing took {packTime}ms");
                Debug.WriteLine($"Width: {Width} Height {Height}");

                foreach (var moleclue in Model.Molecules)
                {
                    foreach (var node in nodes)
                    {
                        if (node.Id.Equals(moleclue.Id))
                        {
                            double dx = node.X - moleclue.BoundingBox.X;
                            double dy = node.Y - moleclue.BoundingBox.Y;
                            moleclue.MoveAllAtoms(dx, dy);
                        }
                    }
                }
            }

            return true;
        }

        private bool Pack(string id, double w, double h, out double x, out double y)
        {
            for (int i = 0; i < _packed.Count; ++i)
            {
                if (w <= _packed[i].W && h <= _packed[i].H)
                {
                    var node = _packed[i];
                    _packed.RemoveAt(i);
                    x = node.X;
                    y = node.Y;
                    double r = x + w;
                    double b = y + h;
                    _packed.Add(new Node(id, r, y, node.Right - r, h));
                    _packed.Add(new Node(id, x, b, w, node.Bottom - b));
                    _packed.Add(new Node(id, r, b, node.Right - r, node.Bottom - b));
                    Width = Math.Max(Width, r);
                    Height = Math.Max(Height, b);

                    // Good Exit
                    return true;
                }
            }

            x = 0;
            y = 0;

            // Bad Exit
            return false;
        }
    }

    public struct Node
    {
        public string Id;
        public double X;
        public double Y;
        public double W;
        public double H;

        public Node(string id, double x, double y, double w, double h)
        {
            Id = id;
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public double Right => X + W;

        public double Bottom => Y + H;

        public double Area => W * H;

        public double Perimeter => W * 2 + H * 2;
    }
}