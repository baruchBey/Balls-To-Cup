using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Baruch
{
    public class ParticleManager : Singleton<ParticleManager>, IInit,IPoolOwner
    {
        static readonly ObjectPool<InCupParticle> _inCupParticlePool = new ObjectPool<InCupParticle>(OnInCupParticleCreate, OnInCupParticleGet, OnInCupParticleRelease);

       

        List<InCupParticle> _activeParticles = new List<InCupParticle>();
        [SerializeField] InCupParticle _inCupParticle;

        private static void OnInCupParticleRelease(InCupParticle particle)
        {
            particle.transform.SetParent(Instance.transform);
            particle.gameObject.SetActive(false);
        }

        private static InCupParticle OnInCupParticleCreate()
        {
            var p = Instantiate(Instance._inCupParticle);

            Instance._activeParticles.Add(p);
            return p; 
        }

        private static void OnInCupParticleGet(InCupParticle particle)
        {

        }
        public void PlayInCup(Marble marble)
        {
            var p = _inCupParticlePool.Get();
            p.SetColor(ColorManager.Instance.GetColor(marble.ID));
            p.transform.SetParent(marble.transform);
            p.transform.localPosition = Vector3.zero;
            p.gameObject.SetActive(true); // Activate the particle
            
            
        }
        public void ReleaseInCup(InCupParticle inCupParticle)
        {
            _inCupParticlePool.Release(inCupParticle);
        }

        public void Init()
        {

        }

        public void ReleaseAll()
        {
            _activeParticles.ForEach(x => ReleaseInCup(x));
            _activeParticles.Clear();
        }
    }
}
