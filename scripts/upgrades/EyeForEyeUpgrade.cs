using System;

namespace Polyblast.scripts.upgrades;

public partial class EyeForEyeUpgrade : Upgrade
{
    public EyeForEyeUpgrade() : base("An Eye for an Eye", "Increases difficulty by 20% but increases player damage by 1, movement speed by 10%")
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
        difficultySetter(currentDifficulty * 1.2f);
        playerDamageSetter(playerDamage + 1);
        playerSpeedSetter(playerSpeed * 1.1f);
    }
}