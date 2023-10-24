using Godot;

namespace Polyblast.scripts;

public partial class GameEnemy : GameMovableCharacter
{
	[Export] protected Node2D Target;
	
	public void Init(Node2D target)
	{
		Target = target;
	}

	public override void _Process(double delta)
	{
		if (IsNodeReady() && Health == 0)
		{
			QueueFree();
		}
	}
}