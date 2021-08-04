using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class GameObjectUtility
    {
        #region Static methods

        public static GameObject CreateChild(string childName, Transform parent)
        {
            return CreateChild(childName, Vector3.zero, Quaternion.identity, Vector3.one, parent);
        }

        public static GameObject CreateChild(string childName, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Transform parent)
        {
            var     containerGO     = (parent is RectTransform) ? new GameObject(childName, typeof(RectTransform)) : new GameObject(childName);

            // set transform properties
            var     containerTrans          = containerGO.transform;
            containerTrans.SetParent(parent);
            containerTrans.localPosition    = localPosition;
            containerTrans.localRotation    = localRotation;
            containerTrans.localScale       = localScale;

            return containerGO;
        }

        public static T CreateGameObjectWithComponent<T>(string name) where T : Component
        {
            return new GameObject(name).AddComponent<T>();
        }

        public static void SetActive(this GameObject[] gameObjects, bool value)
        {
            for (int iter = 0; iter < gameObjects.Length; iter++)
            {
                gameObjects[iter].SetActive(value);
            }
        }

        #endregion
    }
}