namespace GameBrain;

public class TicTacTwoBrain
{
    //RULES:
    //SQUARE BOARD, ANY SIZE
    //GRID STARTS IN THE MIDDLE, ANY SIZE
    //ANY NUM PIECES EACH, DEFAULT 4
    //CAN CHANGE PIECE LOCATION AFTER 2 PIECES
    //

    private EGamePiece[,] _gameBoard;
    public EGamePiece _currentPlayer { get; set; } = EGamePiece.X;

    private GameConfiguration _gameConfiguration;
    public (int x, int y) _currentGridCenter;
    public int _xPiecesPlaced = 0;
    public int _oPiecesPlaced = 0;
    
    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
        _currentGridCenter = (_gameConfiguration.InitialGridCenterX, _gameConfiguration.InitialGridCenterY);
    }

    public EGamePiece[,] GameBoard
    {
        get => GetBoard();
        private set => _gameBoard = value;
    }

    public int DimX => _gameBoard.GetLength(0);
    public int DimY => _gameBoard.GetLength(1);

    private EGamePiece[,] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        for (var x = 0; x < _gameBoard.GetLength(0); x++)
        {
            for (var y = 0; y < _gameBoard.GetLength(1); y++)
            {
                copyOfBoard[x, y] = _gameBoard[x, y];
            }
        }

        return copyOfBoard;
    }


    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x, y] != EGamePiece.Empty)
        {
            return false;
        }
        
        _gameBoard[x, y] = _currentPlayer;
        if (_currentPlayer == EGamePiece.X) _xPiecesPlaced++;
        else _oPiecesPlaced++;
        
        if (CheckWin())
        {
            return true;
        }
        
        _currentPlayer = _currentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }
    
    
    public bool MovePiece(int fromX, int fromY, int toX, int toY)
    {
        if (_gameBoard[fromX, fromY] == _currentPlayer &&
            _gameBoard[toX, toY] == EGamePiece.Empty)
        {
            _gameBoard[toX, toY] = _currentPlayer;
            _gameBoard[fromX, fromY] = EGamePiece.Empty;
            
            if (CheckWin())
            {
                return true;
            }
            
            _currentPlayer = _currentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
            return true;
        }
        return false;
    }
    
    
    public bool ShiftGrid(int newCenterX, int newCenterY)
    {

        if (newCenterX is < 1 or > 3 || newCenterY is < 1 or > 3)
        {
            Console.WriteLine("Invalid grid coordinates");
            return false;
        }
        
        _currentGridCenter = (newCenterX, newCenterY);
        
        if (CheckWin())
        {
            return true;
        }
        
        _currentPlayer = _currentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }
    
    
    public bool CheckWin()
    {
        for (var x = _currentGridCenter.x - 1; x <= _currentGridCenter.x + 1; x++)
        {
            if (_gameBoard[x, _currentGridCenter.y - 1] == _currentPlayer &&
                _gameBoard[x, _currentGridCenter.y] == _currentPlayer &&
                _gameBoard[x, _currentGridCenter.y + 1] == _currentPlayer)
                return true;
        }

        for (var y = _currentGridCenter.y - 1; y <= _currentGridCenter.y + 1; y++)
        {
            if (_gameBoard[_currentGridCenter.x - 1, y] == _currentPlayer &&
                _gameBoard[_currentGridCenter.x, y] == _currentPlayer &&
                _gameBoard[_currentGridCenter.x + 1, y] == _currentPlayer)
                return true;
        }

        if (_gameBoard[_currentGridCenter.x - 1, _currentGridCenter.y - 1] == _currentPlayer &&
            _gameBoard[_currentGridCenter.x, _currentGridCenter.y] == _currentPlayer &&
            _gameBoard[_currentGridCenter.x + 1, _currentGridCenter.y + 1] == _currentPlayer)
            return true;

        if (_gameBoard[_currentGridCenter.x + 1, _currentGridCenter.y - 1] == _currentPlayer &&
            _gameBoard[_currentGridCenter.x, _currentGridCenter.y] == _currentPlayer &&
            _gameBoard[_currentGridCenter.x - 1, _currentGridCenter.y + 1] == _currentPlayer)
            return true;

        return false;
    }
    

    public void ResetGame()
    {
        _gameBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        _currentPlayer = EGamePiece.X;
    }
}