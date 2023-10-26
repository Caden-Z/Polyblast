using System;

namespace Polyblast.scripts.upgrades;

public partial class MovementSpeedUpgrade : Upgrade
{
    public MovementSpeedUpgrade() : base("zoom zoom", "Increases movement speed by 15%")
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
        playerSpeedSetter(playerSpeed * 1.15f);
    }
}