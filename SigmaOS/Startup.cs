using Cosmos.Core;
using Cosmos.HAL;
using Cosmos.System.Audio.IO;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SigmaOS
{
    public class Startup
    {
        [ManifestResourceStream(ResourceName = "SigmaOS.Resources.moai.bmp")]
        private static byte[] moaiImage;
        public static void StartupScreen(VBECanvas canvas, uint swidth, uint sheight)
        {
            canvas.DrawImage(new Bitmap(moaiImage), (int)swidth / 2 - 256, (int)sheight / 2 - 350);
            canvas.DrawString("SigmaOS", PCScreenFont.Default, Color.White, (int)(swidth / 2 - ("SigmaOS".Length * 8) / 2), 600);
            canvas.Display();
            var pit = new PIT();
            pit.Wait(3000);
        }
    }
}
