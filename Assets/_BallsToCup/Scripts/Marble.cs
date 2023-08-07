using DG.Tweening;
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
            _trailRenderer.widthMultiplier = Level.MarbleSize;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].time = 0.5f;
            alphaKeys[0].alpha = 0.5f;
            alphaKeys[1].time = 1f;
            alphaKeys[1].alpha = 0f;

            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].time = 0f;
            colorKeys[0].color = v;
            colorKeys[1].time = 1f;
            colorKeys[1].color = v;

            Gradient gradient = new();
            gradient.SetKeys(colorKeys, alphaKeys);

            _trailRenderer.colorGradient = gradient;
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
            transform.DOKill();

            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }
        public void Free()
        {
            _trailRenderer.minVertexDistance = 0;
        }
    }
}
