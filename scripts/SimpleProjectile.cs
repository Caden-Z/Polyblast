using Godot;

namespace Polyblast.scripts;
public partial class SimpleProjectile : Sprite2D
{
	private static PackedScene _projectilePrefab;
	
	[Export] private Area2D _area;
	
	private Vector2 _dir;
	private float _speed;
	private int _damage;

	private float _lifeTime;
	private float _timeAlive;

	public override void _Ready()
	{
		_area.BodyEntered += OnEnterBody;
	}

	public void Init(Vector2 pos, Vector2 dir, float speed, int damage, CollideType collideType, Texture2D texture, Color? color)
	{
		_dir = dir;
		_speed = speed;
		_damage = damage;
		_lifeTime = Mathf.Pow(0.99f, speed) + 1;
		
		GlobalPosition = pos;
		Rotation = Mathf.Atan2(dir.Y, dir.X);
		
		_area.CollisionLayer = (uint) collideType;
		_area.CollisionMask = _area.CollisionLayer;

		if (texture != null)
		{
			Texture = texture;
		}

		if (color != null)
		{
			Modulate = color.Value;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// move towards the direction
		Position += _dir * (float)delta * _speed * 400 * GameTimeScale.TimeScale;
	}

	public override void _Process(double delta)
	{
		_timeAlive += (float)delta * GameTimeScale.TimeScale;
		
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
		
		if (body is GameMovableCharacter gameMovableCharacter)
		{
			gameMovableCharacter.Health -= _damage;
		}
	}
	
	public static SimpleProjectile Spawn(Vector2 globalPos, Vector2 dir, float speed, int damage, CollideType collideType, Texture2D texture = null, Color? color = null)
	{
		// load if not loaded
		_projectilePrefab ??= GD.Load<PackedScene>("res://scenes/projectile.tscn");
		
		var node = _projectilePrefab.Instantiate();
		var projectile = node as SimpleProjectile;
		WaveSpawner.ProjectilesParent.AddChild(node);
		projectile?.Init(globalPos, dir.Normalized(), speed, damage, collideType, texture, color);

		return projectile;
	}
}
