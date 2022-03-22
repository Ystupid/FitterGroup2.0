using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Effect;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup.Editor
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(FitterView))]
    public class FitterGroupEditor : EditorRunnable, IFitterViewListener,IEffectable
    {
        [SerializeField] private FitterView m_FitterView;

        private EffectGroup m_EffectGroup;
        private LayoutBase<EditorItem> m_Layout;

        public ScrollRect ScrollRect => m_FitterView.ScrollRect;
        public RectTransform TargetRect => m_FitterView.TargetRect;
        public RectTransform ViewRect => m_FitterView.ViewRect;

        protected override void EditorEnable()
        {
            m_FitterView = GetComponent<FitterView>();
            m_EffectGroup = m_FitterView.EffectGroup;
            m_EffectGroup.Init(this);
            m_FitterView.Listener = this;
            ChangeLayout(m_FitterView.LayoutAxis);
        }

        protected override void EditorUpdate()
        {
            m_Layout?.Tick();
        }

        public void OnViewChange(FitterView fitterView)
        {
            ChangeLayout(fitterView.LayoutAxis);
            m_Layout.Refresh();
        }

        protected void ChangeLayout(FitterAxis fitterAxis)
        {
            m_Layout = LayoutHelper.GetLayoutEditor(fitterAxis);
            m_Layout.Init(m_FitterView, null);
        }

        public void LateUpdate()
        {
            m_EffectGroup.LateUpdate(m_Layout);
        }

        public void OnAxisChange(FitterView fitterView)
        {
            ChangeLayout(fitterView.LayoutAxis);
            m_Layout.Refresh();
        }
    }
}