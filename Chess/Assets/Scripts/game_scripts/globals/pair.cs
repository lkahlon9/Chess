/*
 * Name: Akobir Khamidov
 * 
 * */
using UnityEngine;
public class Pair
{
    private Vector2 pos;
    private Piece piece;

	public Pair(Vector2 pos, Piece piece)
	{
        this.pos = pos;
        this.piece = piece;
	}

    public void SetPosition(Vector2 pos)
    {
        this.pos = pos;
    }

    public void setPiece(Piece piece)
    {
        this.piece = piece;
    }

    public Vector2 GetPosition()
    {
        return pos;
    }

    public Piece GetPiece()
    {
        return piece;
    }
}

