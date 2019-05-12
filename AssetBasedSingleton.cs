using System.Linq;
using UnityEngine;

public abstract class AssetBasedSingleton<T> : ScriptableObject where T : AssetBasedSingleton<T>
{
    protected virtual void OnInitialize()
    {
    }

    private static T _instance;

    public static T Instance
    {
        get
        {
            var shouldInitialize = false;

            if (_instance == null)
            {
                shouldInitialize = true;
                var candidates = Resources.LoadAll<T>("");

                if (candidates.Length > 1)
                    Debug.LogError(
                        $"Found multiple instances of {typeof(T)}, there should only be one. Please delete all but one instance for a correct Singleton behaviour");

                _instance = candidates.FirstOrDefault();

#if UNITY_EDITOR
                if (_instance == null)
                {
                    _instance = CreateInstance<T>();
                    UnityEditor.AssetDatabase.CreateAsset(_instance, $"Assets/Resources/{typeof(T)}.asset");
                }
#endif
            }

            if (shouldInitialize)
                _instance.OnInitialize();

            return _instance;
        }
    }
}