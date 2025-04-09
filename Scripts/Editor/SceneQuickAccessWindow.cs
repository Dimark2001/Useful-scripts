using UnityEditor;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneQuickMenu
{
    [MenuItem("Tools/Open Scene %&o", false, 0)]
    private static void ShowSceneMenu()
    {
        GenericMenu menu = new GenericMenu();

        var scenes = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories)
            .OrderBy(path => Path.GetFileNameWithoutExtension(path));

        foreach (var scenePath in scenes)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            menu.AddItem(new GUIContent(sceneName), false, () => OpenScene(scenePath));
        }

        menu.ShowAsContext();
    }

    private static void OpenScene(string path)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(path);
        }
    }
}