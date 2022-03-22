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
    }
}