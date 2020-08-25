using Godot;
using System;

public class Button : TextureButton
{
    [Export]
    private string _textLabel;
    private Label _textDisplay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("button_down",this,nameof(OnButtonDown));
        this.Connect("button_up",this,nameof(OnButtonUp));

        _textDisplay = GetNode<Label>("Label");
        _textDisplay.Text = _textLabel;
        _textDisplay.RectSize = RectSize + new Vector2(0,-7);
    }
    public void OnButtonDown(){
        RectSize += new Vector2(10,0);
        RectPosition -= new Vector2(5,0);
        _textDisplay.RectPosition += new Vector2(5,4);
    }
    public void OnButtonUp(){
        RectSize -= new Vector2(10,0);
        RectPosition += new Vector2(5,0);
        _textDisplay.RectPosition -= new Vector2(5,4);
        
    }
}
