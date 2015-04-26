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
    class ColoredTriangle
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new GameWindow())
            {
                // Initialize all used variables
                Vector2[] trianglePoints = new Vector2[] { new Vector2(-1.0f, 1.0f), new Vector2(0.0f, -1.0f), new Vector2(1.0f, 1.0f) };
                Color4[] triangleColors = new Color4[] { Color4.MidnightBlue, Color4.SpringGreen, Color4.Ivory };
                int pointSelected = 0;
                int colorSelected = 0;
                bool spacePressedLastFrame = false;

                // Enable VSync so that some CPU cycles are released
                game.Load += (sender, e) =>
                {
                    game.VSync = VSyncMode.On;
                };

                // Standard OpenGL/OpenTk resize method
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
                        Console.WriteLine(string.Format("Point selected: {0}", pointSelected));
                    }
                    spacePressedLastFrame = spacePressed;

                    // Move position of selected point with arrow keys
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

                    // Find the color element that is selected (R, G, B)
                    if (game.Keyboard[Key.R])
                    {
                        colorSelected = 0;
                        Console.WriteLine("Red selected.");
                    }
                    else if (game.Keyboard[Key.G])
                    {
                        colorSelected = 1;
                        Console.WriteLine("Green selected.");
                    }
                    else if (game.Keyboard[Key.B])
                    {
                        colorSelected = 2;
                        Console.WriteLine("Blue selected.");
                    }

                    // Change selected color element
                    if (game.Keyboard[Key.Plus] || game.Keyboard[Key.Minus])
                    {
                        // If the plus key is pressed, add to the value. Otherwise, subtract.
                        int modifier = game.Keyboard[Key.Plus] ? 1 : -1;

                        // Modify the chosen color element
                        if (colorSelected == 0) // Red element 
                        {
                            // Ensure values of greater than 0 or less than 1
                            if ((triangleColors[pointSelected].R < 1.0f && modifier == 1) || (triangleColors[pointSelected].R > 0.0f && modifier == -1))
                            {
                                triangleColors[pointSelected].R += ((1.0f / 255.0f) * modifier);
                                Console.WriteLine(string.Format("Red changed: {0}", triangleColors[pointSelected].R));
                            }
                        }
                        else if (colorSelected == 1) // Green element
                        {
                            // Ensure values of greater than 0 or less than 1
                            if ((triangleColors[pointSelected].G < 1.0f && modifier == 1) || (triangleColors[pointSelected].G > 0.0f && modifier == -1))
                            {
                                triangleColors[pointSelected].G += ((1.0f / 255.0f) * modifier);
                                Console.WriteLine(string.Format("Green changed: {0}", triangleColors[pointSelected].G));
                            }
                        }
                        else if (colorSelected == 2) // Blue element 
                        {
                            // Ensure values of greater than 0 or less than 1
                            if ((triangleColors[pointSelected].B < 1.0f && modifier == 1) || (triangleColors[pointSelected].B > 0.0f && modifier == -1))
                            {
                                triangleColors[pointSelected].B += ((1.0f / 255.0f) * modifier);
                                Console.WriteLine(string.Format("Blue changed: {0}", triangleColors[pointSelected].B));
                            }
                        }
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

                    GL.Color4(triangleColors[0]);
                    GL.Vertex2(trianglePoints[0].X, trianglePoints[0].Y);
                    GL.Color4(triangleColors[1]);
                    GL.Vertex2(trianglePoints[1].X, trianglePoints[1].Y);
                    GL.Color4(triangleColors[2]);
                    GL.Vertex2(trianglePoints[2].X, trianglePoints[2].Y);

                    GL.End();

                    // Swap the buffers to display the current one
                    game.SwapBuffers();
                };

                game.Run(60.0);
            }
        }
    }
}
