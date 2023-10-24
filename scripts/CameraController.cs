using Godot;
using GTweens.Builders;
using GTweens.Easings;
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

	public static void Shake(Vector2 dir, float intensity, float zoom = 0)
	{
		GTweenSequenceBuilder.New()
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 6f, 0.05f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 1f, 0.05f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 3f, 0.08f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 0.5f, 0.08f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 1.5f, 0.1f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 0.25f, 0.1f))
			.Append(_instance.TweenOffset(_instance._currentPos + dir * intensity * 0.75f, 0.1f))
			.Append(_instance.TweenOffset(_instance._currentPos, 0.1f).SetEasing(Easing.OutCubic))
			.Build()
			.Play();

		if (zoom > 0)
		{
			_instance.TweenZoom(new Vector2(zoom, zoom), 0.1f)
				.SetEasing(Easing.OutExpo)
				.OnComplete(() =>
				{
					_instance.TweenZoom(new Vector2(1, 1), 0.05f)
						.SetEasing(Easing.InExpo)
						.Play();
				})
				.Play();
		}
	}
}
