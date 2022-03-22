using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Effect
{
    public interface IEffectable
    {
        ScrollRect ScrollRect { get; }
        RectTransform TargetRect { get; }
        RectTransform ViewRect { get; }
    }
}