using Godot;

namespace Polyblast.scripts;

public partial class WaveSpawner : Node2D
{
	public static Node2D ProjectilesParent { get; private set; }
	
	[Export] private PackedScene[] _enemyPrefabs;
	[Export] private Node2D _player;
	[Export] private Node2D _projectilesParent;

	public override void _Ready()
	{
		ProjectilesParent = _projectilesParent;
	}

	public override void _Process(double delta)
	{
		if (!IsNodeReady()) return;
		if (GetChildCount() <= 0)
		{
			foreach (var enemyPrefab in _enemyPrefabs)
			{
				var enemy = enemyPrefab.Instantiate() as GameEnemy;
				enemy.GlobalPosition = new Vector2((float)GD.RandRange(-1f, 1f), (float)GD.RandRange(-1f, 1f)) * 400 + _player.GlobalPosition;
				enemy.Init(_player);
				AddChild(enemy);
				GD.Print("Spawned enemy");
			}
		}
	}
}
