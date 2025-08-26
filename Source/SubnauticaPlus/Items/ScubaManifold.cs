#region

using SubnauticaLibrary.Prefabs;
using UnityObject = UnityEngine.Object;

#endregion


namespace SubnauticaPlus.Items;

public static class ScubaManifold {
    public static PrefabConstructor Prefab { get; private set; }

    public static void CreateAndRegister() {
        ScubaManifold.Prefab = new PrefabConstructor("ScubaManifold", iconPath: null)
            .SetSizeInInventory(3, 2)
            .SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment)
            .SetUnlock(TechType.Rebreather)
            .SetAnalysisTech()
            .SetRecipe(1,
                new Ingredient(TechType.Silicone, 1),
                new Ingredient(TechType.Titanium, 3),
                new Ingredient(TechType.Lubricant, 2)
            )
            .SetStepsToFabricatorTab("Personal", "Equipment")
            .SetFabricatorType(CraftTree.Type.Fabricator)
            .SetCraftingTime(5.0f)
            .SetEquipment(EquipmentType.Tank)
            .SetQuickSlotType(QuickSlotType.Passive)
            .SetPrefabFactory(TechType.Tank, (obj) => {
                obj.GetAllComponentsInChildren<Oxygen>().Do((o) => {
                    UnityObject.DestroyImmediate(o);
                });
                obj.SetActive(false);
            })
            .SetTechTypeOverride(TechType.PlasteelTank);

        ScubaManifold.Prefab.CreateAndRegister();
    }
}
