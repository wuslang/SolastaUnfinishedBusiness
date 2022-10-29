﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SolastaUnfinishedBusiness.Builders;
using SolastaUnfinishedBusiness.ItemCrafting;
using static SolastaUnfinishedBusiness.Api.DatabaseHelper;
using static SolastaUnfinishedBusiness.Api.DatabaseHelper.ItemFlagDefinitions;

namespace SolastaUnfinishedBusiness.Models;

internal static class MerchantTypeContext
{
    internal static readonly List<(MerchantDefinition, MerchantType)> MerchantTypes = new();

    private static readonly string[] RangedWeaponTypes =
    {
        "LightCrossbowType", "HeavyCrossbowType", "ShortbowType", "LongbowType", "DartType"
    };

    private static readonly string[] EquipmentSlots =
    {
        EquipmentDefinitions.SlotTypeBelt, EquipmentDefinitions.SlotTypeBack, EquipmentDefinitions.SlotTypeFeet,
        EquipmentDefinitions.SlotTypeFinger, EquipmentDefinitions.SlotTypeGloves, EquipmentDefinitions.SlotTypeHead,
        EquipmentDefinitions.SlotTypeNeck, EquipmentDefinitions.SlotTypeShoulders,
        EquipmentDefinitions.SlotTypeWrists
    };

    internal static void Load()
    {
        var dbMerchantDefinition = DatabaseRepository.GetDatabase<MerchantDefinition>();

        foreach (var merchant in dbMerchantDefinition)
        {
            MerchantTypes.Add((merchant, GetMerchantType(merchant)));
        }
    }

    internal static MerchantType GetMerchantType(MerchantDefinition merchant)
    {
        var isDocumentMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsDocument);

        var isAmmunitionMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsAmmunition
                && !x.ItemDefinition.Magical);

        var isArmorMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsArmor
                && !x.ItemDefinition.Magical);

        var isMeleeWeaponMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsWeapon
                && !RangedWeaponTypes.Contains(x.ItemDefinition.WeaponDescription.WeaponType)
                && !x.ItemDefinition.Magical);

        var isRangeWeaponMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsWeapon
                && RangedWeaponTypes.Contains(x.ItemDefinition.WeaponDescription.WeaponType)
                && !x.ItemDefinition.Magical);

        var isMagicalAmmunitionMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsAmmunition
                && x.ItemDefinition.Magical);

        var isMagicalArmorMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsArmor
                && x.ItemDefinition.Magical);

        var isMagicalMeleeWeaponMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsWeapon
                && !RangedWeaponTypes.Contains(x.ItemDefinition.WeaponDescription.WeaponType)
                && x.ItemDefinition.Magical);

        var isMagicalRangeWeaponMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsWeapon
                && RangedWeaponTypes.Contains(x.ItemDefinition.WeaponDescription.WeaponType)
                && x.ItemDefinition.Magical);

        var isMagicalEquipment = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.SlotTypes.Any(s => EquipmentSlots.Contains(s))
                && x.ItemDefinition.Magical);

        var isPrimedArmorMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsArmor
                && x.ItemDefinition.ItemPresentation.ItemFlags.Contains(ItemFlagPrimed));

        var isPrimedMeleeWeaponMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsWeapon
                && !RangedWeaponTypes.Contains(x.ItemDefinition.WeaponDescription.WeaponType)
                && x.ItemDefinition.ItemPresentation.ItemFlags.Contains(ItemFlagPrimed));

        var isPrimedRangeWeaponMerchant = merchant.StockUnitDescriptions
            .Any(x =>
                x.ItemDefinition.IsWeapon
                && RangedWeaponTypes.Contains(x.ItemDefinition.WeaponDescription.WeaponType)
                && x.ItemDefinition.ItemPresentation.ItemFlags.Contains(ItemFlagPrimed));

        return new MerchantType
        {
            IsDocument = isDocumentMerchant,
            IsAmmunition = isAmmunitionMerchant,
            IsArmor = isArmorMerchant,
            IsMeleeWeapon = isMeleeWeaponMerchant,
            IsRangeWeapon = isRangeWeaponMerchant,
            IsMagicalAmmunition = isMagicalAmmunitionMerchant,
            IsMagicalArmor = isMagicalArmorMerchant,
            IsMagicalMeleeWeapon = isMagicalMeleeWeaponMerchant,
            IsMagicalRangeWeapon = isMagicalRangeWeaponMerchant,
            IsMagicalEquipment = isMagicalEquipment,
            IsPrimedArmor = isPrimedArmorMerchant,
            IsPrimedMeleeWeapon = isPrimedMeleeWeaponMerchant,
            IsPrimedRangeWeapon = isPrimedRangeWeaponMerchant
        };
    }

    internal sealed class MerchantType
    {
        internal bool IsAmmunition;
        internal bool IsArmor;
        internal bool IsDocument;

        internal bool IsMagicalAmmunition;
        internal bool IsMagicalArmor;
        internal bool IsMagicalMeleeWeapon;
        internal bool IsMagicalRangeWeapon;
        internal bool IsMagicalEquipment;
        internal bool IsMeleeWeapon;

        internal bool IsPrimedArmor;
        internal bool IsPrimedMeleeWeapon;
        internal bool IsPrimedRangeWeapon;
        internal bool IsRangeWeapon;
    }
}

