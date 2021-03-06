using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Erethan.ScreneTransition.Components
{
    public class OverlayScreenTransition : MonoBehaviour, IScreenTransition
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _duration = 1;
        [SerializeField] private AnimationCurve _interpolation = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

        private bool faded = false;
        public bool Faded => faded;

        public event Action FadeInComplete;
        public event Action FadeOutComplete;

        public void FadeIn()
        {
            StopAllCoroutines();
            StartCoroutine(nameof(FadeInRoutine));
        }

        public void FadeOut()
        {
            StopAllCoroutines();
            StartCoroutine(nameof(FadeOutRoutine));
        }


        private IEnumerator FadeInRoutine()
        {
            yield return AlphaChangeRoutine(0, _duration);
            faded = false;
            FadeInComplete?.Invoke();
        }

        private IEnumerator FadeOutRoutine()
        {
            yield return AlphaChangeRoutine(1, _duration);
            faded = true;
            FadeOutComplete?.Invoke();
        }

        private IEnumerator AlphaChangeRoutine(float targetAlpha, float duration)
        {
            float startTime = Time.time;
            float startAlpha = _image.color.a;

            Color color = _image.color;
            _image.color = color;

            while (Time.time <= startTime + duration)
            {
                float ratio = _interpolation.Evaluate((Time.time - startTime) / duration);
                ratio = Mathf.Clamp01(ratio);
                color.a = startAlpha * (1 - ratio) + targetAlpha * (ratio);
                _image.color = color;

                yield return null;
            }
            color.a = targetAlpha;
            _image.color = color;

        }
    }
}