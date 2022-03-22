using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.FitterGroup.Effect;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup
{
    public class FitterGroup<T> : IEffectable, IFitterViewListener, ILayoutListener<T> where T : IFitterItem
    {
        protected IFitterable<T> m_Fitterable;
        protected LayoutBase<T> m_Layout;
        protected FitterView m_FitterView;
        protected EffectGroup m_EffectGroup;

        public ScrollRect ScrollRect => m_FitterView.ScrollRect;
        public RectTransform TargetRect => m_FitterView.TargetRect;
        public RectTransform ViewRect => m_FitterView.ViewRect;

        public FitterGroup(IFitterable<T> fitterable, FitterView fitterView)
        {
            if (fitterable == null || fitterView == null) throw new ArgumentNullException();

            m_Fitterable = fitterable;
            m_FitterView = fitterView;
            m_EffectGroup = fitterView.EffectGroup;

            ChangeLayout(m_FitterView.LayoutAxis);

            m_EffectGroup.Init(this);
            m_FitterView.Listener = this;

            m_FitterView.ScrollRect.onValueChanged.AddListener(OnRectScroll);
        }

        public void OnViewChange(FitterView fitterView) => Refresh();
        public void OnAxisChange(FitterView fitterView)
        {
            ChangeLayout(fitterView.LayoutAxis);
            Refresh();
        }
        public void OnRectScroll(Vector2 rect) => Update();
        public void Update()
        {
            m_EffectGroup.UpdateBefore(m_Layout);
            m_Layout.Tick();
            m_EffectGroup.UpdateAfter(m_Layout);
        }
        public void LateUpdate() => m_EffectGroup?.LateUpdate(m_Layout);
        public void Clear() => m_Layout?.Clear();
        public void Refresh()
        {
            m_Layout.Refresh();
            Update();
        }

        protected void ChangeLayout(FitterAxis fitterAxis)
        {
            Clear();

            m_Layout = LayoutHelper.GetLayout<T>(fitterAxis);

            m_Layout.Init(m_FitterView, this);
        }

        public List<ILayoutModifier> ModifierList => m_EffectGroup.ModifierList;

        public int ItemCount => m_Fitterable.ItemCount;
        public T EnableItem(int index) => m_Fitterable.EnableItem(index);
        public void DisableItem(int index, T item) => m_Fitterable.DisableItem(index, item);
    }
}