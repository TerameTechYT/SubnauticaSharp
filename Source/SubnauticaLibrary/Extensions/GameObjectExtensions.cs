#region

using UnityObject = UnityEngine.Object;

#endregion

namespace SubnauticaLibrary.Extensions;
public static class GameObjectExtensions {
    public static void DontDestroyOnLoad(this GameObject gameObject) {
        UnityObject.DontDestroyOnLoad(gameObject);
    }
    public static void DontDestroyChildrenOnLoad(this GameObject gameObject) {
        foreach (Transform transform in gameObject.transform) {
            UnityObject.DontDestroyOnLoad(transform.gameObject);
        }
    }

    public static void DestroyChildren(this GameObject gameObject) {
        foreach (Transform transform in gameObject.transform) {
            UnityObject.Destroy(transform.gameObject);
        }
    }

    public static void DestroyChildrenImmediate(this GameObject gameObject) {
        foreach (object obj in gameObject.transform) {
            Transform transform = (Transform)obj;
            UnityObject.DestroyImmediate(transform.gameObject);
        }
    }

    public static void EnsureComponents(this GameObject obj, params Type[] types) {
        types.ForEach((t) => {
            obj.EnsureComponent(t);
        });
    }

    public static void EnsureComponents<T>(this GameObject obj, params Type[] types) where T : Component {
        obj.EnsureComponent<T>();
        types.ForEach((t) => {
            obj.EnsureComponent(t);
        });
    }
}
