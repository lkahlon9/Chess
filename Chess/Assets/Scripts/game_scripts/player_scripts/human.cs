﻿ public class Human: Player {
	private Move lastMove;
	private Move currentMove;
	public override Move getMove()
	{
		return new Move();
	}
	public override bool moveSuccessfullyExcecuted()
	{
		return false;
	}
	public override int getId()
	{
		return 0;
	}
	public override char getColor()
	{
		return '0';
	}
}