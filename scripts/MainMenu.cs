using Godot;

namespace Polyblast.scripts;

public partial class MainMenu : Control
{
	[Export] private PackedScene _gameScene;
	[Export] private Button _startGame;
	[Export] private Button _quit;

	public override void _Ready()
	{
		_startGame.Pressed += StartGame;
		_quit.Pressed += Quit;
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToPacked(_gameScene);
	}

	private void Quit()
	{
		GetTree().Quit();
	}
}
