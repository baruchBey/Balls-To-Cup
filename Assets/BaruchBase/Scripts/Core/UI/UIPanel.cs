using System;
using UnityEngine;

namespace Baruch.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        
        public event Action OnActivate;
        public event Action OnDeactivate;
        
        public virtual void Init()
        {
            Reset();
        }
        
        public virtual void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
            OnActivate?.Invoke();
        }

        public virtual void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
            OnDeactivate();
            OnDeactivate?.Invoke();
        }
        
        public virtual void Reset()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }
    }
}