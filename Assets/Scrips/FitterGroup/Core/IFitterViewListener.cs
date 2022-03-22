using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.FitterGroup
{
    public interface IFitterViewListener
    {
        void OnViewChange(FitterView fitterView);
        void OnAxisChange(FitterView fitterView);
        void LateUpdate();
    }
}