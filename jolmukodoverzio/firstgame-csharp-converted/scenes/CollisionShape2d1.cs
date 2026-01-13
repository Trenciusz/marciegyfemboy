using Godot;

// Same as CollisionShape2d but kept as a separate script to preserve the project structure.
public partial class CollisionShape2d1 : CollisionShape2D
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
