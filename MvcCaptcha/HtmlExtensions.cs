
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace MvcCaptcha
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString MvcCaptcha(this HtmlHelper helper)
        {
            //Color.FromArgb(234, 237, 239)
            return MvcCaptcha(helper, Color.Transparent, Color.Transparent);
        }

        public static MvcHtmlString MvcCaptcha(this HtmlHelper helper, Color color1, Color color2)
        {
            var random = new Random();
            var num1 = random.Next(1, 25);
            var num2 = random.Next(1, 25);
            string imgString;

            using (var bmp = new Bitmap(181, 61))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    DrawRoundedRectangle(g, color1, color1, color2, 0, 0, 60, 180, 3);
                    //g.DrawString("您是人類？", new Font("Arial", 8), new SolidBrush(Color.DimGray), 2, 2);
                    g.DrawString(String.Format("{0} + {1} =", num1, num2), new Font("Courier", 16), new SolidBrush(Color.Black), 15, 20);

                    var memStream = new MemoryStream();
                    bmp.Save(memStream, ImageFormat.Png);
                    imgString = Convert.ToBase64String(memStream.ToArray());
                }
            }

            return new MvcHtmlString(String.Format(@"<div id=""captcha"">
                                                <img src=""data:image/png;base64,{0}""/>
                                                <input name=""CaptchaActual"" type=""text"" style=""position: absolute; width: 35px; margin-left: -{1}px; margin-top: 20px;"" />
                                                <input name=""CaptchaExpected"" type=""hidden"" value=""{2}"" />
                                               </div>", imgString, ((4 - (num1.ToString() + num2.ToString()).Length) * 15) + 55,
                                                            Convert.ToBase64String(Encoding.Unicode.GetBytes((num1 + num2).ToString()))));
        }

        public static void DrawRoundedRectangle(Graphics newGraphics, Color boxColor, Color gradFillColor1, Color gradFillColor2, int xPosition, int yPosition,
                   int height, int width, int cornerRadius)
        {
            using (var boxPen = new Pen(boxColor))
            {
                using (var path = new GraphicsPath())
                {
                    path.AddLine(xPosition + cornerRadius, yPosition, xPosition + width - (cornerRadius * 2), yPosition);
                    path.AddArc(xPosition + width - (cornerRadius * 2), yPosition, cornerRadius * 2, cornerRadius * 2, 270, 90);
                    path.AddLine(xPosition + width, yPosition + cornerRadius, xPosition + width,
                                 yPosition + height - (cornerRadius * 2));
                    path.AddArc(xPosition + width - (cornerRadius * 2), yPosition + height - (cornerRadius * 2), cornerRadius * 2,
                                cornerRadius * 2, 0, 90);
                    path.AddLine(xPosition + width - (cornerRadius * 2), yPosition + height, xPosition + cornerRadius,
                                 yPosition + height);
                    path.AddArc(xPosition, yPosition + height - (cornerRadius * 2), cornerRadius * 2, cornerRadius * 2, 90, 90);
                    path.AddLine(xPosition, yPosition + height - (cornerRadius * 2), xPosition, yPosition + cornerRadius);
                    path.AddArc(xPosition, yPosition, cornerRadius * 2, cornerRadius * 2, 180, 90);
                    path.CloseFigure();
                    newGraphics.DrawPath(boxPen, path);

                    var b = new LinearGradientBrush(new Point(xPosition, yPosition),
                                                    new Point(xPosition + width, yPosition + height), gradFillColor1,
                                                    gradFillColor2);

                    newGraphics.FillPath(b, path);
                }
            }
        }
    }
}
