using Godot;

namespace Polyblast.scripts;

public partial class HPUpdater : HBoxContainer
{
	[Export] private Control _heart1;
	[Export] private Control _heart2;
	[Export] private Control _heart3;
	
	public override void _Ready()
	{
		Player.PlayerHealthChanged += UpdateUI;
	}

	public override void _ExitTree()
	{
		Player.PlayerHealthChanged -= UpdateUI;
	}

	private void UpdateUI(int newHealth)
	{
		switch (newHealth)
		{
			// bad code but works for now, dont do this kids
			case 2:
				_heart3.QueueFree();
				break;
			case 1:
				_heart2.QueueFree();
				break;
		}
	}
}
