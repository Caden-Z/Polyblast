using System;

namespace Polyblast.scripts.upgrades;

public partial class HealUpgrade : Upgrade
{
    public HealUpgrade() : base("Hero never die!", "Heal current health by 1")
    {
    }

    public override void OnUpgrade(
        float currentDifficulty, Action<float> difficultySetter, 
        float playerFireRate, Action<float> playerFireRateSetter,
        int playerHealth, Action<int> playerHealthSetter,
        int playerDamage, Action<int> playerDamageSetter,
        float playerSpeed, Action<float> playerSpeedSetter
    )
    {
        playerHealthSetter(playerHealth + 1);
    }
}