using System;

namespace Polyblast.scripts.upgrades;

public partial class FireRateUpgrade : Upgrade
{
    public FireRateUpgrade() : base("pew pew", "Increases fire rate by 10%")
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
        playerFireRateSetter(playerFireRate * 0.9f);
    }
}