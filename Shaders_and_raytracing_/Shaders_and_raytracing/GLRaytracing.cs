using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;
using System.IO;

namespace Shaders_and_raytracing
{
    class GLRaytracing
    {
        Vector3 cameraPosition = new Vector3(2, 3, 4);
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);
        Vector3 campos = new Vector3(0.0f, 0.0f, 0.8f);
        float[] vertdata = { -1f, -1f, 0.0f, -1f, 1f, 0.0f, 1f, -1f, 0.0f, 1f, 1f, 0f };
        
        public float latitude = 47.98f;
        public float longitude = 60.41f;
        public float radius = 5.385f;
        
        public int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;
        int width, height;
        int vertexbuffer;

        // string glVersion = GL.GetString(StringName.Version);
        // string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);

        public void Setup(int _width, int _height)
        {
            width = _width;
            height = _height;

            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)width / height,
                1,
                64);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);

            InitShaders();
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            cameraPosition = new Vector3(0, 0, 0.8f);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            Render();
        }

        private void Draw()
        {
            GL.UseProgram(BasicProgramID);
            GL.Uniform3(GL.GetUniformLocation(BasicProgramID, "campos"), cameraPosition);
            GL.Uniform1(GL.GetUniformLocation(BasicProgramID, "aspect"), width / (float)height);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            GL.UseProgram(0);
        }

        public void Render()
        {
            cameraPosition += new Vector3(0, 0, 0.2f);
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
            BasicProgramID = GL.CreateProgram();
            loadShader("raytracing.vert.txt", ShaderType.VertexShader, BasicProgramID,
            out BasicVertexShader);
            loadShader("raytracing.frag.txt", ShaderType.FragmentShader, BasicProgramID,
            out BasicFragmentShader);

            GL.LinkProgram(BasicProgramID);

            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));

            GL.GenBuffers(1, out vertexbuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          (IntPtr)(sizeof(float) * vertdata.Length),
                           vertdata, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void closeProgram()
        {
            GL.UseProgram(0);
        }
    }
}


