using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup
{
    public class EditorItem : IFitterItem
    {
        public EditorItem(RectTransform rectTransform) => m_RectTransform = rectTransform;

        private RectTransform m_RectTransform;
        public RectTransform RectTransform => m_RectTransform;
    }
}