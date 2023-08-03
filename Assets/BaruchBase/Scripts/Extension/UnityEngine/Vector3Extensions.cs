using System;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch.Extension
{
    public static class Vector3Extensions
    {
        /**
  * <summary>Set X value of this vector</summary>
  */
        public static Vector3 SetX(ref this Vector3 target, float x)
        {
            target.Set(x, target.y, target.z);
            return target;
        }
        /**
     * <summary>Set Y value of this vector</summary>
     */
        public static Vector3 SetY(ref this Vector3 target, float y)
        {
            target.Set(target.x, y, target.z);
            return target;

        }

        /**
     * <summary>Set Z value of this vector</summary>
     */
        public static Vector3 SetZ(ref this Vector3 target, float z)
        {
            target.Set(target.x, target.y, z);
            return target;

        }

        /**
  * <summary>Set X value of this vector</summary>
  */
        public static Vector3 AsX(this Vector3 target, float x)
        {
            target.Set(x, target.y, target.z);
            return target;
        }
        /**
     * <summary>Set Y value of this vector</summary>
     */
        public static Vector3 AsY(this Vector3 target, float y)
        {
            target.Set(target.x, y, target.z);
            return target;

        }

        /**
     * <summary>Set Z value of this vector</summary>
     */
        public static Vector3 AsZ(this Vector3 target, float z)
        {
            target.Set(target.x, target.y, z);
            return target;

        }
        public static Vector2 FromTopDown(this Vector3 target)
        {
            return new Vector2(target.x, target.z);

        }

    }
}