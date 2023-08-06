using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Baruch.UtilEditor
{
    public static class SVGReader
    {
        const float M = 30;

        public static Vector2[] Read(string filePath)
        {
            filePath = "Assets" + filePath.Split("Assets")[^1];

            var pointsList = ((GameObject)AssetDatabase.LoadMainAssetAtPath(filePath)).GetComponent<SpriteRenderer>().sprite.vertices;
            pointsList = PostProcess(pointsList);
            return pointsList;
        }

        private static Vector2[] PostProcess(Vector2[] pointsList)
        {
            pointsList = pointsList.Where((x, i) => i % 2 == 0).ToArray();
            pointsList = Relative(pointsList, M);

            pointsList = Merge(pointsList, 0.5f);


            if (pointsList[0].y > pointsList[1].y)
            {
                pointsList = pointsList.Reverse().ToArray();
                pointsList = Relative(pointsList);

            }
            pointsList = DivideByDistance(pointsList, SVGTubeCreator.TUBE_RADIUS);
            return pointsList;
        }

        private static Vector2[] DivideByDistance(Vector2[] pointsList, float radius)
        {
            var list = new List<Vector2>();
            for (int i = 0; i < pointsList.Length - 1; i++)
            {
                Vector2 currentPoint = pointsList[i];
                Vector2 nextPoint = pointsList[i + 1];
                float distance = Vector2.Distance(currentPoint, nextPoint);

                // Add the current point to the list
                list.Add(currentPoint);

                // If the distance between the current point and the next point is greater than the tube radius,
                // divide the distance and add intermediate points
                if (distance > radius)
                {
                    int divisions = Mathf.FloorToInt(distance / radius);
                    float stepSize = 1.0f / (divisions + 1);

                    for (int j = 1; j <= divisions; j++)
                    {
                        float t = stepSize * j;
                        Vector2 intermediatePoint = Vector2.Lerp(currentPoint, nextPoint, t);
                        list.Add(intermediatePoint);
                    }
                }
            }

            // Add the last point to the list
            list.Add(pointsList[pointsList.Length - 1]);

            return list.ToArray();
        }

        private static Vector2[] Merge(Vector2[] pointsList, float v)
        {
            var merged = new List<Vector2>();
            for (int i = 0; i < pointsList.Length; i++)
            {
                if (!merged.Any(p => Vector2.Distance(pointsList[i], p) < v))
                    merged.Add(pointsList[i]);
            }


            return merged.ToArray();
        }

        private static Vector2[] Relative(Vector2[] pointsList,float m =1f)
        {
            for (int i = pointsList.Length - 1; i >= 0; i--)
            {
                pointsList[i] = (pointsList[i] - pointsList[0]) * m;
            }
            return pointsList;
        }



    }
}
