using Cosmos.System.Graphics;
using System.Drawing;
using System.Numerics;

namespace DisplayTools
{
    public class Draw
    {
        public static void DrawCursor(VBECanvas canvas, uint mx, uint my, uint screenheight)
        {
            //Cut off cursor if at edge of screen
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
        public static void DrawRoundedFilledRectangle(VBECanvas canvas, Color color, int x, int y, int width, int height, int radius)
        {
            int x1 = x + width - 1;
            int y1 = y + height - 1;
            int rSq = radius * radius;

            int cxLeft = x + radius;
            int cxRight = x1 - radius;
            int cyTop = y + radius;
            int cyBottom = y1 - radius;

            for (int yy = y; yy <= y1; yy++)
            {
                for (int xx = x; xx <= x1; xx++)
                {
                    bool fill = true;

                    if (xx < cxLeft && yy < cyTop)
                    {
                        if ((xx - cxLeft) * (xx - cxLeft) + (yy - cyTop) * (yy - cyTop) > rSq) fill = false;
                    }
                    else if (xx > cxRight && yy < cyTop)
                    {
                        if ((xx - cxRight) * (xx - cxRight) + (yy - cyTop) * (yy - cyTop) > rSq) fill = false;
                    }
                    else if (xx < cxLeft && yy > cyBottom)
                    {
                        if ((xx - cxLeft) * (xx - cxLeft) + (yy - cyBottom) * (yy - cyBottom) > rSq) fill = false;
                    }
                    else if (xx > cxRight && yy > cyBottom)
                    {
                        if ((xx - cxRight) * (xx - cxRight) + (yy - cyBottom) * (yy - cyBottom) > rSq) fill = false;
                    }

                    if (fill)
                    {
                        canvas.DrawPoint(color, xx, yy);
                    }
                }
            }
        }
    }
}