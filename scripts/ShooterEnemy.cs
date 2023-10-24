using Godot;

namespace Polyblast.scripts;

public partial class ShooterEnemy : GameEnemy
{
	[Export] private PackedScene _projectile;
	[Export] private float _shootInterval;
	
	private float _timeElapsed;

	public override void _Process(double delta)
	{
		base._Process(delta);
		var targetDirection = Target.GlobalPosition - GlobalPosition;
		Shoot(targetDirection, (float)delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		var dir = Target.GlobalPosition - GlobalPosition;
		dir = dir.LengthSquared() > 90000 ? Vector2.Zero : dir;
		MoveCharacter(-dir);
	}

	private void Shoot(Vector2 targetDirection, float delta)
	{
		_timeElapsed += delta;
		if (!(_timeElapsed > _shootInterval)) return;
		_timeElapsed = 0;
		SimpleProjectile.Spawn(GlobalPosition, targetDirection, 10, CollideType.Player, this);
	}
}
