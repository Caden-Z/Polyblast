using Godot;

namespace Polyblast.scripts;

public partial class GameEnemy : GameMovableCharacter
{
	protected Node2D Target;
	
	public void Init(Node2D target)
	{
		Target = target;
	}
}