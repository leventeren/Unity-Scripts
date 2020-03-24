using UnityEngine;
using UnityEditor;


public class ColliderToFit : MonoBehaviour
{
    [MenuItem("My Tools/Collider/Fit to Children")]
    static void FitToChildren()
    {
        foreach (GameObject rootGameObject in Selection.gameObjects)
        {
            if (!(rootGameObject.GetComponent<BoxCollider>() is BoxCollider))
                continue;

            bool hasBounds = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

            for (int i = 0; i < rootGameObject.transform.childCount; ++i)
            {
                Renderer childRenderer = rootGameObject.transform.GetChild(i).GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    if (hasBounds)
                    {
                        bounds.Encapsulate(childRenderer.bounds);
                    }
                    else
                    {
                        bounds = childRenderer.bounds;
                        hasBounds = true;
                    }
                }
            }

            BoxCollider collider = (BoxCollider)rootGameObject.GetComponent<BoxCollider>();
            collider.center = bounds.center - rootGameObject.transform.position;
            collider.size = bounds.size;
        }
    }
}
