using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace Polyblast.scripts;

public partial class BashEnemy : GameEnemy
{
	[Export] private float _attackInterval;
	[Export] private float _chargingSpeed;
	[Export] private float _maxBashLength;
	[Export] private float _bashSpeed;
	[Export] private float _range;

	[Export] private Area2D _area;
	[Export] private Sprite2D _sprite;

	private float _timeElapsed;
	private bool _attacking;

	public override void _Ready()
	{
		base._Ready();
		_area.BodyEntered += OnBodyEnter;
		_area.CollisionLayer = (uint) CollideType.None;
		_area.CollisionMask = _area.CollisionLayer;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		var dir = Target.GlobalPosition - GlobalPosition;
		this.TweenRotation(Mathf.Atan2(dir.Y, dir.X), 0.1f).Play();
	}

	public override void _PhysicsProcess(double delta)
	{
		var dir = Target.GlobalPosition - GlobalPosition;

		if (_timeElapsed > _attackInterval && dir.LengthSquared() < _range)
		{
			_timeElapsed = 0;
			_attacking = true;

			_sprite.TweenScaleY(0.15f, 0.75f)
				.SetEasing(Easing.OutCubic)
				.OnComplete(() =>
				{
					if (!IsInstanceValid(_area)) return;
					
					_area.CollisionLayer = (uint) CollideType.Player;
					_area.CollisionMask = _area.CollisionLayer;
					
					_sprite.TweenScaleY(0.25f, 0.1f)
						.SetEasing(Easing.OutCubic)
						.Play();
					
					_sprite.TweenModulateRgb(Colors.White, 0.1f)
						.SetEasing(Easing.OutCubic)
						.Play();

					var direction = (Target.GlobalPosition - GlobalPosition).Normalized();
					
					this.TweenGlobalPosition(GlobalPosition + direction * _maxBashLength, _bashSpeed)
						.SetEasing(Easing.OutCubic)
						.OnComplete(() =>
						{
							_attacking = false;
							_area.CollisionLayer = (uint) CollideType.None;
							_area.CollisionMask = _area.CollisionLayer;
						})
						.Play();
				})
				.Play();
			
			_sprite.TweenModulateRgb(Colors.Crimson, 0.75f)
				.SetEasing(Easing.OutCubic)
				.Play();
		}

		if (_attacking)
		{
			MoveCharacter(dir, speed: _chargingSpeed);
			return;
		}

		if (dir.LengthSquared() > _range / 1.8f)
		{
			MoveCharacter(dir);
		}
		
		_timeElapsed += (float)delta * GameTimeScale.TimeScale;
	}

	private void OnBodyEnter(Node2D body)
	{
		var hitDir = (body.GlobalPosition - GlobalPosition).Normalized();
		
		if (body is CharacterBody2D characterBody2D)
		{
			characterBody2D.Velocity += hitDir * 1000;
		}

		if (body is Player player)
		{
			CameraController.Shake(hitDir, GD.RandRange(60, 72), 1.25f);
			player.Health -= 1 + Mathf.FloorToInt(WaveSpawner.WaveOn / 8f);
		}
	}
}
