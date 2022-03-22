using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup.Layout
{
    [System.Serializable]
    public struct Padding
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public float Height => Top + Bottom;
        public float Width => Left + Right;
    }
}