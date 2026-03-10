///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 06/03/2025 18:34
///-----------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable
{
    [CustomPropertyDrawer(typeof(Sign))]
    public class SignPropertyDrawer : PropertyDrawer 
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty type = property.FindPropertyRelative(Sign.Editor.PROPERTY_TYPE);

            float height = EditorGUI.GetPropertyHeight(type);

            if (Sign.Editor.VALUES_BY_SIGN.TryGetValue((Sign.E_Type)type.intValue, out string propertyName))
            {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(propertyName)) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty type = property.FindPropertyRelative(Sign.Editor.PROPERTY_TYPE);

            EditorGUI.BeginProperty(position, label, property);

            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, type, GUIContent.none);

            if(Sign.Editor.VALUES_BY_SIGN.TryGetValue((Sign.E_Type)type.intValue, out string propertyName))
            {
                position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(propertyName), GUIContent.none);
            }

            EditorGUI.EndProperty();
        }
    }
}