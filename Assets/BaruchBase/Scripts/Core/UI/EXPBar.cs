using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Baruch.UI
{
    public class EXPBar : MonoBehaviour
    {
        [SerializeField] SlicedFilledImage _fill;
        [SerializeField] TextMeshProUGUI _level;
        const float SPEED = 1f;
    
        public void SetLevel(float xp, bool instant = false)
        {
            if (instant)
            {
                _fill.fillAmount = (xp % 1f);
                _level.text = Mathf.CeilToInt(xp / 1f).ToString();
                return;
            }
            var target = xp % 1f;
            if (_fill.fillAmount >= target)
            {
                _fill.DOFillAmount(1f, SPEED).SetSpeedBased(true).OnComplete(
                    () =>
                    {
                        _fill.fillAmount = float.Epsilon;
                        _level.text = Mathf.CeilToInt((xp / 1f)+float.Epsilon).ToString();
                        _fill.DOFillAmount(target, SPEED).SetSpeedBased(true);
                    });


            }
            else
            {
                _fill.DOFillAmount(target, SPEED).SetSpeedBased(true);
            }
        }
    }
}
