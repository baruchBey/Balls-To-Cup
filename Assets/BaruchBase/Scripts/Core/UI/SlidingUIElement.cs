using DG.Tweening;
using UnityEngine;

namespace Baruch.UI
{
    [System.Serializable]
    public class SlidingUIElement
    {
        public const float SLIDE_TIME = 0.3f;

        [SerializeField] RectTransform _rectTransform;
        [SerializeField] Vector2 _enabledPosition;
        [SerializeField] Vector2 _disabledPosition;

        public void Slide(bool enable)
        {
            _rectTransform.DOAnchorPos(enable?_enabledPosition:_disabledPosition, SLIDE_TIME).SetEase(enable ? Ease.OutBack : Ease.InBack);
        }


#if UNITY_EDITOR
        public void SetEnabledPosition()
        {
            _enabledPosition = _rectTransform.anchoredPosition;
        }
        public void SetDisabledPosition()
        {
            _disabledPosition = _rectTransform.anchoredPosition;
        }
#endif


    }
}