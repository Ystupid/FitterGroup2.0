using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI.FitterGroup.Effect;

[CustomPropertyDrawer(typeof(GradientEffect))]
public class GradientEffectPropertyDrawer : FitterEffectPropertyDrawerBase
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var gradientColor = property.FindPropertyRelative("m_GradientColor");
        EditorGUILayout.HelpBox("由于技术太菜实现不了",MessageType.Info);
        EditorGUILayout.PropertyField(gradientColor);
    }
}
