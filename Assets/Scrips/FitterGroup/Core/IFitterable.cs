using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI.FitterGroup
{
    public interface IFitterable<T> where T : IFitterItem
    {
        int ItemCount { get; }
        T EnableItem(int index);
        void DisableItem(int index, T item);
    }
}