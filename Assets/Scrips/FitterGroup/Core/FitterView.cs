using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.FitterGroup.Effect;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup
{
    [DisallowMultipleComponent, RequireComponent(typeof(ScrollRect))]
    public class FitterView : EditorRunnable, ILayoutProperty
    {
        protected IFitterViewListener m_Listener;
        public IFitterViewListener Listener
        {
            get => m_Listener;
            set => m_Listener = value;
        }

        [SerializeField] protected Padding m_Padding;
        public virtual Padding Padding
        {
            get => m_Padding;
            set
            {
                m_Padding = value;
                OnViewChange();
            }
        }

        [SerializeField] protected Vector2 m_CellSize = new Vector2(200, 200);
        public virtual Vector2 CellSize
        {
            get => m_CellSize;
            set
            {
                m_CellSize = value;
                OnViewChange();
            }
        }

        [SerializeField] protected Vector2 m_Spacing = new Vector2(50, 50);
        public virtual Vector2 CellSpacing
        {
            get => m_Spacing;
            set
            {
                m_Spacing = value;
                OnViewChange();
            }
        }

        protected FitterAxis m_LastAxis;
        [SerializeField] protected FitterAxis m_LayoutAxis;
        public virtual FitterAxis LayoutAxis
        {
            get => m_LayoutAxis;
            set
            {
                m_LayoutAxis = value;
                OnAxisChange();
            }
        }

        [SerializeField] protected int m_ConstraintCount;
        public virtual int ConstraintCount
        {
            get => m_ConstraintCount;
            set
            {
                m_ConstraintCount = Mathf.Max(1, m_ConstraintCount);
                OnViewChange();
            }
        }

        [SerializeField] protected EffectGroup m_EffectGroup;
        public virtual EffectGroup EffectGroup
        {
            get => m_EffectGroup;
        }

        [SerializeField] protected ScrollRect m_ScrollRect;
        public virtual ScrollRect ScrollRect
        {
            get => m_ScrollRect;
            private set => m_ScrollRect = value;
        }

        public RectTransform TargetRect => m_ScrollRect.content;
        public RectTransform ViewRect => m_ScrollRect.viewport;

        #region RunInEditor
        protected override void EditorInit()
        {
            if (m_ScrollRect == null) m_ScrollRect = GetComponentInChildren<ScrollRect>();
        }

        protected override void EditorValidate()
        {
            ConstraintCount = ConstraintCount;
            OnViewChange();

            if (m_LastAxis != m_LayoutAxis)
                OnAxisChange();

            m_LastAxis = m_LayoutAxis;
        }
        #endregion

        public void OnViewChange()
        {
            m_Listener?.OnViewChange(this);
        }

        public void OnAxisChange()
        {
            m_Listener?.OnAxisChange(this);
        }

        public void LateUpdate()
        {
            m_Listener?.LateUpdate();
        }
    }
}