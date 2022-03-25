using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup.Effect
{
    [System.Serializable]
    public class ElasticityEffect : FitterEffect, ILayoutModifier
    {
        [SerializeField] private int m_CurrentIndex = -1;
        [SerializeField] private SmoothMode m_SmoothMode = SmoothMode.Lerp;
        [SerializeField] private float m_DecelerationRate = 16;

        private Vector2 m_SmoothVelocity;
        public int CurrentIndex => m_CurrentIndex;

        public float DecelerationRate
        {
            get => m_DecelerationRate;
            set => m_DecelerationRate = value;
        }

        public event UnityAction<int> OnIndexChange;

        public override FitterEffectType EffectType => FitterEffectType.Elasticity;

        public override void Init(IEffectable effectable)
        {
            effectable.ScrollRect.inertia = false;
            m_CurrentIndex = -1;
        }

        public override void Active(IEffectable effectable)
        {
            effectable.ScrollRect.inertia = false;
        }

        public override void Disable(IEffectable effectable)
        {
            effectable.ScrollRect.inertia = false;
        }

        public void ModifyContentRect(ref Vector2 rectSize, ILayoutProperty layoutProperty, int itemCount)
        {
            var viewWidth = layoutProperty.ViewRect.rect.width;
            var marginDistance = viewWidth / 2 - layoutProperty.CellSize.x * 0.5f;

            rectSize.x += marginDistance * 2;
        }

        public void ModifyItemPosition(ref Vector2 itemPosition, ILayoutProperty layoutProperty, int itemCount)
        {
            var viewWidth = layoutProperty.ViewRect.rect.width;
            var marginDistance = viewWidth / 2 - layoutProperty.CellSize.x * 0.5f;

            itemPosition.x += marginDistance;
        }

        public void ModifyMaxIndex(ref int index, ILayoutProperty layoutProperty, int itemCount)
        {
            var spacingX = layoutProperty.CellSize.x + layoutProperty.CellSpacing.x;
            var cellSpacingX = layoutProperty.CellSpacing.x;
            var anchorPosition = layoutProperty.TargetRect.anchoredPosition;
            var padding = layoutProperty.Padding;
            var constraintCount = layoutProperty.ConstraintCount;
            var viewWidth = layoutProperty.ViewRect.rect.width;
            var marginDistance = viewWidth / 2 - layoutProperty.CellSize.x * 0.5f;

            index = Mathf.CeilToInt((-anchorPosition.x - spacingX - padding.Left + cellSpacingX + viewWidth - marginDistance) / spacingX) * constraintCount + (constraintCount - 1);
            index = Mathf.Clamp(index, 0, itemCount - 1);
        }

        public void ModifyMinIndex(ref int index, ILayoutProperty layoutProperty, int itemCount)
        {
            var spacingX = layoutProperty.CellSize.x + layoutProperty.CellSpacing.x;
            var anchorPosition = layoutProperty.TargetRect.anchoredPosition;
            var padding = layoutProperty.Padding;
            var constraintCount = layoutProperty.ConstraintCount;

            var viewWidth = layoutProperty.ViewRect.rect.width;
            var marginDistance = viewWidth / 2 - layoutProperty.CellSize.x * 0.5f;

            index = Mathf.CeilToInt((-anchorPosition.x - spacingX - padding.Left - marginDistance) / spacingX) * constraintCount;
            index = Mathf.Clamp(index, 0, itemCount - 1);
        }

        public override void LateUpdate<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable)
        {
            var result = CalculateIndex(keyValues, effectable);

            if (result.item == null) return;

            if (result.index != m_CurrentIndex)
            {
                OnIndexChange?.Invoke(m_CurrentIndex);
                m_CurrentIndex = result.index;
            }

            var currentItem = result.item;

            if (Input.GetMouseButton(0)) return;

            var targetValue = CalculateTargetAnchor(currentItem.RectTransform.anchoredPosition, effectable) * -1;

            switch (m_SmoothMode)
            {
                case SmoothMode.None: effectable.TargetRect.anchoredPosition = targetValue; break;
                case SmoothMode.Lerp: effectable.TargetRect.anchoredPosition = Lerp(effectable.TargetRect.anchoredPosition, targetValue); break;
                case SmoothMode.Smooth: effectable.TargetRect.anchoredPosition = Smooth(effectable.TargetRect.anchoredPosition, targetValue); break;
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

        protected virtual (int index, IFitterItem item) CalculateIndex<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable) where T : IFitterItem
        {
            var minDistance = float.MaxValue;
            var currentIndex = -1;
            var currentItem = default(IFitterItem);
            Vector2 center = effectable.ViewRect.position;

            foreach (var item in keyValues)
            {
                var distance = Vector2.Distance(center, item.Value.RectTransform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    currentIndex = item.Key;
                    currentItem = item.Value;
                }
            }
            return (currentIndex, currentItem);
        }

        protected virtual Vector2 CalculateTargetAnchor(Vector2 anchorPoint, IEffectable effectable)
        {
            var targetPoint = new Vector2(anchorPoint.x - effectable.ViewRect.rect.width / 2, effectable.TargetRect.anchoredPosition.y);
            return targetPoint;
        }
    }
}