using Godot;

public partial class Hud : CanvasLayer
{
	private Label _bottlesLabel;
	private Label _messageLabel;

	private float _messageTimer = 0.0f;

	public override void _Ready()
	{
		_bottlesLabel = GetNodeOrNull<Label>("Label");
		_messageLabel = GetNodeOrNull<Label>("MessageLabel");

		if (_messageLabel != null)
			_messageLabel.Text = "";
	}

	// Updates "Üvegek" + "Forint" (bottles * 50)
	public void SetBottles(int count)
	{
		int forint = count * 50;
		if (_bottlesLabel != null)
			_bottlesLabel.Text = $"Üvegek: {count}  |  Forint: {forint}";
	}

	public void ShowMessage(string text, float seconds = 2.0f)
	{
		if (_messageLabel == null)
			return;

		_messageLabel.Text = text;
		_messageTimer = seconds;
	}

}
