using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Effect;

[CustomPropertyDrawer(typeof(FloatEffect))]
public class FloatEffectPropertyDrawer : FitterEffectPropertyDrawerBase
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var smoothMode = property.FindPropertyRelative("m_SmoothMode");
        var rstoreRate = property.FindPropertyRelative("m_RestoreRate");
        var decelerationRate = property.FindPropertyRelative("m_DecelerationRate");
        var changeReset = property.FindPropertyRelative("m_ChangeReset");
        var floatSpeed = property.FindPropertyRelative("m_FloatSpeed");
        var floatRange = property.FindPropertyRelative("m_FloatRange");

        EditorGUILayout.PropertyField(smoothMode);
        if ((smoothMode.enumValueIndex | (int)SmoothMode.None) != (int)SmoothMode.None)
            EditorGUILayout.PropertyField(decelerationRate);
        EditorGUILayout.PropertyField(rstoreRate);
        EditorGUILayout.PropertyField(changeReset);
        EditorGUILayout.PropertyField(floatSpeed);
        EditorGUILayout.PropertyField(floatRange);
    }
}
