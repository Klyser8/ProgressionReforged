namespace ProgressionReforged.Systems.Reforge;

internal static class PriceHelper
{
    internal static float WeightedDelta(float mult, float weight, bool inverse = false)
    {
        float delta = inverse ? (1f / mult - 1f) : (mult - 1f);
        return delta * weight;
    }

    // 1.00f means “1 % of this stat = 1 % price change”
    // 0.50f ⇒ a 1 % boost adds only 0.5 % cost
    // 2.00f ⇒ a 1 % boost adds 2 % cost
    internal static class PriceWeight
    {
        public const float Damage = 2.50f;
        public const float UseSpeed = 3.25f;   // (inverse of useTime)
        public const float ShootSpeed = 0.80f; // bullet velocity
        public const float Size = 1.50f;
        public const float Knockback = 1.33f;
        public const float ManaCost = 1.22f;   // inverse
        public const float CritChance = 4.00f; // +1 % crit chance
        public const float CritDamage = 2.22f;
        public const float WhipRange = 1.5f;
        public const float WhipTagDamage = 2.66f;
    }
}