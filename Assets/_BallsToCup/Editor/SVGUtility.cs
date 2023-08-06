using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Baruch.Extension;

namespace Baruch.UtilEditor
{
    public static class SVGUtility
    {
        public static Vector2[] RemoveBottleExitVectors(Vector2[] points)
        {
            Vector2[] newArray = new Vector2[points.Length - 4];
            newArray[0] = points[0];
            newArray[^1] = points[^1];
            for (int i = 1; i < newArray.Length - 1; i++)
            {
                newArray[i] = points[i + 2];
            }

            return newArray;

        }

        public static Vector2[] AddBottleExitVectors(Vector2[] points)
        {
            Vector2[] newArray = new Vector2[points.Length + 2];
            for (int i = 0; i < points.Length - 1; i++)
            {
                newArray[i] = points[i];
            }
            newArray[^1] = points[^1];
            for (int i = 0; i < 2; i++)
            {
                newArray[points.Length - 1 + i] = Vector2.Lerp(newArray[points.Length - 2], newArray[^1], 0.33f + i * 0.33f);
            }
            return newArray;
        }

        public static Vector2[] SortArray(Vector2[] array)
        {
            List<Vector2> list = array.ToList();
            List<Vector2> sorted = new();
            sorted.Add(array[0]);
            list.Remove(array[0]);
            for (int i = 0; i < array.Length - 1; i++)
            {
                var closest = sorted[i].Closest(list);
                list.Remove(closest);
                sorted.Add(closest);
            }

            return sorted.ToArray();
        }

        public static void AutoWeld(Mesh mesh, float threshold, float bucketStep)
        {
            Vector3[] oldVertices = mesh.vertices;
            Vector3[] newVertices = new Vector3[oldVertices.Length];
            int[] old2new = new int[oldVertices.Length];
            int newSize = 0;

            // Find AABB
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            for (int i = 0; i < oldVertices.Length; i++)
            {
                if (oldVertices[i].x < min.x)
                    min.x = oldVertices[i].x;
                if (oldVertices[i].y < min.y)
                    min.y = oldVertices[i].y;
                if (oldVertices[i].z < min.z)
                    min.z = oldVertices[i].z;
                if (oldVertices[i].x > max.x)
                    max.x = oldVertices[i].x;
                if (oldVertices[i].y > max.y)
                    max.y = oldVertices[i].y;
                if (oldVertices[i].z > max.z)
                    max.z = oldVertices[i].z;
            }

            // Make cubic buckets, each with dimensions “bucketStep”
            int bucketSizeX = Mathf.FloorToInt((max.x - min.x) / bucketStep) + 1;
            int bucketSizeY = Mathf.FloorToInt((max.y - min.y) / bucketStep) + 1;
            int bucketSizeZ = Mathf.FloorToInt((max.z - min.z) / bucketStep) + 1;
            List<int>[,,] buckets = new List<int>[bucketSizeX, bucketSizeY, bucketSizeZ];

            // Make new vertices
            for (int i = 0; i < oldVertices.Length; i++)
            {
                // Determine which bucket it belongs to
                int x = Mathf.FloorToInt((oldVertices[i].x - min.x) / bucketStep);
                int y = Mathf.FloorToInt((oldVertices[i].y - min.y) / bucketStep);
                int z = Mathf.FloorToInt((oldVertices[i].z - min.z) / bucketStep);

                // Check to see if it’s already been added
                if (buckets[x, y, z] == null)
                    buckets[x, y, z] = new List<int>(); // Make buckets lazily

                bool skip = false;
                for (int j = 0; j < buckets[x, y, z].Count; j++)
                {
                    Vector3 to = newVertices[buckets[x, y, z][j]] - oldVertices[i];
                    if (Vector3.SqrMagnitude(to) < threshold)
                    {
                        old2new[i] = buckets[x, y, z][j];
                        skip = true; // Skip to the next old vertex if this one is already there
                        break;
                    }
                }

                if (!skip)
                {
                    // Add new vertex
                    newVertices[newSize] = oldVertices[i];
                    buckets[x, y, z].Add(newSize);
                    old2new[i] = newSize;
                    newSize++;
                }
            }

            // Make new triangles
            int[] oldTris = mesh.triangles;
            int[] newTris = new int[oldTris.Length];
            for (int i = 0; i < oldTris.Length; i++)
            {
                newTris[i] = old2new[oldTris[i]];
            }

            Vector3[] finalVertices = new Vector3[newSize];
            for (int i = 0; i < newSize; i++)
            {
                finalVertices[i] = newVertices[i];
            }

            mesh.Clear();
            mesh.vertices = finalVertices;
            mesh.triangles = newTris;
            mesh.RecalculateNormals();
            mesh.Optimize();
        }
    }
}
