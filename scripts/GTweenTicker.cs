using Godot;
using GTweensGodot.Contexts;

namespace Polyblast.scripts;

public partial class GTweenTicker : Node
{
	public override void _Process(double delta)
	{
		GodotGTweensContextNode.Context.Tick((float)delta);
	}
}
