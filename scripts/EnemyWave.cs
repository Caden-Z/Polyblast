using System;
using System.Threading.Tasks;
using Godot;

namespace Polyblast.scripts;

public partial class EnemyWave : Resource
{
	[Export] private PackedScene[] _enemies;
	[Export] private float _spawnOverSeconds;

	public EnemyWave() : this(Array.Empty<PackedScene>(), 0)
	{
	}

	public EnemyWave(PackedScene[] enemies, float spawnOverSeconds)
	{
		_enemies = enemies;
		_spawnOverSeconds = spawnOverSeconds;
	}

	public void Spawn(Node2D target, Node2D spawner)
	{
		SpawnInternal(target, spawner);
	}

	private async void SpawnInternal(Node2D target, Node2D spawner)
	{
		var enemyPer = _spawnOverSeconds / _enemies.Length;

		foreach (var scene in _enemies)
		{
			SpawnIndividual(scene, target, spawner);
			await Task.Delay((int)(enemyPer * 1000));
		}
	}

	private void SpawnIndividual(PackedScene scene, Node2D target, Node2D spawner)
	{
		var enemy = scene.Instantiate() as GameEnemy;
		enemy.GlobalPosition = new Vector2((float)GD.RandRange(-1f, 1f), (float)GD.RandRange(-1f, 1f)) * 400 + target.GlobalPosition;
		enemy.Init(target);
		spawner.AddChild(enemy);
	}
}
