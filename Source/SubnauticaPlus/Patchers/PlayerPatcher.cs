#region

using SubnauticaPlus.Items;

#endregion

namespace SubnauticaPlus.Patchers;

[HarmonyPatch]
public static class PlayerPatcher {
    public static Player Player => Player.main;
    public static OxygenManager OxygenManager => PlayerPatcher.Player.oxygenMgr;
    public static Survival Survival => PlayerPatcher.Player.GetComponent<Survival>();
    public static Inventory Inventory => Inventory.main;
    public static ItemsContainer Container => PlayerPatcher.Inventory.container;
    public static Equipment Equipment => PlayerPatcher.Inventory.equipment;

    public const string TankSlot = "Tank";
    public static bool Equipped;
    public static readonly List<Oxygen> sources = [];

    [HarmonyPatch(typeof(Player), "Start")]
    [HarmonyPostfix]
    public static void PlayerStartPostfix(ref Player __instance) {
        PlayerPatcher.sources.ForEach(PlayerPatcher.OxygenManager.UnregisterSource);
        PlayerPatcher.sources.Clear();

        PlayerPatcher.Container.GetItemTypes().ForEach((type) => {
            PlayerPatcher.Container.GetItems(type).Do((item) => {
                Oxygen? oxygen = item?.item?.gameObject?.GetComponent<Oxygen>();
                if (oxygen != null) {
                    PlayerPatcher.sources.Add(oxygen);
                }
            });
        });

        PlayerPatcher.Equipment.onUnequip += OnUnequip;
        PlayerPatcher.Equipment.onEquip += OnEquip;

        PlayerPatcher.Container.onRemoveItem += OnRemoveItem;
        PlayerPatcher.Container.onAddItem += OnAddItem;

        TechType techType = PlayerPatcher.Equipment.GetTechTypeInSlot("Tank");
        PlayerPatcher.Equipped = techType == ScubaManifold.Prefab.TechType;
        if (PlayerPatcher.Equipped) {
            PlayerPatcher.sources.ForEach(PlayerPatcher.OxygenManager.RegisterSource);
        }
    }

    private static void OnUnequip(string slot, InventoryItem item) {
        if (PlayerPatcher.Equipped && slot == PlayerPatcher.TankSlot) {
            PlayerPatcher.Equipped = false;
            PlayerPatcher.sources.ForEach(PlayerPatcher.OxygenManager.UnregisterSource);
        }
    }

    private static void OnEquip(string slot, InventoryItem item) {
        if (slot == PlayerPatcher.TankSlot) {
            TechType? techType = item?.item?.GetTechType();

            PlayerPatcher.Equipped = (techType.GetValueOrDefault() == ScubaManifold.Prefab.TechType) & (techType != null);
            if (PlayerPatcher.Equipped)
                PlayerPatcher.sources.ForEach(PlayerPatcher.OxygenManager.RegisterSource);
            else
                PlayerPatcher.sources.ForEach(PlayerPatcher.OxygenManager.UnregisterSource);
        }
    }

    private static void OnRemoveItem(InventoryItem item) {
        Oxygen? oxygen = item?.item?.gameObject?.GetComponent<Oxygen>();
        if (oxygen != null) {
            PlayerPatcher.sources.Remove(oxygen);
            if (PlayerPatcher.Equipped)
                PlayerPatcher.OxygenManager.UnregisterSource(oxygen);
        }
    }

    private static void OnAddItem(InventoryItem item) {
        Oxygen? oxygen = item?.item?.gameObject?.GetComponent<Oxygen>();
        if (oxygen != null) {
            PlayerPatcher.sources.Add(oxygen);
            if (PlayerPatcher.Equipped)
                PlayerPatcher.OxygenManager.RegisterSource(oxygen);
        }
    }
}