using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatRangeSliderAtribute))]
public class FloatRangeSliderDrawer : PropertyDrawer
{ 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int originalIdentLevel = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel = 0;
        SerializedProperty minProperty = property.FindPropertyRelative("_min");
        SerializedProperty maxProperty = property.FindPropertyRelative("max");
        float minValue = minProperty.floatValue;
        float maxValue = maxProperty.floatValue;
        float fieldWidth = position.width / 4f - 4f;
        float sliderWidth = position.width / 2f;
        position.width = sliderWidth;
        FloatRangeSliderAtribute limit = attribute as FloatRangeSliderAtribute;
        EditorGUI.MinMaxSlider(position, ref minValue, ref minValue, limit.Min, limit.Max);
        position.x += sliderWidth + 4f;
        position.width = fieldWidth;
        maxValue = EditorGUI.FloatField(position, maxValue);
        if (minValue <limit.Min)
        {
            minValue = limit.Min;
        }

        if (maxValue > limit.Max)
        {
            maxValue = limit.Max;
        }
        else if (maxValue < minValue)
        {
            maxValue = minValue;
        }

        minProperty.floatValue = minValue;
        maxProperty.floatValue = maxValue;
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = originalIdentLevel;
    }
}
