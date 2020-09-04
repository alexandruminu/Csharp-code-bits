using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuExtensionFunctions
{
    [MenuItem("CONTEXT/Transform/Log World Position")]
    static void LogWorldPosition(MenuCommand command)
    {
        Transform target = (Transform)command.context;
        Debug.Log($"{target.name} world pos: {target.position}");
    }

    /*
    [MenuItem("Transform Utility/Place In Circle")]
    static void LogSelectedTransformName()
    {
        if (Selection.transforms.Length <= 0) return;
        Transform parent = Selection.transforms[0].parent;

        float radius = 1f;

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / Selection.transforms.Length;
            Vector3 newPos = parent.position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

            Selection.transforms[i].position = newPos;
        }
    }
    */
}
