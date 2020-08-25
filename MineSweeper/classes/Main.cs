using Godot;
using System;
using static Extensions;
public class Main : Node2D
{
    private Camera2D _mainCamera;
    public Camera2D MainCamera{
        get{return _mainCamera;}
        set{_mainCamera = value;}
    }
    
    private bool _isPaused = false;
    public bool IsPaused{
        get{return _isPaused;}
        set{_isPaused = value;}
    }
    [Export]
    private int _nbRows;
    public int NbRows{
        get{return _nbRows;}
        set{_nbRows = value;}
    }
    [Export]
    private int _nbCols;
    public int NbCols{
        get{return _nbCols;}
        set{_nbCols = value;}
    }
    [Export]
    private int _cellSize;
    public int CellSize{
        get{return _cellSize;}
    }
    [Export]
    private int _nbMinesMax;
    public int NbMinesMax{
        get{return _nbMinesMax;}
        set{_nbMinesMax = value;}
    }
    public override void _Ready()
    {        
        var userInterface = GetNode<UserInterface>("UserInterface");
        userInterface.MenuButton.Connect("pressed",this,nameof(OnMenuButtonClick));
        userInterface.ReloadButton.Connect("pressed",this,nameof(OnReloadButtonClick));

        userInterface.MineDisplay.Text = _nbMinesMax.ToString();
        _mainCamera = GetNode<Camera2D>("MainCamera");

    }
    public override void _Process(float _delta)
    {   
        _mainCamera.Position = new Vector2((_nbRows/2) * _cellSize,(_nbCols/2) * _cellSize);
    }

    public void OnMenuButtonClick(){
        // smartload menu scene
    }

    public void OnReloadButtonClick(){
        // smartload menu scene
        Reload();
    }
    public void Reload(){
        GD.Print("reloaded");
        IsPaused = false;

        var previousGrid = this.FindChildrenOfType<Grid>()[0];
        previousGrid.Restart();

        var UI = this.FindChildrenOfType<UserInterface>()[0];
        UI.MineDisplay.Text = NbMinesMax.ToString();
        UI.Seconds = 0;

    }
}
