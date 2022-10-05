using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class PlatformTags : IProcessSceneWithReport
{
    public int callbackOrder => 0;

    [PostProcessSceneAttribute(0)]
    public static void OnPostprocessScene()
    {
        var gameObjects = GetAllObjectsOnlyInScene();

        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] is null) continue;

            if (gameObjects[i].tag.Equals("QuestOnly", System.StringComparison.OrdinalIgnoreCase) && EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
            {
                Object.DestroyImmediate(gameObjects[i]);
            }
            if (gameObjects[i].tag.Equals("PCOnly", System.StringComparison.OrdinalIgnoreCase) && EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                Object.DestroyImmediate(gameObjects[i]);
            }

        }
    }

    private static List<GameObject> GetAllObjectsOnlyInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        OnPostprocessScene();
    }
}
