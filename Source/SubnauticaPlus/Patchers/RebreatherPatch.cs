#region

using SubnauticaLibrary.Extensions;

#endregion


namespace SubnauticaPlus.Patchers;

[HarmonyPatch]
public static class RebreatherPatch {
    [HarmonyPatch(typeof(Player), nameof(Player.GetBreathPeriod))]
    [HarmonyPostfix]
    public static void PlayerGetBreathPeriodPostfix(ref Player __instance, ref float __result) {
        Player.Mode mode = __instance.GetMode();
        if ((mode != Player.Mode.Normal && mode - Player.Mode.Piloting <= 1) || __instance.IsWearing(TechType.Rebreather) || __instance.currentWaterPark != null) {
            __result = 3.0f;
            return;
        }

        switch (__instance.GetDepthClass()) {
            case Ocean.DepthClass.Safe: {
                __result = 3f;
            } return;
            case Ocean.DepthClass.Unsafe: { 
                __result = 2.25f;
            } return;
            case Ocean.DepthClass.Crush: { 
                __result = 1.5f;
            } return;
        }
        __result = float.MaxValue;
    }

    [HarmonyPatch(typeof(Player), "GetOxygenPerBreath")]
    [HarmonyPostfix]
    public static void PlayerGetOxygenPerBreathPostfix(ref Player __instance, float breathingInterval, ref float __result) {
        Ocean.DepthClass depth = __instance.GetDepthClass();

        float multiplier = 1f;
        if (__instance.IsWearing(TechType.Rebreather) && !__instance.IsPiloting()) {
            switch (depth) {
                case Ocean.DepthClass.Unsafe: {
                    multiplier = 1.5f;
                } break;
                case Ocean.DepthClass.Crush: {
                    multiplier = 2.0f;
                } break;
            }
        }

        if (!GameModeUtils.RequiresOxygen()) {
            multiplier = 0.0f;
        }

        __result = breathingInterval * multiplier;
    }
}