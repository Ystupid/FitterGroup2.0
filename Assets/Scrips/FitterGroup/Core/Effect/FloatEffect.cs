using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup.Effect
{
    public class FloatEffect : FitterEffect, ILayoutModifier
    {
        public override FitterEffectType EffectType => FitterEffectType.Float;

        private int m_CurrentIndex;
        private float m_Offset;
        private float m_Time;

        [SerializeField] protected SmoothMode m_SmoothMode = SmoothMode.Lerp;
        public SmoothMode SmoothMode
        {
            get => m_SmoothMode;
            set => m_SmoothMode = value;
        }

        [SerializeField] protected float m_DecelerationRate = 10;
        public float DecelerationRate
        {
            get => m_DecelerationRate;
            set => m_DecelerationRate = value;
        }

        [SerializeField] protected float m_RestoreRate = 10;
        public float RestoreRate
        {
            get => m_RestoreRate;
            set => m_RestoreRate = value;
        }

        [SerializeField] protected bool m_ChangeReset = true;
        public bool ChangeReset
        {
            get => m_ChangeReset;
            set => m_ChangeReset = value;
        }

        [SerializeField] protected float m_FloatSpeed = 2;
        public float FloatSpeed
        {
            get => m_FloatSpeed;
            set => m_FloatSpeed = value;
        }

        [SerializeField] protected float m_FloatRange = 100;
        private Vector2 m_SmoothVelocity;

        public float FloatRange
        {
            get => m_FloatRange;
            set => m_FloatRange = value;
        }

        public override void LateUpdate<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable)
        {
            var result = CalculateIndex(keyValues, effectable);

            if (result.item == null) return;

            if (result.index != m_CurrentIndex)
            {
                m_Time = ChangeReset ? 0 : m_Time;
                m_CurrentIndex = result.index;
            }

            var floatValue = Mathf.Sin(m_Time += Time.deltaTime * m_FloatSpeed) * m_FloatRange * Vector2.one;

            var anchoredPosition = result.item.RectTransform.anchoredPosition;
            anchoredPosition.y = floatValue.y + m_Offset;

            switch (m_SmoothMode)
            {
                case SmoothMode.None: result.item.RectTransform.anchoredPosition = anchoredPosition; break;
                case SmoothMode.Lerp: result.item.RectTransform.anchoredPosition = Lerp(result.item.RectTransform.anchoredPosition, anchoredPosition); break;
                case SmoothMode.Smooth: result.item.RectTransform.anchoredPosition = Smooth(result.item.RectTransform.anchoredPosition, anchoredPosition); break;
            }

            var rectTransform = default(RectTransform);
            foreach (var item in keyValues)
            {
                rectTransform = item.Value.RectTransform;

                if (rectTransform == result.item.RectTransform) continue;

                var targetPosition = rectTransform.anchoredPosition;
                targetPosition.y = m_Offset;

                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * m_RestoreRate);
            }
        }

        protected Vector2 Lerp(Vector2 currentValue, Vector2 targetValue)
        {
            return Vector2.Lerp(currentValue, targetValue, Time.deltaTime * m_DecelerationRate);
        }

        protected Vector2 Smooth(Vector2 currentValue, Vector2 targetValue)
        {
            return Vector2.SmoothDamp(currentValue, targetValue, ref m_SmoothVelocity, Time.deltaTime * m_DecelerationRate);
        }

        protected virtual (int index,IFitterItem item) CalculateIndex<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable) where T : IFitterItem
        {
            var minDistance = float.MaxValue;
            var currentIndex = -1;
            var currentItem = default(IFitterItem);
            Vector2 center = effectable.ViewRect.position;

            foreach (var item in keyValues)
            {
                var distance = Mathf.Abs(item.Value.RectTransform.position.x - center.x);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    currentIndex = item.Key;
                    currentItem = item.Value;
                }
            }
            return (currentIndex, currentItem);
        }

        public void ModifyContentRect(ref Vector2 rectSize, ILayoutProperty layoutProperty, int itemCount) { }
        public void ModifyItemPosition(ref Vector2 itemPosition, ILayoutProperty layoutProperty, int itemCount) => m_Offset = itemPosition.y;
        public void ModifyMinIndex(ref int index, ILayoutProperty layoutProperty, int itemCount) { }
        public void ModifyMaxIndex(ref int index, ILayoutProperty layoutProperty, int itemCount) { }
    }
}