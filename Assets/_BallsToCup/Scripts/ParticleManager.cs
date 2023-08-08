using Baruch.Extension;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Baruch
{
    public class ParticleManager : Singleton<ParticleManager>, IInit, IPoolOwner
    {
        static readonly ObjectPool<InCupParticle> _inCupParticlePool = new ObjectPool<InCupParticle>(OnInCupParticleCreate, OnInCupParticleGet, OnInCupParticleRelease);

        Camera _camera;
        [Header("GainedParticle")]
        [SerializeField] ParticleSystem _gainedParticle;
        [SerializeField] RectTransform _uiFrom;
        [SerializeField] RectTransform _uiTarget;

        Vector3 _from;
        Vector3 _to;



        List<InCupParticle> _activeParticles = new List<InCupParticle>();
        [Header("InCup Particle")]
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
            _camera = Camera.main;


      


        }
        private void Start()
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_uiFrom, GetUIWorldPosition(_uiFrom), _camera, out _from);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_uiTarget, GetUIWorldPosition(_uiTarget), _camera, out _to);

            _from.SetZ(0);
            _to.SetZ(0);
        }
        public void ReleaseAll()
        {
            _activeParticles.ForEach(x => ReleaseInCup(x));
            _activeParticles.Clear();
        }

        [ContextMenu("Play")]
        void PlayGained()
        {
            PlayGained(50);
        }
        internal void PlayGained(int amount)
        {
            ParticleSystem.Burst burst = new ParticleSystem.Burst(0, (short)amount/2);
            ParticleSystem.Burst[] bursts = { burst };
            _gainedParticle.emission.SetBursts(bursts);

            ParticleSystem.ShapeModule shapeModule = _gainedParticle.shape;
            shapeModule.shapeType = ParticleSystemShapeType.Sphere;
            shapeModule.position = _from - _to;

            _gainedParticle.transform.position = _to;

            _gainedParticle.Play();
        }
        private Vector3 GetUIWorldPosition(RectTransform uiTransform)
        {
            Vector3[] corners = new Vector3[4];
            uiTransform.GetWorldCorners(corners);
            Vector3 worldPosition = (corners[0] + corners[2]) / 2f;
            return worldPosition;
        }
    }
}