internal static class MerchantContext
{
    private static StockUnitDescriptionBuilder _stockBuilder;
    private static StockUnitDescriptionBuilder StockBuilder => _stockBuilder ??= BuildStockBuilder();

    private static readonly List<(ItemDefinition, ShopItemType)> ShopItems = new();

    internal static void Load()
    {
        AddToShops();
        AddToEditor();
    }

    internal static void AddItem(ItemDefinition item, ShopItemType type)
    {
        ShopItems.Add((item, type));
    }
    
    private static void AddToShops()
    {
        if (Main.Settings.AddNewWeaponsAndRecipesToShops)
        {
            MerchantContext.GiveAssortment(ShopItems, MerchantTypeContext.MerchantTypes);
        }
    }

    private static void AddToEditor()
    {
        if (!Main.Settings.AddNewWeaponsAndRecipesToEditor)
        {
            return;
        }

        foreach (var (item, _) in ShopItems)
        {
            item.inDungeonEditor = true;
        }
    }
    
    internal static void TryAddItemsToUserMerchant(MerchantDefinition merchant)
    {
        if (Main.Settings.AddNewWeaponsAndRecipesToShops)
        {
            MerchantContext.GiveAssortment(ShopItems, merchant, MerchantTypeContext.GetMerchantType(merchant));
        }
    }

    public static void GiveAssortment(List<(ItemDefinition, ShopItemType)> items,
        [NotNull] IEnumerable<(MerchantDefinition, MerchantTypeContext.MerchantType)> merchants)
    {
        foreach (var (merchant, type) in merchants)
        {
            GiveAssortment(items, merchant, type);
        }
    }

    public static void GiveAssortment([NotNull] List<(ItemDefinition, ShopItemType)> items,
        MerchantDefinition merchant,
        MerchantTypeContext.MerchantType type)
    {
        foreach (var (item, itemType) in items)
        {
            if (itemType.Filter.Matches(type))
            {
                StockItem(merchant, item, itemType.Status);
            }
        }
    }

    private static void StockItem([NotNull] MerchantDefinition merchant, ItemDefinition item,
        [NotNull] BaseDefinition status)
    {
        merchant.StockUnitDescriptions.Add(StockBuilder
            .SetItem(item)
            .SetFaction(merchant.FactionAffinity, status.Name)
            .Build()
        );
    }

    private static StockUnitDescriptionBuilder BuildStockBuilder()
    {
        return new StockUnitDescriptionBuilder()
            .SetStock(initialAmount: 1)
            .SetRestock(1);
    }
}

internal static class RecipeHelper
{
    public static RecipeDefinition BuildRecipe([NotNull] ItemDefinition item, int hours, int difficulty,
        params ItemDefinition[] ingredients)
    {
        return RecipeDefinitionBuilder
            .Create($"RecipeEnchant{item.Name}")
            .SetGuiPresentation(item.GuiPresentation.Title, GuiPresentationBuilder.EmptyString)
            .SetCraftedItem(item)
            .SetCraftingCheckData(hours, difficulty, ToolTypeDefinitions.EnchantingToolType)
            .AddIngredients(ingredients)
            .AddToDB();
    }

    [NotNull]
    public static ItemDefinition BuildRecipeManual([NotNull] ItemDefinition item, int hours, int difficulty,
        params ItemDefinition[] ingredients)
    {
        return BuildManual(BuildRecipe(item, hours, difficulty, ingredients));
    }

    [NotNull]
    public static ItemDefinition BuildManual([NotNull] RecipeDefinition recipe)
    {
        var reference = ItemDefinitions.CraftingManualScrollOfVampiricTouch;
        var manual = ItemDefinitionBuilder
            .Create($"CraftingManual{recipe.Name}")
            .SetGuiPresentation(Category.Item, reference)
            .SetItemPresentation(reference.ItemPresentation)
            .SetMerchantCategory(MerchantCategoryDefinitions.Crafting)
            .SetSlotTypes(SlotTypeDefinitions.ContainerSlot)
            .SetItemTags(TagsDefinitions.ItemTagStandard, TagsDefinitions.ItemTagPaper)
            .SetDocumentInformation(recipe, reference.DocumentDescription.ContentFragments)
            .SetGold(Main.Settings.RecipeCost)
            .AddToDB();

        manual.inDungeonEditor = false;

        return manual;
    }

