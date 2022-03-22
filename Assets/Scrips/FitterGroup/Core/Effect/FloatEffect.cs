using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Effect
{
    public class FloatEffect : FitterEffect
    {
        public override FitterEffectType EffectType => FitterEffectType.Float;

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
    }
}