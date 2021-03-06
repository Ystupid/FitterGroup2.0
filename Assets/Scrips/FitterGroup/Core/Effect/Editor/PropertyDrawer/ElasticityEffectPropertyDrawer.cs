using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.FitterGroup.Effect;

[CustomPropertyDrawer(typeof(ElasticityEffect))]
public class ElasticityEffectPropertyDrawer : FitterEffectPropertyDrawerBase
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var decelerationRate = property.FindPropertyRelative("m_DecelerationRate");
        var currentIndex = property.FindPropertyRelative("m_CurrentIndex");
        var smoothMode = property.FindPropertyRelative("m_SmoothMode");

        EditorGUILayout.PropertyField(smoothMode);
        if ((smoothMode.intValue | (int)SmoothMode.None) != (int)SmoothMode.None)
            EditorGUILayout.PropertyField(decelerationRate);
        EditorGUILayout.HelpBox("CurrentIndex: " + currentIndex.intValue, MessageType.None);
    }
}
