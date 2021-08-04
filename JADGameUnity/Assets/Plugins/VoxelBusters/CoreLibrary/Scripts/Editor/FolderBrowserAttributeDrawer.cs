using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
    [CustomPropertyDrawer(typeof(FolderBrowserAttribute))]
    public class FolderBrowserAttributeDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // draw property
            var valueRect   = new Rect(position.x, position.y, position.width - 50, position.height);
            var buttonRect  = new Rect(position.xMax - 45, position.y, 45, position.height);

            EditorGUI.PropertyField(valueRect, property, label);
            if (GUI.Button(buttonRect, new GUIContent("Select")))
            {
                EditorApplication.delayCall += () =>
                {
                    OpenFolderBrowser(property);
                }; 
            }

            EditorGUI.EndProperty();
        }
            
        private void OpenFolderBrowser(SerializedProperty property)
        {
            var value   = EditorUtility.OpenFolderPanel("Select folder", property.stringValue, string.Empty);
            if (!string.IsNullOrEmpty(value))
            {
                property.stringValue    = value;
                UnityEditorUtility.SetIsEditorDirty(true);
            }
        }
    }
}