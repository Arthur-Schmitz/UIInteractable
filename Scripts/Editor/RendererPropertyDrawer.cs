///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 07/03/2025 12:18
///-----------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable 
{
    public abstract class RendererPropertyDrawer : PropertyDrawer
    {
        private const string NOT_INTERACTABLE = " not interactable";

        private static readonly GUIContent LABEL_TARGET = new GUIContent("on");

        protected abstract string ParameterName { get; }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return
              EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER))
            + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER_NOTINTERACTABLE))
            + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_TARGETS), true)
            + EditorGUIUtility.standardVerticalSpacing * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty parameter                 = property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER);
            SerializedProperty parameter_notInteractable = property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER_NOTINTERACTABLE);
            SerializedProperty targets                   = property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_TARGETS);

            EditorGUI.BeginProperty(position, label, property);

            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, parameter, new GUIContent(ParameterName));

            position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, parameter_notInteractable, new GUIContent(ParameterName + NOT_INTERACTABLE));

            position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, targets, LABEL_TARGET, true);

            EditorGUI.EndProperty();
        }
    }

    public abstract class RendererSettingsPropertyDrawer : PropertyDrawer
    {
        private static readonly GUIContent LABEL_PARAMETER                 = new GUIContent("Custom");
        private static readonly GUIContent LABEL_PARAMETER_NOTINTERACTABLE = new GUIContent("Custom not interactable");
        private static readonly GUIContent LABEL_TARGET                    = new GUIContent("on");

        protected abstract string ParameterName { get; }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty setting = property.FindPropertyRelative(Renderer<int, SO_RendererSettings<int>, int>.Editor.PROPERTY_SETTING);
            float height = EditorGUI.GetPropertyHeight(setting);
            
            if (setting.objectReferenceValue == null)
            {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER)) + EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER_NOTINTERACTABLE));
            }

            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_TARGETS), true) + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty setting = property.FindPropertyRelative(Renderer<int, SO_RendererSettings<int>, int>.Editor.PROPERTY_SETTING);

            EditorGUI.BeginProperty(position, label, property);

            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, setting, new GUIContent(ParameterName));

            if(setting.objectReferenceValue == null)
            {
                position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER), LABEL_PARAMETER);

                position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_PARAMETER_NOTINTERACTABLE), LABEL_PARAMETER_NOTINTERACTABLE);
            }

            position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, property.FindPropertyRelative(Renderer<int, int>.Editor.PROPERTY_TARGETS), LABEL_TARGET, true);

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(SetActive))]
    public class SetActivePropertyDrawer  : RendererPropertyDrawer         { protected override string ParameterName => nameof(SetActive); }

    [CustomPropertyDrawer(typeof(Enable))]
    public class EnablePropertyDrawer     : RendererPropertyDrawer         { protected override string ParameterName => nameof(Enable   ); }

    [CustomPropertyDrawer(typeof(Scale))]
    public class ScalePropertyDrawer      : RendererSettingsPropertyDrawer { protected override string ParameterName => nameof(Scale    ); }

    [CustomPropertyDrawer(typeof(Tint))]
    public class TintPropertyDrawer       : RendererSettingsPropertyDrawer { protected override string ParameterName => nameof(Tint     ); }

    [CustomPropertyDrawer(typeof(SetSprite))]
    public class SetSpritePropertyDrawer  : RendererSettingsPropertyDrawer { protected override string ParameterName => nameof(SetSprite); }
    
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundPropertyDrawer      : RendererSettingsPropertyDrawer { protected override string ParameterName => nameof(Sound     ); }

    [CustomPropertyDrawer(typeof(StyleSheet))]
    public class StyleSheetPropertyDrawer : RendererSettingsPropertyDrawer { protected override string ParameterName => nameof(StyleSheet); }

    [CustomPropertyDrawer(typeof(Alpha))]
    public class AlphaPropertyDrawer      : RendererSettingsPropertyDrawer { protected override string ParameterName => nameof(Alpha     ); }

    [CustomPropertyDrawer(typeof(SetState))]
    public class SetStatePropertyDrawer   : RendererPropertyDrawer         { protected override string ParameterName => nameof(SetState  ); }
}