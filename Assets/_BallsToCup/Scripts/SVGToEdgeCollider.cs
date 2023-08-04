using Baruch.Extension;
using UnityEngine;

namespace Baruch
{
    public class SVGToEdgeCollider : MonoBehaviour
    {
        [ContextMenu("Setup")]
        void ToEdgeCollider()
        {
            var points = SVGReader.Read("tube1hull");
            points.ForEach(x => Debug.Log(x));

            var edge = gameObject.AddComponent<EdgeCollider2D>();
            edge.points = points;

        }
    }
}
