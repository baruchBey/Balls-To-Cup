using System;
using UnityEngine;

namespace Baruch.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SubUI : MonoBehaviour, IInit
    {
        protected CanvasGroup CanvasGroup;
        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
        public abstract void Init();


        public abstract void OnEnable();

        internal void SetActive(bool v)
        {
            gameObject.SetActive(v);
        }



    }
}