using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ShaderEx
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new GameWindow())
            {
                Vector2[] trianglePoints = new Vector2[] { new Vector2(-1.0f, 1.0f), new Vector2(0.0f, -1.0f), new Vector2(1.0f, 1.0f) };
                int pointSelected = 0;
                bool spacePressedLastFrame = false;

                game.Load += (sender, e) =>
                {
                    game.VSync = VSyncMode.On;
                };

                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };

                game.UpdateFrame += (sender, e) =>
                {
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }


                    // Handle keyboard input

                    bool spacePressed = game.Keyboard[Key.Space];
                    // Check if point selection has changed
                    if (spacePressed && !spacePressedLastFrame)
                    {
                        pointSelected++;
                        if (pointSelected >= 3)
                        {
                            pointSelected = 0;
                        }
                    }
                    spacePressedLastFrame = spacePressed;

                    // Handle keyboard input
                    if (game.Keyboard[Key.Up])
                    {
                        trianglePoints[pointSelected].Y += 0.1f;
                    }
                    else if (game.Keyboard[Key.Down])
                    {
                        trianglePoints[pointSelected].Y -= 0.1f;
                    }

                    if (game.Keyboard[Key.Left])
                    {
                        trianglePoints[pointSelected].X -= 0.1f;
                    }
                    else if (game.Keyboard[Key.Right])
                    {
                        trianglePoints[pointSelected].X += 0.1f;
                    }
                };

                game.RenderFrame += (sender, e) =>
                {
                    // Render graphics

                    // Set background color and clear the screen
                    //GL.ClearColor(Color.Red);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    // Load the current camera projection matrix 
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

                    // Draw the triangle
                    GL.Begin(PrimitiveType.Triangles);

                    GL.Color3(Color.MidnightBlue);
                    GL.Vertex2(trianglePoints[0].X, trianglePoints[0].Y);
                    GL.Color3(Color.SpringGreen);
                    GL.Vertex2(trianglePoints[1].X, trianglePoints[1].Y);
                    GL.Color3(Color.Ivory);
                    GL.Vertex2(trianglePoints[2].X, trianglePoints[2].Y);

                    GL.End();

                    game.SwapBuffers();
                };

                game.Run(60.0);
            }
        }
    }
}
