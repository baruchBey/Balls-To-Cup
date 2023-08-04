using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Baruch
{
    public class MeshToEdgeCollider : MonoBehaviour
    {
        [ContextMenu("Setup")]
        void ToEdgeCollider()
        {
            var mesh = GetComponent<MeshFilter>().sharedMesh;
            List<Vector3> points = new();
            mesh.GetVertices(points);
            var edge = gameObject.AddComponent<EdgeCollider2D>();
            edge.points = points.ConvertAll(x=>(Vector2)x).ToArray();

        }
    }
}
