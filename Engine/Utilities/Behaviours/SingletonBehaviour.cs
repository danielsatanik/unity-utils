using UnityEngine;
using UnityUtils.Attributes;

namespace UnityUtils.Utilities.Behaviours
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        // Analysis disable StaticFieldInGenericType
        static T mInstance;
        static bool mInstantiated;
        // Analysis restore StaticFieldInGenericType

        public static T Instance
        {
            get
            {
                if (mInstantiated)
                    return mInstance;

                var type = typeof(T);
                var objects = FindObjectsOfType<T>();

                if (objects.Length > 0)
                {
                    Instance = objects[0];
                    if (objects.Length > 1)
                    {
                        Debug.LogWarningFormat("There is more than one Singleton instance of type \"{0}\". Keeping the first destroying the other.", type);
                        for (var i = 1; i < objects.Length; ++i)
                            DestroyImmediate(objects[i].gameObject);
                    }
                    mInstantiated = true;
                    return mInstance;
                }

                var attribute = System.Attribute.GetCustomAttribute(type, typeof(PrefabAttribute)) as PrefabAttribute;
                GameObject gameobject;

                if (attribute != null)
                {
                    var prefabName = attribute.Name;

                    if (System.String.IsNullOrEmpty(prefabName))
                    {
                        Debug.AssertFormat(true, "Prefab name is empty for Singleton of type \"{0}\"", type);
                        Application.Quit();
                        return null;
                    }

                    gameobject = Instantiate(Resources.Load<GameObject>(prefabName)) as GameObject;

                    if (gameobject == null)
                    {
                        Debug.AssertFormat(true, "Could not find Prefab \"{0}\" on Resources for Singleton of type \"{1}\".", prefabName, type);
                        Application.Quit();
                        return null;
                    }
                }
                else
                {
                    gameobject = new GameObject();
                }

                gameobject.name = type.Name;
                Instance = gameobject.GetComponent<T>();

                if (!mInstantiated)
                {
                    if (attribute != null)
                        Debug.LogWarningFormat("There wasn't a component of type \"{0}\" inside prefab \"{1}\". Creating one.", type, type.Name);
                    Instance = gameobject.AddComponent<T>();
                }

                if (attribute != null && attribute.Persistent)
                    DontDestroyOnLoad(mInstance.gameObject);

                mInstance.Initialize();

                return mInstance;
            }
            set
            {
                mInstance = value;
                mInstantiated = value != null;
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}