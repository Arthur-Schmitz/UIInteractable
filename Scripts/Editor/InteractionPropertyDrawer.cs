///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 07/03/2025 11:55
///-----------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable {

    [CustomPropertyDrawer(typeof(Interaction), true)]
    public class InteractionPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty state  = property.FindPropertyRelative(Interaction<int>.Editor.PROPERTY_STATE);
            float              height = EditorGUI.GetPropertyHeight(state);

            if(state.intValue > 0)
            {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Interaction<int>.Editor.PROPERTY_SIGNS), true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty state = property.FindPropertyRelative(Interaction<int>.Editor.PROPERTY_STATE);
            SerializedProperty signs = property.FindPropertyRelative(Interaction<int>.Editor.PROPERTY_SIGNS);

            EditorGUI.BeginProperty(position, label, property);

            position.height = EditorGUIUtility.singleLineHeight;

            //if different from none
            if(state.intValue > 0)
            {
                EditorGUI.PropertyField(position, state, label);

                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, signs, true);
            }
            else
            {
                EditorGUI.PropertyField(position, state, label);
            }

            EditorGUI.EndProperty();
        }
    }
}