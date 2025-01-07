namespace GameBrain;

public class TicTacTwoBrain
{

    public GameConfiguration GameConfiguration;
    public GameState _gameState;
    public (int x, int y) _currentGridTopLeft;
    public int _xPiecesPlaced = 0;
    public int _oPiecesPlaced = 0;
    
    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        var gameBoard = new EGamePiece[gameConfiguration.BoardSizeWidth][];
        
        for (int x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[gameConfiguration.BoardSizeHeight];
        }
        
        _currentGridTopLeft = (gameConfiguration.InitialGridTopLeftX, gameConfiguration.InitialGridTopLeftY);
        _gameState = new GameState(gameBoard, gameConfiguration);
        GameConfiguration = gameConfiguration;
    }

    public string GetGameStateJson()
    {
        return _gameState.ToString();
    }

    public string GetConfigName()
    {
        return _gameState.GameConfiguration.Name;
    }
    
    public void SetPasswords(string x, string o)
    {
        _gameState.OPass = o;
        _gameState.XPass = x;
    }

    public void SetName(string name)
    {
        _gameState.Name = name;
    }
    
    public void SetAi(bool b)
    {
        _gameState.Ai = b;
    }

    public EGamePiece[][] GameBoard
    {
        get => GetBoard();
        set => _gameState.GameBoard = value;
    }

    public int DimX => _gameState.GameBoard.Length;
    public int DimY => _gameState.GameBoard[0].Length;

    private EGamePiece[][] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameState.GameBoard.GetLength(0)][];
            //, _gameState.GameBoard.GetLength(1)];
        for (var x = 0; x < _gameState.GameBoard.Length; x++)
        {
            copyOfBoard[x] = new EGamePiece[_gameState.GameBoard[x].Length];
            
            for (var y = 0; y < _gameState.GameBoard[x].Length; y++)
            {
                copyOfBoard[x][y] = _gameState.GameBoard[x][y];
            }
        }

        return copyOfBoard;
    }


    public bool MakeAMove(int x, int y)
    {
        if (x > _gameState.GameConfiguration.BoardSizeWidth - 1 || x < 0 || y > _gameState.GameConfiguration.BoardSizeHeight - 1 || y < 0)
        {
            Console.WriteLine("Cannot place piece outside of board");
            return false;
        }
        if ((_gameState.CurrentPlayer == EGamePiece.X && _xPiecesPlaced == _gameState.GameConfiguration.MaxGamePieces) || 
            (_gameState.CurrentPlayer == EGamePiece.O && _oPiecesPlaced == _gameState.GameConfiguration.MaxGamePieces))
        {
            Console.WriteLine("Cannot place a piece because there are no pieces left");
            return false;
        }
        if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
        {
            return false;
        }
        
        _gameState.GameBoard[x][y] = _gameState.CurrentPlayer;
        if (_gameState.CurrentPlayer == EGamePiece.X) _xPiecesPlaced++;
        else _oPiecesPlaced++;
        
        if (CheckWin())
        {
            return true;
        }
        
        _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }
    
    
    public bool MovePiece(int fromX, int fromY, int toX, int toY)
    {
        if (toX > _gameState.GameConfiguration.BoardSizeWidth - 1 || toX < 0 || toY > _gameState.GameConfiguration.BoardSizeHeight - 1 || toY < 0)
        {
            Console.WriteLine("Cannot move piece outside of board");
            return false;
        }
        if (_xPiecesPlaced == 0 || _oPiecesPlaced == 0)
        {
            Console.WriteLine("Cannot move a piece because there are no pieces to move!");
            return false;
        }
        if (_gameState.GameBoard[fromX][fromY] == _gameState.CurrentPlayer &&
            _gameState.GameBoard[toX][toY] == EGamePiece.Empty)
        {
            _gameState.GameBoard[toX][toY] = _gameState.CurrentPlayer;
            _gameState.GameBoard[fromX][fromY] = EGamePiece.Empty;
            
            if (CheckWin())
            {
                return true;
            }
            
            _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
            return true;
        }
        return false;
    }
    
    
    public bool ShiftGrid(int newTopLeftX, int newTopLeftY)
    {
        if (newTopLeftX + _gameState.GameConfiguration.GridSize >= _gameState.GameConfiguration.BoardSizeWidth + 1 ||
            newTopLeftY + _gameState.GameConfiguration.GridSize >= _gameState.GameConfiguration.BoardSizeHeight + 1 ||
            newTopLeftX < 0 || newTopLeftY < 0)
        {
            Console.WriteLine("invalid grid position");
            return false;
        }
        _currentGridTopLeft = (newTopLeftX, newTopLeftY);
        
        if (CheckWin())
        {
            return true;
        }
        
        _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }
    
    
    public bool CheckWin()
    {
        int winCount = _gameState.GameConfiguration.WinCondition;

        // Check rows
        for (int y = _currentGridTopLeft.y; y < DimY; y++)
        {
            for (int x = _currentGridTopLeft.x; x <= DimX - winCount; x++)
            {
                if (CheckDirection(x, y, 1, 0, winCount))
                {
                    return true;
                }
            }
        }

        // Check columns
        for (int x = _currentGridTopLeft.x; x < DimX; x++)
        {
            for (int y = _currentGridTopLeft.y; y <= DimY - winCount; y++)
            {
                if (CheckDirection(x, y, 0, 1, winCount))
                {
                    return true;
                }
            }
        }

        // Check diagonals (top-left to bottom-right)
        for (int x = _currentGridTopLeft.x; x <= DimX - winCount; x++)
        {
            for (int y = _currentGridTopLeft.y; y <= DimY - winCount; y++)
            {
                if (CheckDirection(x, y, 1, 1, winCount))
                {
                    return true;
                }
            }
        }

        // Check diagonals (bottom-left to top-right)
        for (int x = _currentGridTopLeft.x; x <= DimX - winCount; x++)
        {
            for (int y = _currentGridTopLeft.y + winCount - 1; y < DimY; y++)
            {
                if (CheckDirection(x, y, 1, -1, winCount))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckDirection(int startX, int startY, int dx, int dy, int winCount)
    {
        EGamePiece startPiece = GameBoard[startX][startY];
        if (startPiece == EGamePiece.Empty)
        {
            return false;
        }

        for (int i = 1; i < winCount; i++)
        {
            int nextX = startX + i * dx;
            int nextY = startY + i * dy;
            if (nextX >= DimX || nextY >= DimY || nextY < 0 || GameBoard[nextX][nextY] != startPiece)
            {
                return false;
            }
        }
        return true;
    }
    

    public void ResetGame()
    {
        var gameBoard = new EGamePiece[_gameState.GameConfiguration.BoardSizeWidth][];
        for (int x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[_gameState.GameConfiguration.BoardSizeHeight];
        }
        
        _gameState.GameBoard = gameBoard;
        _gameState.CurrentPlayer = EGamePiece.X;
    }
    
    public void SetGameStateJson(string gameState)
    {
        _gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(gameState) ?? throw new InvalidOperationException();
    }

}