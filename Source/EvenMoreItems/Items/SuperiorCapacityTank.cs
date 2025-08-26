#region

using SubnauticaLibrary.Prefabs;

#endregion

namespace SubnauticaPlus.Items;
public static class SuperiorCapacityTank {
    public static PrefabConstructor Prefab { get; private set; }
    public static void CreateAndRegister() {
        SuperiorCapacityTank.Prefab = new PrefabConstructor("sctank", TechType.HighCapacityTank)
            .SetSizeInInventory(1, 2)
            .SetEquipment(EquipmentType.Tank)
            .SetRecipe(1,
                new Ingredient(LightweightUltraHighCapacityTank.Prefab.TechType, 1),
                new Ingredient(TechType.PrecursorIonCrystal, 4),
                new Ingredient(TechType.PrecursorIonBattery, 1)
            )
            .SetCraftingTime(15.0f)
            .SetFabricatorType(CraftTree.Type.Workbench)
            .SetPdaGroupCategoryAfter(TechGroup.Personal, TechCategory.Equipment, LightweightUltraHighCapacityTank.Prefab.TechType)
            .SetUnlocks(LightweightUltraHighCapacityTank.Prefab.TechType, TechType.PrecursorIonBattery)
            .SetAnalysisTech()
            .SetPrefabFactory(TechType.HighCapacityTank, (obj) => {
                obj.GetAllComponentsInChildren<Oxygen>().Do((o) => {
                    o.oxygenCapacity = 360.0f;
                });
                obj.SetActive(false);
            })
            .SetTechTypeOverride(TechType.PlasteelTank);

        SuperiorCapacityTank.Prefab.CreateAndRegister();
    }
}
