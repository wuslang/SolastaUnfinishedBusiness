﻿using System.Diagnostics.CodeAnalysis;
using System.Text;
using HarmonyLib;
using JetBrains.Annotations;

namespace SolastaUnfinishedBusiness.Patches;

public static class FeatureDefinitionPointPoolPatcher
{
    [HarmonyPatch(typeof(FeatureDefinitionPointPool), "FormatDescription")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class FormatDescription_Patch
    {
        public static bool Prefix([NotNull] FeatureDefinitionPointPool __instance, ref string __result)
        {
            var choices = __instance.RestrictedChoices;

            if (__instance.poolType != HeroDefinitions.PointsPoolType.Tool || choices == null || choices.Empty())
            {
                return true;
            }

            var builder = new StringBuilder();
            var separator = Gui.ListSeparator();

            foreach (var restrictedChoice in choices)
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }

                var tool = DatabaseRepository.GetDatabase<ToolTypeDefinition>().GetElement(restrictedChoice);

                builder.Append(Gui.Localize(tool.GuiPresentation.Title));
            }

            __result = Gui.Format(__instance.GuiPresentation.Description, __instance.PoolAmount.ToString(),
                builder.ToString());

            return false;
        }
    }
}
