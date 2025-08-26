#region

#endregion

using SubnauticaLibrary.Prefabs;

namespace SubnauticaPlus.Items;
public class LeadLinedReinforcedGloves {
    public static PrefabConstructor Prefab { get; private set; }
    public static void CreateAndRegister() {
        LeadLinedReinforcedGloves.Prefab = new PrefabConstructor("llrg", TechType.ReinforcedGloves)
            .SetSizeInInventory(2, 2)
            .SetEquipment(EquipmentType.Gloves)
            .SetRecipe(1,
                new Ingredient(TechType.ReinforcedGloves, 1),
                new Ingredient(TechType.RadiationGloves, 1)
            )
            .SetCraftingTime(5.0f)
            .SetFabricatorType(CraftTree.Type.Workbench)
            .SetPdaGroupCategoryAfter(TechGroup.Personal, TechCategory.Equipment, TechType.ReinforcedGloves)
            .SetUnlocks(TechType.ReinforcedGloves, TechType.RadiationGloves)
            .SetAnalysisTech()
            .SetPrefabFactory(TechType.ReinforcedGloves)
            .SetTechTypeOverride(TechType.ReinforcedGloves);

        LeadLinedReinforcedGloves.Prefab.CreateAndRegister();
    }
}
