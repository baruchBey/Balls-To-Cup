using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class InCupParticle : MonoBehaviour
    {
        [SerializeField] ParticleSystem _particleSystem;
        internal void SetColor(Color color)
        {
            var main = _particleSystem.main;
            main.startColor = color;
        }
        private void OnValidate()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
      
    }
}
