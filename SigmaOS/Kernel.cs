using Cosmos.System.Graphics;
using System.Drawing;
using Sys = Cosmos.System;
using CurrentTime;
using Cosmos.System.Graphics.Fonts;
using Cosmos.System;
using Cosmos.Core;
using Cosmos.Core.Memory;
using DisplayTools;
using System;

namespace SigmaOS
{
    public class Kernel : Sys.Kernel
    {
        //Initialize variables
        VBECanvas canvas;
        Mode mode;
        uint swidth;
        uint sheight;
        int frames;
        long secondsFPS;
        int FPS;
        ulong TRAM;
        Boolean sysinfo;
        int HColltimer;
        string URAM;
        protected override void BeforeRun()
        {
            //Define screen width and height
            swidth = 1024;
            sheight = 768;

            //Get available RAM
            TRAM = GCImplementation.GetAvailableRAM();

            //Initialize FPS counter
            secondsFPS = Time.UnixTimeSeconds();

            //Define mouse screen width and height
            MouseManager.ScreenWidth = swidth;
            MouseManager.ScreenHeight = sheight;

            //Define canvas
            mode = new Mode(swidth, sheight, ColorDepth.ColorDepth32);
            canvas = new VBECanvas(mode);

            //Clear canvas
            canvas.Clear(Color.Blue);

            //Display buffered frame
            canvas.Display();
        }

        protected override void Run()
        {
            //Define strings
            string text = "Welcome to SigmaOS!";
            string timeString = Time.TimeString();

            //Clear canvas
            canvas.Clear(Color.Blue);

            //Define integers for text centering
            int textX1 = (int)(swidth / 2 - (text.Length * 8) / 2);
            int textX2 = (int)(swidth / 2 - (timeString.Length * 8) / 2);
            int textY1 = (int)(sheight / 2 - 16 / 2) - 20;
            int textY2 = (int)(sheight / 2 - 16 / 2);

            //Get mouse position
            uint mx = MouseManager.X;
            uint my = MouseManager.Y;
            //Draw mouse to frame buffer
            Draw.DrawCursor(canvas, mx, my, swidth, sheight);

            //Read currently pressed key
            KeyEvent keyevent;
            KeyboardManager.TryReadKey(out keyevent);
            //Execute code based on current key
            if (keyevent.Key == ConsoleKeyEx.R)
            {
                Power.Reboot();
            }
            else if (keyevent.Key == ConsoleKeyEx.S)
            {
                Power.Shutdown();
            }
            else if (keyevent.Key == ConsoleKeyEx.I)
            {
                sysinfo = !sysinfo;
            }

            //Reset FPS and used ram timer
            if (secondsFPS < Time.UnixTimeSeconds())
            {
                secondsFPS = Time.UnixTimeSeconds();
                FPS = frames;
                frames = 0;
                URAM = (GCImplementation.GetUsedRAM() / 1000000).ToString();
            }
            else
            {
                frames++;
            }

            //Draw center text to frame buffer
            canvas.DrawString(text, PCScreenFont.Default, Color.White, textX1, textY1);
            canvas.DrawString(timeString, PCScreenFont.Default, Color.White, textX2, textY2);
            //Draw system info to frame buffer
            canvas.DrawString("System Information (press i to toggle)", PCScreenFont.Default, Color.White, 20, 20);
            if (sysinfo == true)
            {
                canvas.DrawString("FPS: " + FPS.ToString(), PCScreenFont.Default, Color.White, 20, 40);
                canvas.DrawString("Used RAM / Available RAM: " + URAM + "/" + TRAM.ToString(), PCScreenFont.Default, Color.White, 20, 60);
                canvas.DrawString("Mouse x: " + mx.ToString() + " y: " + my.ToString(), PCScreenFont.Default, Color.White, 20, 80);
                canvas.DrawString("Version: 0.1.2", PCScreenFont.Default, Color.White, 20, 100);
            }
            //Draw bottom right text to frame buffer
            canvas.DrawString("Press r to restart, s to shutdown", PCScreenFont.Default, Color.White, 735, 750);
            //Display buffered frame
            canvas.Display();
            //Call Heap.Collect() every four frames
            if (HColltimer > 4)
            {
                HColltimer = 0;
                Heap.Collect();
            }
            else
            {
                HColltimer++;
            }
        }
    }
}