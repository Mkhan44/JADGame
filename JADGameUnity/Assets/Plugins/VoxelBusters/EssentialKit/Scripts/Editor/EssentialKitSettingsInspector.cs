using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.AddressBookCore.Simulator;
using VoxelBusters.EssentialKit.BillingServicesCore.Simulator;
using VoxelBusters.EssentialKit.CloudServicesCore.Simulator;
using VoxelBusters.EssentialKit.GameServicesCore.Simulator;
using VoxelBusters.EssentialKit.NotificationServicesCore.Simulator;
using VoxelBusters.EssentialKit.MediaServicesCore.Simulator;

namespace VoxelBusters.EssentialKit.Editor
{
    [CustomEditor(typeof(EssentialKitSettings))]
    public class EssentialKitSettingsInspector : UnityEditor.Editor
    {
        #region Fields

        // internal properties
        private     PropertyGroupMeta[]     m_propertyMetaArray             = null;

        private     SerializedProperty[]    m_properties                    = null;

        private     int                     m_propertyCount                 = 0;

        private     SerializedProperty      m_activeProperty                = null;

        // custom gui styles
        private     GUIStyle                m_groupBackgroundStyle          = null;

        private     GUIStyle                m_headerStyle                   = null;

        private     GUIStyle                m_headerFoldoutStyle            = null;

        private     GUIStyle                m_headerLabelStyle              = null;

        private     GUIStyle                m_headerToggleStyle             = null;

        // assets
        private     Texture2D               m_logoIcon                      = null;

        private     Texture2D               m_toggleOnIcon                  = null;

        private     Texture2D               m_toggleOffIcon                 = null;

        #endregion

        #region Unity methods

        private void OnEnable()
        {
            // set properties
            m_propertyMetaArray     = new PropertyGroupMeta[]
            {
                new PropertyGroupMeta() { displayName = "Application",              serializedPropertyName = "m_applicationSettings",           onAfterPropertyDraw = DrawApplicationSettingsControls},
                new PropertyGroupMeta() { displayName = "Address Book",             serializedPropertyName = "m_addressBookSettings",           onAfterPropertyDraw = DrawAddressBookSettingsControls },
                new PropertyGroupMeta() { displayName = "Billing Services",         serializedPropertyName = "m_billingServicesSettings",       onAfterPropertyDraw = DrawBillingServicesSettingsControls },
                new PropertyGroupMeta() { displayName = "Cloud Services",           serializedPropertyName = "m_cloudServicesSettings",         onAfterPropertyDraw = DrawCloudServicesSettingsControls },
                new PropertyGroupMeta() { displayName = "Deep Link Services",       serializedPropertyName = "m_deepLinkServicesSettings",      onAfterPropertyDraw = DrawDeepLinkServicesSettingsControls   },
                new PropertyGroupMeta() { displayName = "Game Services",            serializedPropertyName = "m_gameServicesSettings",          onAfterPropertyDraw = DrawGameServicesSettingsControls },
                new PropertyGroupMeta() { displayName = "Network Services",         serializedPropertyName = "m_networkServicesSettings",       onAfterPropertyDraw = DrawNetworkServicesSettingsControls },
                new PropertyGroupMeta() { displayName = "Notification Services",    serializedPropertyName = "m_notificationServicesSettings",  onAfterPropertyDraw = DrawNotificationServicesSettingsControls },
                new PropertyGroupMeta() { displayName = "Media Services",           serializedPropertyName = "m_mediaServicesSettings",         onAfterPropertyDraw = DrawMediaServicesSettingsControls },
                new PropertyGroupMeta() { displayName = "Sharing Services",         serializedPropertyName = "m_sharingServicesSettings",       onAfterPropertyDraw = DrawSharingServicesSettingsControls },
                new PropertyGroupMeta() { displayName = "Native UI",                serializedPropertyName = "m_nativeUISettings",              onAfterPropertyDraw = DrawNativeUISettingsControls },
                new PropertyGroupMeta() { displayName = "WebView",                  serializedPropertyName = "m_webViewSettings",               onAfterPropertyDraw = DrawWebViewSettingsControls   },
            };
            m_properties            = Array.ConvertAll(m_propertyMetaArray, (element) => serializedObject.FindProperty(element.serializedPropertyName));
            m_propertyCount         = m_properties.Length;

            // load assets
            m_logoIcon              = AssetDatabaseUtility.LoadAssetAtPath<Texture2D>(Constants.kPluginEditorResourcesFullPath + "essential-kit-logo.png");
            m_toggleOnIcon          = AssetDatabaseUtility.LoadAssetAtPath<Texture2D>(Constants.kPluginEditorResourcesFullPath + "toggle-on.png");
            m_toggleOffIcon         = AssetDatabaseUtility.LoadAssetAtPath<Texture2D>(Constants.kPluginEditorResourcesFullPath + "toggle-off.png");
        }

