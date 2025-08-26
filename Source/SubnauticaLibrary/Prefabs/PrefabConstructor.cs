#region

using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using SubnauticaLibrary.Extensions;
using SubnauticaLibrary.Patchers;
using UWE;

#endregion

namespace SubnauticaLibrary.Prefabs;

public interface IPrefabConstuctor {
    CustomPrefab Prefab { get; }

    void CreateAndRegister();
}

public class PrefabConstructor(PrefabInfo info) : IPrefabConstuctor {
    public CustomPrefab Prefab => this._prefab;
    public PrefabInfo PrefabInfo => this.Prefab.Info;
    public TechType TechType => this.PrefabInfo.TechType;

    private readonly CustomPrefab _prefab = new(info);
    private readonly List<Action<CustomPrefab>> _actions = [];

    public PrefabConstructor(string classId, string? displayName, string? description, Assembly? assembly = null)
        : this(PrefabInfo.WithTechType(classId, displayName, description, techTypeOwner: assembly ?? Assembly.GetCallingAssembly())) { }

    public PrefabConstructor(string classId, string? displayName, string? description, TechType inheritIcon = TechType.None, Assembly? assembly = null) 
        : this(classId, displayName, description, SpriteManager.Get(inheritIcon), assembly ?? Assembly.GetCallingAssembly()) { }

    public PrefabConstructor(string classId, string? displayName, string? description, Sprite icon, Assembly? assembly = null) 
        : this(classId, description, description, assembly) {
        this.PrefabInfo.WithIcon(icon);
    }
    public PrefabConstructor(string classId, string? displayName, string? description, string iconPath, Assembly? assembly = null) 
        : this(classId, description, description, assembly) {
        this.PrefabInfo.WithIcon(ImageUtils.LoadSpriteFromFile(iconPath ?? $"{Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)}/Assets/Textures/{classId}.png"));
    }

    public PrefabConstructor(string classId, TechType inheritIcon = TechType.None, Assembly? assembly = null) 
        : this(classId, null, null, SpriteManager.Get(inheritIcon), assembly ?? Assembly.GetCallingAssembly()) { }

    public PrefabConstructor(string classId, Sprite icon, Assembly? assembly = null) 
        : this(classId, null, null, assembly) {
        this.PrefabInfo.WithIcon(icon);
    }
    public PrefabConstructor(string classId, string? iconPath, Assembly? assembly = null)
        : this(classId, null, null, assembly) {
        this.PrefabInfo.WithIcon(ImageUtils.LoadSpriteFromFile(iconPath ?? $"{Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)}/Assets/Textures/{classId}.png"));
    }

