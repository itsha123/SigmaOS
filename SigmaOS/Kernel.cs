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
using IL2CPU.API.Attribs;

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
        [ManifestResourceStream(ResourceName = "SigmaOS.Resources.wallpaper.bmp")]
        private static byte[] wallpaperData;
        Bitmap bitmap;
        protected override void BeforeRun()
        {
            //Define screen width and height
            swidth = 1024;
            sheight = 768;

            //Load wallpaper
            bitmap = new Bitmap(wallpaperData);

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

            //Show startup screen
            Startup.StartupScreen(canvas, swidth, sheight);

            //Draw wallpaper
            canvas.DrawImage(bitmap, 0, 0, (int)swidth, (int)sheight);

            //Display buffered frame
            canvas.Display();
            bitmap = canvas.GetImage(0, 0, (int)swidth, (int)sheight);
        }

        protected override void Run()
        {
            //Define strings
            string text = "Welcome to SigmaOS!";
            string text2 = "Press r to restart, s to shutdown";

            //Draw wallpaper
            canvas.DrawImage(bitmap, 0, 0);

            //Define integers for text centering
            int textX1 = (int)(swidth / 2 - (text.Length * 8) / 2);
            int textX2 = (int)(swidth / 2 - (text2.Length * 8) / 2);
            int textY1 = (int)(sheight / 2 - 16 / 2) - 20;
            int textY2 = (int)(sheight / 2 - 16 / 2);

            //Get mouse position
            uint mx = MouseManager.X;
            uint my = MouseManager.Y;
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
            canvas.DrawString(text2, PCScreenFont.Default, Color.White, textX2, textY2);
            //Draw system info to frame buffer
            canvas.DrawString("System Information (press i to toggle)", PCScreenFont.Default, Color.White, 20, 20);
            if (sysinfo == true)
            {
                canvas.DrawString("FPS: " + FPS.ToString(), PCScreenFont.Default, Color.White, 20, 40);
                canvas.DrawString("Used RAM / Available RAM: " + URAM + "/" + TRAM.ToString(), PCScreenFont.Default, Color.White, 20, 60);
                canvas.DrawString("Mouse x: " + mx.ToString() + " y: " + my.ToString(), PCScreenFont.Default, Color.White, 20, 80);
                canvas.DrawString("Version: 0.1.2", PCScreenFont.Default, Color.White, 20, 100);
            }
            //Draw taskbar
            Draw.DrawTaskbar(canvas, swidth, sheight);
            //Draw mouse to frame buffer
            Draw.DrawCursor(canvas, mx, my, sheight);
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