using System;
using Godot;

namespace Polyblast.scripts;
public partial class SimpleProjectile : Sprite2D
{
	
	private static PackedScene _projectilePrefab;
	private static Node _projectilesParent;
	
	[Export] private Area2D area;
	
	private Vector2 _dir;
	private float _speed;

	private float _lifeTime;
	private float _timeAlive;

	public override void _Ready()
	{
		area.BodyEntered += OnEnterBody;
	}

	public void Init(Vector2 pos, Vector2 dir, float speed, CollideType collideType)
	{
		_dir = dir;
		_speed = speed;
		_lifeTime = Mathf.Pow(0.99f, speed) + 1;
		
		GlobalPosition = pos;
		// simple trigonometry
		Rotation = Mathf.Atan2(dir.Y, dir.X);

		area.CollisionLayer = collideType switch
		{
			CollideType.Player => 1,
			CollideType.Enemy => 2,
			_ => throw new ArgumentOutOfRangeException(nameof(collideType), collideType, null)
		};
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
		GD.Print("Hit!");
		QueueFree();
		CameraController.Shake(36, 0.25f);
	}
	
	public static SimpleProjectile Spawn(Vector2 globalPos, Vector2 dir, float speed, CollideType collideType)
	{
		// load if not loaded
		_projectilePrefab ??= GD.Load<PackedScene>("res://scenes/projectile.tscn");
		_projectilesParent ??= (Engine.GetMainLoop() as SceneTree)?.Root.GetChild(0).GetChild(1);
		
		var node = _projectilePrefab.Instantiate();
		_projectilesParent?.AddChild(node);

		var projectile = node as SimpleProjectile;
		projectile?.Init(globalPos, dir.Normalized(), speed, collideType);

		return projectile;
	}
}
