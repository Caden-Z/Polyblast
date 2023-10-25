using Godot;

namespace Polyblast.scripts;

public partial class GameEnemy : GameMovableCharacter
{
	[Export] protected Node2D Target;
	
	public void Init(Node2D target)
	{
		Target = target;
		Health += Mathf.FloorToInt(WaveSpawner.WaveOn * 0.1f);
	}

	protected override void OnHealthChanged(int newHealth)
	{
		if (IsNodeReady() && newHealth == 0)
		{
			QueueFree();
		}
	}
}