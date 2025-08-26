#region

#endregion

using Nautilus.Handlers;

namespace SubnauticaLibrary;

[BepInDependency("com.snmodding.nautilus", "1.0.0.0")]
[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin {
    public static Plugin Instance { get; private set; }
    public static Harmony HarmonyInstance { get; private set; }

    [UsedImplicitly]
    public void Awake()
    {
        LanguageHandler.RegisterLocalizationFolder();
        Plugin.Instance = this;
        Plugin.HarmonyInstance = new Harmony(PluginInfo.GUID);

        try {
            Plugin.HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        } catch (Exception ex) {
            this.Logger.LogError("Failed to patch harmony.");
            this.Logger.LogError(ex);
        }
    }
}

internal struct PluginInfo
{
    public const string GUID = "subnauticalibrary";
    public const string Name = "SubnauticaLibrary";
    public const string Version = "1.0.0";
}