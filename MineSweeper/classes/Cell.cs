using Godot;
using System;
using static Extensions;
public class Cell : Node2D
{

    private int _x;
    private int _y;
    private Vector2 _size;
    private int _countMines;
    public int CountMines{
        get{return _countMines;}
    }
    private bool _isMine;
    private Vector2 _gridSize;
    public bool IsMine{
        get { return _isMine ; }
        set { SetIsMine(value);}
    }
    private bool _isFlagged;
    private Grid _parent;
    private bool _isRevealed;
    public bool IsRevealed{
        get{return _isRevealed;}
        set{_isRevealed = true;
            _cellNeighborhood.Visible = true;
            _cellTexture.Frame = (_isMine)?2:1;}
    }
    private ColorRect _colorCell;
    private Sprite _cellTexture;
    private RichTextLabel _cellNeighborhood;
    public void _Initialize(int _x, int _y, int _cellSize,Vector2 _gridSize)
    {
        this._x = _x;
        this._y = _y;
        _size = _cellTexture.Texture.GetSize();
        this._gridSize = _gridSize;
        Position = new Vector2(_x * _cellSize, _y * _cellSize);
        _isRevealed = false;
        _isMine = false;
        _isFlagged = false;
        _countMines = 0;
        var nbCellMax = _gridSize.x * _gridSize.y ;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _parent = GetParent<Grid>();
        var hitBox = GetNode<Area2D>("HitBox");
        hitBox.Connect("input_event",this,nameof(OnHitBoxClick));
        _cellTexture = GetNode<Sprite>("Sprite");
        _cellNeighborhood =  GetNode<RichTextLabel>("NumberOfMines");
    }

    public void OnHitBoxClick(Viewport node, InputEvent inputEvent, int shapeIdx){
        if (inputEvent is InputEventMouseButton mouseEvent ){
            if( (ButtonList)mouseEvent.ButtonIndex == ButtonList.Left && ! mouseEvent.Pressed && !_isFlagged){
                if(! _parent.GetParent<Main>().IsPaused){
                    if (!RevealCell())
                    {
                        _parent.GameOver();
                    }
                    else{
                        _parent.CheckVictory();
                    }
                }
            }
            else if( (ButtonList)mouseEvent.ButtonIndex == ButtonList.Right && ! mouseEvent.Pressed  && !IsRevealed){
                if(! _parent.GetParent<Main>().IsPaused){
                    _isFlagged = ! _isFlagged;
                    _cellTexture.Frame = (_isFlagged)?3:0;
                }
            }
        }
    }

    public void CountMinesNeighborhoud()
    {
        if(_isMine){
            this._countMines = -1;
            return;
        }
        
        for (int i=-1; i<=1; i++){
            for (int j=-1 ; j<=1;j++){
                if( IsCoordinatesValids(_x+i,_y+j) && _parent.GetCellByCoordinates(_x+i,_y+j)._isMine)
                {
                    this._countMines ++;
                }
            }
        }
        _cellNeighborhood.Text = (_countMines>0)?_countMines.ToString():"";
    }

    private bool IsCoordinatesValids(int x, int y){
        return (
                x >=0 && x < _gridSize.x && 
                y >=0 && y < _gridSize.y && 
                 ! (this._x == x && this._y == y)
                );
    }

    public void SetIsMine(bool _isMine){
        this._isMine = _isMine;
    }

    public bool RevealCell()
    {
        
        if(_countMines == -1){
            IsRevealed = true;
            _cellTexture.Frame = 2;
            return false;
        }
        if(_countMines == 0){
            _parent.Flood(_x,_y);
        }
        else{
            IsRevealed = true;
            _cellNeighborhood.Visible = true;
            
        }
        return true;
    }

    public override string ToString()
    {
        return $"Cellule ({_x} ; {_y}) // isMine = {_isMine}, isRevealed = {_isRevealed}, mines autours = {_countMines}";
    }
}
