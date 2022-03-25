using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup.Effect
{
    [System.Serializable]
    public abstract class FitterEffect
    {
        protected bool m_IsActive;
        public bool IsActive => m_IsActive;

        public abstract FitterEffectType EffectType { get; }

        public virtual void Init(IEffectable effectable)
        {
        }

        public virtual void Active(IEffectable effectable) => m_IsActive = true;
        public virtual void Disable(IEffectable effectable) => m_IsActive = false;
        public virtual void UpdateBefore<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable) where T : IFitterItem { }
        public virtual void UpdateAfter<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable) where T : IFitterItem { }
        public virtual void LateUpdate<T>(IEnumerable<KeyValuePair<int, T>> keyValues, IEffectable effectable) where T : IFitterItem { }
    }
}