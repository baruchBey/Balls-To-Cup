using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;
using Cinemachine;
#pragma warning disable 1591

namespace DG.Tweening
{
    public static class DOTweenModuleCinemachine
    {
        #region Shortcuts

        #region Cinemachine

        /// <summary>Tweens a CanvasGroup's alpha color to the given value.
        /// Also stores the canvasGroup as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOFollowOffset(this CinemachineFramingTransposer target, Vector3 endValue, float duration)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.m_TrackedObjectOffset, x => target.m_TrackedObjectOffset = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DOScreenY(this CinemachineFramingTransposer target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.m_ScreenY, x => target.m_ScreenY = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOFollowOffset(this CinemachineTransposer target, Vector3 endValue, float duration)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.m_FollowOffset, x => target.m_FollowOffset = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        public static TweenerCore<Vector3, Vector3, VectorOptions> DOFollowOffset(this CinemachineComposer target, Vector3 endValue, float duration)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.m_TrackedObjectOffset, x => target.m_TrackedObjectOffset = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DOScreenY(this CinemachineComposer target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.m_ScreenY, x => target.m_ScreenY = x, endValue, duration);
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

        #endregion

        public static class Utils
        {

        }
    }
}
