using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using static Baruch.UtilEditor.SVGUtility;
using static Baruch.UtilEditor.SVGTubeCreator;

namespace Baruch.UtilEditor
{
    public static class SVGLevelCreator 
    {
        const float FLASK_RADIUS = 7.3f;
        const float FLASK_AREA = FLASK_RADIUS * FLASK_RADIUS * Mathf.PI;

        public static void CreateLevel(string fileContent, int id, int marbleCount, int targetCount)
        {
            var points = SVGReader.Read(fileContent);

            points = AddBottleExitVectors(points);

            var tube = CreateMesh(points, out Vector2[] hull);
            tube.name = $"TubeMesh SVG {id}";
            var tubePrefab = CreateTubePrefab(id, tube, hull, out GameObject sceneObject);
            GameObject.DestroyImmediate(sceneObject);
            CreateLevelPrefab(tubePrefab, id, marbleCount, targetCount);
        }

        private static void CreateLevelPrefab(GameObject tubePrefab, int id, int marbleCount, int targetCount)
        {
            if (GameObject.Find($"Level SVG {id}"))
            {
                GameObject.DestroyImmediate(GameObject.Find($"Level SVG {id}"));
            }

            GameObject newLevel = new()
            {
                name = $"Level SVG {id}"
            };



            GameObject parent = new()
            {
                name = "Parent"
            };

            parent.transform.SetParent(newLevel.transform);

            GameObject freeMarbleParent = new()
            {
                name = "FreeMarbleParent"
            };
            freeMarbleParent.transform.SetParent(newLevel.transform);


            var tube = PrefabUtility.InstantiatePrefab(tubePrefab);
            //PrefabUtility.RevertObjectOverride(tube, InteractionMode.AutomatedAction);

            (tube as GameObject).transform.SetParent(parent.transform);



            GameObject marbleParent = new()
            {
                name = "Marbles"
            };
            marbleParent.transform.SetParent(parent.transform);
            marbleParent.transform.position = Vector3.down * 6.5f;

            var constraint = marbleParent.AddComponent<ParentConstraint>();

            var constraintSource = new ConstraintSource
            {
                sourceTransform = (tube as GameObject).transform,
                weight = 1f

            };
            constraint.AddSource(constraintSource);
            constraint.SetTranslationOffset(0, Vector3.down * 6.5f);
            constraint.constraintActive = true;
            constraint.locked = true;

            var ground = AssetDatabase.LoadAssetAtPath("Assets/_BallsToCup/Prefabs/Ground.prefab", typeof(GameObject));
            var gr = PrefabUtility.InstantiatePrefab(ground) as GameObject;
            gr.transform.SetParent(newLevel.transform);

            var finish = AssetDatabase.LoadAssetAtPath("Assets/_BallsToCup/Prefabs/Finish.prefab", typeof(GameObject));
            var fin = PrefabUtility.InstantiatePrefab(finish) as GameObject;
            fin.transform.SetParent(newLevel.transform);



            var marblePrefab = AssetDatabase.LoadAssetAtPath("Assets/_BallsToCup/Prefabs/Marble.prefab", typeof(GameObject));
            Transform[] marbles = new Transform[marbleCount];
            for (int i = 0; i < marbleCount; i++)
            {
                var marble = PrefabUtility.InstantiatePrefab(marblePrefab) as GameObject;
                marble.transform.SetParent(marbleParent.transform);
                marble.transform.localPosition = Vector3.zero;
                marbles[i] = marble.transform;
            }
            var targetArea = 0.6f * FLASK_AREA / marbleCount;

            float marbleRadius = Mathf.Sqrt(targetArea / Mathf.PI);

            ArrangeGridInRange(marbles, marbleRadius * 2, FLASK_RADIUS * 0.9f);


            var level = newLevel.AddComponent<Level>();
            level.Create(targetCount, marbleRadius);

            Physics2D.simulationMode = SimulationMode2D.Script;
            for (int i = 0; i < 960; i++)
            {
                Physics2D.Simulate(1 / 240f);

            }
            Physics2D.simulationMode = SimulationMode2D.Update;


            PrefabUtility.SaveAsPrefabAsset(newLevel, $"Assets/_BallsToCup/Prefabs/LevelPrefabs/SVG Levels/{newLevel.name}.prefab");

            GameObject.DestroyImmediate(newLevel);//Destroy Scene object
        }

        private static void ArrangeGridInRange(Transform[] marbles, float marbleDiameter, float flaskRadius)
        {

            // Calculate the maximum number of marbles that can fit within the flask area
            int maxMarblesCount = Mathf.FloorToInt((Mathf.PI * flaskRadius * flaskRadius) / (marbleDiameter * marbleDiameter));

            // Calculate the number of rows and columns for a dense grid
            int rows = Mathf.CeilToInt(Mathf.Sqrt(maxMarblesCount));
            int columns = Mathf.CeilToInt(maxMarblesCount / (float)rows);

            // Calculate the spacing between marbles to create a dense grid
            float spacingX = 2.0f * flaskRadius / columns;
            float spacingY = 2.0f * flaskRadius / rows;

            // Arrange the marbles in a dense grid within the flask area
            int marbleIndex = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (marbleIndex >= marbles.Length)
                    {
                        break;
                    }

                    float x = -flaskRadius + col * spacingX + spacingX * 0.5f;
                    float y = -flaskRadius + row * spacingY + spacingY * 0.5f;

                    Vector2 position = new Vector2(x, y);

                    // Check if the marble is within the flask area
                    if (Vector2.Distance(Vector2.zero, position) <= flaskRadius)
                    {
                        marbles[marbleIndex].localPosition = position;
                        marbleIndex++;
                    }
                }
            }
        }
    }
}
