// ColorHex struct - Mainly used as a way to create a Color32 using a hex code
// @Author: Travis Abendshien (https://github.com/CyanVoxel)
// @Version: 1.1

// A simple C# script that allows for creating UnityEngine Color32 variables via a hex code string
// Getting Started

// Getting ColorHex into your Unity project is as simple as dropping the ColorHex.cs file into your Assets/Scripts project folder.
// Usage

// In order to have access to ColorHex in your script, make sure to include using ColorHexUtility; at the top of any script that requires it.

// Instead of using Color32’s constructor which requires you to pass byte values, you can now use the ColorHex constructor to pass in a hex code string. Since ColorHex is designed to be interoperable with Color32, an implementation can look like this:

// public Color32 tintRed500 = new ColorHex("#F14234");



namespace ColorHexUtility
{
    public struct ColorHex
    {
        byte r;
        byte g;
        byte b;
        byte a;

        // Normal Color32-style byte constructor.
        public ColorHex(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        // String hex constructor, handles optional '#' character as well as optional alpha values.
        public ColorHex(string hex)
        {
            string h = hex;

            if (h.Contains("#"))
            {
                h = h.Remove(hex.IndexOf("#"), 1);
            }

            switch (h.Length)
            {
                case 6:
                    this.r = byte.Parse(h.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    this.g = byte.Parse(h.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    this.b = byte.Parse(h.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    this.a = 255;
                    break;

                case 8:
                    this.r = byte.Parse(h.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    this.g = byte.Parse(h.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    this.b = byte.Parse(h.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    this.a = byte.Parse(h.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    break;

                default:
                    this.r = 0;
                    this.g = 0;
                    this.b = 0;
                    this.a = 0;
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            bool typeCheck = false;

            if (this.GetType().Equals(obj.GetType()) || obj is UnityEngine.Color32)
            {
                typeCheck = true;
            }

            if (obj == null || !typeCheck)
            {
                return false;
            }
            else
            {
                ColorHex c = (ColorHex)obj;
                return (r == c.r && g == c.g && b == c.b && a == c.a);
            }
        }

        public static bool operator ==(UnityEngine.Color32 left, ColorHex right)
        {
            if (left.r == right.r
                && left.g == right.g
                && left.b == right.b
                && left.a == right.a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(ColorHex left, ColorHex right)
        {
            if (left.r == right.r
                && left.g == right.g
                && left.b == right.b
                && left.a == right.a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(UnityEngine.Color32 left, ColorHex right)
        {
            if (left.r != right.r
                || left.g != right.g
                || left.b != right.b
                || left.a != right.a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(ColorHex left, ColorHex right)
        {
            if (left.r != right.r
                || left.g != right.g
                || left.b != right.b
                || left.a != right.a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static implicit operator UnityEngine.Color32(ColorHex c)
        {
            return new UnityEngine.Color32(c.r, c.g, c.b, c.a);
        }

        public override int GetHashCode()
        {
            return r ^ g ^ b ^ a;
        }
    }
}
