using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME: Change the state transitions such that:
// 1. Animations actually work
// 2. All effects apply before player attacks

public class Board : MonoBehaviour
{
    private static Board _instance;
    public static Board Instance { get { return _instance; } }
    public List<List<BoardPosition>> board = new List<List<BoardPosition>>();
    public GameObject uiCanvas;
    public List<CardInfo> startingCards;
    public int fightIndex = 0;

    // Tiny finite state machine for combat
    public enum combatPhase
    {
        Play,
        PlayerFight,
        EnemyFight,
        FightAnimation,
        PlayedEffects,
        EndTurn,
        Win,
        GameOver
    }

    public combatPhase currentPhase;
    public combatPhase previousPhase;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            board.Add(new List<BoardPosition>());
            
            for (int j = 1; j < 4; j++)
            {
                BoardPosition pos = uiCanvas.transform.GetChild(i * 3 + j).gameObject.GetComponent<BoardPosition>();
                pos.setBoardPosition(i, j - 1);
                board[i].Add(pos);

                if (i == 0)
                {
                    assignEnemy(i, j, pos.transform);
                }
                else if (i == 2)
                {
                    assignHero(i, j, pos.transform);
                }
            }
        }

        currentPhase = combatPhase.Play;
        previousPhase = combatPhase.Play;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentPhase)
        {
            case combatPhase.Play:
                // Currently: Nothing we need to do.
                break;

            case combatPhase.PlayerFight:
                updatePlayerActions();
                break;

            case combatPhase.EnemyFight:
                updateEnemyActions();
                break;

            case combatPhase.FightAnimation:
                awaitAniEnd();
                break;

            case combatPhase.PlayedEffects:
                updateCardEffects();
                break;

            case combatPhase.EndTurn:
                updateEndTurn();
                break;

            case combatPhase.Win:
                Win();
                break;

            case combatPhase.GameOver:
                GameOver();
                break;
        }
    }

    public void removeCard(int row, int col)
    {
        board[row][col].clearCard();
    }

    // FIXME: assign the enemy saved from Map here instead of one from the starting cards list
    // (or just make the starting cards list the cards you saved)
    public void assignEnemy(int row, int col, Transform parentTransform)
    {
        GameObject card = HandHandler.Instance.generateCard(startingCards[col-1], parentTransform);
        card.GetComponent<CardMover>().locked = true; // prevent this card from being dragged
        board[row][col-1].setCard(card.GetComponent<CardView>());
    }

    // FIXME: assign the hero (enzo, claudio, sofia, etc.) saved from Map here instead of one from the starting cards list
    // (or just make the starting cards list the cards you saved)
    public void assignHero(int row, int col, Transform parentTransform)
    {
        GameObject card = HandHandler.Instance.generateCard(startingCards[2 + col], parentTransform);
        card.GetComponent<CardMover>().locked = true; // prevent this card from being dragged
        board[row][col-1].setCard(card.GetComponent<CardView>());
    }

    // STATE MACHINE FUNCTIONS START HERE
    
    // Handles the player aspect of the what happens after pressing Fight
    public void updatePlayerActions()
    {
        // If a player card exists here, perform its tasks
        if (hasCard(fightIndex, 2))
        {
            if (board[2][fightIndex].card.canEffect)
            {
                doEffect(2, fightIndex, board[2][fightIndex].card.cardInfo);
            }
            else
            {
                board[2][fightIndex].card.incrementTWE();
            }

            // FIXME: There should be an animation playing here, but there isn't. Has to do with needing to refactor to a Coroutine.
            // Set the card to handle its fight with an enemy card.
            CardMover mover = board[2][fightIndex].card.gameObject.GetComponent<CardMover>();
            mover.startPos = mover.transform.position;
            mover.endPos = Vector3.Lerp(mover.startPos, getNearestEnemyPos(2, fightIndex), 0.2f);
            mover.elapsedTime = 0f;
            mover.state = CardMover.moveState.Fight;


            currentPhase = combatPhase.FightAnimation;
            previousPhase = combatPhase.PlayerFight;
        }
        else if (fightIndex == 2)
        {
            currentPhase = combatPhase.EnemyFight;
            fightIndex = 0;
            return;
        }
        else
        {
            fightIndex += 1;
        }
    }
    
    // Handles the enemy aspect of what happens after pressing Fight
    public void updateEnemyActions()
    {
        // If a player card exists here, perform its tasks
        if (hasCard(fightIndex, 0))
        {
            if (board[0][fightIndex].card.canEffect)
            {
                doEffect(0, fightIndex, board[0][fightIndex].card.cardInfo);
            }
            else
            {
                board[0][fightIndex].card.incrementTWE();
            }

            // FIXME: There should be an animation playing here, but there isn't. Has to do with needing to refactor to a Coroutine.
            // Set the card to handle its fight with an enemy card.
            CardMover mover = board[0][fightIndex].card.gameObject.GetComponent<CardMover>();
            mover.startPos = mover.transform.position;
            mover.endPos = Vector3.Lerp(mover.startPos, getNearestEnemyPos(0, fightIndex), 0.2f);
            mover.elapsedTime = 0f;
            mover.state = CardMover.moveState.Fight;

            currentPhase = combatPhase.FightAnimation;
            previousPhase = combatPhase.EnemyFight;
            return;
        }
        else if (fightIndex == 2)
        {
            currentPhase = combatPhase.EndTurn;
            fightIndex = 0;
            return;
        }
        else
        {
            fightIndex += 1;
        }
    }

    // Handles waiting for an animation from a key card or enemy card (not items and not helpers).
    public void awaitAniEnd()
    {
        bool wasPlayerPhase = false;
        if (previousPhase == combatPhase.PlayerFight) wasPlayerPhase = true;
        int row = wasPlayerPhase ? 2 : 0;
        float maxWait = 0f;
        CardMover checkMover = board[row][fightIndex].card.gameObject.GetComponent<CardMover>();

        doAtk(row, fightIndex);
        checkWin();

        // Check in the case that we lose or win after the attack.
        if (currentPhase != combatPhase.FightAnimation)
        {
            return;
        }

        previousPhase = currentPhase;
        
        if (fightIndex == 2)
        {
            fightIndex = 0;
            // If we have done all cards in a column, we are either
            previousPhase = combatPhase.FightAnimation;
            
            if (wasPlayerPhase)
            {
                currentPhase = combatPhase.EnemyFight;
            }
            else
            {
                currentPhase = combatPhase.EndTurn;
            }
        }
        else
        {
            fightIndex += 1;
            // If all player actions not done, next played card, else go back to enemies
            if (wasPlayerPhase)
            {
                currentPhase = combatPhase.PlayedEffects;
            }
            else
            {
                currentPhase = combatPhase.EnemyFight;
            }

            previousPhase = combatPhase.FightAnimation;
        }
    }

    // Handles effects of items and helpers.
    public void updateCardEffects()
    {
        // Perform a card effect if there is one.
        if (hasCard(fightIndex, 1))
        {
            if (board[1][fightIndex].card.cardInfo.effect == CardInfo.effectType.OnFight) doEffect(1, fightIndex, board[1][fightIndex].card.cardInfo);
            if (board[1][fightIndex].card.cardInfo.playType == CardInfo.subType.Item) board[1][fightIndex].card.handleAtk(1);
        }

        // Transition to the player fight phase.
        currentPhase = combatPhase.PlayerFight;
        previousPhase = combatPhase.PlayedEffects;
    }

    // Call this function when you press the fight button.
    public void fightButton()
    {
        if (currentPhase != combatPhase.Play)
        {
            return;
        }

        lockMiddleRow(true);

        currentPhase = combatPhase.PlayedEffects;
        previousPhase = combatPhase.Play;
    }

    public void lockMiddleRow(bool lockVal)
    {
        for (int i = 0; i < 3; i++)
        {
            if (hasCard(i, 1)) board[1][i].card.gameObject.GetComponent<CardMover>().locked = lockVal;
        }
    }

    public void updateEndTurn()
    {
        lockMiddleRow(false);

        // reset the values of atking creatures and handle end of turn effects
        resetAtkDef();
        doEOTEffects(); // It's important for this to be after the reset.

        // Return cards to hand
        currentPhase = combatPhase.Play;
    }

    public void resetAtkDef()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[0][i].card != null)
            {
                board[0][i].card.resetAtk();
                board[0][i].card.resetDef();
                board[0][i].card.resetUses();
            }
            if (board[2][i].card != null)
            {
                board[2][i].card.resetAtk();
                board[2][i].card.resetDef();
                board[2][i].card.resetUses();
            }
        }
    }

    public void doEOTEffects()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!hasCard(j, i)) continue;
                if (board[i][j].card.cardInfo.effect == CardInfo.effectType.OnEnd && board[i][j].card.canEffect) doEffect(i, j, board[i][j].card.cardInfo);
                else if (!(board[i][j].card.canEffect))
                {
                    board[i][j].card.incrementTWE();
                }
            }
        }
    }

    // STATE MACHINE FUNCTIONS END HERE

    // Effect functions start here

    // Add any generic effects that take place during the fight step here.
    public void doEffectStep(ref List<int> args, int row, int column, ref int index, string effect) 
    { 
        switch(effect)
        {
            case "Change Stats":
                setStats(ref args, row, column, ref index);
                return;

            case "Check Adjacency":
                hasAdjacency(ref args, row, column, ref index);
                return;

            case "Afflict Nearest":
                afflictNearest(ref args, row, column, ref index);
                return;

            case "Disable Atk":
                disableAtk(ref args, row, column, ref index);
                return;

            case "Disalbe Effect":
                disableEff(ref args, row, column, ref index);
                return;
        }

        index = -1;
    }

    public void doEffect(int row, int column, CardInfo data)
    {
        int nextStepIndex = 0;

        for (int i = 0; i < data.effectList.Count; i++)
        {
            // We use ref here because you are going to be copying this list more times than I can count.
            doEffectStep(ref data.targets, row, column, ref nextStepIndex, data.effectList[i]);
            if (nextStepIndex == -1)
            {
                // In case an ability fails for some reason, we have an out.
                break;
            }
        }
    }

    /*
     * 0: x offset on board
     * 1: y offset on board
     * 2: atk buff amount
     * 3: health buff amount
     * 4: def buff amount
     * Result: Sets the stats of a card on board to the specified numbers
     */
    public void setStats(ref List<int> args, int row, int column, ref int index)
    {
        int yTarget = row + args[1 + index];
        int xTarget = column + args[0 + index];

        if (yTarget < 3 && yTarget >= 0 && xTarget >= 0 && xTarget < 3 && hasCard(xTarget, yTarget))
        {
            board[yTarget][xTarget].card.setStats(args[3 + index], args[2 + index], args[4 + index]);
        }
        else
        {
            // return a special value indicating we failed
            index += 5;
            return;
        }

        index += 5;

    }

    /*
     * 0: x offset on board
     * 1: y offset on board
     * Result: Confirms that there is a card where the effect wants to look
     */
    public void hasAdjacency(ref List<int> args, int row, int column, ref int index)
    {
        int yTarget = row + args[1 + index];
        int xTarget = column + args[0 + index];

        if (!hasCard(xTarget, yTarget))
        {
            index = -1;
            return;
        }

        index += 2;

    }

    /*
     * 0: atk buff amount
     * 1: health buff amount
     * 2: def buff amount
     */
    public void afflictNearest(ref List<int> args, int row, int column, ref int index)
    {
        CardView card = getNearestCard(row, column);
        List<int> newArgs = new List<int>();
        newArgs.Add(column - card.boardCol);
        newArgs.Add(card.boardRow - row);
        newArgs.Add(args[0 + index]);
        newArgs.Add(args[1 + index]);
        newArgs.Add(args[2 + index]);
        setStats(ref newArgs, row, column, ref index);
        index -= 2;
    }

    /*
     * 0: x offset on board
     * 1: y offset on board
     * 2: Amount of turns to disable for
     * Result: Confirms that there is a card where the effect wants to look
     */
    public void disableAtk(ref List<int> args, int row, int column, ref int index)
    {
        int yTarget = row + args[1 + index];
        int xTarget = column + args[0 + index];

        if (!hasCard(xTarget, yTarget))
        {
            index = -1;
            return;
        }

        board[yTarget][xTarget].card.canAtk = false;
        board[yTarget][xTarget].card.turnsWoAtk = args[2 + index];
        index += 3;
    }

    /*
     * 0: x offset on board
     * 1: y offset on board
     * 2: Amount of turns to disable for
     * Result: Confirms that there is a card where the effect wants to look
     */
    public void disableEff(ref List<int> args, int row, int column, ref int index)
    {
        int yTarget = row + args[1 + index];
        int xTarget = column + args[0 + index];

        if (!hasCard(xTarget, yTarget))
        {
            index = -1;
            return;
        }

        board[yTarget][xTarget].card.canEffect = false;
        board[yTarget][xTarget].card.turnsWoEff = args[2 + index];
        index += 3;
    }

    // Effect functions end here

    public bool hasCard(int column, int row)
    {
        return (board[row][column].hasCard());
    }

    public CardView getNearestCard(int row, int column)
    {
        int targetRow = 0;
        if (row == 0) targetRow = 2;

        for (int i = 0; i < 3; i++)
        {
            if (column - i >= 0 && hasCard(column - i, targetRow))
            {
                if (targetRow == 2 && hasCard(column - i, 1) && board[1][column - i].card.cardInfo.playType == CardInfo.subType.Helper) return board[1][column - i].card;
                return board[targetRow][column - i].card;
            }
            else if (column + i < 3 && hasCard(column + i, targetRow))
            {
                if (targetRow == 2 && hasCard(column + i, 1) && board[1][column + i].card.cardInfo.playType == CardInfo.subType.Helper) return board[1][column + i].card;
                return board[targetRow][column + i].card;
            }
        }

        return null;
    }

    public Vector3 getNearestEnemyPos(int row, int column)
    {
        int targetRow = 0;
        if (row == 0) targetRow = 2;

        for (int i = 0; i < 3; i++)
        {
            if (column - i >= 0 && hasCard(column - i, targetRow))
            {
                return board[targetRow][column - i].transform.position;
            }
            else if (i != 0 && column + i < 3 && hasCard(column + i, targetRow))
            {
                return board[targetRow][column + i].transform.position;
            }
        }

        return new Vector3(0f, 0f, 0f);
    }

    public void doAtk(int row, int column)
    {
        if (!(board[row][column].card.canAtk))
        {
            print(row);
            print(column);
            print("can't attack");
            board[row][column].card.incrementTWA();
            return;
        }

        int atk = board[row][column].card.atkV + board[row][column].card.tempAtk;
        CardView card = getNearestCard(row, column);
        card.handleAtk(atk);
    }

    public void checkWin()
    {
        bool enemyWin = true;
        bool playerWin = true;

        for (int i = 0; i < 3; i++)
        {
            if (hasCard(i, 0)) playerWin = false;
            if (hasCard(i, 2)) enemyWin = false;

            if (!enemyWin && !playerWin) return;
        }

        if (enemyWin)
        {
            currentPhase = combatPhase.GameOver;
        }
        else if (playerWin)
        {
            currentPhase = combatPhase.Win;
        }
    }

    public void Win()
    {
        print("player wins");
    }

    public void GameOver()
    {
        print("you lose");
    }
}
