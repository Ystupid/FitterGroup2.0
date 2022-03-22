using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public interface ILayoutProperty
    {
        Padding Padding { get; }
        Vector2 CellSize { get; }
        Vector2 CellSpacing { get; }
        int ConstraintCount { get; }
        ScrollRect ScrollRect { get; }
        RectTransform TargetRect { get; }
        RectTransform ViewRect { get; }
    }
}

