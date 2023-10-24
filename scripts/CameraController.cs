using Godot;

namespace Polyblast.scripts;

public partial class CameraController : Camera2D
{
	private static CameraController _instance;
	
	private Vector2 _currentPos;
	private bool _shake;
	private float _shakeIntensity;
	private float _shakeTime;
	private float _elapsedTime;
	
	public override void _Ready()
	{
		_currentPos = Offset;
		_instance = this;
	}

	public override void _Process(double delta)
	{
		if (!_shake) return;
		if (_elapsedTime < _shakeTime)
		{
			Offset = new Vector2(GD.Randf(), GD.Randf()) * _shakeIntensity;
			_elapsedTime += (float) delta;
		}
		else
		{
			_shake = false;
			_elapsedTime = 0;
			Offset = _currentPos;
		}
	}

	public static void Shake(float intensity, float duration)
	{
		_instance._shake = true;
		_instance._shakeIntensity = intensity;
		_instance._shakeTime = duration;
	}
}
