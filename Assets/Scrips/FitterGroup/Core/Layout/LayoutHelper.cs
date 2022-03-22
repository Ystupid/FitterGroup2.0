using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public static class LayoutHelper
    {
        public static LayoutBase<TItem> GetLayout<TItem>(FitterAxis fitterAxis) where TItem : IFitterItem
        {
            switch (fitterAxis)
            {
                case FitterAxis.Horizontal: return new HorizontalLayout<TItem>();
                case FitterAxis.Vertical: return new VerticalLayout<TItem>();
                default: return new VerticalLayout<TItem>();
            }
        }

        public static LayoutBase<EditorItem> GetLayoutEditor(FitterAxis fitterAxis)
        {
            switch (fitterAxis)
            {
                case FitterAxis.Horizontal: return new EditorHorizontalLayout();
                case FitterAxis.Vertical: return new EditorVerticalLayout();
                default: return new EditorVerticalLayout();
            }
        }
    }
}