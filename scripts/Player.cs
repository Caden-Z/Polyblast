using System;
using Godot;
using GTweens.Easings;
using GTweens.Extensions;
using GTweensGodot.Extensions;

namespace Polyblast.scripts;

public partial class Player : GameMovableCharacter
{
	public static event Action<int> PlayerHealthChanged;

	[Export] private AudioStreamPlayer2D _soundEffectPlayer;
	[Export] private AudioStream _shootSfx;
	[Export] private AudioStream _hurtSfx;

	[Export] public float _fireRate;
	[Export] public int _damage;

	private float _timeElapsed;

	public override void _PhysicsProcess(double delta)
	{
		var inputVelocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		MoveCharacter(inputVelocity);

		RotationDegrees += Mathf.Max(
			(float)delta * Velocity.LengthSquared() * 0.0005f * GameTimeScale.TimeScale,
			(float)delta * 30 * GameTimeScale.TimeScale
		);
	}

	public override void _Process(double delta)
	{
		_timeElapsed += (float)delta * GameTimeScale.TimeScale;

		if (_timeElapsed > _fireRate && Input.IsActionPressed("attack"))
		{
			_timeElapsed = 0;
			SimpleProjectile.Spawn(GlobalPosition, GetGlobalMousePosition() - GlobalPosition, 10, _damage,
				CollideType.Enemy, color: Colors.LightCyan);

			// this can be abstracted away, but i am doing this for the sake of saving some time
			_soundEffectPlayer.Stream = _shootSfx;
			_soundEffectPlayer.Play();
		}
	}

	protected override void OnHealthChanged(int newHealth)
	{
		PlayerHealthChanged?.Invoke(newHealth);
		_soundEffectPlayer.Stream = _hurtSfx;
		_soundEffectPlayer.Play();

		if (newHealth == 0)
		{
			GTweenExtensions.Tween(
					() => GameTimeScale.TimeScale,
					x => GameTimeScale.TimeScale = x,
					0,
					1f
				)
				.SetEasing(Easing.OutCubic)
				.OnComplete(() =>
				{
					GameTimeScale.TimeScale = 1;
					GetTree().ReloadCurrentScene();
				})
				.Play();
		}
	}
}
