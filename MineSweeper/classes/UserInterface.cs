using Godot;
using System;

public class UserInterface : CanvasLayer
{
    private Vector2 _viewportSize;
    private int _seconds;
    public int Seconds{
        set{_seconds = value;}
    }
    private NinePatchRect _background;
    private Label _timeDisplay;
    private Label _mineDisplay;
    public Label MineDisplay{
        get{return _mineDisplay;}
    }
    private TextureButton _menuButton;
    public TextureButton MenuButton{
        get{return _menuButton;}
    }
    private TextureButton _reloadButton;
    public TextureButton ReloadButton{
        get{return _reloadButton;}
    }

    private Menu _menu;
    public Menu Menu{
        get{return _menu;}
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _seconds = 0;
        _background = GetNode<NinePatchRect>("Background");
        _menuButton = _background.GetNode<TextureButton>("MenuButton");

        _reloadButton = _background.GetNode<TextureButton>("ReloadButton");
        
        _timeDisplay = _background.GetNode<Label>("TimeDisplay");
        _mineDisplay = _background.GetNode<Label>("NbMineDisplay");

        _background.GetNode<Timer>("Timer").Connect("timeout",this,nameof(OnSecondTimeout));
        
        _menu = GetNode<Menu>("Menu");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _viewportSize = GetViewport().Size;        
        _background.RectPosition = new Vector2(_viewportSize.x /2 - _background.RectSize.x /2,0);
        _menu.RectPosition = new Vector2(_viewportSize.x/2-_menu.RectSize.x /2, _viewportSize.y/2 - _menu.RectSize.y /2);
        if(!GetParent<Main>().IsPaused){
            _timeDisplay.Text = $"{_seconds} S";
        }
    }

    public void OnSecondTimeout(){
        _seconds++;
    }


}