    [NotNull]
    public static ItemDefinition BuildPrimingManual(ItemDefinition item, ItemDefinition primed)
    {
        return BuildManual(ItemRecipeGenerationHelper.CreatePrimingRecipe(item, primed));
    }
}

internal sealed class MerchantFilter
{
    internal bool? IsAmmunition = null;
    internal bool? IsArmor = null;
    internal bool? IsMeleeWeapon;
    internal bool? IsRangeWeapon;
    internal bool? IsDocument;

    internal bool? IsMagicalAmmunition = null;
    internal bool? IsMagicalArmor = null;
    internal bool? IsMagicalMeleeWeapon;
    internal bool? IsMagicalRangeWeapon;
    internal bool? IsMagicalEquipment = null;

    internal bool? IsPrimedArmor = null;
    internal bool? IsPrimedMeleeWeapon;
    internal bool? IsPrimedRangeWeapon;

    internal bool Matches(MerchantTypeContext.MerchantType merchantType)
    {
        return (IsAmmunition == null || IsAmmunition == merchantType.IsAmmunition) &&
               (IsArmor == null || IsArmor == merchantType.IsArmor) &&
               (IsDocument == null || IsDocument == merchantType.IsDocument) &&
               (IsMagicalAmmunition == null || IsMagicalAmmunition == merchantType.IsMagicalAmmunition) &&
               (IsMagicalArmor == null || IsMagicalArmor == merchantType.IsMagicalArmor) &&
               (IsMagicalMeleeWeapon == null || IsMagicalMeleeWeapon == merchantType.IsMagicalMeleeWeapon) &&
               (IsMagicalRangeWeapon == null || IsMagicalRangeWeapon == merchantType.IsMagicalRangeWeapon) &&
               (IsMagicalEquipment == null || IsMagicalEquipment == merchantType.IsMagicalEquipment) &&
               (IsMeleeWeapon == null || IsMeleeWeapon == merchantType.IsMeleeWeapon) &&
               (IsPrimedArmor == null || IsPrimedArmor == merchantType.IsPrimedArmor) &&
               (IsPrimedMeleeWeapon == null || IsPrimedMeleeWeapon == merchantType.IsPrimedMeleeWeapon) &&
               (IsPrimedRangeWeapon == null || IsPrimedRangeWeapon == merchantType.IsPrimedRangeWeapon) &&
               (IsRangeWeapon == null || IsRangeWeapon == merchantType.IsRangeWeapon);
    }

    internal static readonly MerchantFilter GenericMelee = new() {IsMeleeWeapon = true};
    internal static readonly MerchantFilter MagicMelee = new() {IsMagicalMeleeWeapon = true};
    internal static readonly MerchantFilter PrimedMelee = new() {IsPrimedMeleeWeapon = true};
    internal static readonly MerchantFilter GenericRanged = new() {IsRangeWeapon = true};
    internal static readonly MerchantFilter MagicRanged = new() {IsMagicalRangeWeapon = true};
    internal static readonly MerchantFilter PrimedRanged = new() {IsPrimedRangeWeapon = true};
    internal static readonly MerchantFilter CraftingManual = new() {IsDocument = true};
}

internal sealed class ShopItemType
{
    internal readonly MerchantFilter Filter;
    internal readonly FactionStatusDefinition Status;

    internal ShopItemType(FactionStatusDefinition status, MerchantFilter filter)
    {
        Status = status;
        Filter = filter;
    }

    internal static readonly ShopItemType ShopGenericMelee =
        new(FactionStatusDefinitions.Indifference, MerchantFilter.GenericMelee);

    internal static readonly ShopItemType ShopPrimedMelee =
        new(FactionStatusDefinitions.Sympathy, MerchantFilter.PrimedMelee);

    internal static readonly ShopItemType ShopMeleePlus1 =
        new(FactionStatusDefinitions.Alliance, MerchantFilter.MagicMelee);

    internal static readonly ShopItemType ShopMeleePlus2 =
        new(FactionStatusDefinitions.Brotherhood, MerchantFilter.MagicMelee);

    internal static readonly ShopItemType ShopGenericRanged =
        new(FactionStatusDefinitions.Indifference, MerchantFilter.GenericRanged);

    internal static readonly ShopItemType ShopPrimedRanged =
        new(FactionStatusDefinitions.Sympathy, MerchantFilter.PrimedRanged);

    internal static readonly ShopItemType ShopRangedPlus1 =
        new(FactionStatusDefinitions.Alliance, MerchantFilter.MagicRanged);

    internal static readonly ShopItemType ShopRangedPlus2 =
        new(FactionStatusDefinitions.Brotherhood, MerchantFilter.MagicRanged);

    internal static readonly ShopItemType ShopCrafting =
        new(FactionStatusDefinitions.Alliance, MerchantFilter.CraftingManual);
}
