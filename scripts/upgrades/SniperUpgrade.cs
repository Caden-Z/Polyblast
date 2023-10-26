using System;

namespace Polyblast.scripts.upgrades;

public partial class SniperUpgrade : Upgrade
{
    public SniperUpgrade() : base("snipe", "Increases player damage by 2 but decreases fire rate by 20%")
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
        playerDamageSetter(playerDamage + 2);
        playerFireRateSetter(playerFireRate * 1.2f);
    }
}