using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class Marble : MonoBehaviour
    {
        private byte _id;
        [SerializeField] TrailRenderer _trailRenderer;

        public byte ID { get => _id; set { _id = value; SetTrailColor(ColorManager.Instance.GetColor(value)); } }

        private void SetTrailColor(Color v)
        {
            _trailRenderer.startColor = v;
            _trailRenderer.endColor = v;

        }

        private void FixedUpdate()
        {
            if (transform.position.y < -100)
            {
                Destroy(this);
            }
        }
        private void OnDestroy()
        {
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }
    }
}
