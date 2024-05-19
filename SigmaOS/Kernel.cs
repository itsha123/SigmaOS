using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sys = Cosmos.System;
using CurrentTime;
using Cosmos.System.Graphics.Fonts;

namespace SigmaOS
{
    public class Kernel : Sys.Kernel
    {
        VBECanvas canvas;
        Time time;
        Font font;
        Mode mode;
        protected override void BeforeRun()
        {
            mode = new Mode(800, 600, ColorDepth.ColorDepth32);
            canvas = new VBECanvas(mode);
            canvas.Clear(Color.Blue);
            canvas.Display();
        }

        protected override void Run()
        {
            canvas.Clear(Color.Blue);
            canvas.DrawString("Welcome to SigmaOS!", PCScreenFont.Default, Color.White, 348, 274);
            canvas.DrawString(time.TimeString(), PCScreenFont.Default, Color.White, 348, 292);
            canvas.Display();
        }
    }
}
