using Godot;
using System.Threading.Tasks;

public partial class Npc : Area2D
{
	[Export] public int RequiredBottles { get; set; } = 7;
	[Export] public string EndScenePath { get; set; } = "res://scenes/game_over.tscn";

	private Player _playerRef;
	private bool _endingStarted = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player p)
			_playerRef = p;
	}

	private void OnBodyExited(Node body)
	{
		if (body == _playerRef)
			_playerRef = null;
	}

	public override void _Process(double delta)
	{
		if (_playerRef == null || _endingStarted)
			return;

		if (Input.IsActionJustPressed("interact"))
		{
			var hud = GetTree().GetFirstNodeInGroup("hud") as Hud;
			if (hud == null)
			{
				GD.Print("HIBA: HUD nincs 'hud' groupban!");
				return;
			}

			if (_playerRef.Bottles >= RequiredBottles)
			{
				hud.ShowMessage("Vége a játéknak!", 2.0f);
				_endingStarted = true;
				_ = GoToEndSceneAfterDelay(2.0);
			}
			else
			{
				hud.ShowMessage($"Nincs elég üveg! ({_playerRef.Bottles}/{RequiredBottles})", 2.5f);
			}
		}
	}

	private async Task GoToEndSceneAfterDelay(double seconds)
	{
		await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
		if (!string.IsNullOrEmpty(EndScenePath))
			GetTree().ChangeSceneToFile(EndScenePath);
	}
}
