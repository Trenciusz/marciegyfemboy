using Godot;

// Disables the platform collider for a moment when pressing "down".
public partial class CollisionShape2d : CollisionShape2D
{
	private Timer _timer;

	public override void _Ready()
	{
		_timer = GetNodeOrNull<Timer>("Timer");
		if (_timer != null)
			_timer.Timeout += OnTimerTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("down"))
		{
			Disabled = true;
			_timer?.Start();
		}
	}

	private void OnTimerTimeout()
	{
		Disabled = false;
	}
}
