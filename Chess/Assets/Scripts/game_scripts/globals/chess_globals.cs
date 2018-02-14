using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChessGlobals
{
    public enum PIECE_TYPES { PAWN, ROOK, KNIGHT, BISHOP, QUEEN, KING};
	public enum GAME_STATE {WHITE_WIN, BLACK_WIN, DRAW};
	public class COLOR
	{
		public static bool BLACK = false;
		public static bool WHITE = true;
	}
	public class Tuple2<T1,T2>
	{
		public T1 t1;
		public T2 t2;
		public Tuple2(T1 t1, T2 t2)
		{
			this.t1 = t1;
			this.t2 = t2;
		}
	}
	public class Tuple3<T1,T2,T3>
	{
		public T1 t1;
		public T2 t2;
		public T3 t3;
		public Tuple3(T1 t1, T2 t2, T3 t3)
		{
			this.t1 = t1;
			this.t2 = t2;
			this.t3 = t3;
		}
	}
	public static class BoardConstants
	{
		public const int BOARD_MINIMUM = 0;
		public const int BOARD_MAXIMUM = 7;
		public const int TEAM_ROWS = 2;
		
	}
}