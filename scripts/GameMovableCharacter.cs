using Godot;

namespace Polyblast.scripts;

public partial class GameMovableCharacter : CharacterBody2D
{
	[Export] private float _speed = 300.0f;
	[Export] private float _acceleration = 0.05f;
	[Export] private float _friction = 0.05f;

	[Export] private float _maxHealth;

	private float _health;
	public float Health
	{
		get => _health;
		set => _health = Mathf.Min(Mathf.Max(value, 0), _maxHealth);
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

		MoveAndSlide();
	}
}
