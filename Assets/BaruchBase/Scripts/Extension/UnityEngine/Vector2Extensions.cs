using System;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch.Extension
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Rotates vector by degrees
        /// </summary>
        /// <param name="v"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
        /// <summary>
        /// Returns (X,Y) to (X,0,Y)
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 TopDown(this Vector2 target)
        {
            return new Vector3(target.x, 0, target.y);

        }
        public static Vector2 RemapMagnitude(this Vector2 target,float fromMin,float fromMax,float toMin,float toMax)
        {
            return target.normalized * target.magnitude.Remap(fromMin, fromMax, toMin, toMax);
        }

        
        /**
      * <summary>Set X value of this vector</summary>
      */
        public static Vector2 SetX(ref this Vector2 target, float x)
        {
            target.Set(x, target.y);
            return target;

        }
        /**
     * <summary>Set Y value of this vector</summary>
     */
        public static Vector2 SetY(ref this Vector2 target, float y)
        {
            target.Set(target.x, y);
            return target;

        }
        /**
     * <summary>Set X value of this vector</summary>
     */
        public static Vector2 AsX(this Vector2 target, float x)
        {
            target.Set(x, target.y);
            return target;

        }
        /**
     * <summary>Set Y value of this vector</summary>
     */
        public static Vector2 AsY(this Vector2 target, float y)
        {
            target.Set(target.x, y);
            return target;

        }
        public static Vector2 Closest(this Vector2 from, List<Vector2> enumarable)
        {
            if (enumarable == null)
                throw new ArgumentNullException(nameof(enumarable));

            float distance = float.MaxValue;
            Vector2 closest = default;
            foreach (var item in enumarable)
            {
                float d = Vector2.Distance(item, from);
                if (d < distance)
                {
                    closest = item;
                    distance = d;
                }
            }
         

            return closest;
        }

    }
}