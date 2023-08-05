using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Baruch.Extension;

namespace Baruch.UtilEditor
{
    public class SVGReaderWindow : EditorWindow
    {
        public const int WINDOW_WIDTH = 512;

        private static readonly Vector2 _windowSize = new(WINDOW_WIDTH, 640f);

        [MenuItem("Sunday/SVGReader")]
        public static void ShowWindow()
        {
            var window = GetWindow<SVGReaderWindow>("SVGReader");
            window.maxSize = _windowSize;
            window.minSize = _windowSize;
        }
        private void OnGUI()
        {
            var folderPath = Application.dataPath + "/Resources/SVGs";
            if (GUILayout.Button("Create New Levels"))
            {
                string[] filePaths = Directory.GetFiles(folderPath, "*.svg");

                foreach (string filePath in filePaths)
                {
                    string fileContent = File.ReadAllText(filePath);

                    CreateTube(fileContent);
                }


            }
        }



        private static void CreateTube(string fileContent)
        {
            var points = SVGReader.Read(fileContent);
            //var parent = new GameObject().transform;
            //for (int i = 0; i < points.Length; i++)
            //{
            //    var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //    go.transform.position = points[i];
            //    go.name = i.ToString();
            //    go.transform.SetParent(parent);
            //}


            CreateMesh(points);

        }

        const float TUBE_RADIUS = 3.4042f;
        private static void CreateMesh(Vector2[] points)
        {
            var tangents = new Vector2[points.Length];
            for (int i = 0; i < points.Length - 1; i++)
                tangents[i] = (points[i + 1] - points[i]).normalized;
            tangents[^1] = tangents[^2];

            int verticesPerPoint = 17; // You can adjust the number of vertices per point to control the tube's smoothness
            int numVertices = points.Length * verticesPerPoint;
            int numTriangles = (points.Length - 1) * 2 * (verticesPerPoint - 1);

            Vector3[] vertices = new Vector3[numVertices];
            var normals = new Vector3[numVertices];
            int[] triangles = new int[numTriangles * 3];

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 point = points[i];
                float angleStep = 360f / (float)(verticesPerPoint - 1);
                int startIndex = i * verticesPerPoint;

                var stable = Vector3.Cross(tangents[i], Vector3.back);
                for (int j = 0; j < verticesPerPoint; j++)
                {
                    float angle = j * angleStep;
                    var rotatedStable = Quaternion.AngleAxis(angle, tangents[i]) * stable;
                    normals[startIndex + j] = rotatedStable;
                    vertices[startIndex + j] = (Vector3)point + rotatedStable* TUBE_RADIUS;
                }

                if (i < points.Length - 1)
                {
                    int startTriangleIndex = i * (verticesPerPoint - 1) * 6;
                    int startVertexIndex = i * verticesPerPoint;

                    for (int j = 0; j < verticesPerPoint - 1; j++)
                    {
                        int triangleIndex = startTriangleIndex + j * 6;
                        int vertexIndex = startVertexIndex + j;

                        triangles[triangleIndex] = vertexIndex;
                        triangles[triangleIndex + 1] = vertexIndex + 1;
                        triangles[triangleIndex + 2] = vertexIndex + verticesPerPoint;
                        triangles[triangleIndex + 3] = vertexIndex + 1;
                        triangles[triangleIndex + 4] = vertexIndex + verticesPerPoint + 1;
                        triangles[triangleIndex + 5] = vertexIndex + verticesPerPoint;
                    }
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            // If you want smooth shading, you can calculate normals and tangents here
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            MeshFilter filter = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>();
            filter.mesh = mesh;
        }
    }
}
