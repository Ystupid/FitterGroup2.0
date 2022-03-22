using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public class VerticalLayout<T> : LayoutBase<T> where T : IFitterItem
    {
        public override void Init(ILayoutProperty layoutProperty, ILayoutListener<T> layoutListener)
        {
            base.Init(layoutProperty, layoutListener);

            ScrollRect.horizontal = false;
            ScrollRect.vertical = true;

            TargetRect.anchorMin = new Vector2(0, 1);
            TargetRect.anchorMax = new Vector2(1, 1);
            TargetRect.pivot = new Vector2(0.5f, 1);
        }

        public override void Refresh()
        {
            base.Refresh();
            var anchorPosition = TargetRect.anchoredPosition;
            anchorPosition.x = 0;
            TargetRect.anchoredPosition = anchorPosition;
        }

        protected override int CalculateMinIndex()
        {
            int index = Mathf.CeilToInt((AnchorPosition.y - Spacing.y - Padding.Top) / Spacing.y) * ConstraintCount;
            index = Mathf.Clamp(index, 0, ItemCount - 1);
            return index;
        }

        protected override int CalculateMaxIndex()
        {
            int index = Mathf.CeilToInt((AnchorPosition.y - Spacing.y - Padding.Top + CellSpacing.y + ViewSize.y) / Spacing.y) * ConstraintCount + (ConstraintCount - 1);
            index = Mathf.Clamp(index, 0, ItemCount - 1);
            return index;
        }

        protected override Vector2 CalculatePosition(RectTransform itemRect, int index)
        {
            var anchoredPosition = itemRect.anchoredPosition;

            anchoredPosition.x = Spacing.x * (index % ConstraintCount) + Padding.Left + (CellSize.x * itemRect.pivot.x);
            anchoredPosition.y = Spacing.y * (-index / ConstraintCount) - Padding.Top;
            anchoredPosition.y -= (CellSize.y * itemRect.pivot.y);

            return anchoredPosition;
        }

        protected override Vector2 CalculateContentSize()
        {
            var contentSize = new Vector2();
            contentSize.x = Spacing.x * ConstraintCount - CellSpacing.x + Padding.Width;
            contentSize.y = Mathf.Ceil(ItemCount * 1.0f / ConstraintCount) * Spacing.y - CellSpacing.y + Padding.Height;
            return contentSize;
        }
    }
}