    public void CreateAndRegister() {
        foreach (Action<CustomPrefab> action in this._actions) {
            try {
                action?.Invoke(this._prefab);
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }
        this._prefab.Register();
    }

    public PrefabConstructor Queue(Action<CustomPrefab> action) {
        this._actions.Add(action);
        return this;
    }
    public PrefabConstructor CreateCreatureEgg(int requiredAcuSize = 1)
        => this.Queue((prefab) => CreateCreatureEgg(prefab, requiredAcuSize));
    public static void CreateCreatureEgg(CustomPrefab prefab, int requiredAcuSize = 1) {
        prefab?.CreateCreatureEgg();
    }

    public PrefabConstructor CreateFabricator()
        => this.Queue((prefab) => CreateFabricator(prefab));
    public static void CreateFabricator(CustomPrefab prefab) {
        
    }

    public PrefabConstructor CreateFragment(TechType blueprint, float scanTime, int fragmentsToScan = 1, string? encyKey = null, bool destroyAfterScan = true, bool isFragment = true)
        => this.Queue((prefab) => CreateFragment(prefab, blueprint, scanTime, fragmentsToScan, encyKey, destroyAfterScan, isFragment));
    public static void CreateFragment(CustomPrefab prefab, TechType blueprint, float scanTime, int fragmentsToScan = 1, string? encyKey = null, bool destroyAfterScan = true, bool isFragment = true) {
        prefab?.CreateFragment(blueprint, scanTime, fragmentsToScan, encyKey, destroyAfterScan, isFragment);
    }

    public PrefabConstructor SetTechTypeOverride(TechType techType)
        => this.Queue((prefab) => SetTechTypeOverride(prefab, techType));
    public static void SetTechTypeOverride(CustomPrefab prefab, TechType techType) {
        EquipmentPatcher.OverrideMap.TryAdd(prefab.Info.TechType, techType);
    }


    public PrefabConstructor SetGameObject(GameObject obj)
        => this.Queue((prefab) => SetGameObject(prefab, obj));
    public static void SetGameObject(CustomPrefab prefab, GameObject obj) {
        prefab?.SetGameObject(obj);
    }

    public PrefabConstructor SetPrefabFactory(TechType type, Action<GameObject>? modify = null)
        => this.Queue((prefab) => SetPrefabFactory(prefab, type, modify));
    public static void SetPrefabFactory(CustomPrefab prefab, TechType type, Action<GameObject>? modify = null) {
        prefab?.SetGameObject(new CloneTemplate(prefab.Info, type) {
            ModifyPrefab = (obj) => {
                obj.DontDestroyOnLoad();
                modify?.Invoke(obj);
            },
        });
    }

    public PrefabConstructor SetPrefabFactoryAsync(TechType type, Func<GameObject, IEnumerator>? modifyAsync = null)
        => this.Queue((prefab) => SetPrefabFactoryAsync(prefab, type, modifyAsync));
    public static void SetPrefabFactoryAsync(CustomPrefab prefab, TechType type, Func<GameObject, IEnumerator>? modifyAsync = null) {
        prefab?.SetGameObject(new CloneTemplate(prefab.Info, type) {
            ModifyPrefabAsync = (obj) => {
                obj.DontDestroyOnLoad();
                return modifyAsync?.Invoke(obj);
            },
        });
    }

    public PrefabConstructor SetRecipe(RecipeData recipeData)
        => this.Queue((prefab) => SetRecipe(prefab, recipeData));
    public static void SetRecipe(CustomPrefab prefab, RecipeData recipeData) {
        prefab?.SetRecipe(recipeData);
    }

    public PrefabConstructor SetRecipe(int amount, params Ingredient[] ingredients)
        => this.Queue((prefab) => SetRecipe(prefab, amount, ingredients));
    public static void SetRecipe(CustomPrefab prefab, int amount, params Ingredient[] ingredients) {
        prefab?.SetRecipe(new RecipeData() {
            craftAmount = amount,
            Ingredients = [.. ingredients],
        });
    }

    public PrefabConstructor SetRecipeSetJson(string path)
        => this.Queue((prefab) => SetRecipeSetJson(prefab, path));
    public static void SetRecipeSetJson(CustomPrefab prefab, string path) {
        prefab?.SetRecipeFromJson(path);
    }

    public PrefabConstructor SetUnlock(TechType requiredForUnlock, int fragmentsToScan = 1)
        => this.Queue((prefab) => SetUnlock(prefab, requiredForUnlock, fragmentsToScan));
    public static void SetUnlock(CustomPrefab prefab, TechType requiredForUnlock, int fragmentsToScan = 1) {
        prefab?.SetUnlock(requiredForUnlock, fragmentsToScan);
    }
    public PrefabConstructor SetUnlocks(List<TechType> techTypes)
        => this.Queue((prefab) => SetUnlocks(prefab, techTypes));
    public static void SetUnlocks(CustomPrefab prefab, List<TechType> techTypes) {
        prefab?.GetGadget<ScanningGadget>()?.WithCompoundTechsForUnlock(techTypes);
    }
    public PrefabConstructor SetUnlocks(params TechType[] techTypes)
        => this.Queue((prefab) => SetCompoundTechsForUnlock(prefab, techTypes));
    public static void SetCompoundTechsForUnlock(CustomPrefab prefab, params TechType[] techTypes) {
        prefab?.GetGadget<ScanningGadget>()?.WithCompoundTechsForUnlock([.. techTypes]);
    }

    public PrefabConstructor SetPdaGroupCategory(TechGroup group, TechCategory category)
        => this.Queue((prefab) => SetPdaGroupCategory(prefab, group, category));
    public static void SetPdaGroupCategory(CustomPrefab prefab, TechGroup group, TechCategory category) {
        prefab?.SetPdaGroupCategory(group, category);
    }

    public PrefabConstructor SetPdaGroupCategoryAfter(TechGroup group, TechCategory category, TechType target)
        => this.Queue((prefab) => SetPdaGroupCategoryAfter(prefab, group, category, target));
    public static void SetPdaGroupCategoryAfter(CustomPrefab prefab, TechGroup group, TechCategory category, TechType target) {
        prefab?.SetPdaGroupCategoryAfter(group, category, target);
    }

    public PrefabConstructor SetPdaGroupCategoryBefore(TechGroup group, TechCategory category, TechType target)
        => this.Queue((prefab) => SetPdaGroupCategoryBefore(prefab, group, category, target));
    public static void SetPdaGroupCategoryBefore(CustomPrefab prefab, TechGroup group, TechCategory category, TechType target) {
        prefab?.SetPdaGroupCategoryBefore(group, category, target);
    }

    public PrefabConstructor SetEquipment(EquipmentType equipmentType) => this.Queue((prefab) => SetEquipment(prefab, equipmentType));
    public static void SetEquipment(CustomPrefab prefab, EquipmentType equipmentType) {
        prefab?.SetEquipment(equipmentType);
    }

    public PrefabConstructor SetVehicleUpgradeModule(EquipmentType equipmentType = EquipmentType.VehicleModule, QuickSlotType slotType = QuickSlotType.Passive)
        => this.Queue((prefab) => SetVehicleUpgradeModule(prefab));
    public static void SetVehicleUpgradeModule(CustomPrefab prefab, EquipmentType equipmentType = EquipmentType.VehicleModule, QuickSlotType slotType = QuickSlotType.Passive) {
        prefab?.SetVehicleUpgradeModule(equipmentType, slotType);
    }

    public PrefabConstructor SetSpawns(params SpawnLocation[] spawnLocations)
        => this.Queue((prefab) => SetSpawns(prefab, spawnLocations));
    public static void SetSpawns(CustomPrefab prefab, params SpawnLocation[] spawnLocations) {
        prefab?.SetSpawns(spawnLocations);
    }

    public PrefabConstructor SetSpawns(WorldEntityInfo entityInfo, params LootDistributionData.BiomeData[] spawnLocations)
        => this.Queue((prefab) => SetSpawns(prefab, spawnLocations));
    public static void SetSpawns(CustomPrefab prefab, WorldEntityInfo entityInfo, params LootDistributionData.BiomeData[] spawnLocations) {
        prefab?.SetSpawns(entityInfo, spawnLocations);
    }

    public PrefabConstructor SetSpawns(params LootDistributionData.BiomeData[] spawnLocations)
        => this.Queue((prefab) => SetSpawns(prefab, spawnLocations));
    public static void SetSpawns(CustomPrefab prefab, params LootDistributionData.BiomeData[] spawnLocations) {
        prefab?.SetSpawns(spawnLocations);
    }

    public PrefabConstructor SetBuildable(bool buildable = true)
        => this.Queue((prefab) => SetBuildable(prefab, buildable));
    public static void SetBuildable(CustomPrefab prefab, bool buildable = true) {
        prefab?.GetGadget<ScanningGadget>()?.SetBuildable(buildable);
    }

    public PrefabConstructor SetHardLocked(bool locked = true)
        => this.Queue((prefab) => SetHardLocked(prefab, locked));
    public static void SetHardLocked(CustomPrefab prefab, bool locked = true) {
        prefab?.GetGadget<ScanningGadget>()?.SetHardLocked(locked);
    }

    public PrefabConstructor SetSizeInInventory(int x, int y)
        => this.Queue((prefab) => SetSizeInInventory(prefab, x, y));
    public static void SetSizeInInventory(CustomPrefab prefab, int x, int y) {
        prefab?.Info.WithSizeInInventory(new Vector2int(x, y));
    }

    public PrefabConstructor SetStepsToFabricatorTab(params string[] steps)
        => this.Queue((prefab) => SetStepsToFabricatorTab(prefab, steps));
    public static void SetStepsToFabricatorTab(CustomPrefab prefab, params string[] steps) {
        prefab?.GetGadget<CraftingGadget>()?.WithStepsToFabricatorTab(steps);
    }

    public PrefabConstructor SetFabricatorType(CraftTree.Type type)
        => this.Queue((prefab) => SetFabricatorType(prefab, type));
    public static void SetFabricatorType(CustomPrefab prefab, CraftTree.Type type) {
        prefab?.GetGadget<CraftingGadget>()?.WithFabricatorType(type);
    }

    public PrefabConstructor SetCraftingTime(float time = 1.0f)
        => this.Queue((prefab) => SetCraftingTime(prefab, time));
    public static void SetCraftingTime(CustomPrefab prefab, float time = 1.0f) {
        prefab?.GetGadget<CraftingGadget>()?.WithCraftingTime(time);
    }

    public PrefabConstructor SetQuickSlotType(QuickSlotType type)
        => this.Queue((prefab) => SetQuickSlotType(prefab, type));
    public static void SetQuickSlotType(CustomPrefab prefab, QuickSlotType type) {
        prefab?.GetGadget<EquipmentGadget>()?.WithQuickSlotType(type);
    }

    public PrefabConstructor SetEncyclopediaEntry(string path, Sprite popupSprite, Texture2D? encyImage = null, FMODAsset? unlockSound = null, FMODAsset? encyAudio = null)
        => this.Queue((prefab) => SetEncyclopediaEntry(prefab, path, popupSprite, encyImage, unlockSound, encyAudio));
    public static void SetEncyclopediaEntry(CustomPrefab prefab, string path, Sprite popupSprite, Texture2D? encyImage = null, FMODAsset? unlockSound = null, FMODAsset? encyAudio = null) {
        prefab?.GetGadget<ScanningGadget>()?.WithEncyclopediaEntry(path, popupSprite, encyImage, unlockSound, encyAudio);
    }

    public PrefabConstructor SetScannerEntry(TechType blueprint, float scanTime, bool isFragment = false, string? encyKey = null, bool destroyAfterScan = false)
        => this.Queue((prefab) => SetScannerEntry(prefab, blueprint, scanTime, isFragment, encyKey, destroyAfterScan));
    public static void SetScannerEntry(CustomPrefab prefab, TechType blueprint, float scanTime, bool isFragment = false, string? encyKey = null, bool destroyAfterScan = false) {
        prefab?.GetGadget<ScanningGadget>()?.WithScannerEntry(blueprint, scanTime, isFragment, encyKey, destroyAfterScan);
    }

    public PrefabConstructor SetAnalysisTech(Sprite? popupSprite = null, FMODAsset? unlockSound = null, string? unlockMessage = null)
        => this.Queue((prefab) => SetAnalysisTech(prefab, popupSprite, unlockSound, unlockMessage));
    public static void SetAnalysisTech(CustomPrefab prefab, Sprite? popupSprite = null, FMODAsset? unlockSound = null, string? unlockMessage = null) {
        prefab?.GetGadget<ScanningGadget>()?.WithAnalysisTech(popupSprite, unlockSound, unlockMessage);
    }
}