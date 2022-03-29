using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup.Effect
{
    [Serializable]
    public class EffectGroup
    {
        public const string Namespace = "UnityEngine.UI.FitterGroup.Effect.";

        [SerializeField]
        protected FitterEffectType m_Effects;

        [SerializeReference]
        protected List<FitterEffect> m_EffectList;
        public List<FitterEffect> EffectList => m_EffectList;

        protected IEffectable m_Effectable;

        public EffectGroup() => m_EffectList = m_EffectList ?? new List<FitterEffect>();
        public void Init(IEffectable effectable)
        {
            m_Effectable = effectable;
            Foreach(match => match.Init(m_Effectable));
        }

        public bool Contains(FitterEffectType effectType) => m_Effects.HasFlag(effectType);
        public bool Create(FitterEffectType effectType)
        {
            if (Contains(effectType)) return false;

            m_Effects |= effectType;
            var effect = CreateEffect(effectType);
            effect.Init(m_Effectable);
            effect.Active(m_Effectable);
            m_EffectList.Add(effect);
            return true;
        }

        public bool Add(FitterEffect fitterEffect)
        {
            if (Contains(fitterEffect.EffectType)) return false;

            fitterEffect.Active(m_Effectable);

            m_EffectList.Add(fitterEffect);

            return true;
        }

        public bool Remove(FitterEffectType effectType)
        {
            if (!Contains(effectType)) return false;

            m_Effects &= ~effectType;

            var findIndex = m_EffectList.FindIndex(match => match.EffectType == effectType);

            if (findIndex == -1) return false;

            var effect = m_EffectList[findIndex];
            effect.Disable(m_Effectable);

            m_EffectList.RemoveAt(findIndex);

            return true;
        }

        public void Clear()
        {
            m_Effects = 0;
            for (int i = m_EffectList.Count - 1; i >= 0; i--)
            {
                m_EffectList[i].Disable(m_Effectable);
                m_EffectList.RemoveAt(i);
            }
        }

        protected List<ILayoutModifier> m_ModifierList;
        public List<ILayoutModifier> ModifierList
        {
            get
            {
                if (m_ModifierList == null)
                    m_ModifierList = new List<ILayoutModifier>();

                m_ModifierList.Clear();

                var effect = default(FitterEffect);
                for (int i = 0; i < m_EffectList.Count; i++)
                {
                    effect = m_EffectList[i];
                    if (effect is ILayoutModifier)
                        m_ModifierList.Add(effect as ILayoutModifier);
                }

                return m_ModifierList;
            }
        }

        public void LateUpdate<T>(IOptimizeEnumerator<T> keyValues) where T : IFitterItem
        {
            for (int i = 0; i < m_EffectList.Count; i++)
                m_EffectList[i].LateUpdate(keyValues,m_Effectable);
        }

        public void UpdateBefore<T>(IOptimizeEnumerator<T> keyValues) where T : IFitterItem
        {
            for (int i = 0; i < m_EffectList.Count; i++)
                m_EffectList[i].UpdateBefore(keyValues, m_Effectable);
        }

        public void UpdateAfter<T>(IOptimizeEnumerator<T> keyValues) where T : IFitterItem
        {
            for (int i = 0; i < m_EffectList.Count; i++)
                m_EffectList[i].UpdateAfter(keyValues, m_Effectable);
        }

        public void Foreach(Action<FitterEffect> action)
        {
            if (action == null) return;

            for (int i = 0; i < m_EffectList.Count; i++)
                action(m_EffectList[i]);
        }

        #region Create
        public static T CreateEffect<T>() where T : FitterEffect => CreateEffect(typeof(T)) as T;

        public static FitterEffect CreateEffect(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Parse Error:" + type);
                return null;
            }

            return Activator.CreateInstance(type) as FitterEffect;
        }

        public static FitterEffect CreateEffect(FitterEffectType effectType)
        {
            var typeName = new StringBuilder(Namespace).Append(effectType.ToString()).Append("Effect");

            var type = Type.GetType(typeName.ToString());

            return CreateEffect(type);
        }
        #endregion
    }
}