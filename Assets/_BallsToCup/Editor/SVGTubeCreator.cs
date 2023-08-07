using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Baruch.UtilEditor.SVGUtility;

namespace Baruch.UtilEditor
{
    public static class SVGTubeCreator
    {
        public const float TUBE_RADIUS = 3.4042f;
        public const float EDGE_RADIUS = 0.3f;

        public static GameObject CreateTubePrefab(int id, Mesh tube, Vector2[] hull, out GameObject sceneObject)
        {
            var newTube = new GameObject();
            newTube.name = $"Tube SVG {id}";

            AssetDatabase.DeleteAsset($"Assets/_BallsToCup/Prefabs/LevelPrefabs/SVG Levels/Level SVG {id}.prefab");
            AssetDatabase.DeleteAsset($"Assets/_BallsToCup/Prefabs/Tubes/SVG Tubes/{newTube.name}.prefab");


            var visual = new GameObject();
            visual.transform.SetParent(newTube.transform);
            visual.name = "Visual";
            var renderer = visual.AddComponent<MeshRenderer>();
            var filter = visual.AddComponent<MeshFilter>();
            filter.sharedMesh = tube;
            AssetDatabase.CreateAsset(tube, $"Assets/_BallsToCup/Prefabs/Tubes/SVG Tubes/Meshes/{tube.name}.asset");

            renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/_BallsToCup/Materials/_Shaders/Glass.mat");

            var edgeCollider = new GameObject();
            edgeCollider.transform.SetParent(newTube.transform);

            edgeCollider.name = "EdgeCollider";
            edgeCollider.layer = LayerMask.NameToLayer("Hull");
            var cl = edgeCollider.AddComponent<EdgeCollider2D>();
            cl.edgeRadius = EDGE_RADIUS;
            cl.points = hull;

            var bottleExit = new GameObject();
            bottleExit.transform.SetParent(newTube.transform);

            bottleExit.name = "BottleExit";
            bottleExit.transform.position = (hull[0] + hull[^1]) / 2;
            bottleExit.layer = LayerMask.NameToLayer("Trigger");
            var exitCl = bottleExit.AddComponent<CircleCollider2D>();
            exitCl.isTrigger = true;
            exitCl.radius = 3f;
            bottleExit.AddComponent<BottleExit>();

            sceneObject = newTube;

            return PrefabUtility.SaveAsPrefabAsset(newTube, $"Assets/_BallsToCup/Prefabs/Tubes/SVG Tubes/{newTube.name}.prefab");
        }

        public static Mesh CreateMesh(Vector2[] points, out Vector2[] hull)
        {

            var tangents = new Vector2[points.Length];
            for (int i = 0; i < points.Length - 1; i++)
                tangents[i] = (points[i + 1] - points[i]).normalized;
            tangents[^1] = tangents[^2];

            int verticesPerPoint = 17;
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
                    vertices[startIndex + j] = (Vector3)point + rotatedStable * TUBE_RADIUS * ((i == points.Length - 3 || i == points.Length - 2) ? 1.15f : 1f);
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



            Mesh tubeMesh = new Mesh();
            tubeMesh.vertices = vertices;
            tubeMesh.triangles = triangles;
            tubeMesh.normals = normals;

            tubeMesh.RecalculateNormals();
            tubeMesh.RecalculateTangents();



            var emptyTubeMesh = AssetDatabase.LoadAssetAtPath<MeshFilter>("Assets/_BallsToCup/3D/EmptyTube.fbx").sharedMesh;




            //
            CombineInstance[] combine = new CombineInstance[2];

            combine[0].mesh = emptyTubeMesh;
            combine[0].transform = Matrix4x4.identity;
            combine[1].mesh = tubeMesh;
            combine[1].transform = Matrix4x4.identity;

            var combined = new Mesh();
            combined.CombineMeshes(combine);
            AutoWeld(combined, 1f, 0.1f);
            MeshUtility.Optimize(combined);
            MeshUtility.SetMeshCompression(combined, ModelImporterMeshCompression.High);

            HashSet<Vector2> hullList = new();
            foreach (var item in combined.vertices)
            {
                if (Mathf.Abs(item.z) < 0.1f)
                {
                    hullList.Add(item);
                }
            }

            hull = hullList.ToArray().Reverse().ToArray();
            hull = SortArray(hull);
            hull = RemoveBottleExitVectors(hull);
            //
            return combined;


        }
    }
}
