#region

#endregion

namespace SubnauticaLibrary.Extensions;
public static class PlayerExtensions {
    public static bool HasItem(this Player player, TechType techType) {
        return Inventory.main.HasItem(techType);
    }
    public static bool IsWearing(this Player player, TechType techType) {
        return Inventory.main.equipment.IsWearing(techType);
    }
    public static bool IsSitting(this Player player) {
        return player.GetMode() == Player.Mode.Sitting;
    }
}
