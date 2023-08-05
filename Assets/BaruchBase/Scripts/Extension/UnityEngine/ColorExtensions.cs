using System;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch.Extension
{
    public static class ColorExtensions
    {
        public static void SetAlpha(ref this Color target, float a)
        {
            target = new Color(target.r, target.g, target.b, a);
        }

        public static Color AsValue(ref this Color target, float V)
        {
            Color.RGBToHSV(target, out float h, out float s, out _);
            return Color.HSVToRGB(h, s, V);
            
        }public static void SetSaturation(ref this Color target, float S)
        {
            Color.RGBToHSV(target, out float h, out float s, out float v);
            target = Color.HSVToRGB(h, S, v);
        }
        public static void SetHue(ref this Color target, float H)
        {
            Color.RGBToHSV(target,out float h, out float s, out float v);
            target = Color.HSVToRGB(H,s,v);
        }
    }
}