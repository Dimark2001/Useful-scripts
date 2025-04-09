using UnityEditor;

public class ExtensionsWindow : EditorWindow
{
    [MenuItem("Tools/Reload Domain")]
    private static void DomainReload()
    {
        EditorUtility.RequestScriptReload();
    }
}