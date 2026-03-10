///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 19/03/2025 14:36
///-----------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Com.ArthurSchmitz.UIInteractable
{
    // Register a SettingsProvider using IMGUI for the drawing framework:
    static class MyCustomSettingsIMGUIRegister
    {
        internal static SO_UIInteractableProjectSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<SO_UIInteractableProjectSettings>(UIInteractable.PATH_PROJECTSETTINGS_ASSET);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<SO_UIInteractableProjectSettings>();
                AssetDatabase.CreateAsset(settings, UIInteractable.PATH_PROJECTSETTINGS_ASSET);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings() => new SerializedObject(GetOrCreateSettings());

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/UIInteractable", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "UIInteractable",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = GetSerializedSettings();

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(SO_UIInteractableProjectSettings.defaultSettings)), new GUIContent("default settings"));
                    if (EditorGUI.EndChangeCheck()) {
                        Undo.RecordObject(settings.targetObject, nameof(SO_UIInteractableProjectSettings));
                        settings.ApplyModifiedProperties();
                    }
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "UIInteractable" })
            };

            return provider;
        }
    }
}