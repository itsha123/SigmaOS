using Cosmos.System.Graphics;
using System.Drawing;
using System.Numerics;

namespace DisplayTools
{
    public class Draw
    {
        public static void DrawCursor(VBECanvas canvas, uint mx, uint my, uint screenwidth, uint screenheight)
        {
            int closeness = (int)(screenheight - my);
            if (closeness < 6)
            {
                canvas.DrawFilledRectangle(Color.Black, (int)mx, (int)my, 6, 6-(6-closeness));
                canvas.DrawFilledRectangle(Color.White, (int)mx, (int)my, 4, 4-(6-closeness));
            } else
            {
                canvas.DrawFilledRectangle(Color.Black, (int)mx, (int)my, 6, 6);
                canvas.DrawFilledRectangle(Color.White, (int)mx, (int)my, 4, 4);
            }
        }
    }
}