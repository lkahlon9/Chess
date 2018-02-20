using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ChessGlobals;

//Shorten Declarations
using ListOfPieceTypesAndPositions =  System.Collections.Generic.List< ChessGlobals.Tuple2<ChessGlobals.PIECE_TYPES,UnityEngine.Vector2> >;

public class ChessGameController : MonoBehaviour 
{

	[SerializeField] private DrawBoard drawBoard;
	[SerializeField] private DrawPiece drawPiece;
	private MoveModel moveModel;
	public Text turnDisplay;

	private Board board;
	public Player whitePlayer;
	public Player blackPlayer;
	public Player activePlayer;
	ChessGlobals.GameState gameState;
	private List<Piece> pieces;
	private List<Piece> capturedPieces;
	// Initialize positions to spots the user cannot choose. I chose Vector3.down because it contains a negative value and be unity has a shorthand for it.
	private Vector3 movePieceFrom = Vector3.down;
	private Vector3 movePieceTo = Vector3.down;
	private Vector3 movePieceFromOld = Vector3.down;
	List<Vector2> legalMovesForAPiece;
	bool isPieceMove = false;
	Piece currentlySelectedPiece;


	private float startMoveTime = -1;
	private float moveTime = 0;
	private const int waitTime = 1;
	private const int negativeTime = -1;

