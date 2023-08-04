using Baruch.Extension;
using System;
using UnityEngine;

namespace Baruch
{
    public class Level : MonoBehaviour, IConfigure
    {
        public Transform Tube;
        [SerializeField] Transform _marbleParent;
        [SerializeField, Range(0.5f, 9f)] float _flaskRadius;

#if UNITY_EDITOR
        [SerializeField] Vector2[] _marblePositions;
#endif
        public void Configure()
        {

        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_marbleParent.position, _flaskRadius);
            _marbleParent.ForEach(x => Gizmos.DrawSphere(x.position, x.localScale.x));
        }

        internal Transform[] GetMarbles()
        {
            return _marbleParent.GetChilds();
        }

#if UNITY_EDITOR
        [ContextMenu("CapturePositions")]
        void CaptureMarblePositions()
        {
            _marblePositions = new Vector2[_marbleParent.childCount];
            for (int i = 0; i < _marbleParent.childCount; i++)
            {
                _marblePositions[i] = _marbleParent.GetChild(i).position;
            }
        }
        [ContextMenu("Set")]
        void SetPositions()
        {
            for (int i = 0; i < _marbleParent.childCount; i++)
            {
                _marbleParent.GetChild(i).position = _marblePositions[i];
            }
        }
#endif

    }
}