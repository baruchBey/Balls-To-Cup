using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Baruch
{
    public class GainParticle : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        public List<ParticleCollisionEvent> _collisionEvents;
        int _collideCount;
        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _collisionEvents = new List<ParticleCollisionEvent>();

        }
        
        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = _particleSystem.GetCollisionEvents(other, _collisionEvents);

            _collideCount += numCollisionEvents;

            CurrencyManager.Instance.Gain(numCollisionEvents*2);
            AudioManager.Instance.Play(Audio.AudioItemType.BalanceGain);


        }
    }
}
