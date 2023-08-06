using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine;
using static Unity.VectorGraphics.VectorUtils;

namespace Baruch
{
    public static class SVGReader
    {
        const float M = 1f/3f;

        public static Vector2[] Read(string svgContent)
        {


            Vector2[] pointsList;


            var sceneInfo = SVGParser.ImportSVG(new StringReader(svgContent));
            var geometries = VectorUtils.TessellateScene(sceneInfo.Scene, new VectorUtils.TessellationOptions
            {
                StepDistance = 1.5f,
                SamplingStepSize = 0.3f,
                MaxCordDeviation = 0,
                MaxTanAngleDeviation = 0,
            });
            pointsList = geometries[0].Vertices;

            //PostProcess
            pointsList = Merge(pointsList, 5f);
            if (pointsList[0].y > pointsList[1].y)
                pointsList = pointsList.Reverse().ToArray();
            pointsList = Relative(pointsList);

            return pointsList;
        }

        private static Vector2[] Merge(Vector2[] pointsList, float v)
        {
            var merged = new List<Vector2>();
            for (int i = 0; i < pointsList.Length; i++)
            {
                if (!merged.Any(p => Vector2.Distance(pointsList[i],p)<v))
                    merged.Add(pointsList[i]);
            }


            return merged.ToArray();
        }

        private static Vector2[] Relative(Vector2[] pointsList)
        {
            for (int i = pointsList.Length - 1; i >= 0; i--)
            {
                pointsList[i] = (pointsList[i] - pointsList[0])* M;
            }
            return pointsList;
        }

        

    }
}
