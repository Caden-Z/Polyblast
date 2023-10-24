using Godot;

namespace Polyblast.scripts;

public partial class Player : GameMovableCharacter
{
	public override void _PhysicsProcess(double delta)
	{
		var inputVelocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		MoveCharacter(inputVelocity);

		RotationDegrees += Mathf.Max(
			(float)delta * Velocity.LengthSquared() * 0.0005f,
			(float)delta * 30
		);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("attack"))
		{
			SimpleProjectile.Spawn(GlobalPosition, GetGlobalMousePosition() - GlobalPosition, 10, CollideType.Enemy, this);
		}
	}
}
