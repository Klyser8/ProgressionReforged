using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ProgressionReforged;

public class ProgressionReforgedConfig : ModConfig
{
    public static ProgressionReforgedConfig Instance => ModContent.GetInstance<ProgressionReforgedConfig>();

    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Header("Life_Crystal_Availability")]
    [Label("Initial_Life_crystal_density_multiplier")]
    [Range(0f, 1f)]
    [DefaultValue(0.2f)]
    public float InitialLifeCrystalMultiplier { get; set; } = 0.33f;

    [Header("Life_Crystal_Costs")]
    [Label("Base_life_crystal_cost_per_heart")] 
    [Range(1, 10)]
    [DefaultValue(1)]
    public int LifeCrystalBaseCost { get; set; } = 1;

    [Label("Hearts_added_before_cost_increases")]
    [Tooltip("How_many_heart_upgrades_share_the_same_cost_before_the_cost_increases_again.")]
    [Range(1, 10)]
    [DefaultValue(3)]
    public int LifeCrystalCostIncreaseFrequency { get; set; } = 3;

    [Label("Additional_crystals_added_to_the_cost_each_step")]
    [Range(0, 10)]
    [DefaultValue(1)]
    public int LifeCrystalCostIncreaseAmount { get; set; } = 1;

    [Header("Boss_Life_Crystal_Rewards")]
    [Label("King_Slime_reward_crystals")]
    [Range(0, 100)]
    [DefaultValue(35)]
    public int KingSlimeLifeCrystals { get; set; } = 35;

    [Label("Eye_of_Cthulhu_reward_crystals")]
    [Range(0, 100)]
    [DefaultValue(40)]
    public int EyeOfCthulhuLifeCrystals { get; set; } = 40;

    [Label("Evil_Boss_reward_crystals")]
    [Range(0, 100)]
    [DefaultValue(60)]
    public int EvilBossLifeCrystals { get; set; } = 60;

    [Label("Queen_Bee_reward_crystals")]
    [Range(0, 100)]
    [DefaultValue(30)]
    public int QueenBeeLifeCrystals { get; set; } = 30;

    [Label("Deerclops_reward_crystals")]
    [Range(0, 100)]
    [DefaultValue(45)]
    public int DeerclopsLifeCrystals { get; set; } = 45;

    [Label("Skeletron_reward_crystals")]
    [Range(0, 100)]
    [DefaultValue(60)]
    public int SkeletronLifeCrystals { get; set; } = 60;
}