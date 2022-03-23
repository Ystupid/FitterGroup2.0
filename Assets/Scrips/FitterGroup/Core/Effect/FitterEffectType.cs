using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI.FitterGroup.Effect
{
    [Flags]
    public enum FitterEffectType
    {
        Scale = 1,
        Gradient = 2,
        Elasticity = 4,
        Float = 8,
    }
}