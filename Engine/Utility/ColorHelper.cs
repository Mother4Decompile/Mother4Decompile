// Decompiled with JetBrains decompiler
// Type: Carbine.Utility.ColorHelper
// Assembly: Carbine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F138E832-4582-46AC-9AAC-9FED0C56FD24
// Assembly location: D:\OddityPrototypes\Mother 4 -- 2018\Carbine.dll

using SFML.Graphics;
using System;
using System.Globalization;

namespace Carbine.Utility
{
    public static class ColorHelper
    {
        public static Color FromInt(int color) => ColorHelper.FromInt((uint)color);

        public static Color FromHexString(string hexString)
        {
            try
            {
                return ColorHelper.FromInt(int.Parse(hexString, NumberStyles.HexNumber));
            }
            catch (Exception ex)
            {
                return Color.White;
            }
        }

        public static Color FromInt(uint color)
        {
            byte alpha = (byte)(color >> 24);
            return new Color((byte)(color >> 16), (byte)(color >> 8), (byte)color, alpha);
        }

        public static Color Blend(Color col1, Color col2, float amount)
        {
            float num = 1f - amount;
            return new Color((byte)((double)col1.R * (double)num + (double)col2.R * (double)amount), (byte)((double)col1.G * (double)num + (double)col2.G * (double)amount), (byte)((double)col1.B * (double)num + (double)col2.B * (double)amount), byte.MaxValue);
        }

        public static Color BlendAlpha(Color col1, Color col2, float amount)
        {
            float num = 1f - amount;
            return new Color((byte)((double)col1.R * (double)num + (double)col2.R * (double)amount), (byte)((double)col1.G * (double)num + (double)col2.G * (double)amount), (byte)((double)col1.B * (double)num + (double)col2.B * (double)amount), (byte)((double)col1.A * (double)num + (double)col2.A * (double)amount));
        }

        public static Color Invert(this Color color) => new Color((byte)((uint)byte.MaxValue - (uint)color.R), (byte)((uint)byte.MaxValue - (uint)color.G), (byte)((uint)byte.MaxValue - (uint)color.B), color.A);
    }
}