	private Vector3 firstCameraPos;
	private Vector3 secondCameraPos;
	private float firstCameraRot;
	private float secondCameraRot;
	public ChessGameController()
	{
		legalMovesForAPiece = new List<Vector2> ();
		gameState = new GameState(ChessGlobals.GameState.WHITE_TURN);
		//turnDisplay = new Text ();
		//eventuall this constructor will be deleted but to allow for game to work as is it's needed
	}
	public ChessGameController(ChessGlobals.MODES mode)
	{
		int depth = 2;
		//pvp
		if (mode == ChessGlobals.MODES.PVP) 
		{

			Player blackPlayer = new Human(this);
			Player whitePlayer = new Human(this);
		}
		//pvc
		else if (mode == ChessGlobals.MODES.PVC) 
		{

			Player blackPlayer = new Human(this);
			Player whitePlayer = new AI(this,depth);
		}
		//cvc for debugging purposes, determining draw for Example
		else if (mode == ChessGlobals.MODES.CVC) 
		{

			Player blackPlayer = new AI(this, depth);
			Player whitePlayer = new AI(this, depth);
		}
			
	}
	// Use this for initialization
	void Start () 
	{
		legalMovesForAPiece = new List<Vector2> ();
		//drawBoard = gameObject.AddComponent<DrawBoard> ();
		//drawPiece = gameObject.AddComponent<DrawPiece> ();
		//Initialize Model
		board = new Board();
		pieces = new List<Piece> ();
		//Initialize Movement Model
		moveModel = gameObject.AddComponent<MoveModel> ();
		capturedPieces = new List<Piece> ();

		//Initialize View
		drawBoard.InitBoard();
		Tuple2<  ListOfPieceTypesAndPositions, ListOfPieceTypesAndPositions > startingPositions = drawPiece.InitPieces ();
		Assert.AreEqual(startingPositions.t1.Count,startingPositions.t2.Count);// this should never be violated
		for (int i = 0; i < startingPositions.t1.Count; ++i) 
		{
			Piece blackPiece = createPieceAt(startingPositions.t1[i].t1, startingPositions.t1[i].t2, ChessGlobals.Teams.BLACK_TEAM);
			Piece whitePiece = createPieceAt(startingPositions.t2[i].t1, startingPositions.t2[i].t2, ChessGlobals.Teams.WHITE_TEAM);
			//These two assertions should never be violated 
			Assert.AreNotEqual(blackPiece, null);
			Assert.AreNotEqual(whitePiece, null);

			//after all they represent the same things
			Assert.AreEqual (startingPositions.t1[i].t2, blackPiece.GetPiecePosition() );
			Assert.AreEqual (startingPositions.t2[i].t2, whitePiece.GetPiecePosition() );

			board.Mark(blackPiece.GetPiecePosition(), blackPiece);
			board.Mark(whitePiece.GetPiecePosition(), whitePiece);
			//board.ToString();

			pieces.Add (whitePiece);
			pieces.Add (blackPiece);
		}
		MonoBehaviour.print ("Before board test: "+ this.board.GetPieceAt (0, 0));
		//BoardTest.ValidateInitialPiecePositions(board);
		/*BoardTest.ValidateMark(board);
		BoardTest.ValidateUnMark(board);
		BoardTest.ValidateClear(board);
		BoardTest.ValidateGetPieceAt(board);*/

		//set camera
		firstCameraPos = Camera.main.transform.position;
		secondCameraPos = new Vector3 (firstCameraPos.x, firstCameraPos.y, firstCameraPos.z + 18);
		firstCameraRot = Camera.main.transform.eulerAngles.y;
		secondCameraRot = Camera.main.transform.eulerAngles.y + 180;
		//set turn text
		//turnDisplay.text = "White Turn";

	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.A)) 
		{
			startMoveTime = Time.time;
			print ("first cam pos: " + firstCameraPos);
			print ("second cam pos:" + secondCameraPos);
			switchTurnDisplay();
		}
		if (Time.time - moveTime > waitTime && isPieceMove == true) 
		{
			startMoveTime = Time.time;
			switchTurnDisplay();
			isPieceMove = false;
		}
		if (startMoveTime != negativeTime)
			SwitchCamera ();
		
		//handles moving a piece
		if ((DrawBoard.IsClicked || DrawPiece.IsClicked) && movePieceFrom != Vector3.down) 
		{
			MonoBehaviour.print ("Apples\n");
			//here must disallow moving pieces to anywhere other than a legal square
			if (legalMovesForAPiece != null)
			{
				movePieceTo = DrawBoard.IsClicked ? DrawBoard.SquarePosition : DrawPiece.PiecePosition;
				if (board.IsOccupied (movePieceTo)) 
				{
					//if the color of the piece matches the current turn
					if (board.GetPieceAt (movePieceTo).GetTeam () == gameState.getState())
					{
						movePieceTo = Vector3.down;
						movePieceFrom = Vector3.down;
						drawBoard.ClearHighlights ();
						return;
					}	
				}
				Vector2 chosenMove = movePieceTo;
				bool found = false;
				foreach (Vector2 pos in legalMovesForAPiece) 
				{
					if (pos == chosenMove) 
					{
						found = true;
						break;
					}
				}
				if (found == false)
				{
					movePieceTo = Vector3.down;
				} 
			}
		}
		//handles clicking a piece
		else if (DrawPiece.IsClicked)
		{
			selectPiece ();
		} 
		// If the user has clicked on a space to move and a piece to move, move the piece and reset the vectors to numbers the user cannot choose.
		// In the real game we would also have to check if it is a valid move.
		if (movePieceFrom != Vector3.down && movePieceTo != Vector3.down)
		{
			moveModel.MovePiece(movePieceFrom, movePieceTo);
			board.Mark (movePieceTo, currentlySelectedPiece);
			board.UnMark (movePieceFrom);
			movePieceTo = movePieceFrom = Vector3.down;
			drawBoard.ClearHighlights();
			moveTime = Time.time;
			isPieceMove = true;
		}
	}
	private void selectPiece()
	{
		movePieceFrom = DrawBoard.IsClicked ? DrawBoard.SquarePosition : DrawPiece.PiecePosition;
		drawBoard.HighLightGrid (movePieceFrom);
		MonoBehaviour.print ("Bananas\n");
		/*Ari movement highlight Will go here
			 * For now can determine which piece is click by board position BUT this depends on board being consistent with drawboard,
			 * is there a better way?
			*/
		MonoBehaviour.print ((movePieceFrom.x + " f" + movePieceFrom.y) + "\n");
		currentlySelectedPiece = board.GetPieceAt (new Vector2 (movePieceFrom.x, movePieceFrom.y));
		Assert.AreNotEqual (currentlySelectedPiece, null);
		legalMovesForAPiece = currentlySelectedPiece.LegalMoves (this.board);
		//Assert.AreNotEqual (legalMovesForAPiece, null);
		MonoBehaviour.print (legalMovesForAPiece.Count + "\n");
		if (legalMovesForAPiece.Count == 0)
			legalMovesForAPiece = null;
		else
			drawBoard.HighLightGrid (legalMovesForAPiece);
	}
	private void switchTurn()
	{
		if (gameState.getState() == ChessGlobals.GameState.WHITE_TURN) 
		{
			gameState.setState( ChessGlobals.GameState.BLACK_TURN);
		}
		if (gameState.getState() == ChessGlobals.GameState.BLACK_TURN)
		{
			gameState.setState(ChessGlobals.GameState.WHITE_TURN);
		}
	}
	private void switchTurnDisplay()
	{
		if (gameState.getState() == ChessGlobals.GameState.WHITE_TURN/*turnDisplay.text == "White Turn"*/) 
		{
			//turnDisplay.text = "Black Turn";
			gameState.setState(ChessGlobals.GameState.BLACK_TURN);
		} 
		else if (/*turnDisplay.text == "Black Turn"*/gameState.getState() == ChessGlobals.GameState.BLACK_TURN) 
		{
			//turnDisplay.text = "White Turn";
			gameState.setState(ChessGlobals.GameState.WHITE_TURN);
		}
	}
	private void SwitchCamera()
	{
		var deltaTime = Time.time - startMoveTime;
		RotateCamera (deltaTime);
		MoveCamera (deltaTime);
		if (deltaTime >= 1) 
		{
			var temp = firstCameraPos;
			firstCameraPos = secondCameraPos;
			secondCameraPos = temp;

			var temp2 = firstCameraRot;
			firstCameraRot = secondCameraRot;
			secondCameraRot = temp2;

			startMoveTime = negativeTime;
		}
	}

	private void RotateCamera(float dTime)
	{
		var currentRot = Camera.main.transform.eulerAngles;
		Camera.main.transform.eulerAngles = new Vector3 (currentRot.x, Mathf.LerpAngle(firstCameraRot, secondCameraRot, dTime), currentRot.z);
	}

	private void MoveCamera(float dTime)
	{
		Camera.main.transform.position = Vector3.Lerp(firstCameraPos, secondCameraPos, dTime);
	}
	private Piece createPiece(PIECE_TYPES pieceType)
	{
		if (pieceType == PIECE_TYPES.KING) 
		{
			return new King();
		}
		else if (pieceType == PIECE_TYPES.QUEEN) 
		{
			return new Queen();
		}
		else if (pieceType == PIECE_TYPES.ROOK) 
		{
			return new Rook();
		}
		else if (pieceType == PIECE_TYPES.BISHOP) 
		{
			return new Bishop();
		}
		else if (pieceType == PIECE_TYPES.KNIGHT) 
		{
			return new Knight();
		}
		else //if (pieceType == PIECE_TYPES.PAWN) 
		{
			return new Pawn();
		}
	}
	private Piece createPieceAt(PIECE_TYPES pieceType, Vector2 pos, bool team)
	{
		Piece piece = createPiece (pieceType);
		piece.SetPosition ( (int)pos.x, (int)pos.y);
		piece.SetTeam(team);
		return piece;
	}
	public Piece createPieceAt(PIECE_TYPES pieceType, Vector2 pos, int team)
	{
		Piece piece = createPiece (pieceType);
		piece.SetPosition ( (int)pos.x, (int)pos.y);
		piece.SetTeam(team);
		return piece;
	}
	public void swapActivePlayer()
	{
		if (activePlayer == whitePlayer)
			activePlayer = blackPlayer;
		if (activePlayer == blackPlayer)
			activePlayer = whitePlayer;
	}
	public void setPlayer(Player whitePlayer, Player blackPlayer)
	{
		this.whitePlayer = whitePlayer;
		this.blackPlayer = blackPlayer;
	}
	public List<Piece> getPieces()
	{
		return this.pieces;
	}
	public Board getBoard()
	{
		//because some bs with c# and const, cloneing will definently prevent unintentional modifications to the game board 
		return board;
	}
	public Board getBoardClone()
	{
		return (Board)board.Clone();
	}
	public Move movePiece(Move move)
	{
		return null;
	}
	public void undoMove(Move move)
	{
	}
	public bool endConditionReached()
	{
		return false;
	}
	//not sure if this is needed
	public Vector2 getNonCapturedPieceAt(Vector2 pos)
	{
		if (isNonCapturedPieceAt (pos)) 
		{
			return pos;
		}
		return new Vector2 (-1, -1);//invalid position for now
	}
	public bool isNonCapturedPieceAt(Vector2 pos)
	{
		if (board.IsOccupied (pos)) 
		{
			Piece p = board.GetPieceAt (pos);
			if (p == null)
				return false;
			else
				if (!p.IsTaken ())
					return true;
		}
		return false;
	}
	public GameState getGameState()
	{
		return gameState;
	} 
	private int getStateOfGame()
	{
		return gameState.getState ();
	}
}
