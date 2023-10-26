using System;

namespace Polyblast.scripts.upgrades;

public partial class JackOfAllTradesUpgrade : Upgrade
{
    public JackOfAllTradesUpgrade() : base("Jack of all trades", "Increases difficulty by 15%, but increases player fire rate by 5%, player speed by 20%")
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
        difficultySetter(currentDifficulty * 1.15f);
        playerFireRateSetter(playerFireRate * 0.95f);
        playerSpeedSetter(playerSpeed * 1.2f);
    }
}