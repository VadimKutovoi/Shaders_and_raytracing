using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Shaders_and_raytracing
{
    public partial class Form1 : Form
    {
        GLgraphics glgraphics = new GLgraphics();
        GLRaytracing raytracing = new GLRaytracing(); 

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            //glgraphics.Setup(glControl1.Width, glControl1.Height);
            raytracing.Setup(glControl1.Width, glControl1.Height);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            //glgraphics.Update();
            raytracing.Update();
            glControl1.SwapBuffers();
            raytracing.closeProgram();
        }

        private void Application_Idle(object sender, PaintEventArgs e)
        {

            glControl1_Paint(sender, e);

        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            //float widthCoef = (e.X - glControl1.Width * 0.5f) / (float)glControl1.Width;
            //float heightCoef = (-e.Y + glControl1.Height * 0.5f) / (float)glControl1.Height;
            //glgraphics.latitude = heightCoef * 180;
            //glgraphics.longitude = widthCoef * 360;

        }
    }
}
