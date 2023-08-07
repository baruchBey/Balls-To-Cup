using Baruch.Extension;
using DG.Tweening;
using System;
using UnityEngine;

namespace Baruch.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SubUI : MonoBehaviour, IInit
    {
        protected CanvasGroup CanvasGroup;
        [SerializeField] protected SlidingUIElement[] SlidingUIElements;

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
        public abstract void Init();


        public virtual void Enable()
        {
            SlidingUIElements.ForEach(s => s.Slide(true));

        }
        public virtual void Disable()
        {
            SlidingUIElements.ForEach(s => s.Slide(false));
            DOVirtual.DelayedCall(SlidingUIElement.SLIDE_TIME, () => { gameObject.SetActive(false); });
        }

        internal void SetActive(bool v)
        {
            if (v)
            {
                if (gameObject.activeSelf)
                    return;

                gameObject.SetActive(true);
                Enable();
            }
            else
            {
                Disable();
            }
        }


#if UNITY_EDITOR
        [ContextMenu("CaptureEnabled")]
        void CaptureEnabled()
        {
            SlidingUIElements.ForEach(z => z.SetEnabledPosition());
        }
        [ContextMenu("CaptureDisabled")]
        void CaptureDisabled()
        {
            SlidingUIElements.ForEach(z => z.SetDisabledPosition());

        }
#endif

    }
}