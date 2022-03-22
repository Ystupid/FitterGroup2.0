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
        var minSize = property.FindPropertyRelative("m_MinSize"); 
        var spacing = property.FindPropertyRelative("m_Spacing");

        minSize.floatValue = Mathf.Max(0,minSize.floatValue);

        EditorGUILayout.PropertyField(minSize);
        EditorGUILayout.PropertyField(spacing);
    }
}
