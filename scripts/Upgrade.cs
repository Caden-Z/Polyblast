using System;
using Godot;

namespace Polyblast.scripts;

public abstract partial class Upgrade : Resource
{
    [Export] public string Title;
    [Export] public string Description;

    public Upgrade() : this("", "")
    {
    }

    public Upgrade(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public abstract void OnUpgrade(
        float currentDifficulty, Action<float> difficultySetter,
        float playerFireRate, Action<float> playerFireRateSetter, 
        int playerHealth, Action<int> playerHealthSetter,
        int playerDamage, Action<int> playerDamageSetter,
        float playerSpeed, Action<float> playerSpeedSetter
    );
}