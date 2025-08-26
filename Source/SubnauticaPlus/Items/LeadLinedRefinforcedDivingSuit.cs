#region

#endregion

using SubnauticaLibrary.Prefabs;

namespace SubnauticaPlus.Items;
public class LeadLinedRefinforcedDivingSuit {
    public static PrefabConstructor Prefab { get; private set; }
    public static void CreateAndRegister() {
        LeadLinedRefinforcedDivingSuit.Prefab = new PrefabConstructor("llrds", TechType.ReinforcedDiveSuit)
            .SetSizeInInventory(2, 2)
            .SetEquipment(EquipmentType.Body)
            .SetRecipe(1,
                new Ingredient(TechType.ReinforcedDiveSuit, 1),
                new Ingredient(TechType.RadiationSuit, 1)
            )
            .SetCraftingTime(5.0f)
            .SetFabricatorType(CraftTree.Type.Workbench)
            .SetPdaGroupCategoryAfter(TechGroup.Personal, TechCategory.Equipment, TechType.ReinforcedDiveSuit)
            .SetUnlocks(TechType.ReinforcedDiveSuit, TechType.RadiationSuit)
            .SetAnalysisTech()
            .SetPrefabFactory(TechType.ReinforcedDiveSuit)
            .SetTechTypeOverride(TechType.ReinforcedDiveSuit);

        LeadLinedRefinforcedDivingSuit.Prefab.CreateAndRegister();
    }
}
