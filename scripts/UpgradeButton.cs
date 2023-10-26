using System;
using Godot;

namespace Polyblast.scripts;

public partial class UpgradeButton : Button
{
	public event Action<Upgrade> OnUpgrade;
	private Upgrade _upgrade;

	[Export] private Label _title;
	[Export] private Label _description;

	public void Init(Upgrade upgrade)
	{
		_upgrade = upgrade;
		_title.Text = upgrade.Title;
		_description.Text = upgrade.Description;
	}
	
	public override void _Ready()
	{
		Pressed += () => OnUpgrade?.Invoke(_upgrade);
	}
}
