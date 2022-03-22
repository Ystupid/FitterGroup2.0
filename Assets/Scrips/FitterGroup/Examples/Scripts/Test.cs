using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.FitterGroup;

public class Test : MonoBehaviour/*,IFitterable*/
{
    //[SerializeField] private FitterGroup m_FitterGroup;

    //private GameObject m_Prefab;
    //protected Queue<RectTransform> m_CacheQueue = new Queue<RectTransform>();
    //private List<int> m_ItemData = new List<int>();

    //private void Awake()
    //{
    //    m_Prefab = Resources.Load<GameObject>("Item");
    //    for (int i = 0; i < 40; i++)
    //        m_ItemData.Add(i);

    //    m_FitterGroup.Init(this);
    //}

    //public int ItemCount => m_ItemData.Count;

    //public void DisableItem(int index, RectTransform itemRect)
    //{
    //    Debug.LogError($"DisableItem:{index}");

    //    itemRect.gameObject.SetActive(false);
    //    m_CacheQueue.Enqueue(itemRect);
    //}

    //public RectTransform EnableItem(int index)
    //{
    //    Debug.Log($"EnableItem:{index}");

    //    RectTransform entityRect = null;
    //    if(m_CacheQueue.Count > 0)
    //    {
    //        entityRect = m_CacheQueue.Dequeue();
    //        entityRect.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        entityRect = Instantiate(m_Prefab, m_FitterGroup.TargetRect).GetComponent<RectTransform>();
    //    }
    //    entityRect.GetComponentInChildren<Text>().text = index.ToString();
    //    return entityRect;
    //}
}
