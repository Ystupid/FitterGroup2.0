using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public class HorizontalLayout<T> : LayoutBase<T> where T : IFitterItem
    {
        public override void Init(ILayoutProperty layoutProperty, ILayoutListener<T> layoutListener)
        {
            base.Init(layoutProperty, layoutListener);

            ScrollRect.horizontal = true;
            ScrollRect.vertical = false;

            TargetRect.anchorMin = new Vector2(0, 0);
            TargetRect.anchorMax = new Vector2(0, 1);
            TargetRect.pivot = new Vector2(0, 0.5f);
        }

        public override void Refresh()
        {
            base.Refresh();
            var anchorPosition = TargetRect.anchoredPosition;
            anchorPosition.y = 0;
            TargetRect.anchoredPosition = anchorPosition;
        }

        protected override int CalculateMinIndex()
        {
            int index = Mathf.CeilToInt((-AnchorPosition.x - Spacing.x - Padding.Left) / Spacing.x) * ConstraintCount;
            index = Mathf.Clamp(index, 0, ItemCount - 1);
            return index;
        }

        protected override int CalculateMaxIndex()
        {
            int index = Mathf.CeilToInt((-AnchorPosition.x - Spacing.x - Padding.Left + CellSpacing.x + ViewSize.x) / Spacing.x) * ConstraintCount + (ConstraintCount - 1);
            index = Mathf.Clamp(index, 0, ItemCount - 1);
            return index;
        }

        protected override Vector2 CalculatePosition(RectTransform itemRect, int index)
        {
            var anchoredPosition = itemRect.anchoredPosition;

            anchoredPosition.y = Spacing.y * (-index % ConstraintCount) - Padding.Top - (CellSize.y * itemRect.pivot.y);
            anchoredPosition.x = Spacing.x * (index / ConstraintCount) + Padding.Left;
            anchoredPosition.x += (CellSize.x * itemRect.pivot.x);

            return anchoredPosition;
        }

        protected override Vector2 CalculateContentSize()
        {
            var contentSize = new Vector2();
            contentSize.x = Mathf.Ceil(ItemCount * 1.0f / ConstraintCount) * Spacing.x - CellSpacing.x + Padding.Width;
            contentSize.y = Spacing.y * ConstraintCount - CellSpacing.y + Padding.Height;
            return contentSize;
        }
    }
}