using Godot;

public partial class Bottle : Area2D
{
	private bool _playerInside = false;
	private Player _playerRef;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player p)
		{
			_playerInside = true;
			_playerRef = p;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body == _playerRef)
		{
			_playerInside = false;
			_playerRef = null;
		}
	}
	public override void _Process(double delta)
    {
        if (_playerInside && Input.IsActionJustPressed("interact") && _playerRef != null)
        {
            _playerRef.CollectBottle();
            QueueFree();
        }
    }
}
