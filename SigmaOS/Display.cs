using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

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
        public static void DrawRoundedRectangle(VBECanvas canvas, Color color, int x, int y, int width, int height, int radius)
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
                    // Top and bottom straight edges
                    if ((yy == y && xx >= cxLeft && xx <= cxRight) ||
                        (yy == y1 && xx >= cxLeft && xx <= cxRight))
                    {
                        canvas.DrawPoint(color, xx, yy);
                        continue;
                    }

                    // Left and right straight edges
                    if ((xx == x && yy >= cyTop && yy <= cyBottom) ||
                        (xx == x1 && yy >= cyTop && yy <= cyBottom))
                    {
                        canvas.DrawPoint(color, xx, yy);
                        continue;
                    }

                    // Four rounded corners: draw when a point lies approximately on the circle perimeter
                    int dx, dy, dSq;
                    if (xx < cxLeft && yy < cyTop)    // top-left
                    {
                        dx = xx - cxLeft; dy = yy - cyTop;
                    }
                    else if (xx > cxRight && yy < cyTop)   // top-right
                    {
                        dx = xx - cxRight; dy = yy - cyTop;
                    }
                    else if (xx < cxLeft && yy > cyBottom) // bottom-left
                    {
                        dx = xx - cxLeft; dy = yy - cyBottom;
                    }
                    else if (xx > cxRight && yy > cyBottom) // bottom-right
                    {
                        dx = xx - cxRight; dy = yy - cyBottom;
                    }
                    else
                    {
                        continue;
                    }

                    dSq = dx * dx + dy * dy;
                    // if distance² is within ~radius of the perfect circle, plot it
                    if (System.Math.Abs(dSq - rSq) <= radius)
                    {
                        canvas.DrawPoint(color, xx, yy);
                    }
                }
            }
        }
        public static void DrawTaskbar(VBECanvas canvas, uint swidth, uint sheight)
        {
            DrawRoundedFilledRectangle(canvas, Color.FromArgb(200, 255, 255, 255), 10, (int)sheight - 65, (int)swidth - 20, 55, 10);
            DrawRoundedRectangle(canvas, Color.FromArgb(205, 255, 255, 255), 10, (int)sheight - 65, (int)swidth - 20, 55, 10);
            canvas.DrawString(DateTime.Now.ToString("hh:mm:ss tt"), PCScreenFont.Default, Color.Black, (int)swidth - 25 - DateTime.Now.ToString("hh:mm:ss tt").Length * 8, (int)sheight - 55);
            canvas.DrawString(DateTime.Now.ToString("M/d/yyyy"), PCScreenFont.Default, Color.Black, (int)swidth - 25 - DateTime.Now.ToString("M/d/yyyy").Length * 8, (int)sheight - 35);
        }
    }
}