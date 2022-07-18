﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SolastaCommunityExpansion.Classes.Magus;
using SolastaCommunityExpansion.Classes.Monk;
using SolastaCommunityExpansion.Classes.Tinkerer;
using SolastaCommunityExpansion.Classes.Warlock;
using SolastaCommunityExpansion.Classes.Witch;

namespace SolastaCommunityExpansion.Models;

internal static class ClassesContext
{
    internal static HashSet<CharacterClassDefinition> Classes { get; private set; } = new();

    private static void SortClassesFeatures()
    {
        var dbCharacterClassDefinition = DatabaseRepository.GetDatabase<CharacterClassDefinition>();

        foreach (var characterClassDefinition in dbCharacterClassDefinition)
        {
            characterClassDefinition.FeatureUnlocks.Sort((a, b) =>
            {
                var result = a.Level - b.Level;

                if (result == 0)
                {
                    result = String.Compare(a.FeatureDefinition.FormatTitle(), b.FeatureDefinition.FormatTitle(),
                        StringComparison.CurrentCulture);
                }

                return result;
            });
        }
    }

    internal static void Load()
    {
        LoadClass(Monk.BuildClass());
        LoadClass(TinkererClass.BuildTinkererClass());
        LoadClass(Warlock.BuildWarlockClass());
        LoadClass(Witch.Instance);
        LoadClass(Magus.BuildMagusClass());
        Classes = Classes.OrderBy(x => x.FormatTitle()).ToHashSet();
    }

    internal static void LateLoad()
    {
        if (Main.Settings.EnableSortingFutureFeatures)
        {
            SortClassesFeatures();
        }
    }

    private static void LoadClass([NotNull] CharacterClassDefinition characterClassDefinition)
    {
        if (!Classes.Contains(characterClassDefinition))
        {
            Classes.Add(characterClassDefinition);
        }

        UpdateClassVisibility(characterClassDefinition);
    }

    private static void UpdateClassVisibility([NotNull] BaseDefinition characterClassDefinition)
    {
        characterClassDefinition.GuiPresentation.hidden =
            !Main.Settings.ClassEnabled.Contains(characterClassDefinition.Name);
    }

    internal static void Switch(CharacterClassDefinition characterClassDefinition, bool active)
    {
        if (!Classes.Contains(characterClassDefinition))
        {
            return;
        }

        var name = characterClassDefinition.Name;

        if (active)
        {
            Main.Settings.ClassEnabled.TryAdd(name);
        }
        else
        {
            Main.Settings.ClassEnabled.Remove(name);
        }

        UpdateClassVisibility(characterClassDefinition);
    }
}
