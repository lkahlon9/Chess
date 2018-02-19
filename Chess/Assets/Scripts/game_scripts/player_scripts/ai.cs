﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ChessGlobals;
public class AI: Player
{
	private Move lastMove;
	private Move currentMove;
	private int depth;
	private ChessGameController cgs;
	public AI(ChessGameController cgs, int depth)//Rules now in each piece
	{
		Assert.AreNotEqual (cgs, null);
		this.cgs = cgs;
		this.depth = depth;
		lastMove = null;
	}
	public void setMove (Move move)
	{
		currentMove = move;
	}
	public Move getMove()
	{
		return currentMove;
	}
	public Move getLastMove()
	{
		return lastMove;
	}
	private void executeMove(Move move)
	{
		this.cgs.movePiece (move);
	}
	private void undoMove(Move move)
	{
		this.cgs.undoMove (move);
	}
	public Move getBestMove()
	{
		//get all possible valid moves
		List<Move> allMoves = generateAllLegalMoves();
		int bestResult = Int32.MinValue;
		Move bestMove = null;
		foreach(Move move in allMoves)
		{
			executeMove (move);
			int evaluationResult = -1 * negMax (this.depth);
			undoMove (move);
			if (evaluationResult > bestResult)
			{
				bestResult = evaluationResult;
				bestMove = move;
			}
		}
		//foreach move call negmax
		return bestMove;
	}
	public int negMax(int depth)
	{
		if (depth <= 0 || this.cgs.getGameState () == ChessGlobals.GAME_STATE.BLACK_WIN || this.cgs.getGameState () == ChessGlobals.GAME_STATE.WHITE_WIN)
			return evaluateGameState ();
		List<Move> moves = generateAllLegalMoves();
		int currentMax = Int32.MinValue;
		foreach(Move currentMove in moves)
		{
			executeMove(currentMove);
			int score = -1 * negMax(depth - 1);
			undoMove(currentMove);
			if(score > currentMax)
				currentMax = score;
		}
		return currentMax;
	}
	private List<Move> generateAllLegalMoves()
	{
		List<Piece> allPieces = cgs.getPieces ();
		List<Move> allValidMoves = new List<Move>();
		List<Vector2> validMovesForASinglePiece = null;
		for (int i = 0; i < allPieces.Count; ++i) 
		{
			Move move = null;
			//generate LegalMoves for every piece in the game
			validMovesForASinglePiece = generateLegalMovesForAPiece(allPieces[i]);
			for(int j = 0; j < validMovesForASinglePiece.Count; ++j)
			{
				move = new Move (allPieces [i], validMovesForASinglePiece [i]);
				allValidMoves.Add (move);
			}
		}
		return allValidMoves;
	}
	private List<Vector2> generateLegalMovesForAPiece (Piece piece)
	{
		Assert.AreNotEqual (piece, null);
		return piece.LegalMoves(cgs.getBoard());
	}

	private int evaluateGameState()
	{
		int whitePlayerScore = 0;
		int blackPlayerScore = 0;
		foreach (Piece piece in this.cgs.getPieces()) 
		{
			if (piece.GetTeam () == ChessGlobals.COLOR.BLACK) 
			{
				blackPlayerScore += getScoreForPieceType (piece);
				blackPlayerScore += getScoreForPiecePosition (piece.GetPiecePosition());
			} 
			else if (piece.GetTeam () == ChessGlobals.COLOR.WHITE) 
			{
				whitePlayerScore += getScoreForPieceType (piece);
				whitePlayerScore += getScoreForPiecePosition (piece.GetPiecePosition());
			} 
			else
			{
				//some illegal state exception
			}
			
		}
	    ChessGlobals.GAME_STATE gameState = this.cgs.getGameState ();
		if (gameState == ChessGlobals.GAME_STATE.BLACK)
			return blackPlayerScore - whitePlayerScore;
		else if (gameState == ChessGlobals.GAME_STATE.WHITE)
			return whitePlayerScore - blackPlayerScore;
		else if (gameState == ChessGlobals.GAME_STATE.WHITE_WIN ||
		         gameState == ChessGlobals.GAME_STATE.BLACK_WIN ||
		         gameState == ChessGlobals.GAME_STATE.DRAW)
			return Int32.MinValue + 1;
		else
			//some illegate state exception
			return 0;
	}
	private int getScoreForPieceType(Piece piece)
	{
		if (piece.GetType() == typeof(King)) 
			return 9999;
		else if (piece.GetType() == typeof(Queen)) 
			return 90;
		else if (piece.GetType() == typeof(Knight))
			return 30;
		else if (piece.GetType() == typeof(Bishop))
			return 30;
		else if (piece.GetType() == typeof(Rook)) 
			return 50;
		else if (piece.GetType() == typeof(Pawn)) 
			return 10;
		else  
		{
			//some invalid state exception
		}
		return 0;
	}
	private int getScoreForPiecePosition(Vector2 pos)
	{
		return getScoreForPiecePosition((int)pos.x, (int)pos.y);
	}
	private int getScoreForPiecePosition(int x, int y)
	{
		int[,] positionWeight = new int[,]
		{ 	{1,1,1,1,1,1,1,1},
			{2,2,2,2,2,2,2,2},
			{2,2,3,3,3,3,2,2},
			{2,2,3,4,4,3,2,2},
			{2,2,3,4,4,3,2,2},
			{2,2,3,3,3,3,2,2},
			{2,2,2,2,2,2,2,2},
			{1,1,1,1,1,1,1,1}
		};
		return positionWeight[x,y];
	}
}
