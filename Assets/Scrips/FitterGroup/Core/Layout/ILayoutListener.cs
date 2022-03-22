using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public interface ILayoutListener<T> where T : IFitterItem
    {
        int ItemCount { get; }
        T EnableItem(int index);
        void DisableItem(int index, T item);
        List<ILayoutModifier> ModifierList { get; }
    }
}