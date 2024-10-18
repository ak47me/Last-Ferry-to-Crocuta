using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An Effect step is just one part of an ability that a card may have. This is gonna get complicated.
public class EffectStep
{
    public virtual int Activate(ref List<int> args, int index, int row, int column)
    {
        return index;
    }
}
