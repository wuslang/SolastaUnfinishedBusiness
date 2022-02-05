using SolastaModApi.Infrastructure;
using AK.Wwise;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System;
using System.Text;
using System.CodeDom.Compiler;
using TA.AI;
using TA;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using  static  ActionDefinitions ;
using  static  TA . AI . DecisionPackageDefinition ;
using  static  TA . AI . DecisionDefinition ;
using  static  RuleDefinitions ;
using  static  BanterDefinitions ;
using  static  Gui ;
using  static  BestiaryDefinitions ;
using  static  CursorDefinitions ;
using  static  AnimationDefinitions ;
using  static  CharacterClassDefinition ;
using  static  CreditsGroupDefinition ;
using  static  CampaignDefinition ;
using  static  GraphicsCharacterDefinitions ;
using  static  GameCampaignDefinitions ;
using  static  TooltipDefinitions ;
using  static  BaseBlueprint ;
using  static  MorphotypeElementDefinition ;

namespace SolastaModApi.Extensions
{
    /// <summary>
    /// This helper extensions class was automatically generated.
    /// If you find a problem please report at https://github.com/SolastaMods/SolastaModApi/issues.
    /// </summary>
    [TargetType(typeof(CreditsEntryDescription)), GeneratedCode("Community Expansion Extension Generator", "1.0.0")]
    public static partial class CreditsEntryDescriptionExtensions
    {
        public static T SetEntryType<T>(this T entity, CreditsGroupDefinition.EntryType value)
            where T : CreditsEntryDescription
        {
            entity.EntryType = value;
            return entity;
        }

        public static T SetImageHeight<T>(this T entity, System.Single value)
            where T : CreditsEntryDescription
        {
            entity.SetField("imageHeight", value);
            return entity;
        }

        public static T SetSpriteReference<T>(this T entity, UnityEngine.AddressableAssets.AssetReferenceSprite value)
            where T : CreditsEntryDescription
        {
            entity.SetField("spriteReference", value);
            return entity;
        }

        public static T SetTextContent<T>(this T entity, System.String value)
            where T : CreditsEntryDescription
        {
            entity.TextContent = value;
            return entity;
        }
    }
}