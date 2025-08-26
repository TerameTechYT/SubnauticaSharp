#region

#endregion

namespace SubnauticaLibrary.Patchers;

[HarmonyPatch]
public static class EquipmentPatcher {
    public readonly static Dictionary<TechType, TechType> OverrideMap = [];

    [HarmonyPatch(typeof(Equipment), nameof(Equipment.GetTechTypeInSlot))]
    [HarmonyPostfix]
    public static void EquipmentGetTechTypeInSlotPostfix(ref Equipment __instance, string slot, ref TechType __result) {
        if (EquipmentPatcher.OverrideMap.TryGetValue(__result, out TechType techType)) {
            __result = techType;
        }
    }

    [HarmonyPatch(typeof(Equipment), nameof(Equipment.GetCount))]
    [HarmonyPostfix]
    public static void EquipmentGetCountPostfix(ref Equipment __instance, TechType techType, ref int __result) {
        if (EquipmentPatcher.OverrideMap.TryGetValue(techType, out TechType overrideTechType)) {
            Dictionary<TechType, int> equippedCount = Traverse.Create(__instance).Field("equippedCount").GetValue<Dictionary<TechType, int>>();

            equippedCount.TryGetValue(overrideTechType, out __result);
        }
    }
}