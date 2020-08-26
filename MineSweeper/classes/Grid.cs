using Godot;
using System;
using static Extensions;

public class Grid : Node2D
{
    // variables
    private Cell[,] _grid;
    private int _nbRows;
    private int _nbCols;
    private ColorRect _background;
    // exports variables
    [Export]
    private string _sceneCellPath;
    private int _cellSize;
    private int _nbMinesMax;
    private Main parent;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        parent = GetParent<Main>();
        Initialize();
        // parent.MainCamera = parent.GetNode<Camera2D>("MainCamera");
    }

    public void Flood(int x, int y){
        if(x >=0 && x < _nbRows && y >= 0 && y < _nbCols && !_grid[x,y].IsMine && !_grid[x,y].IsRevealed)
        {
            if(_grid[x,y].CountMines > 0){
                _grid[x,y].IsRevealed = true;
            }else
            {
                _grid[x,y].IsRevealed = true;
                Flood(x-1,y);
                Flood(x+1,y);
                Flood(x,y-1);
                Flood(x,y+1);
            }

        }
        
    }
    public Cell GetCellByCoordinates(int x, int y){

        return _grid[x,y];
    }

    public void GameOver(){
        GD.Print("GameOver");
        foreach (Cell cell in _grid)
        {
            if(cell.IsMine)
            {
               cell.IsRevealed = true;
            }
        }
        GetParent<Main>().IsPaused = true;
    }
    public void CheckVictory(){
        GD.Print("Checking");
        foreach (Cell cell in _grid)
        {
            if(!cell.IsRevealed && ! cell.IsMine)
            {
                return;
            }
        }
        // Victoire
        GD.Print("Victoire");
        GetParent<Main>().IsPaused = true;
    }
    public void Restart(){
        Initialize();
    }

    private void Initialize(){

        _nbCols = parent.NbCols;
        _nbRows = parent.NbRows;
        _nbMinesMax = parent.NbMinesMax;
        _cellSize = parent.CellSize;
        if (_grid != null &&_grid.Length != 0){
            foreach(Cell cell in _grid){
                this.RemoveChild(cell);
            }
            Array.Clear(_grid,0,_grid.Length-1);
        }else{
            _grid = new Cell[_nbRows,_nbCols];
        }
        _background = GetNode<ColorRect>("Background");
        for (int i = 0; i < _nbRows; i++)
        {
            for (int j = 0; j < _nbCols; j++)
            {
                _grid[i,j] =SmartLoader<Cell>(_sceneCellPath);
                AddChild(_grid[i,j]);
                _grid[i,j]._Initialize(i,j, _cellSize,new Vector2(_nbRows,_nbCols));
            }
        }
        
        for(int nbMines=0 ; nbMines<_nbMinesMax; nbMines++){
            Random rng = new Random();
            int x,y;
            do
            {
                x = rng.Next(0,_nbRows);
                y = rng.Next(0,_nbCols);
            } while(_grid[x,y].IsMine);
            _grid[x,y].IsMine = true;
        }

        for (int i = 0; i < _nbRows; i++)
        {
            for (int j = 0; j < _nbCols; j++)
            {
                _grid[i,j].CountMinesNeighborhoud();
            }
        }

        _background.RectPosition = _grid[0,0].GetNode<Sprite>("Sprite").GlobalPosition - new Vector2(_cellSize/2,_cellSize/2);
        _background.RectSize = new Vector2(_cellSize*_nbRows, _cellSize*_nbCols);
    }
}
