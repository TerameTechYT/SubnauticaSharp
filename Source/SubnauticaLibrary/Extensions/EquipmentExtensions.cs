#region

#endregion

namespace SubnauticaLibrary.Extensions;
public static class EquipmentExtensions {
    public static bool IsWearing(this Equipment equipment, TechType techType) {
        return equipment.GetCount(techType) > 0;
    }
}
