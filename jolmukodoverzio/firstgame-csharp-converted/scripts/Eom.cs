using Godot;

public partial class Eom : Area2D
{
	private Timer _timer;
	private Player _player;     // melyik player lépett bele
	private bool _triggered = false;

	public override void _Ready()
	{
		_timer = GetNodeOrNull<Timer>("Timer");

		BodyEntered += OnBodyEntered;

		if (_timer != null)
			_timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node body)
	{
		// ✅ csak a Player indítsa el
		if (body is not Player player)
			return;

		// ne induljon el többször
		if (_triggered)
			return;

		_triggered = true;
		_player = player;

		// slow motion
		Engine.TimeScale = 0.5f;

		// timer után halál+reset+reload
		if (_timer != null)
			_timer.Start();
		else
			OnTimerTimeout(); // ha nincs Timer node, azonnal
	}

	private void OnTimerTimeout()
	{
		// vissza normál sebességre
		Engine.TimeScale = 1.0f;

		// üvegek reset + scene reload (üvegek újra spawnolnak)
		_player?.DieAndRestart();
	}
}
