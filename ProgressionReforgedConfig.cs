using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ProgressionReforged;

public class ProgressionReforgedConfig : ModConfig
{
    public static ProgressionReforgedConfig Instance => ModContent.GetInstance<ProgressionReforgedConfig>();

    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Header("Life Crystal Availability")]
    [Label("Initial life crystal density multiplier")]
    [Range(0f, 1f)]
    [DefaultValue(0.2f)]
    public float InitialLifeCrystalMultiplier { get; set; } = 0.2f;

    [Header("Life Crystal Costs")]
    [Label("Base life crystal cost per heart")] 
    [Range(1, 10)]
    [DefaultValue(1)]
    public int LifeCrystalBaseCost { get; set; } = 1;

    [Label("Hearts added before cost increases")]
    [Tooltip("How many heart upgrades share the same cost before the cost increases again.")]
    [Range(1, 10)]
    [DefaultValue(2)]
    public int LifeCrystalCostIncreaseFrequency { get; set; } = 2;

    [Label("Additional crystals added to the cost each step")]
    [Range(0, 10)]
    [DefaultValue(1)]
    public int LifeCrystalCostIncreaseAmount { get; set; } = 1;

    [Header("Boss Life Crystal Rewards")]
    [Label("King Slime reward crystals")]
    [Range(0, 30)]
    [DefaultValue(5)]
    public int KingSlimeLifeCrystals { get; set; } = 5;

    [Label("Eye of Cthulhu reward crystals")]
    [Range(0, 30)]
    [DefaultValue(5)]
    public int EyeOfCthulhuLifeCrystals { get; set; } = 5;

    [Label("Eater of Worlds reward crystals")]
    [Range(0, 30)]
    [DefaultValue(6)]
    public int EaterOfWorldsLifeCrystals { get; set; } = 6;

    [Label("Brain of Cthulhu reward crystals")]
    [Range(0, 30)]
    [DefaultValue(6)]
    public int BrainOfCthulhuLifeCrystals { get; set; } = 6;

    [Label("Skeletron reward crystals")]
    [Range(0, 30)]
    [DefaultValue(8)]
    public int SkeletronLifeCrystals { get; set; } = 8;
}
