using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Effect
{
    public class GradientEffect : FitterEffect
    {
        public override FitterEffectType EffectType => FitterEffectType.Gradient;

        [SerializeField] protected Gradient m_GradientColor;
        public Gradient GradientColor
        {
            get => m_GradientColor;
            set => m_GradientColor = value;
        }

        private Dictionary<IFitterItem, GraphicGradient> m_CacheMap;

        private Dictionary<IFitterItem, GraphicGradient> CacheMap
        {
            get
            {
                if (m_CacheMap == null) m_CacheMap = new Dictionary<IFitterItem, GraphicGradient>();
                return m_CacheMap;
            }
        }

        private GraphicGradient this[IFitterItem fitterItem]
        {
            get
            {
                if (!CacheMap.ContainsKey(fitterItem))
                {
                    var graphicGradient = fitterItem.RectTransform.gameObject.GetComponent<GraphicGradient>();
                    if (graphicGradient == null)
                        graphicGradient = fitterItem.RectTransform.gameObject.AddComponent<GraphicGradient>();
                    CacheMap.Add(fitterItem, graphicGradient);
                }

                return CacheMap[fitterItem];
            }
        }

        public override void UpdateAfter<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable)
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

                var startRatio = Mathf.Clamp(viewPosition.x / (viewSize.x * 2), 0, 1);
                var endRatio = Mathf.Clamp((viewPosition.x + fitterItem.rect.width) / (viewSize.x * 2), 0, 1);

                var graphicGradient = this[item.Value];
                var gradient = graphicGradient.GradientColor;

                var colorKeys = new GradientColorKey[]
                {
                     new GradientColorKey(m_GradientColor.Evaluate(startRatio),0),
                     new GradientColorKey(m_GradientColor.Evaluate(endRatio),1)
                };
                var alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(m_GradientColor.Evaluate(startRatio).a,0),
                    new GradientAlphaKey(m_GradientColor.Evaluate(endRatio).a,1),
                };

                gradient.SetKeys(colorKeys, alphaKeys);
                graphicGradient.Apply();

            }

        }

        private Vector2 CalculateOffsetPosition(Vector2 anchoredPosition, Vector2 viewSize, IEffectable effectable)
        {
            return new Vector2
            {
                x = anchoredPosition.x + effectable.TargetRect.anchoredPosition.x,
                y = anchoredPosition.y + effectable.TargetRect.anchoredPosition.y
            };
        }
    }
}