using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
    public class SimulatorDatabase : ScriptableObject
    {
        #region Constants

        private     const   string  kSimulatorDataFilePath  = "Assets/Plugins/VoxelBusters/NativePlugins/EditorResources/SimulatorDatabase.asset";

        #endregion

        #region Fields

        [SerializeField]
        private     Texture2D[]                 m_images    = new Texture2D[0];

        [SerializeField]
        private     List<StringKeyValuePair>    m_savedData = new List<StringKeyValuePair>();

        #endregion

        #region Static properties

        public static SimulatorDatabase Instance
        {
            get
            {
                return CreateOrSelectSimulatorDatabase();
            }
        }

        #endregion

        #region Static methods

        private static SimulatorDatabase CreateOrSelectSimulatorDatabase()
        {
            string  fileName    = kSimulatorDataFilePath;
            var     data        = AssetDatabaseUtility.LoadAssetAtPath<SimulatorDatabase>(kSimulatorDataFilePath);
            // create if asset is not found
            if (null == data)
            {
                data    = CreateInstance<SimulatorDatabase>();
                AssetDatabaseUtility.CreateAssetAtPath(data, fileName);
                AssetDatabase.Refresh();
            }

            return data;
        }

        #endregion

        #region Public methods

        public Texture2D GetRandomImage()
        {
            // find random image
            string  imagePath   = null;
            if (m_images.Length == 0)
            {
                var     textureGuids    = AssetDatabase.FindAssets("t:texture");
                int     randomIndex     = Random.Range(0, textureGuids.Length);

                imagePath               = AssetDatabase.GUIDToAssetPath(textureGuids[randomIndex]);
            }
            else
            {
                int     randomIndex     = Random.Range(0, m_images.Length);
                var     randomImage     = m_images[randomIndex];

                imagePath               = AssetDatabase.GetAssetPath(randomImage);
            }

            // create file from texture data
            byte[]  fileData    = IOServices.ReadFileData(imagePath);
            var     texture     = new Texture2D(2, 2);
            texture.LoadImage(fileData, false);

            return texture;
        }

        #endregion

        #region Data methods

        public void SetObject(string key, object obj)
        {
            // convert object to serialized form
            string  serializedData      = JsonUtility.ToJson(obj);
            var     newItem             = new StringKeyValuePair() { Key = key, Value = serializedData };

            // check whether key exists
            int     keyIndex            = FindSavedItemIndex(key);
            if (keyIndex == -1)
            {
                m_savedData.Add(newItem);
            }
            else
            {
                m_savedData[keyIndex]   = newItem;
            }

            // mark that object is dirty
            EditorUtility.SetDirty(this);
        }

        public T GetObject<T>(string key)
        {
            int     keyIndex            = FindSavedItemIndex(key);
            if (keyIndex != -1)
            {
                string  serializedData  = m_savedData[keyIndex].Value;
                return JsonUtility.FromJson<T>(serializedData);
            }

            return default(T);
        }

        public void RemoveObject(string key)
        {
            int     keyIndex            = FindSavedItemIndex(key);
            if (keyIndex != -1)
            {
                m_savedData.RemoveAt(keyIndex);
            }

            // mark that object is dirty
            EditorUtility.SetDirty(this);
        }

        public void RemoveAllObjects()
        {
            // remove existing data
            m_savedData.Clear();

            // mark that object is dirty
            EditorUtility.SetDirty(this);
        }

        private int FindSavedItemIndex(string key)
        {
            return m_savedData.FindIndex((item) => string.Equals(item.Key, key));
        }

        #endregion
    }
}