using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;
using System.IO;

namespace Shaders_and_raytracing
{
    class GLgraphics
    {
        Vector3 cameraPosition = new Vector3(0, 0, 0.8f);
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 1, 0);

        public float latitude = 47.98f;
        public float longitude = 60.41f;
        public float radius = 5.385f;

        public int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;
        int vaoHandle;

       // string glVersion = GL.GetString(StringName.Version);
       // string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);

        public void Setup(int width, int height)
        {
            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                width / (float)height,
                1,
                64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            cameraPosition = new Vector3(
                (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Cos(Math.PI / 180.0f * longitude)),
                (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Sin(Math.PI / 180.0f * longitude)),
                (float)(radius * Math.Sin(Math.PI / 180.0f * latitude)));

            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            Render();
        }

        private void Draw()
        {
            //GL.Begin(PrimitiveType.Polygon);

            //GL.Color3(Color.Blue);
            //GL.Vertex3(-1.0f, -1.0f, -1.0f);

            //GL.Color3(Color.Red);
            //GL.Vertex3(-1.0f, 1.0f, -1.0f);

            //GL.Color3(Color.White);
            //GL.Vertex3(1.0f, 1.0f, -1.0f);

            //GL.End();

            InitShaders();

            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.UseProgram(0);

        }

        public void Render()
        {
            Draw();
        }

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private void InitShaders()
        {
            float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
            float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };
            int[] vboHandlers = new int[2];

            BasicProgramID = GL.CreateProgram();
            loadShader("..\\..\\basic.vs.txt", ShaderType.VertexShader, BasicProgramID,
            out BasicVertexShader);
            loadShader("..\\..\\basic.fs.txt", ShaderType.FragmentShader, BasicProgramID,
            out BasicFragmentShader);

            GL.LinkProgram(BasicProgramID);

            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));

            GL.GenBuffers(2, vboHandlers);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                            (IntPtr)(sizeof(float) * positionData.Length),
                            positionData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                            (IntPtr)(sizeof(float) * colorData.Length),
                             colorData, BufferUsageHint.StaticDraw);

            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }
    }
}