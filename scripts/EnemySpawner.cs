using System;
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace Polyblast.scripts;

public partial class EnemySpawner : Node2D
{
	public static Node2D ProjectilesParent { get; private set; }

	[Export] private PackedScene[] _enemyTypes;
	[Export] private Node2D _player;
	[Export] private Node2D _projectilesParent;

	[Export] private Control _waveCompleteUI;
	[Export] private Control _waveCompleteText;

	public static int WaveOn { get; private set; }
	private bool _readyForNextWave;
	private bool _justStarted;

	public override void _Ready()
	{
		ProjectilesParent = _projectilesParent;
		_readyForNextWave = true;
	}

	public override void _Process(double delta)
	{
		if (!IsNodeReady()) return;
		if (GetChildCount() > 0) return;
		
		if (WaveOn != 0 && _readyForNextWave && !_justStarted)
		{
			_readyForNextWave = false;
			_justStarted = true;
			StartNextWave();
		}

		if (!_readyForNextWave) return;

		var enemies = new PackedScene[GD.RandRange(Mathf.Max(WaveOn + 1, 1), Mathf.Max(WaveOn + 2, WaveOn * 2))];
		GD.Print("Enemies Count: " + enemies.Length);
		
		for (var i = 0; i < enemies.Length; i++)
		{
			enemies[i] = _enemyTypes[GD.RandRange(0, _enemyTypes.Length - 1)];
		}
		
		new EnemyWave(enemies, -MathF.Log10(10 * (WaveOn + 1)) + 3)
			.Spawn(_player, this);
		
		WaveOn++;
		_justStarted = false;
	}

	private async void StartNextWave()
	{
		_waveCompleteUI.TweenModulateAlpha(1, 0.5f)
			.Play();

		var pos = _waveCompleteText.Position;
		var ogPos = pos;
		pos.X = 0;
		
		_waveCompleteText.Position = pos;
		var textTween = GTweenSequenceBuilder.New()
			.Append(_waveCompleteText.TweenPositionX(ogPos.X, 1).SetEasing(Easing.OutCubic))
			.Join(_waveCompleteText.TweenScale(new Vector2(1.25f, 1.25f), 1).SetEasing(Easing.OutCubic))
			.Build();
		textTween.Play();

		await TaskHelper.WaitUntil(() => textTween.IsCompleted);
		
		textTween = GTweenSequenceBuilder.New()
					.Append(_waveCompleteText.TweenPositionX(ogPos.X * 2, 1).SetEasing(Easing.InCubic))
					.Join(_waveCompleteText.TweenScale(new Vector2(1f, 1f), 1).SetEasing(Easing.InCubic))
					.Build();
		textTween.Play();
		
		_waveCompleteUI.TweenModulateAlpha(0, 0.8f)
			.Play();

		await TaskHelper.WaitUntil(() => textTween.IsCompleted);

		_waveCompleteText.Position = ogPos;
		_readyForNextWave = true;
		GD.Print($"Starting wave: {WaveOn}");
	}
}
