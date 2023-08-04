using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

namespace Baruch
{
    public static class SVGReader
    {
        internal static Vector2[] Read(string svgFileName)
        {
            List<Vector2> pointsList = new List<Vector2>();

            TextAsset svg = Resources.Load<TextAsset>(svgFileName);
            if (svg == null)
            {
                Debug.LogError("SVG file not found in Resources folder: " + svgFileName);
                return pointsList.ToArray();
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(svg.text);

            // Find all "polyline" elements in the SVG file
            XmlNodeList polylineNodes = xmlDoc.GetElementsByTagName("polyline");

            foreach (XmlNode polylineNode in polylineNodes)
            {
                if (polylineNode.Attributes["points"] != null)
                {
                    string pointsValue = polylineNode.Attributes["points"].Value;
                    var polylinePoints = ExtractPointsFromAttributeValue(pointsValue);
                    pointsList.AddRange(polylinePoints);
                }
            }

            return pointsList.ToArray();
        }

        private static HashSet<Vector2> ExtractPointsFromAttributeValue(string pointsAttributeValue)
        {
            HashSet<Vector2> pointsList = new ();

            string[] pointPairs = pointsAttributeValue.Split(' ');

            foreach (string pointPair in pointPairs)
            {
                string[] coords = pointPair.Split(',');
                if (coords.Length == 2 && float.TryParse(coords[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(coords[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                {
                    pointsList.Add(new Vector2(x, y)/100f);
                }
            }

            return pointsList;
        }
    }
}
