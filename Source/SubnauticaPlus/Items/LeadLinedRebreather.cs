#region

#endregion

using SubnauticaLibrary.Prefabs;

namespace SubnauticaPlus.Items;
public class LeadLinedRebreather {
    public static PrefabConstructor Prefab { get; private set; }
    public static void CreateAndRegister() {
        LeadLinedRebreather.Prefab = new PrefabConstructor("llrebreather", TechType.Rebreather)
            .SetSizeInInventory(2, 2)
            .SetEquipment(EquipmentType.Head)
            .SetRecipe(1,
                new Ingredient(TechType.Rebreather, 1),
                new Ingredient(TechType.RadiationHelmet, 1)
            )
            .SetCraftingTime(5.0f)
            .SetFabricatorType(CraftTree.Type.Workbench)
            .SetPdaGroupCategoryAfter(TechGroup.Personal, TechCategory.Equipment, TechType.Rebreather)
            .SetUnlocks(TechType.Rebreather, TechType.RadiationHelmet)
            .SetAnalysisTech()
            .SetPrefabFactory(TechType.Rebreather)
            .SetTechTypeOverride(TechType.Rebreather);

        LeadLinedRebreather.Prefab.CreateAndRegister();
    }
}
