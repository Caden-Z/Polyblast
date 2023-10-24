using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

namespace Polyblast.scripts;

public partial class CameraController : Camera2D
{
	private static CameraController _instance;
	private Vector2 _currentPos;

	public override void _Ready()
	{
		_currentPos = Offset;
		_instance = this;
	}

	public static void Shake(Vector2 dir, float intensity)
	{
		var tween = GTweenSequenceBuilder.New()
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 6, 0.05f))
			.Append(_instance.TweenOffset(_instance._currentPos - dir * intensity * 0.0025f, 0.05f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 3f, 0.08f))
			.Append(_instance.TweenOffset(_instance._currentPos - dir * intensity * 0.0025f, 0.08f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 1.5f, 0.1f))
			.Append(_instance.TweenOffset(_instance._currentPos - dir * intensity * 0.00125f, 0.1f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 0.75f, 0.1f))
			.Append(_instance.TweenOffset(_instance._currentPos - dir * intensity * 0.00125f, 0.1f))
			.Append(_instance.TweenOffset(_instance._currentPos, 0.1f))
			.Build();

		tween.Play();
	}
}
