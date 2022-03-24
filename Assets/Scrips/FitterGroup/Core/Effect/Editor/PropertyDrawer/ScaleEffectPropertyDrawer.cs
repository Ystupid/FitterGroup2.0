using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Effect;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ScaleEffect))]
public class ScaleEffectPropertyDrawer : FitterEffectPropertyDrawerBase
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var decelerationRate = property.FindPropertyRelative("m_DecelerationRate");
        var smoothMode = property.FindPropertyRelative("m_SmoothMode");
        var minSize = property.FindPropertyRelative("m_MinSize");
        var spacing = property.FindPropertyRelative("m_Spacing");

        minSize.floatValue = Mathf.Max(0, minSize.floatValue);
        smoothMode.enumValueIndex = Mathf.Min(1, smoothMode.enumValueIndex);

        EditorGUILayout.HelpBox("暂时不支持Smooth模式", MessageType.None);
        EditorGUILayout.PropertyField(smoothMode);
        if ((smoothMode.enumValueIndex | (int)SmoothMode.None) != (int)SmoothMode.None)
            EditorGUILayout.PropertyField(decelerationRate);

        EditorGUILayout.PropertyField(minSize);
        EditorGUILayout.PropertyField(spacing);
    }
}
