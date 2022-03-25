using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.FitterGroup;
using UnityEngine.UI.FitterGroup.Effect;

[CustomEditor(typeof(FitterView))]
public class FitterViewEditor : Editor
{
    private SerializedProperty m_Padding;
    private SerializedProperty m_CellSize;
    private SerializedProperty m_Spacing;
    private SerializedProperty m_LayoutAxis;
    private SerializedProperty m_ConstraintCount;
    private SerializedProperty m_ScrollRect;
    private SerializedProperty m_EffectGroup;
    private SerializedProperty m_Effects;
    private SerializedProperty m_EffectArray;

    private FitterView m_FitterView;
    private int m_LastFlag;
    private List<FitterEffect> m_EffectList;

    private FitterEffectType[] m_TypeArray;
    private Queue<FitterEffectType> m_FiltrateQueue;
    private Dictionary<int, EffectProperty> m_EffectPropertyMap;

    private EffectProperty this[int effectType]
    {
        get
        {
            if (!m_EffectPropertyMap.ContainsKey(effectType))
            {
                var effectProperty = new EffectProperty
                {
                    IsShow = false,
                    Spread = false,
                    EffectType = (FitterEffectType)effectType,
                };

                m_EffectPropertyMap.Add(effectType, effectProperty);
            }

            return m_EffectPropertyMap[effectType];
        }
    }

    private void OnEnable()
    {
        m_FitterView = target as FitterView;

        m_Padding = serializedObject.FindProperty("m_Padding");
        m_CellSize = serializedObject.FindProperty("m_CellSize");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_LayoutAxis = serializedObject.FindProperty("m_LayoutAxis");
        m_ConstraintCount = serializedObject.FindProperty("m_ConstraintCount");
        m_ScrollRect = serializedObject.FindProperty("m_ScrollRect");
        m_EffectGroup = serializedObject.FindProperty("m_EffectGroup"); 
        m_Effects = m_EffectGroup.FindPropertyRelative("m_Effects");
        m_EffectArray = m_EffectGroup.FindPropertyRelative("m_EffectList");

        Init();
    }

    private void Init()
    {
        m_FiltrateQueue = new Queue<FitterEffectType>();
        m_EffectPropertyMap = new Dictionary<int, EffectProperty>();

        var typeArray = Enum.GetValues(typeof(FitterEffectType));
        m_TypeArray = new FitterEffectType[typeArray.Length];
        typeArray.CopyTo(m_TypeArray, 0);

        m_LastFlag = m_Effects.intValue;

        m_EffectList = m_FitterView.EffectGroup.EffectList;

        foreach (var item in m_EffectList)
        {
            var effectProperty = new EffectProperty
            {
                IsShow = false,
                Spread = false,
                EffectType = item.EffectType,
                Value = item
            };
            m_EffectPropertyMap.Add((int)item.EffectType, effectProperty);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_Padding);
        EditorGUILayout.PropertyField(m_CellSize);
        EditorGUILayout.PropertyField(m_Spacing);
        EditorGUILayout.PropertyField(m_LayoutAxis);
        EditorGUILayout.PropertyField(m_ConstraintCount);
        EditorGUILayout.PropertyField(m_ScrollRect);

        ShowEffectGroup();

        serializedObject.ApplyModifiedProperties();
    }

    private int FiltrateEffectList()
    {
        var flag = m_Effects.intValue;

        if (m_LastFlag != flag)
        {
            m_EffectList.Clear();
            m_EffectArray.ClearArray();
        }

        m_LastFlag = flag;

        m_FiltrateQueue.Clear();

        foreach (var item in m_TypeArray)
        {
            var value = (int)item;
            var result = (flag | value) == flag;

            this[value].IsShow = result;

            if (result)
                m_FiltrateQueue.Enqueue(item);
        }

        return m_FiltrateQueue.Count;
    }

    private void ShowEffectGroup()
    {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("无法在运行时修改Effects", MessageType.Warning);
        EditorGUILayout.PropertyField(m_Effects);

        FiltrateEffectList();

        while (m_FiltrateQueue.Count > 0)
        {
            var effectType = m_FiltrateQueue.Dequeue();
            var effectProperty = this[(int)effectType];

            if (!effectProperty.IsShow) continue;

            var findIndex = m_EffectList.FindIndex(match => match.EffectType == effectType);

            if (findIndex == -1)
                m_FitterView.EffectGroup.EffectList.Add(effectProperty.Value);

            if (GUILayout.Button(effectProperty.EffectType.ToString(), GroupStyle))
                effectProperty.Spread = !effectProperty.Spread;

            if (!effectProperty.Spread) continue;

            if (findIndex >= 0 && findIndex < m_EffectArray.arraySize)
            {
                var property = m_EffectArray.GetArrayElementAtIndex(findIndex);

                EditorGUILayout.PropertyField(property);
            }
        }
    }

    private GUIStyle GroupStyle
    {
        get
        {
            var groupStyle = GUI.skin.button;
            groupStyle.fontStyle = FontStyle.Bold;
            groupStyle.alignment = TextAnchor.MiddleLeft;
            return groupStyle;
        }
    }

    private class EffectProperty
    {
        private bool m_IsShow;
        private bool m_Spread;
        private FitterEffectType m_EffectType;
        private FitterEffect m_Value;

        public bool IsShow { get => m_IsShow; set => m_IsShow = value; }
        public bool Spread { get => m_Spread; set => m_Spread = value; }
        public FitterEffectType EffectType { get => m_EffectType; set => m_EffectType = value; }
        public FitterEffect Value
        {
            get
            {
                if (m_Value == null)
                    m_Value = EffectGroup.CreateEffect(EffectType);
                return m_Value;
            }
            set => m_Value = value;
        }
    }
}
