using Godot;
using System;

public class Menu : Control
{

    private Main _main;
    private HSlider _editNbCols;
    private HSlider _editNbRows;
    private HSlider _editNbMines;

    private Label _lblNbCols;
    private Label _lblNbRows;
    private Label _lblNbMines;

    [Signal]
    public delegate void Restarted(int nbCols,int nbRows,int nbMines);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _main = GetParent<UserInterface>().GetParent<Main>();
        _editNbCols = GetNode<HSlider>("editNbCols");
        _editNbRows = GetNode<HSlider>("editNbRows");
        _editNbMines = GetNode<HSlider>("editNbMines");

        _editNbCols.Value = _main.NbCols;
        _editNbRows.Value = _main.NbRows;
        _editNbMines.Value = _main.NbMinesMax;


        _editNbCols.Connect("value_changed",this,nameof(OnSlidersValueChange));
        _editNbRows.Connect("value_changed",this,nameof(OnSlidersValueChange));
        _editNbMines.Connect("value_changed",this,nameof(OnSlidersValueChange));

        _lblNbCols = GetNode<Label>("NbCols");
        _lblNbRows = GetNode<Label>("NbRows");
        _lblNbMines = GetNode<Label>("NbMines");
        _lblNbCols.Text = _main.NbCols.ToString();
        _lblNbRows.Text = _main.NbRows.ToString();
        _lblNbMines.Text = _main.NbMinesMax.ToString();

        GetNode<TextureButton>("RestartButton").Connect("pressed",this,nameof(OnRestartButtonClick));
        
    }

    private void OnRestartButtonClick(){
        var nbCols = _editNbCols.Value;
        var nbRows = _editNbRows.Value;
        var nbMines = _editNbMines.Value;
        EmitSignal(nameof(Restarted),nbCols,nbRows,nbMines);
    }

    private void OnSlidersValueChange(float value){
        _lblNbCols.Text = _editNbCols.Value.ToString();
        _lblNbRows.Text = _editNbRows.Value.ToString();
        _lblNbMines.Text = _editNbMines.Value.ToString();
    }
}
