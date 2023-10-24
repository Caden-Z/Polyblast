using Godot;

namespace Polyblast.scripts;

public partial class GameEnemy : GameMovableCharacter
{
	[Export] protected Node2D Target;
	
	public void Init(Node2D target)
	{
		Target = target;
	}

	protected override void OnHealthChanged(float newHealth)
	{
		if (IsNodeReady() && newHealth == 0)
		{
			QueueFree();
		}
	}
}