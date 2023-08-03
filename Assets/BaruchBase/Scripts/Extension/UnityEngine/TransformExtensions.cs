using System;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch.Extension
{
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
                UnityEngine.Object.Destroy(child.gameObject);
        } 
        public static void DestroyImmediateAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
                UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
        public static Transform GetRandomChild(this Transform transform)
        {
            return transform.GetChild(UnityEngine.Random.Range(0, transform.childCount));
        }
        public static void DisableAllChildren(this Transform transform)
        {
            foreach (Transform item in transform)
                item.gameObject.SetActive(false);
        }
        public static T Closest<T>(this Transform from, IEnumerable<T> enumarable) where T : Component
        {
            if (enumarable == null)
                throw new ArgumentNullException(nameof(enumarable));

            float distance = float.MaxValue;
            T closest = null;
            foreach (var item in enumarable)
            {
                float d = Vector3.Distance(item.transform.position, from.position);
                if (d < distance)
                {
                    closest = item;
                    distance = d;
                }
            }
            if (closest == null)
                throw new ArgumentNullException(nameof(closest));

            return closest;
        }
        public static Transform Closest(this Transform from, IEnumerable<Transform> enumarable)
        {
            if (enumarable == null)
                throw new ArgumentNullException(nameof(enumarable));

            float distance = float.MaxValue;
            Transform closest = null;
            foreach (var item in enumarable)
            {
                float d = Vector3.Distance(item.position, from.position);
                if (d < distance)
                {
                    closest = item;
                    distance = d;
                }
            }
            if (closest == null)
                throw new ArgumentNullException(nameof(closest));
            return closest;
        }
        public enum GraphSearchType
        {
            BreadFirstSearch,
            DepthFirstSearch
        }

        public static Transform FindDeepChild(this Transform aParent, string aName, GraphSearchType type = GraphSearchType.BreadFirstSearch)
        {
            if (type.Equals(GraphSearchType.BreadFirstSearch))
            {
                Queue<Transform> queue = new Queue<Transform>();
                queue.Enqueue(aParent);
                while (queue.Count > 0)
                {
                    var c = queue.Dequeue();
                    if (c.name == aName)
                        return c;
                    foreach (Transform t in c)
                        queue.Enqueue(t);
                }
                return null;
            }

            else
            {
                foreach (Transform child in aParent)
                {
                    if (child.name == aName)
                        return child;
                    var result = child.FindDeepChild(aName);
                    if (result != null)
                        return result;
                }
                return null;
            }
        }

    }
}