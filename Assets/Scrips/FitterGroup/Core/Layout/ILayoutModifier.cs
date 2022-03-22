using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public interface ILayoutModifier
    {
        void ModifyContentRect(ref Vector2 rectSize, ILayoutProperty layoutProperty, int itemCount);
        void ModifyItemPosition(ref Vector2 itemPosition, ILayoutProperty layoutProperty, int itemCount);
        void ModifyMinIndex(ref int index, ILayoutProperty layoutProperty, int itemCount);
        void ModifyMaxIndex(ref int index, ILayoutProperty layoutProperty, int itemCount);
    }
}