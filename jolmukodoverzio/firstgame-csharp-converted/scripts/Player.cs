using Godot;

// Player movement + bottle counter + respawn
public partial class Player : CharacterBody2D
{
	private const float Speed = 200.0f;
	private const float JumpVelocity = -300.0f;

	// If the player's global Y goes above this value, we respawn.
	[Export] public float FallLimit { get; set; } = 1200.0f;

	private AnimatedSprite2D _animatedSprite2D;
	private Vector2 _spawnPos;

	// Bottle count the player currently has.
	public int Bottles { get; private set; } = 0;

	private Hud _hud;

	public override void _Ready()
	{
		_spawnPos = GlobalPosition;
		_animatedSprite2D = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		_hud = GetTree().GetFirstNodeInGroup("hud") as Hud;

		// Initialize HUD if present.
		_hud?.SetBottles(Bottles);
	}

	public override void _PhysicsProcess(double delta)
	{
		var d = (float)delta;

		// Fall check (no collision needed)
		if (GlobalPosition.Y > FallLimit)
		{
			DieAndRestart();
			return;
		}


		// Gravity
		if (!IsOnFloor())
		{
			Velocity += GetGravity() * d;
			if (_animatedSprite2D != null)
				_animatedSprite2D.Animation = "jump";
		}

		// Jump
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			Velocity = new Vector2(Velocity.X, JumpVelocity);

		// Horizontal movement
		float direction = Input.GetAxis("left", "right");
		if (!Mathf.IsZeroApprox(direction))
		{
			Velocity = new Vector2(direction * Speed, Velocity.Y);
			if (_animatedSprite2D != null)
				_animatedSprite2D.Animation = "run";
		}
		else
		{
			Velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Velocity.Y);
			if (_animatedSprite2D != null)
				_animatedSprite2D.Animation = "idle";
		}

		if (_animatedSprite2D != null)
		{
			if (direction > 0)
				_animatedSprite2D.FlipH = false;
			else if (direction < 0)
				_animatedSprite2D.FlipH = true;
		}

		MoveAndSlide();
	}

	public void Respawn()
	{
		GlobalPosition = _spawnPos;
		Velocity = Vector2.Zero;
	}

	public void CollectBottle()
	{
		Bottles += 1;
		_hud?.SetBottles(Bottles);
		GD.Print($"Üvegek: {Bottles}");
	}
	public void DieAndRestart()
	{
	// üvegek elvesznek
	Bottles = 0;
	_hud?.SetBottles(Bottles);

	// visszaspawnol minden (üvegek is)
	GetTree().ReloadCurrentScene();
	}
}
