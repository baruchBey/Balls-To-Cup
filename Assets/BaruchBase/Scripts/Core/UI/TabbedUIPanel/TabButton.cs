using System;
using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    [DisallowMultipleComponent]
    public class TabButton : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] protected Button button = default;
        
        [Header("Texts")]
        [SerializeField] protected Text titleText = default;
        
        [Header("Images")]
        [SerializeField] protected Image iconImage = default;
        [SerializeField] protected Image backgroundImage = default;

        [Header("Sprites")]
        [SerializeField] protected Sprite activeSprite = default;
        [SerializeField] protected Sprite inactiveSprite = default;

        public event Action Clicked; 
        
        public virtual void Init(string title, Sprite icon)
        {
            SetTitle(title);
            SetIcon(icon);
            SetBackground(inactiveSprite);
            button.onClick.AddListener(OnClicked);
        }

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public void SetTitle(string title)
        {
            titleText.text = title.Trim();
        }

        public void SetBackground(Sprite background)
        {
            backgroundImage.sprite = background;
        }

        protected virtual void OnClicked()
        {
            OnSelected();
            Clicked?.Invoke();
        }

        public virtual void OnSelected()
        {
            SetBackground(activeSprite);
        }

        public virtual void OnDeselected()
        {
            SetBackground(inactiveSprite);
        }
    }
}