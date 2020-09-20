using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TransformUtilityWindow : EditorWindow
{
    Transform pivot;
    float radius = 1f;
    bool preserveYPosition = true;
    float roateAroundPivotAmmount = 10f;
    float rotateAmmount = 90f;
    Vector3 rotateAxis = Vector3.up;
    float lineElementsDistance = 1f;
    Vector3 lineDirection = Vector3.right;

    private Dictionary<Material, List<Transform>> cildrenByMaterialDictionary = new Dictionary<Material, List<Transform>>();

    Vector2 scrollPos;

    [MenuItem("Tools/Transform Utility")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        TransformUtilityWindow window = (TransformUtilityWindow)GetWindow(typeof(TransformUtilityWindow));
        window.Show();
    }
    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);

        EditorGUILayout.LabelField("Pivot");
        pivot = (Transform)EditorGUILayout.ObjectField(pivot, typeof(Transform), true);
        radius = EditorGUILayout.Slider("Radius", radius, 0.1f, 100f);
        preserveYPosition = EditorGUILayout.Toggle("preserveYPosition", preserveYPosition);
        if (GUILayout.Button("Position in Circle"))
        {
            PositionTransformsInCircle();
        }
        if (GUILayout.Button("Look at Pivot"))
        {
            LookAtPivot();
        }

        rotateAmmount = EditorGUILayout.Slider("Rotate Around Pivot Ammount", rotateAmmount, 0f, 360f);
        rotateAxis = EditorGUILayout.Vector3Field("Axis", rotateAxis);
        if (GUILayout.Button("Rotate Selection"))
        {
            RotateSelection();
        }
        roateAroundPivotAmmount = EditorGUILayout.Slider("Rotate Around Pivot Ammount", roateAroundPivotAmmount, 0f, 360f);
        if (GUILayout.Button("Rotate Around Pivot"))
        {
            RotateAroundPivot();
        }

        EditorGUILayout.LabelField("// Acts on Selected Transform's Parent");
        if (GUILayout.Button("Parent to Parent's Parent"))
        {
            ParentToParentsParent();
        }

        EditorGUILayout.LabelField("// Acts on Selected Transforms");

        lineElementsDistance = EditorGUILayout.FloatField("Distance between elements", lineElementsDistance);
        lineDirection = EditorGUILayout.Vector3Field("Line direction", lineDirection);
        if (GUILayout.Button("Place objects in Line"))
        {
            PlaceObjectsInLine();
        }

        if (GUILayout.Button("Parent Children to Parent"))
        {
            ParentChildrenToParent();
        }
        if (GUILayout.Button("Unpack Prefabs"))
        {
            UnpacKPrefabs();
        }
        if (GUILayout.Button("Group Children by Material"))
        {
            GroupChildrenByMaterial();
        }
        if (GUILayout.Button("Rename after Parent Hierarchy"))
        {
            RenameAfterParentHigherarchy();
        }

        EditorGUILayout.EndScrollView();
    }

    private void PositionTransformsInCircle()
    {
        if (Selection.transforms.Length <= 0) return;
        Transform target = pivot;
        if (target == null)
        {
            target = Selection.transforms[0].parent;
        }

        Undo.RecordObjects(Selection.transforms, "PositionTransformsInCircle");

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / Selection.transforms.Length;
            Vector3 newPos = target.position + new Vector3(Mathf.Cos(angle) * radius, preserveYPosition ? Selection.transforms[i].position.y : 0f, Mathf.Sin(angle) * radius);

            Selection.transforms[i].position = newPos;
        }
    }
    private void LookAtPivot()
    {
        if (Selection.transforms.Length <= 0) return;
        Transform target = pivot;
        if (target == null)
        {
            target = Selection.transforms[0].parent;
        }
        Vector3 targetPsoition = target.position;

        Undo.RecordObjects(Selection.transforms, "LookAtPivot");

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            if (preserveYPosition)
            {
                targetPsoition.y = Selection.transforms[i].position.y;
            }
            Selection.transforms[i].LookAt(targetPsoition);
        }
    }
    private void RotateAroundPivot()
    {
        if (Selection.transforms.Length <= 0) return;
        Transform target = pivot;
        if (target == null)
        {
            target = Selection.transforms[0].parent;
        }

        Undo.RecordObjects(Selection.transforms, "RotateAroundPivot");

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            Selection.transforms[i].RotateAround(target.position, Vector3.up, roateAroundPivotAmmount);
        }
    }
    private void ParentToParentsParent()
    {
        if (Selection.transforms.Length <= 0) return;

        for (int i = Selection.transforms.Length - 1; i >= 0; i--)
        {
            Selection.transforms[i].SetParent(Selection.transforms[i].parent.parent);
        }
    }
    private void ParentChildrenToParent()
    {
        if (Selection.transforms.Length <= 0) return;

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            if (Selection.transforms[i].childCount > 0) continue;

            for (int k = Selection.transforms[i].childCount - 1; k >= 0; k--)
            {
                Selection.transforms[i].GetChild(k).SetParent(Selection.transforms[i].parent);
            }
        }
    }
    private void UnpacKPrefabs()
    {
        if (Selection.transforms.Length <= 0) return;

        foreach (Transform transform in Selection.transforms)
        {
            if (PrefabUtility.IsAnyPrefabInstanceRoot(transform.gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(transform.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
        }
    }
    private void GroupChildrenByMaterial()
    {
        if (Selection.activeTransform == null) return;

        Transform[] allChildrenOfActiveTransform = Selection.activeTransform.GetComponentsInChildren<Transform>();

        // Populate dictionary
        foreach (Transform child in allChildrenOfActiveTransform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                if (cildrenByMaterialDictionary.ContainsKey(childRenderer.sharedMaterial))
                {
                    cildrenByMaterialDictionary[childRenderer.sharedMaterial].Add(child);
                }
                else
                {
                    cildrenByMaterialDictionary.Add(childRenderer.sharedMaterial, new List<Transform>() { child });
                }
            }
        }

        // Create object for each material and parent the respective objects
        foreach (KeyValuePair<Material, List<Transform>> keyValuePair in cildrenByMaterialDictionary)
        {
            GameObject materialGroup = new GameObject($"{keyValuePair.Key.name}");
            Undo.RegisterCreatedObjectUndo(materialGroup, "Create material group object");
            materialGroup.transform.SetParent(Selection.activeTransform);
            materialGroup.transform.localPosition = Vector3.zero;

            Undo.RegisterFullObjectHierarchyUndo(materialGroup, "Material group hierarchy change");
            for (int i = 0; i < keyValuePair.Value.Count; i++)
            {
                keyValuePair.Value[i].SetParent(materialGroup.transform);
            }
        }

        cildrenByMaterialDictionary.Clear();
    }
    private void RenameAfterParentHigherarchy()
    {
        if (Selection.transforms.Length <= 0) return;

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            string objectName;
            string parentHierarchyNames = "";
            Renderer renderer = Selection.transforms[i].GetComponent<Renderer>();
            if (renderer)
            {
                objectName = Selection.transforms[i].GetComponent<Renderer>().sharedMaterial.name;
            }
            else
            {
                objectName = Selection.transforms[i].name;
            }

            Transform currentParent = Selection.transforms[i].parent;
            while (currentParent != null)
            {
                parentHierarchyNames = currentParent.name + parentHierarchyNames;
                currentParent = currentParent.parent;
            }

            Selection.transforms[i].name = parentHierarchyNames + objectName;
        }
    }
    private void PlaceObjectsInLine()
    {
        if (Selection.transforms.Length <= 1) return;

        for (int i = 1; i < Selection.transforms.Length; i++)
        {
            Selection.transforms[i].position = Selection.transforms[0].position + lineDirection * lineElementsDistance * i;
        }
    }
    private void RotateSelection()
    {
        if (Selection.transforms.Length <= 0) return;

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            Selection.transforms[i].Rotate(rotateAxis, rotateAmmount);
        }
    }
}
