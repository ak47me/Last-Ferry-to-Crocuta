using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChange : EffectStep
{
    /*
     * 0: x offset on board
     * 1: y offset on board
     * 2: atk buff amount
     * 3: health buff amount
     * 4: def buff amount
     */
    public override int Activate(ref List<int> args, int index, int row, int column)
    {
        int xTarget = row + args[1];
        int yTarget = column + args[0];

        if (Board.Instance.hasCard(xTarget, yTarget)) {
            Board.Instance.board[yTarget][xTarget].card.setStats(args[3], args[2], args[4]);
        }
        else
        {
            // return a special value indicating we failed
            return -1;
        }

        return index + 5;
    }
}
