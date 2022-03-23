using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Effect
{
    public class ScaleEffect : FitterEffect
    {
        public override FitterEffectType EffectType => FitterEffectType.Scale;

        [SerializeField] protected float m_MinSize = 0.5f;
        [SerializeField] protected Vector2 m_Spacing = Vector2.zero;
        [SerializeField] protected SmoothMode m_SmoothMode = SmoothMode.None;
        [SerializeField] protected float m_DecelerationRate = 16;

        public float MinSize
        {
            get => m_MinSize;
            set => m_MinSize = value;
        }

        public Vector2 Spacing
        {
            get => m_Spacing;
            set => m_Spacing = value;
        }

        public override void LateUpdate<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable)
        {
            var fitterItem = default(RectTransform);

            var viewSize = new Vector2
            {
                x = effectable.ViewRect.rect.width / 2,
                y = effectable.ViewRect.rect.height / 2
            };

            foreach (var item in keyValues)
            {
                fitterItem = item.Value.RectTransform;

                var viewPosition = CalculateOffsetPosition(fitterItem.anchoredPosition, viewSize, effectable);

                var distanceRatio = new Vector2
                {
                    x = 1 - Mathf.Clamp(Mathf.Abs(viewPosition.x) / (viewSize.x + m_Spacing.x), 0, 1),
                    y = 1 - Mathf.Clamp(Mathf.Abs(viewPosition.y) / (viewSize.y + m_Spacing.y), 0, 1)
                };

                distanceRatio.x = Mathf.Clamp(distanceRatio.x, m_MinSize, 1);
                distanceRatio.y = Mathf.Clamp(distanceRatio.y, m_MinSize, 1);

                var targetValue = Vector3.one * distanceRatio.x;

                switch (m_SmoothMode)
                {
                    case SmoothMode.None: fitterItem.localScale = targetValue; break;
                    case SmoothMode.Lerp: fitterItem.localScale = Lerp(fitterItem.localScale, targetValue); break;
                }
            }
        }

        protected Vector3 Lerp(Vector3 currentValue, Vector3 targetValue)
        {
            return Vector3.Lerp(currentValue, targetValue, Time.deltaTime * m_DecelerationRate);
        }

        private Vector2 CalculateOffsetPosition(Vector2 anchoredPosition, Vector2 viewSize, IEffectable effectable)
        {
            var offset = new Vector2
            {
                x = anchoredPosition.x + effectable.TargetRect.anchoredPosition.x,
                y = anchoredPosition.y + effectable.TargetRect.anchoredPosition.y
            };

            return new Vector2(offset.x - viewSize.x, offset.y - viewSize.y);
        }
    }
}