using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class InCupParticle : MonoBehaviour
    {
        [SerializeField] ParticleSystemRenderer _particleSystemRenderer;
        internal void SetColor(Color color)
        {
            _particleSystemRenderer.material.color = color;
        }
        private void OnValidate()
        {
            _particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
        }
      
    }
}
