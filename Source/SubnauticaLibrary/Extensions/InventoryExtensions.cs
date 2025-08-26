#region

#endregion

namespace SubnauticaLibrary.Extensions;
public static class InventoryExtensions {
    public static bool HasItem(this Inventory inventory, TechType techType) {
        return inventory.container.HasItem(techType);
    }
    public static bool HasItem(this ItemsContainer container, TechType techType) {
        return container.GetCount(techType) > 0;
    }
}
