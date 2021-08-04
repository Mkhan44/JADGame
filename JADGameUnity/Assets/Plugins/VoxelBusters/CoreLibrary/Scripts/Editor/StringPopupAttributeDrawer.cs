using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
    [CustomPropertyDrawer(typeof(StringPopupAttribute), true)]
    public class StringPopupAttributeDrawer : PropertyDrawer 
    {
        #region Base class methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label   = EditorGUI.BeginProperty(position, label, property);

            // show popup
            string[]    options         = ((StringPopupAttribute)attribute).Options;
            int         selectedIndex   = Array.FindIndex(options, (item) => string.Equals(item, property.stringValue));
            selectedIndex               = EditorGUI.Popup(position, label.text, selectedIndex, options);

            // assign value
            if (options.Length > 0)
            {
                property.stringValue    = (selectedIndex == -1) ? options[0] : options[selectedIndex];
            }

            EditorGUI.EndProperty();
        }

        #endregion
    }
}