using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace Polyblast.scripts;

public partial class ShooterEnemy : GameEnemy
{
	[Export] private Sprite2D _sprite;
	[Export] private PackedScene _projectile;
	[Export] private float _shootInterval;
	[Export] private float _projectileSpeed;

	private float _timeElapsed;

	public override void _Process(double delta)
	{
		base._Process(delta);
		var targetDirection = Target.GlobalPosition - GlobalPosition;
		Rotation = Mathf.Atan2(targetDirection.Y, targetDirection.X);

		if (_timeElapsed > _shootInterval)
		{
			_timeElapsed = 0;
			SimpleProjectile.Spawn(GlobalPosition, targetDirection, _projectileSpeed, 1 + Mathf.FloorToInt(WaveSpawner.WaveOn / 6f), CollideType.Player);
			_sprite.TweenScaleY(0.4f, 0.1f)
				.SetEasing(Easing.InSine)
				.OnComplete(() =>
				{
					if (!IsInstanceValid(_sprite)) return;
					_sprite.TweenScaleY(0.25f, 0.1f)
						.SetEasing(Easing.OutExpo)
						.Play();
				})
				.Play();
			return;
		}

		_timeElapsed += (float)delta * GameTimeScale.TimeScale;
		var scale = _sprite.Scale;
		var progress = Mathf.Remap(Mathf.Max(_timeElapsed - _shootInterval / 2f, 0), 0, _shootInterval, 0, 1);

		scale.Y = Mathf.Lerp(scale.Y, 0.15f, progress);
		_sprite.Scale = scale;
	}

	public override void _PhysicsProcess(double delta)
	{
		var dir = Target.GlobalPosition - GlobalPosition;
		dir = dir.LengthSquared() < 90000 ? dir :
			dir.LengthSquared() > 110000 ? -dir : dir;
		MoveCharacter(-dir);
	}
}
