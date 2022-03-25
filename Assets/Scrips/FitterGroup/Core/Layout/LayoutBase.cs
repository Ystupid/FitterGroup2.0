using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public abstract class LayoutBase<T> : IEnumerable<KeyValuePair<int, T>> where T : IFitterItem
    {
        protected ILayoutProperty m_LayoutProperty;
        protected ILayoutListener<T> m_LayoutListener;

        protected Queue<int> m_ClearQueue;
        protected Dictionary<int, T> m_ItemMap;
        protected RectTransform m_ViewRect;
        protected int m_CurrentMinIndex;
        protected int m_CurrentMaxIndex;
        protected int m_LastMinIndex;
        protected int m_LastMaxIndex;
        protected Vector2 m_ItemPosition;

        protected virtual int ItemCount => m_LayoutListener.ItemCount;
        protected Vector2 CellSize => m_LayoutProperty.CellSize;
        protected int ConstraintCount => m_LayoutProperty.ConstraintCount;
        protected Vector2 CellSpacing => m_LayoutProperty.CellSpacing;
        protected Padding Padding => m_LayoutProperty.Padding;
        protected ScrollRect ScrollRect => m_LayoutProperty.ScrollRect;
        protected RectTransform TargetRect => m_LayoutProperty.TargetRect;
        protected List<ILayoutModifier> ModifierList => m_LayoutListener.ModifierList;
        protected Vector2 Spacing => new Vector2(CellSize.x + CellSpacing.x, CellSize.y + CellSpacing.y);

        public int CurrentMinIndex => m_CurrentMinIndex;
        public int CurrentMaxIndex => m_CurrentMaxIndex;
        public int LastMinIndex => m_LastMinIndex;
        public int LastMaxIndex => m_LastMaxIndex;

        protected Vector2 AnchorPosition => TargetRect.anchoredPosition;
        protected Vector2 ViewSize
        {
            get
            {
                if (m_ViewRect == null)
                    m_ViewRect = TargetRect.parent.GetComponent<RectTransform>();
                return m_ViewRect.rect.size;
            }
        }

        public LayoutBase()
        {
            m_ClearQueue = new Queue<int>();
            m_ItemMap = new Dictionary<int, T>();
        }

        public virtual void Init(ILayoutProperty layoutProperty, ILayoutListener<T> layoutListener)
        {
            m_LayoutProperty = layoutProperty;
            m_LayoutListener = layoutListener;
        }

        public virtual void Tick()
        {
            ResetVisualScope();

            RefreshView();

            m_LastMinIndex = m_CurrentMinIndex;
            m_LastMaxIndex = m_CurrentMaxIndex;
        }

        public virtual void Clear()
        {
            foreach (var item in m_ItemMap.Keys)
                m_ClearQueue.Enqueue(item);

            while (m_ClearQueue.Count > 0)
                DisableItem(m_ClearQueue.Dequeue());

            m_ItemMap.Clear();
        }

        public virtual void Refresh()
        {
            Clear();
            ResetContentRect();
            Tick();
        }

        /// <summary>
        /// 重置可视范围
        /// </summary>
        protected void ResetVisualScope()
        {
            var minIndex = CalculateMinIndex();
            var maxIndex = CalculateMaxIndex();

            ForeachModifier(modifier => modifier.ModifyMinIndex(ref minIndex, m_LayoutProperty, ItemCount));
            ForeachModifier(modifier => modifier.ModifyMaxIndex(ref maxIndex, m_LayoutProperty, ItemCount));

            m_CurrentMinIndex = minIndex;
            m_CurrentMaxIndex = maxIndex;
        }

        /// <summary>
        /// 刷新视图
        /// </summary>
        protected virtual void RefreshView()
        {
            for (int i = m_LastMinIndex; i < m_CurrentMinIndex; i++)
                DisableItem(i);
            for (int i = Mathf.Max(m_CurrentMinIndex, 0); i < m_LastMinIndex; i++)
                EnableItem(i);

            for (int i = Mathf.Max(m_LastMaxIndex, 0); i <= m_CurrentMaxIndex; i++)
                EnableItem(i);
            for (int i = m_CurrentMaxIndex + 1; i <= m_LastMaxIndex; i++)
                DisableItem(i);
        }

        /// <summary>
        /// 重置锚点
        /// </summary>
        /// <param name="itemRect"></param>
        protected virtual T ResetAnchor(T item)
        {
            var itemRect = item.RectTransform;
            itemRect.pivot = new Vector2(0.5f, 0.5f);
            itemRect.anchorMin = new Vector2(0, 1);
            itemRect.anchorMax = new Vector2(0, 1);
            itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CellSize.x);
            itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CellSize.y);

            return item;
        }

        /// <summary>
        /// 重置坐标
        /// </summary>
        /// <param name="itemRect"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected T ResetPosition(T item, int index)
        {
            var position = CalculatePosition(item.RectTransform, index);

            ForeachModifier(modifier => modifier.ModifyItemPosition(ref position, m_LayoutProperty, ItemCount));

            item.RectTransform.anchoredPosition = position;

            return item;
        }

        /// <summary>
        /// 计算Item位置
        /// </summary>
        /// <param name="itemRect"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected abstract Vector2 CalculatePosition(RectTransform itemRect, int index);

        /// <summary>
        /// 计算最小下标
        /// </summary>
        /// <returns></returns>
        protected abstract int CalculateMinIndex();

        /// <summary>
        /// 计算最大下标
        /// </summary>
        /// <returns></returns>
        protected abstract int CalculateMaxIndex();

        /// <summary>
        /// 重置TargetRect大小
        /// </summary>
        /// <param name="size"></param>
        protected virtual void ResetContentRect()
        {
            var size = CalculateContentSize();

            ForeachModifier(modifier => modifier.ModifyContentRect(ref size,m_LayoutProperty,ItemCount));

            TargetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            TargetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }

        /// <summary>
        /// 计算ContentSizes
        /// </summary>
        /// <returns></returns>
        protected abstract Vector2 CalculateContentSize();

        /// <summary>
        /// 查询是否包含指定下标项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected bool HasItem(int index) => m_ItemMap.ContainsKey(index);

        /// <summary>
        /// 激活指定下标项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual void EnableItem(int index)
        {
            if (HasItem(index)) return;
            var item = m_LayoutListener.EnableItem(index);
            if (item == null) return;
            m_ItemMap.Add(index, ResetPosition(ResetAnchor(item), index));
        }

        /// <summary>
        /// 失活指定下标项
        /// </summary>
        /// <param name="index"></param>
        protected virtual void DisableItem(int index)
        {
            if (!HasItem(index)) return;
            m_LayoutListener.DisableItem(index, m_ItemMap[index]);
            m_ItemMap.Remove(index);
        }

        /// <summary>
        /// 遍历修改器
        /// </summary>
        /// <param name="action"></param>
        protected virtual void ForeachModifier(UnityAction<ILayoutModifier> action)
        {
            var modifierList = ModifierList;
            if (action == null || modifierList == null) return;

            for (int i = 0; i < modifierList.Count; i++)
                action(modifierList[i]);
        }

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator() => m_ItemMap.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => m_ItemMap.GetEnumerator();
    }
}