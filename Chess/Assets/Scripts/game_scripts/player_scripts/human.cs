﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ChessGlobals; 
public class Human: Player 
{
	private Move lastMove;
	private Move currentMove;
	private ChessGameController cgs;
	public Human(ChessGameController cgs)
	{
		Assert.AreNotEqual (cgs, null);
		this.cgs = cgs;
		lastMove = null;
	}
	public void setMove (Move move)
	{
		currentMove = move;
	}
	public Move getMove()
	{
		return new Move();
	}
	public Move getLastMove()
	{
		return lastMove;
	}
	public List<Vector2> generateLegalMoves (Piece piece)
	{
		Assert.AreNotEqual (piece, null);
		return piece.LegalMoves(cgs.getBoard());
	}
}