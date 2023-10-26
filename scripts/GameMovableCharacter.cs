using Godot;

namespace Polyblast.scripts;

public abstract partial class GameMovableCharacter : CharacterBody2D
{
	[Export] public float _speed = 300.0f;
	[Export] private float _acceleration = 0.05f;
	[Export] private float _friction = 0.05f;

	[Export] private int _maxHealth;

	private int _health;

	public int Health
	{
		get => _health;
		set
		{
			_health = Mathf.Min(Mathf.Max(value, 0), _maxHealth);
			OnHealthChanged(_health);
		}
	}

	public override void _Ready()
	{
		_health = _maxHealth;
	}

	protected void MoveCharacter(Vector2 dir, float speed = -1, float acceleration = -1, float friction = -1)
	{
		var inputVelocity = dir.Normalized() * (speed == -1 ? _speed : speed);

		if (inputVelocity.LengthSquared() > 0)
			Velocity = Velocity.Lerp(inputVelocity, (acceleration == -1 ? _acceleration : acceleration));
		else
			Velocity = Velocity.Lerp(Vector2.Zero, (friction == -1 ? _friction : friction));

		Velocity *= GameTimeScale.TimeScale;
		MoveAndSlide();
	}

	protected abstract void OnHealthChanged(int newHealth);
}
