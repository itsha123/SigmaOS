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
        VBECanvas canvas;
        Time time;
        Mode mode;
        uint swidth;
        uint sheight;
        uint mx;
        uint my;
        int frames = 0;
        long secondsFPS;
        int FPS;
        ulong TRAM;
        Boolean sysinfo;
        Boolean rbdown;
        Boolean rbwdown;
        int HColltimer;
        protected override void BeforeRun()
        {
            swidth = 1024;
            sheight = 768;
            TRAM = GCImplementation.GetAvailableRAM();
            secondsFPS = time.UnixTimeSeconds();
            MouseManager.ScreenWidth = swidth;
            MouseManager.ScreenHeight = sheight;
            mode = new Mode(swidth, sheight, ColorDepth.ColorDepth32);
            canvas = new VBECanvas(mode);
            canvas.Clear(Color.Blue);
            canvas.Display();
        }

        protected override void Run()
        {
            string text = "Welcome to SigmaOS!";
            string timeString = time.TimeString();

            canvas.Clear(Color.Blue);

            int textX1 = (int)(swidth / 2 - (text.Length * 8) / 2);
            int textX2 = (int)(swidth / 2 - (timeString.Length * 8) / 2);
            int textY1 = (int)(sheight / 2 - 16 / 2) - 20;
            int textY2 = (int)(sheight / 2 - 16 / 2);
            mx = MouseManager.X;
            my = MouseManager.Y;

            canvas.DrawString(text, PCScreenFont.Default, Color.White, textX1, textY1);
            canvas.DrawString(timeString, PCScreenFont.Default, Color.White, textX2, textY2);
            canvas.DrawString("System Information (press i to toggle)", PCScreenFont.Default, Color.White, 20, 20);
            if (sysinfo == true)
            {
                canvas.DrawString("FPS: " + FPS.ToString(), PCScreenFont.Default, Color.White, 20, 40);
                canvas.DrawString("Used RAM / Available RAM: " + (GCImplementation.GetUsedRAM() / 1000000).ToString() + "/" + TRAM.ToString(), PCScreenFont.Default, Color.White, 20, 60);
                canvas.DrawString("Mouse x: " + mx.ToString() + " y: " + my.ToString(), PCScreenFont.Default, Color.White, 20, 80);
            }
            canvas.DrawString("Press r to restart, s to shutdown", PCScreenFont.Default, Color.White, 735, 750);
            Draw.DrawCursor(canvas, mx, my, swidth, sheight);
            canvas.Display();
            KeyEvent keyevent;
            KeyboardManager.TryReadKey(out keyevent);
            if (keyevent.Key == ConsoleKeyEx.R)
            {
                Power.Reboot();
            }
            if (keyevent.Key == ConsoleKeyEx.S)
            {
                Power.Shutdown();
            }
            if (keyevent.Key == ConsoleKeyEx.I)
            {
                rbdown = true;
            } else
            {
                rbdown = false;
            }
            if (rbdown == true && rbwdown == false)
            {
                sysinfo = !sysinfo;
                rbwdown = true;
            } else if (rbdown == false)
            {
                rbwdown = false;
            }
            if (secondsFPS < time.UnixTimeSeconds())
            {
                secondsFPS = time.UnixTimeSeconds();
                FPS = frames;
                frames = 0;
            } else
            {
                frames++;
            }
            if (HColltimer > 4)
            {
                HColltimer = 0;
                Heap.Collect();
            } else
            {
                HColltimer++;
            }
        }
    }
}