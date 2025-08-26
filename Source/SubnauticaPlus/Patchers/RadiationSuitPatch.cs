#region

using SubnauticaPlus.Items;
using SubnauticaLibrary.Extensions;

#endregion

namespace SubnauticaPlus.Patchers;

[HarmonyPatch]
public static class RadiationSuitPatch {
    [HarmonyPatch(typeof(RadiatePlayerInRange), "Radiate")]
    [HarmonyPrefix]
    public static bool RadiatePlayerInRangeRadiatePrefix(ref RadiatePlayerInRange __instance) {
        PlayerDistanceTracker tracker = Traverse.Create(__instance).Field("tracker").GetValue<PlayerDistanceTracker>();

        bool flag = GameModeUtils.HasRadiation() && (NoDamageConsoleCommand.main == null || !NoDamageConsoleCommand.main.GetNoDamageCheat());
        if (tracker.distanceToPlayer <= __instance.radiateRadius && flag && __instance.radiateRadius > 0f) {
            float num = Mathf.Clamp01(1f - tracker.distanceToPlayer / __instance.radiateRadius);
            float num2 = num;
            if (Inventory.main.equipment.IsWearing(TechType.RadiationSuit) || Inventory.main.equipment.IsWearing(LeadLinedRefinforcedDivingSuit.Prefab.TechType)) {
                num -= num2 * 0.5f;
            }

            if (Inventory.main.equipment.IsWearing(TechType.RadiationHelmet) || Inventory.main.equipment.IsWearing(LeadLinedRebreather.Prefab.TechType)) {
                num -= num2 * 0.23f * 2f;
            }

            if (Inventory.main.equipment.IsWearing(TechType.RadiationGloves) || Inventory.main.equipment.IsWearing(LeadLinedReinforcedGloves.Prefab.TechType)) {
                num -= num2 * 0.23f;
            }

            num = Mathf.Clamp01(num);
            Player.main.SetRadiationAmount(num);
        } else {
            Player.main.SetRadiationAmount(0f);
        }
        return false;
    }
}
