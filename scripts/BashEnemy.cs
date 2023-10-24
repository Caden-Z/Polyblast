using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace Polyblast.scripts;

public partial class BashEnemy : GameEnemy
{
	[Export] private Node2D _target;
	[Export] private float _attackInterval;
	[Export] private float _bashDuration;
	[Export] private float _bashSpeed;
	[Export] private float _chargingSpeed;

	[Export] private Area2D _area;
	[Export] private Sprite2D _sprite;

	private bool _attacking;
	private bool _bashing;
	private float _timeElapsed;
	private Vector2 _bashedDir;

	public override void _Ready()
	{
		Target = _target;
		_area.BodyEntered += OnBodyEnter;
	}

	public override void _Process(double delta)
	{
		var dir = Target.GlobalPosition - GlobalPosition;
		this.TweenRotation(Mathf.Atan2(dir.Y, dir.X), 0.1f).Play();
	}

	public override void _PhysicsProcess(double delta)
	{
		var dir = Target.GlobalPosition - GlobalPosition;

		if (_timeElapsed > _attackInterval && dir.LengthSquared() < 300000)
		{
			_attacking = true;
			_timeElapsed = 0;

			_sprite.TweenScaleY(0.15f, 0.75f)
				.SetEasing(Easing.OutCubic)
				.OnComplete(() =>
				{
					_sprite.TweenScaleY(0.25f, 0.1f)
						.SetEasing(Easing.OutCubic)
						.Play();

					_bashing = true;
					_bashedDir = dir;
				})
				.Play();
		}

		if (_attacking)
		{
			if (_bashing)
			{
				MoveCharacter(dir, speed: _bashSpeed);
				_timeElapsed += (float)delta;

				if (_timeElapsed > _bashDuration)
				{
					StopAttack();
				}
			}
			else
			{
				MoveCharacter(dir, speed: _chargingSpeed);
			}

			return;
		}

		if (dir.LengthSquared() > 200000)
		{
			MoveCharacter(dir);
		}
		
		_timeElapsed += (float)delta;
	}

	private void StopAttack()
	{
		_bashing = false;
		_attacking = false;
		_timeElapsed = 0;
	}

	private void OnBodyEnter(Node2D body)
	{
		GD.Print("HIT!");
	}
}
