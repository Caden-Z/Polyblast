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
	[Export] private Player _player;
	[Export] private Node2D _projectilesParent;

	// this logic can be separated into a different script, but its in here to save time in development
	// definitely should change when refactoring
	[Export] private Control _waveCompleteUI;
	[Export] private Control _waveCompleteText;

	[Export] private Control _upgradeUI;
	[Export] private UpgradeButton _upgrade1;
	[Export] private UpgradeButton _upgrade2;
	[Export] private UpgradeButton _upgrade3;
	[Export] private Upgrade[] _upgradesAvailable;

	public static float Difficulty { get; private set; }
	
	private int _waveOn;
	private bool _readyForNextWave;
	private bool _upgradeChosen;
	private bool _justStarted;

	public override void _Ready()
	{
		ProjectilesParent = _projectilesParent;
		_readyForNextWave = true;
		Difficulty = 0;

		_upgrade1.OnUpgrade += ChooseUpgrade;
		_upgrade2.OnUpgrade += ChooseUpgrade;
		_upgrade3.OnUpgrade += ChooseUpgrade;
	}

	public override void _Process(double delta)
	{
		if (!IsNodeReady()) return;
		if (GetChildCount() > 0) return;
		
		if (_waveOn != 0 && _readyForNextWave && !_justStarted)
		{
			_readyForNextWave = false;
			_justStarted = true;
			StartNextWave();
		}

		if (!_readyForNextWave) return;

		var enemies = new PackedScene[GD.RandRange(Mathf.Max(_waveOn + 1, 1), Mathf.Max(_waveOn + 2, _waveOn * 2))];
		GD.Print("Enemies Count: " + enemies.Length);
		
		for (var i = 0; i < enemies.Length; i++)
		{
			enemies[i] = _enemyTypes[GD.RandRange(0, _enemyTypes.Length - 1)];
		}
		
		new EnemyWave(enemies, -MathF.Log10(10 * (_waveOn + 1)) + 3)
			.Spawn(_player, this);
		
		_waveOn++;
		Difficulty++;
		
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
		
		_upgradeUI.TweenModulateAlpha(1, 0.5f)
			.Play();
		
		_upgrade1.Init(RandUpgrade());
		_upgrade2.Init(RandUpgrade());
		_upgrade3.Init(RandUpgrade());

		await TaskHelper.WaitUntil(() => _upgradeChosen);
		
		_readyForNextWave = true;
		_upgradeChosen = false;
	}

	private async void ChooseUpgrade(Upgrade upgrade)
	{
		upgrade.OnUpgrade(
			Difficulty, f => Difficulty = f,
			_player._fireRate, f => _player._fireRate = f,
			_player.Health, i => _player.Health = i,
			_player._damage, i => _player._damage = i,
			_player._speed, f => _player._speed = f
		);

		var tween = _upgradeUI.TweenModulateAlpha(0, 0.5f);
		tween.Play();

		await TaskHelper.WaitUntil(() => tween.IsCompleted);
		
		_upgradeChosen = true;
	}

	private Upgrade RandUpgrade()
	{
		return _upgradesAvailable[GD.RandRange(0, _upgradesAvailable.Length - 1)];
	}
}
