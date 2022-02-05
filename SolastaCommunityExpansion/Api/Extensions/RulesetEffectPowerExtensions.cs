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
    [TargetType(typeof(RulesetEffectPower)), GeneratedCode("Community Expansion Extension Generator", "1.0.0")]
    public static partial class RulesetEffectPowerExtensions
    {
        public static T SetName<T>(this T entity, System.String value)
            where T : RulesetEffectPower
        {
            entity.Name = value;
            return entity;
        }

        public static T SetOriginItem<T>(this T entity, RulesetItemDevice value)
            where T : RulesetEffectPower
        {
            entity.SetField("originItem", value);
            return entity;
        }

        public static T SetOriginItemGuid<T>(this T entity, System.UInt64 value)
            where T : RulesetEffectPower
        {
            entity.SetField("originItemGuid", value);
            return entity;
        }

        public static T SetSourceDefinition<T>(this T entity, FeatureDefinitionPower value)
            where T : RulesetEffectPower
        {
            entity.SetField("sourceDefinition", value);
            return entity;
        }

        public static T SetUsableDeviceFunction<T>(this T entity, RulesetDeviceFunction value)
            where T : RulesetEffectPower
        {
            entity.SetField("usableDeviceFunction", value);
            return entity;
        }

        public static T SetUsablePower<T>(this T entity, RulesetUsablePower value)
            where T : RulesetEffectPower
        {
            entity.SetField("usablePower", value);
            return entity;
        }

        public static T SetUser<T>(this T entity, RulesetCharacter value)
            where T : RulesetEffectPower
        {
            entity.SetField("user", value);
            return entity;
        }

        public static T SetUserId<T>(this T entity, System.UInt64 value)
            where T : RulesetEffectPower
        {
            entity.SetField("userId", value);
            return entity;
        }
    }
}