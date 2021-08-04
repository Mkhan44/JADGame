using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
    [CustomPropertyDrawer(typeof(PBXFramework))]
    public class PBXFrameworkDrawer : PropertyDrawer 
    {
        #region Unity methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        {
            return EditorGUIUtility.singleLineHeight;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            // show property name label
            label       = EditorGUI.BeginProperty(position, label, property);
            position    = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

            // show property attributes
            Rect    nameRect        = new Rect(position.x, position.y, position.width - 25f, position.height);
            Rect    weakRect        = new Rect(nameRect.xMax + 5f, position.y, 20f, position.height);
            int     indentLevel     = EditorGUI.indentLevel;

            EditorGUI.indentLevel   = 0;
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("m_name"), GUIContent.none);
            EditorGUI.PropertyField(weakRect, property.FindPropertyRelative("m_isWeak"), GUIContent.none);
            EditorGUI.indentLevel   = indentLevel;
            
            EditorGUI.EndProperty();
        }
        
        #endregion
    }
}