        public override void OnInspectorGUI()
        {
            LoadStyles();

            // draw controls
            DrawProductInfoSection();
            DrawTopBarButtons();
            EditorGUI.BeginChangeCheck();
            for (int iter = 0; iter < m_propertyCount; iter++)
            {
                var     property    = m_properties[iter];
                if (property != null)
                {
                    var     propertyMeta    = m_propertyMetaArray[iter];
                    DrawPropertyGroup(property, propertyMeta);
                }
            }
            // save changes
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }

        #endregion

        #region Section methods

        private void DrawProductInfoSection()
        {
            GUILayout.BeginHorizontal(m_groupBackgroundStyle);

            // logo section
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(m_logoIcon, GUILayout.Height(64f), GUILayout.Width(64f));
            GUILayout.Space(2f);
            GUILayout.EndVertical();

            // product info
            GUILayout.BeginVertical();
            GUILayout.Label(Constants.kProductDisplayName, "HeaderLabel");
            GUILayout.Label(Constants.kProductVersion, "MiniLabel");
            GUILayout.Label(Constants.kProductCopyrights, "MiniLabel");
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawTopBarButtons()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Documentation", "ButtonLeft"))
            {
                ProductResources.OpenDocumentation();
            }
            if (GUILayout.Button("Tutorials", "ButtonMid"))
            {
                ProductResources.OpenTutorials();
            }
            if (GUILayout.Button("Forum", "ButtonMid"))
            {
                ProductResources.OpenForum();
            }
            if (GUILayout.Button("Discord", "ButtonMid"))
            {
                ProductResources.OpenSupport();
            }
            if (GUILayout.Button("Write Review", "ButtonRight"))
            {
                ProductResources.OpenAssetStorePage(true);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // check whether we have any suggestions
            if (!NativeFeatureUnitySettingsBase.CanToggleFeatureUsageState())
            {
                EditorGUILayout.HelpBox("Stripping unused features is not available with current build configuration. Please update stripping level to Medium/High in Player Settings for complete support.", MessageType.Warning);
                /*if (GUILayout.Button("Fix Now!"))
                {
                    NativeFeatureUnitySettingsBase.UpdateBuildConfigurationToSupportToggleFeatureUsageState();
                }*/
            }
        }

        private void DrawPropertyGroup(SerializedProperty property, PropertyGroupMeta propertyMeta)
        {
            EditorGUILayout.BeginVertical(m_groupBackgroundStyle);
            if (DrawControlHeader(property, propertyMeta.displayName))
            {
                bool    oldGUIState         = GUI.enabled;
                var     enabledProperty     = property.FindPropertyRelative("m_isEnabled");

                // update gui state
                GUI.enabled     = (enabledProperty == null || enabledProperty.boolValue);

                // show internal properties
                EditorGUI.indentLevel++;
                if (enabledProperty != null)
                {
                    DrawSettingsInternalProperties(property);
                }
                else
                {
                    DrawControlInternalProperties(property);
                }
                if (propertyMeta.onAfterPropertyDraw != null)
                {
                    propertyMeta.onAfterPropertyDraw();
                }
                EditorGUI.indentLevel--;

                // reset gui state
                GUI.enabled     = oldGUIState;
            }
            EditorGUILayout.EndVertical();
        }

        private bool DrawControlHeader(SerializedProperty property, string displayName)
        {
            // draw rect
            var     rect                = EditorGUILayout.GetControlRect(false, 30f);
            GUI.Box(rect, GUIContent.none, m_headerStyle);

            // draw foldable control
            bool    isSelected          = property == m_activeProperty;
            var     foldOutRect         = new Rect(rect.x, rect.y, 50f, rect.height);
            EditorGUI.LabelField(foldOutRect, isSelected ? "-" : "+", m_headerFoldoutStyle);

            // draw label 
            var     labelRect           = new Rect(rect.x + 25f, rect.y, rect.width - 100f, rect.height);
            EditorGUI.LabelField(labelRect, displayName, m_headerLabelStyle);

            // draw selectable rect
            var     selectableRect      = new Rect(rect.x, rect.y, rect.width - 100f, rect.height);
            if (DrawTransparentButton(selectableRect, string.Empty))
            {
                isSelected              = OnPropertyHeaderSelect(property);
            }

            // draw toggle button
            var     enabledProperty     = property.FindPropertyRelative("m_isEnabled");
            if ((enabledProperty != null) /*&& NativeFeatureUnitySettingsBase.CanToggleFeatureUsageState()*/)
            {
                Rect    toggleRect                  = new Rect(rect.xMax - 64f, rect.y, 64f, 25f);
                if (GUI.Button(toggleRect, enabledProperty.boolValue ? m_toggleOnIcon : m_toggleOffIcon, m_headerToggleStyle))
                {
                    enabledProperty.boolValue       = !enabledProperty.boolValue;

#if UNITY_ANDROID
                    //TODO : Fire an event if any feature toggles and listent for adding the dependencies
                    EditorPrefs.SetBool("refresh-feature-dependencies", true);
#endif

                }
                
            }
            return isSelected;
        }

        private static void DrawControlInternalProperties(SerializedProperty property)
        {
            // move pointer to first element
            var     currentProperty  = property.Copy();
            var     endProperty      = default(SerializedProperty);

            // start iterating through the properties
            bool    firstTime   = true;
            while (currentProperty.NextVisible(enterChildren: firstTime))
            {
                if (firstTime)
                {
                    endProperty = property.GetEndProperty();
                    firstTime   = false;
                }
                if (SerializedProperty.EqualContents(currentProperty, endProperty))
                {
                    break;
                }
                EditorGUILayout.PropertyField(currentProperty, true);
            }
        }

        private bool OnPropertyHeaderSelect(SerializedProperty property)
        {
            var     oldProperty     = m_activeProperty;
            if (m_activeProperty == null)
            {
                property.isExpanded = true;

                m_activeProperty    = property;

                return true;
            }
            if (m_activeProperty == property)
            {
                property.isExpanded = false;

                m_activeProperty    = null;

                return false;
            }

            property.isExpanded     = true;
            oldProperty.isExpanded  = false;
            
            m_activeProperty        = property;

            return true;
        }

        #endregion

        #region Settings group methods

        private static void DrawSettingsInternalProperties(SerializedProperty settingsProperty)
        {
            // move pointer to first element
            var     currentProperty  = settingsProperty.Copy();
            currentProperty.NextVisible(enterChildren: true);
            var     endProperty      = settingsProperty.GetEndProperty();

            // start iterating through the properties
            while (currentProperty.NextVisible(enterChildren: false))
            {
                if (SerializedProperty.EqualContents(currentProperty, endProperty))
                {
                    break;
                }
                EditorGUILayout.PropertyField(currentProperty, true);
            }
        }

        #endregion

        #region Features methods

        private void DrawApplicationSettingsControls()
        { }

        private void DrawAddressBookSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kAddressBook);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                AddressBookSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        private void DrawBillingServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kBillingServices);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                BillingStoreSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        private void DrawCloudServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kCloudServices);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                CloudServicesSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        private void DrawDeepLinkServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kDeepLinkServices);
            }
            GUILayout.EndVertical();
        }

        private void DrawGameServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kGameServices);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                GameServicesSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        private void DrawNetworkServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kNetworkServices);
            }
            GUILayout.EndVertical();
        }

        private void DrawNotificationServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kNotificationServices);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                NotificationServicesSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        private void DrawNativeUISettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kNativeUI);
            }
            GUILayout.EndVertical();
        }

        private void DrawMediaServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kMediaServices);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                MediaServicesSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        private void DrawSharingServicesSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.KSharingServices);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                MediaServicesSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        private void DrawWebViewSettingsControls()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kWebView);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                MediaServicesSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        #endregion

        #region GUIStyles methods

        private void LoadStyles()
        {
            // check whether styles are already loaded
            if (null != m_groupBackgroundStyle)
            {
                return;
            }

            // bg style
            m_groupBackgroundStyle          = new GUIStyle("HelpBox");
            var     bgOffset                = m_groupBackgroundStyle.margin;
            bgOffset.bottom                 = 5;
            m_groupBackgroundStyle.margin   = bgOffset;

            // header style
            m_headerStyle                   = new GUIStyle("PreButton");
            m_headerStyle.fixedHeight       = 0;

            // foldout style
            m_headerFoldoutStyle            = new GUIStyle("WhiteBoldLabel");
            m_headerFoldoutStyle.fontSize   = 20;
            m_headerFoldoutStyle.alignment  = TextAnchor.MiddleLeft;

            // label style
            m_headerLabelStyle              = new GUIStyle("WhiteBoldLabel");
            m_headerLabelStyle.fontSize     = 12;
            m_headerLabelStyle.alignment    = TextAnchor.MiddleLeft;

            // enabled style
            m_headerToggleStyle             = new GUIStyle("InvisibleButton");
        }

        private bool DrawTransparentButton(Rect rect, string label)
        {
            var     originalColor   = GUI.color;
            try
            {
                GUI.color   = Color.clear;
                return GUI.Button(rect, string.Empty);
            }
            finally
            {
                GUI.color   = originalColor;
            }
        }

        #endregion

        #region Nested types

        private struct PropertyGroupMeta
        {
            public  string      serializedPropertyName;

            public  string      displayName;

            public  Action      onAfterPropertyDraw;
        }

        #endregion
    }
}