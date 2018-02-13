using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChessGlobals;

public class ChessGameController : MonoBehaviour {
	public DrawBoard drawBoard;
	public DrawPiece drawPiece;
	public Text turnDisplay;

	//private Rules rules;
	private Board board;
	public Player playerOne;
	public Player playerTwo;
	public Player activePlayer;
	private List<Piece> pieces;
	private List<Piece> capturedPieces;


	private float startMoveTime = -1;

	private Vector3 firstCameraPos;
	private Vector3 secondCameraPos;
	private float firstCameraRot;
	private float secondCameraRot;

	// Use this for initialization
	void Start () {
		//drawBoard = GetComponent<DrawBoard> ();
		drawBoard.InitBoard();
		drawPiece.InitPieces ();
		firstCameraPos = Camera.main.transform.position;
		secondCameraPos = new Vector3 (firstCameraPos.x, firstCameraPos.y, firstCameraPos.z + 18);
		firstCameraRot = Camera.main.transform.eulerAngles.y;
		secondCameraRot = Camera.main.transform.eulerAngles.y + 180;
		turnDisplay.text = "White Turn";

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) 
		{
			startMoveTime = Time.time;
			print ("first cam pos: " + firstCameraPos);
			print ("second cam pos:" + secondCameraPos);
			switchTurnDisplay();
		}
		if (startMoveTime != -1)
			SwitchCamera ();
	}
	private void switchTurnDisplay()
	{
		if(turnDisplay.text == "White Turn")
			turnDisplay.text = "Black Turn";
		else if(turnDisplay.text == "Black Turn")
			turnDisplay.text = "White Turn";
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

			startMoveTime = -1;
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
	public void swapActivePlayer()
	{
		if (activePlayer == playerOne)
			activePlayer = playerTwo;
		if (activePlayer == playerTwo)
			activePlayer = playerOne;
	}
	public void setPlayer(Player playerOne, Player playerTwo)
	{
		this.playerOne = playerOne;
		this.playerTwo = playerTwo;
	}
	/*public void startGame()
	{
	}
	public void createAndAddPiece(PIECE_TYPES pieceType)
	{
	}
	public Move movePiece(Move move)
	{
	}
	public void waitForMove()
	{
	}
	public void undoMove(Move move)
	{
	}
	public bool endConditionReached()
	{
	}
	public Position getNonCapturedPieceAtPosition(Position pos)
	{
	}
	public bool isNonCapturedPieceAtPosition(Position pos)
	{
	}
	public GAME_STATE getGameState()
	{
	}*/


}
