using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif


namespace VoxelBusters.EssentialKit.Editor
{
    public class EssentialKitSettingsProvider : SettingsProvider
    {
#region Fields

        private     EssentialKitSettingsInspector      m_settingsInspector;

#endregion

#region Constructors

        public EssentialKitSettingsProvider(string path, SettingsScope scopes)
            : base(path, scopes)
        {
            // set properties
            keywords    = GetSearchKeywordsFromSerializedObject(new SerializedObject(EssentialKitSettingsEditorUtility.DefaultSettings));
        }

#endregion

#region Static methods

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new EssentialKitSettingsProvider(path: "Project/Voxel Busters/Essential Kit", scopes: SettingsScope.Project);
        }

#endregion

#region Base class methods

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            m_settingsInspector = UnityEditor.Editor.CreateEditor(EssentialKitSettingsEditorUtility.DefaultSettings) as EssentialKitSettingsInspector;
        }

        public override void OnTitleBarGUI()
        {
            var     settings    = EssentialKitSettingsEditorUtility.DefaultSettings;
            EditorGUILayout.InspectorTitlebar(false, settings);
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            m_settingsInspector.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);
            EditorGUILayout.EndHorizontal();
        }

#endregion
    }
}