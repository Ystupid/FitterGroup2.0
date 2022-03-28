using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    public interface IOptimizeEnumerator<T>
    {
        Dictionary<int, T>.Enumerator GetEnumerator();
    }
}