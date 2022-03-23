using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup.Effect
{
    public class FloatEffect : FitterEffect,ILayoutModifier
    {
        public override FitterEffectType EffectType => FitterEffectType.Float;

        private float m_Time;

        [SerializeField] protected float m_FloatSpeed = 2;
        public float FloatSpeed
        {
            get => m_FloatSpeed;
            set => m_FloatSpeed = value;
        }

        [SerializeField] protected float m_FloatRange = 100;
        public float FloatRange
        {
            get => m_FloatRange;
            set => m_FloatRange = value;
        }

        public override void LateUpdate<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable)
        {
            var result = CalculateIndex(keyValues, effectable);

            if (result.item == null) return;

            var floatValue =  Mathf.Sin(m_Time += Time.deltaTime * m_FloatSpeed) * m_FloatRange * Vector2.one;

            var anchoredPosition = result.item.RectTransform.anchoredPosition;
            var offset = anchoredPosition.y;

            anchoredPosition.y = floatValue.y - 200;
            result.item.RectTransform.anchoredPosition = anchoredPosition;
        }

        public override void UpdateAfter<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable)
        {
            //var result = CalculateIndex(keyValues, effectable);

            //var rectTransform = default(RectTransform);

            //foreach (var item in keyValues)
            //{
            //    rectTransform = item.Value.RectTransform;

            //    if (item.Value.RectTransform == result.item.RectTransform) continue;

            //}
        }

        protected virtual (int index, IFitterItem item) CalculateIndex<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable) where T : IFitterItem
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

        public void ModifyContentRect(ref Vector2 rectSize, ILayoutProperty layoutProperty, int itemCount)
        {
        }

        public void ModifyItemPosition(ref Vector2 itemPosition, ILayoutProperty layoutProperty, int itemCount)
        {
            
        }

        public void ModifyMinIndex(ref int index, ILayoutProperty layoutProperty, int itemCount)
        {
        }

        public void ModifyMaxIndex(ref int index, ILayoutProperty layoutProperty, int itemCount)
        {
        }
    }
}