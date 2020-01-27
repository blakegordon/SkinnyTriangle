using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Power4
{
    public partial class Form1 : Form
    {
        PresentParameters pParams;
        Device device;
        VertexBuffer buffer;
        Type vertexType = typeof(CustomVertex.TransformedColored);
        const VertexFormats format = VertexFormats.Diffuse | VertexFormats.Transformed;

        public Form1()
        {
            InitializeComponent();
            pParams = new PresentParameters { Windowed = true, SwapEffect = SwapEffect.Discard };
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, pParams);
            buffer = new VertexBuffer(vertexType, 3, device, Usage.None, format, Pool.Default);
            DefineTriange();
        }

        private void DefineTriange()
        {
            GraphicsStream stream = buffer.Lock(0, 0, 0);
            var vert0 = new CustomVertex.TransformedColored { Color = 128, Rhw = 1, X = 150, Y = 50, Z = .5F };
            var vert1 = new CustomVertex.TransformedColored { Color = 8192, Rhw = 1, X = 250, Y = 250, Z = .5F };
            var vert2 = new CustomVertex.TransformedColored { Color = 327670, Rhw = 1, X = 50, Y = 250, Z = .5F };
            stream.Write(new[] { vert0, vert1, vert2 });
            buffer.Unlock();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            device.BeginScene();
            device.SetStreamSource(0, buffer, 0);
            device.VertexFormat = format;
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            device.EndScene();
            if (device.CheckCooperativeLevel())
            {
                device.Present();
            }
            else
            {
                Thread.Sleep(100);
                device.Reset(pParams);
            }
        }
    }
}
