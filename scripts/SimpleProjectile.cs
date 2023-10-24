using System;
using Godot;

namespace Polyblast.scripts;
public partial class SimpleProjectile : Sprite2D
{
	private static PackedScene _projectilePrefab;
	
	[Export] private Area2D _area;
	
	private Vector2 _dir;
	private float _speed;

	private float _lifeTime;
	private float _timeAlive;

	public override void _Ready()
	{
		_area.BodyEntered += OnEnterBody;
	}

	public void Init(Vector2 pos, Vector2 dir, float speed, CollideType collideType)
	{
		_dir = dir;
		_speed = speed;
		_lifeTime = Mathf.Pow(0.99f, speed) + 1;
		
		GlobalPosition = pos;
		Rotation = Mathf.Atan2(dir.Y, dir.X);

		_area.CollisionLayer = collideType switch
		{
			CollideType.Player => 1,
			CollideType.Enemy => 2,
			_ => throw new ArgumentOutOfRangeException(nameof(collideType), collideType, null)
		};

		_area.CollisionMask = _area.CollisionLayer;
	}

	public override void _PhysicsProcess(double delta)
	{
		// move towards the direction
		Position += _dir * (float)delta * _speed * 400;
	}

	public override void _Process(double delta)
	{
		_timeAlive += (float)delta;
		
		var modulate = Modulate;
		modulate.A = 1 - (float)Mathf.Remap(_timeAlive, 0, _lifeTime, 0.1, 1);
		Modulate = modulate;
		
		if (_timeAlive > _lifeTime)
		{
			QueueFree();
		}
	}

	private void OnEnterBody(Node2D body)
	{
		QueueFree();
		
		if (body is Player)
		{
			var hitDir = (body.GlobalPosition - GlobalPosition).Normalized();
			CameraController.Shake(hitDir, GD.RandRange(30, 38));
		}
		else if (body is GameMovableCharacter gameMovableCharacter)
		{
			gameMovableCharacter.Health -= 1;
		}
	}
	
	public static SimpleProjectile Spawn(Vector2 globalPos, Vector2 dir, float speed, CollideType collideType, Node spawner)
	{
		// load if not loaded
		_projectilePrefab ??= GD.Load<PackedScene>("res://scenes/projectile.tscn");
		
		var node = _projectilePrefab.Instantiate();
		var projectile = node as SimpleProjectile;
		spawner.GetNode("%WorldProjectiles")?.AddChild(node);
		projectile?.Init(globalPos, dir.Normalized(), speed, collideType);

		return projectile;
	}
}
