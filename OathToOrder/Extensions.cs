#region Header

// OathToOrder >OathToOrder >Extensions.cs\n Copyright (C) Adonis Deliannis, 2020\nCreated 18 04, 2020

#endregion

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using Newtonsoft.Json;

namespace OathToOrder {
    internal static class Extensions {
        public static bool IsEmpty(this string s) {
            return string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
        }

        public static string ToJson(this Config self) {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }

        public static double ToByte< T >(this T t) where T : struct,
            IComparable,
            IComparable< T >,
            IConvertible,
            IEquatable< T >,
            IFormattable {
            return t.ToDouble(CultureInfo.InvariantCulture);
        }

        public static double ToKilobyte< T >(this T t) where T : struct,
            IComparable,
            IComparable< T >,
            IConvertible,
            IEquatable< T >,
            IFormattable {
            return Math.Round(t.ToDouble(CultureInfo.InvariantCulture) / 1024);
        }

        public static Bitmap To16bppBitmap(this Image img) {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format16bppRgb555);
            using Graphics gr = Graphics.FromImage(bmp);
            gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }
    }
}