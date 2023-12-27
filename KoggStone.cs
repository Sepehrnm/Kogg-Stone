using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace koggstone
{
    internal class KoggStone
    {
        public class Layer
        {
            public Layer()
            {
                Nodes = new List<int>();
            }
            public List<int> Nodes { get; set; }

            public Layer CreateNextLayer(int layerIndedx)
            {
                var layer = new Layer();
                var c = (int)Math.Pow(2, layerIndedx);
                for (int i = 0; i < c && i < Nodes.Count; i++)
                {
                    layer.Nodes.Add(Nodes[i]);
                }
                for (int i = c; i < Nodes.Count; i++)
                {
                    layer.Nodes.Add(Nodes[i] + Nodes[i - c]);
                }
                return layer;
            }

            public void Log()
            {
                for (int i = Nodes.Count - 1; i >= 0; i--)
                {
                    Console.Write(Nodes[i] + "  ");
                }
                Console.WriteLine();
            }
        }

        public List<Layer> Layers { get; set; }

        public void Create(int[] nodesValues)
        {
            Layers = new List<Layer>();

            Layer layer = new Layer();
            for (int i = nodesValues.Length - 1; i >= 0; i--)
            {
                layer.Nodes.Add(nodesValues[i]);
            }
            Layers.Add(layer);

            for (int i = 0; Math.Pow(2, i) < nodesValues.Length; i++)
            {
                Layers.Add(Layers[i].CreateNextLayer(i));
            }

        }

        public void Log()
        {
            foreach (var l in Layers)
                l.Log();
        }

        public Bitmap CreateImage(int dx, int dy, int fs)
        {
            var nc = Layers[0].Nodes.Count;

            int w = (nc + 1) * dx;
            int h = (Layers.Count ) * (2 * dy);

            var linePen = new Pen(Color.Black, 3);
            var nodePen = new Pen(Color.Red, 3);
            var font = new Font("Tahoma", fs);

            Bitmap bmp = new Bitmap(w, h);
            Graphics gr = Graphics.FromImage(bmp);
            gr.Clear(Color.White);

            for (int i = 0; i < nc; i++)
            {
                gr.DrawLine(linePen, dx + i * dx, dy, dx + i * dx, h - dy);
            }

            for (int i = 0; i < Layers.Count; i++)
            {
                for (int j = 0; j < nc; j++)
                {
                    var x = dx + j * dx;
                    var y = dy + i * 2 * dy + 5;
                    gr.DrawString(Layers[i].Nodes[nc - j - 1] + "", font, Brushes.Black, x, y);
                }
            }

            for (int i = 1; i < Layers.Count; i++)
            {
                var c = (int)Math.Pow(2, i - 1);
                for(int j = c; j < nc; j++)
                {
                    var x1 = dx + (nc - j + c - 1) * dx;
                    var y1 = 2 * dy + (i - 1) * 2 * dy; 

                    var x2 = dx + (nc - j - 1) * dx;
                    var y2 = y1 + dy;
                    
                    gr.DrawLine(linePen, x1, y1, x2, y2);
                    gr.FillEllipse(Brushes.Red, new Rectangle(x2 - 5, y2 - 5, 10, 10));
                }
            }

            gr.Dispose();
            return bmp;
        }
    }
}
