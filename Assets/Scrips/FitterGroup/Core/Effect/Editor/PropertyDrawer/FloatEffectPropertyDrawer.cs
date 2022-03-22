using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Effect;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(FloatEffect))]
public class FloatEffectPropertyDrawer : FitterEffectPropertyDrawerBase
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var floatSpeed = property.FindPropertyRelative("m_FloatSpeed");
        var floatRange = property.FindPropertyRelative("m_FloatRange");

        EditorGUILayout.HelpBox("功能开发中",MessageType.Info);
        EditorGUILayout.PropertyField(floatSpeed);
        EditorGUILayout.PropertyField(floatRange);
    }
}
