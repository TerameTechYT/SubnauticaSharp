#region

using SubnauticaPlus.Patchers;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using SubnauticaLibrary.Prefabs;
using UnityObject = UnityEngine.Object;

#endregion

namespace SubnauticaPlus.Items;

public static class LightweightUltraHighCapacityTank {
    public static PrefabConstructor Prefab { get; private set; }
    public static void CreateAndRegister() {
        LightweightUltraHighCapacityTank.Prefab = new PrefabConstructor("lwuhtank", TechType.HighCapacityTank)
            .SetSizeInInventory(1, 2)
            .SetEquipment(EquipmentType.Tank)
            .SetRecipe(1,
                new Ingredient(TechType.HighCapacityTank, 1),
                new Ingredient(TechType.PlasteelTank, 1),
                new Ingredient(TechType.Lubricant, 2),
                new Ingredient(TechType.HydrochloricAcid, 1)
            )
            .SetCraftingTime(7.0f)
            .SetFabricatorType(CraftTree.Type.Workbench)
            .SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment)
            .SetUnlocks(TechType.HighCapacityTank, TechType.PlasteelTank)
            .SetAnalysisTech()
            .SetPrefabFactory(TechType.PlasteelTank, (obj) => {
                obj.GetAllComponentsInChildren<Oxygen>().Do((o) => {
                    o.oxygenCapacity = 180.0f;
                });
                obj.SetActive(false);
            })
            .SetTechTypeOverride(TechType.PlasteelTank);

        LightweightUltraHighCapacityTank.Prefab.CreateAndRegister();
    }
}
