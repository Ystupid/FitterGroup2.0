using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.FitterGroup;

public class Main : MonoBehaviour, IFitterable<ColorItem>
{
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private FitterView m_FitterView;

    private FitterGroup<ColorItem> m_FitterGroup;

    private List<int> m_DataList;
    private Queue<ColorItem> m_CacheQueue;

    public int ItemCount => m_DataList.Count;

    public void DisableItem(int index, ColorItem item)
    {
        item.gameObject.SetActive(false);
        m_CacheQueue.Enqueue(item);
    }

    public ColorItem EnableItem(int index)
    {
        var item = default(ColorItem);

        if (m_CacheQueue.Count <= 0)
        {
            item = Instantiate(m_Prefab, m_FitterGroup.TargetRect).GetComponent<ColorItem>();
        }
        else
        {
            item = m_CacheQueue.Dequeue();
            item.gameObject.SetActive(true);
        }

        item.Refresh(m_DataList[index]);

        return item;
    }

    private void Start()
    {
        m_DataList = new List<int>();
        for (int i = 0; i < 100; i++)
            m_DataList.Add(i);

        m_CacheQueue = new Queue<ColorItem>();
        m_FitterGroup = new FitterGroup<ColorItem>(this, m_FitterView);
        m_FitterGroup.Refresh();
    }
}
