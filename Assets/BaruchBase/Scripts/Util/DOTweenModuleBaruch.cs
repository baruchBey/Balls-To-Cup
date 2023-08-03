using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;
using Baruch.UI;

#pragma warning disable 1591

namespace DG.Tweening
{
    public static class DOTweenModuleBaruch
    {
        #region Shortcuts

        #region Image

        /// <summary>Tweens a CanvasGroup's alpha color to the given value.
        /// Also stores the canvasGroup as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<float, float, FloatOptions> DOFillAmount(this SlicedFilledImage target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DOFillAmount(this Image target, float endValue, float duration)
        {
            if (endValue > 1) endValue = 1;
            else if (endValue < 0) endValue = 0;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        #endregion



        #region Graphic
        public static TweenerCore<float, float, FloatOptions> DOSaturation(this Graphic target, float endValue, float duration)
        {
            Color.RGBToHSV(target.color, out float h, out float s, out float v);
            
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() =>
            {
                Color.RGBToHSV(target.color, out float h, out float s, out float v);
                return s;
            },
                x =>
                {
                    target.color = Color.HSVToRGB(h, x, v);

                }
            , endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DOValue(this Graphic target, float endValue, float duration)
        {
            Color.RGBToHSV(target.color, out float h, out float s, out float v);

            TweenerCore<float, float, FloatOptions> t = DOTween.To(() =>
            {
                Color.RGBToHSV(target.color, out float h, out float s, out float v);
                return v;
            },
                x =>
                {
                    target.color = Color.HSVToRGB(h, s, x);

                }
            , endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DoHue(this Graphic target, float endValue, float duration)
        {
            Color.RGBToHSV(target.color, out float h, out float s, out float v);

            TweenerCore<float, float, FloatOptions> t = DOTween.To(() =>
            {
                Color.RGBToHSV(target.color, out float h, out float s, out float v);
                return h;
            },
                x =>
                {
                    target.color = Color.HSVToRGB(x, s, v);

                }
            , endValue, duration);
            t.SetTarget(target);
            return t;
        }
        #endregion

        #region SkinnedMeshRenderer
        /// <summary>
        /// 0 to 100
        /// </summary>
        /// <param name="target"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static TweenerCore<float, float, FloatOptions> DOBlendShape(this SkinnedMeshRenderer target, float endValue, float duration, int index)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.GetBlendShapeWeight(index), x => target.SetBlendShapeWeight(index, x), endValue, duration);
            t.SetTarget(target);
            return t;
        }
        #endregion

        #endregion



        public static class Utils
        {

        }
    }
}
