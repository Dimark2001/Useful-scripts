using UnityEngine;

public static class DiInitializer
{
    public static bool IsDiInitialized;
    public static bool IsInjectorInitialized;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitDi()
    {
        DiContainer.RegisterDependencies();
        IsDiInitialized = true;
        Injector.InjectDependencies();
        IsInjectorInitialized = true;
    }
}