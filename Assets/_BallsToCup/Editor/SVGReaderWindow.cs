using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Baruch.Extension;
using System.Linq;
using UnityEngine.Animations;

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
        int[] _marbleCounts;
        int[] _targetCounts;
        bool[] _foldout;
        string _folderPath;
        private void OnGUI()
        {


            string[] filePaths = Directory.GetFiles(_folderPath, "*.svg");

            if (GUILayout.Button("Bulk Create"))
            {

                for (int i = 0; i < filePaths.Length; i++)
                {
                    string filePath = filePaths[i];
                    CreateLevel(filePath, filePath[^5] - '0', _marbleCounts[i], _targetCounts[i]);
                }


            }
         


            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];
                var name = filePath.Split('\\')[^1];
                _foldout[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_foldout[i], name);
                if (!_foldout[i])
                {
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    EditorGUI.EndChangeCheck();
                    continue;
                }
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Level Parameters");
                _marbleCounts[i] = EditorGUILayout.IntField("Marble Count:", _marbleCounts[i]);
                _targetCounts[i] = EditorGUILayout.IntField("Target Count:", _targetCounts[i]);

                EditorGUILayout.EndVertical();

                if (GUILayout.Button($"Create {name}",GUILayout.Height(100)))
                {
                    CreateLevel(filePath, filePath[^5] - '0', _marbleCounts[i], _targetCounts[i]);

                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            if (EditorGUI.EndChangeCheck())
                SaveArraysToEditorPrefs();
        }

        private void OnEnable()
        {
            _folderPath = Application.dataPath + "/_BallsToCup/SVGs";

            var vectorCount = Directory.GetFiles(_folderPath, "*.svg").Length;
            _foldout = new bool[vectorCount];
            _marbleCounts = new int[vectorCount];
            _targetCounts = new int[vectorCount];

            // Load the data from EditorPrefs and convert back to arrays
            string foldoutJson = EditorPrefs.GetString("FoldoutData", "");
            string marbleCountsJson = EditorPrefs.GetString("MarbleCountsData", "");
            string targetCountsJson = EditorPrefs.GetString("TargetCountsData", "");

            if (!string.IsNullOrEmpty(foldoutJson))
            {
                _foldout = JsonUtility.FromJson<Wrapper<bool>>(foldoutJson).Data;
            }

            if (!string.IsNullOrEmpty(marbleCountsJson))
            {
                _marbleCounts = JsonUtility.FromJson<Wrapper<int>>(marbleCountsJson).Data;
            }

            if (!string.IsNullOrEmpty(targetCountsJson))
            {
                _targetCounts = JsonUtility.FromJson<Wrapper<int>>(targetCountsJson).Data;
            }

            SaveArraysToEditorPrefs();

        }
        private void SaveArraysToEditorPrefs()
        {
            string foldoutJson = JsonUtility.ToJson(new Wrapper<bool> { Data = _foldout });
            string marbleCountsJson = JsonUtility.ToJson(new Wrapper<int> { Data = _marbleCounts });
            string targetCountsJson = JsonUtility.ToJson(new Wrapper<int> { Data = _targetCounts });

            EditorPrefs.SetString("FoldoutData", foldoutJson);
            EditorPrefs.SetString("MarbleCountsData", marbleCountsJson);
            EditorPrefs.SetString("TargetCountsData", targetCountsJson);
        }

        [System.Serializable]
        public class Wrapper<T>
        {
            public T[] Data;
        }
        private static void CreateLevel(string fileContent, int id, int marbleCount, int targetCount)
        {
            var points = SVGReader.Read(fileContent);

            //for (int i = 0; i < points.Length; i++)
            //{
            //    var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //    go.name = i.ToString();
            //    go.transform.position = points[i];
            //}
            

            points = AddBottleExitVectors(points);

            var tube = CreateMesh(points, out Vector2[] hull);
            tube.name = $"TubeMesh SVG {id}";
            var tubePrefab = CreateTubePrefab(id, tube, hull, out GameObject sceneObject);
            DestroyImmediate(sceneObject);
            CreateLevelPrefab(tubePrefab, id);
        }

        private static void CreateLevelPrefab(GameObject tubePrefab, int id)
        {
            var newLevel = new GameObject();
            newLevel.name = $"Level SVG {id}";


            var parent = new GameObject();
            parent.name = "Parent";
            parent.transform.SetParent(newLevel.transform);

            var freeMarbleParent = new GameObject();
            freeMarbleParent.name = "FreeMarbleParent";
            freeMarbleParent.transform.SetParent(newLevel.transform);


            var tube = PrefabUtility.InstantiatePrefab(tubePrefab);
            PrefabUtility.RevertObjectOverride(tube, InteractionMode.AutomatedAction);

            (tube as GameObject).transform.SetParent(parent.transform);



            var marbleParent = new GameObject();
            marbleParent.name = "Marbles";
            marbleParent.transform.SetParent(parent.transform);
            marbleParent.transform.position = Vector3.down * 6.5f;

            var constraint = marbleParent.AddComponent<ParentConstraint>();
            var c = new ConstraintSource();
            c.sourceTransform = (tube as GameObject).transform;
            c.weight = 1f;
            constraint.AddSource(c);
            constraint.constraintActive = true;
            constraint.locked = true;

            var ground = AssetDatabase.LoadAssetAtPath("Assets/_BallsToCup/Prefabs/Ground.prefab", typeof(GameObject));
            var gr = PrefabUtility.InstantiatePrefab(ground) as GameObject;
            gr.transform.SetParent(newLevel.transform);

            var finish = AssetDatabase.LoadAssetAtPath("Assets/_BallsToCup/Prefabs/Finish.prefab", typeof(GameObject));
            var fin = PrefabUtility.InstantiatePrefab(finish) as GameObject;
            fin.transform.SetParent(newLevel.transform);

            var level = newLevel.AddComponent<Level>();
            level.Reset();

            PrefabUtility.SaveAsPrefabAsset(newLevel, $"Assets/_BallsToCup/Prefabs/LevelPrefabs/SVG Levels/{newLevel.name}.prefab");
        }

        private static GameObject CreateTubePrefab(int id, Mesh tube, Vector2[] hull, out GameObject sceneObject)
        {
            var newTube = new GameObject();
            newTube.name = $"Tube SVG {id}";



            var visual = new GameObject();
            visual.transform.SetParent(newTube.transform);
            visual.name = "Visual";
            var renderer = visual.AddComponent<MeshRenderer>();
            var filter = visual.AddComponent<MeshFilter>();
            filter.mesh = tube;
            AssetDatabase.CreateAsset(tube, $"Assets/_BallsToCup/Prefabs/Tubes/SVG Tubes/Meshes/{tube.name}.asset");

            renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/_BallsToCup/Materials/_Shaders/Glass.mat");

            var edgeCollider = new GameObject();
            edgeCollider.transform.SetParent(newTube.transform);

            edgeCollider.name = "EdgeCollider";
            edgeCollider.layer = LayerMask.NameToLayer("Hull");
            var cl = edgeCollider.AddComponent<EdgeCollider2D>();
            cl.points = hull;

            var bottleExit = new GameObject();
            bottleExit.transform.SetParent(newTube.transform);

            bottleExit.name = "BottleExit";
            bottleExit.transform.position = (hull[0] + hull[^1]) / 2;
            bottleExit.layer = LayerMask.NameToLayer("Trigger");
            var exitCl = bottleExit.AddComponent<CircleCollider2D>();
            exitCl.isTrigger = true;
            exitCl.radius = 3f;
            sceneObject = newTube;
            return PrefabUtility.SaveAsPrefabAsset(newTube, $"Assets/_BallsToCup/Prefabs/Tubes/SVG Tubes/{newTube.name}.prefab");
        }

        public const float TUBE_RADIUS = 3.4042f;
        private static Mesh CreateMesh(Vector2[] points, out Vector2[] hull)
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
            // If you want smooth shading, you can calculate normals and tangents here
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

        private static Vector2[] RemoveBottleExitVectors(Vector2[] points)
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

        private static Vector2[] AddBottleExitVectors(Vector2[] points)
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
