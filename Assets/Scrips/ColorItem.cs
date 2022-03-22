using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.FitterGroup;

public class ColorItem : MonoBehaviour, IFitterItem
{
    [SerializeField] private Text m_Label;

    private RectTransform m_RectTransform;
    public RectTransform RectTransform => m_RectTransform == null ? m_RectTransform = GetComponent<RectTransform>() : m_RectTransform;

    public void Refresh(int data)
    {
        m_Label.text = data.ToString();
    }
}
