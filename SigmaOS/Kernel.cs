using Cosmos.System.Graphics;
using System.Drawing;
using Sys = Cosmos.System;
using CurrentTime;
using Cosmos.System.Graphics.Fonts;

namespace SigmaOS
{
    public class Kernel : Sys.Kernel
    {
        VBECanvas canvas;
        Time time;
        Mode mode;
        uint swidth;
        uint sheight;
        protected override void BeforeRun()
        {
                swidth = 1024;
                sheight = 768;
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

            canvas.DrawString(text, PCScreenFont.Default, Color.White, textX1, textY1);
            canvas.DrawString(timeString, PCScreenFont.Default, Color.White, textX2, textY2);
            canvas.Display();
        }
    }
}
//What to add next: RAM Info, CPU time