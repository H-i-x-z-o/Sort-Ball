using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    public List<Bottle> bottles;
    public GameGraphic gameGraphic;
    private int maxBallsInBottle = 4;
    public int selectedBotleIndex = -1;

    private void Awake()
    {
        // instance = this;
    }
    private void Start()
    {
        // bottles.Add(new Bottle()
        //     {
        //         Balls = new List<Ball>()
        //             { new Ball() {type = 1},new Ball() {type = 2} }
        //     }
        // );
        // bottles.Add(new Bottle()
        //     {
        //         Balls = new List<Ball>(){ 
        //             new Ball() {type =2}, new Ball() {type = 1}, new Ball() {type = 1} }
        //     }
        // );
        // bottles.Add(new Bottle()
        //     {
        //         Balls = new List<Ball>(){ 
        //             new Ball() {type =2}, new Ball() {type = 2}, new Ball() {type =1} }
        //     }
        // );
        // bottles.Add(new Bottle()
        //     {
        //         Balls = new List<Ball>(){}
        //     }
        // );
        // gameGraphic.RefreshGraphic(bottles);
    }

    public void PrintBottles()
    {
        //Debug.Log("====== Bottles =====");
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < bottles.Count; i++)
        {
            Bottle bottle = bottles[i];
            sb.Append("Bottle " + (i + 1) + ": ");
            foreach(Ball ball in bottle.Balls)
            {
                sb.Append(ball.type + " ");
            }
            //Debug.Log(sb.ToString());
            sb.Clear();
        }

        // bool isWin = CheckWinCondition();
        // if(isWin) LevelManager.Instance.IsWin();
        // Debug.Log("Win: " + isWin);
    }


    public void LoadLevel(List<int[]> listArray)
    {
        bottles = new List<Bottle>();
        foreach (var array in listArray)
        {
            Bottle b = new Bottle(){};
            List<Ball> balls = new List<Ball>();
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] == 0) continue;
                balls.Add(new Ball(){type = array[i]});
            }
            b.Balls = balls;
            bottles.Add(b);
        }
        gameGraphic.CreateBottleGraphic(bottles);
        gameGraphic.RefreshGraphic(bottles);
        // FillSize.Instance.Fill(bottles.Count);
        // PrintBottles();
    }

    public void SwapBalls(Bottle bottle_1, Bottle bottle_2)      // swap ball in bottle 1 to bottle 2
    {
        int count = bottle_1.Balls.Count;
        if(count == 0) return;
        int type = bottle_1.Balls[count - 1].type;
        if(!CheckCanSwap(bottle_1,bottle_2)) return;
        for (int i = count - 1; i >= 0; i--)
        {
            Ball ball = bottle_1.Balls[i];
            if(ball.type == type)
            {
                bottle_1.Balls.RemoveAt(i);
                bottle_2.Balls.Add(ball);
            }
            else break;
            if(bottle_2.Balls.Count == maxBallsInBottle) break;
        }
        // gameGraphic.RefreshGraphic(bottles);
    }
    public void SwapBalls(int idx_bottle_1, int idx_bottle_2)
    {
        if(idx_bottle_1 < 0 || idx_bottle_1 >= bottles.Count || idx_bottle_2 < 0 || idx_bottle_2 >= bottles.Count) return;
        SwapBalls(bottles[idx_bottle_1], bottles[idx_bottle_2]);
        
    }

    public bool CheckCanSwap(Bottle bottle_1, Bottle bottle_2)
    {
        int count1 = bottle_1.Balls.Count, count2 = bottle_2.Balls.Count;
        if (count2 == 0) return true;
        if (count1 == 0) return false;
        int type1 = bottle_1.Balls[count1 - 1].type, type2 = bottle_2.Balls[count2 - 1].type;
        return type1 == type2 && count2 != maxBallsInBottle;
    }

    public bool CheckCanSwap(int idx_bottle_1, int idx_bottle_2)
    {
        if(idx_bottle_1 < 0 || idx_bottle_1 >= bottles.Count || idx_bottle_2 < 0 || idx_bottle_2 >= bottles.Count) return false;
        return CheckCanSwap(bottles[idx_bottle_1], bottles[idx_bottle_2]);
    }

    public List<SwitchBallCommand> CheckSwapBall(int idxBottle_1, int idxBottle_2)
    {
        List<SwitchBallCommand> commands = new List<SwitchBallCommand>();
        Bottle bottle_1 = bottles[idxBottle_1];
        Bottle bottle_2 = bottles[idxBottle_2];
        List<Ball> balls_1 = bottle_1.Balls;
        List<Ball> balls_2 = bottle_2.Balls;
        if(balls_1.Count == 0 || balls_2.Count == 4) return commands;
        int idx = balls_1.Count - 1;
        Ball b = balls_1[idx];
        var type = b.type;
        if(balls_2.Count > 0 && balls_2[balls_2.Count - 1].type != type) return commands;
        int targetIdx = balls_2.Count;
        for (int i = idx; i >= 0; i--)
        {
            Ball ball = balls_1[i];
            if (ball.type == type)
            {
                int fromBottleIndex = idxBottle_1;
                int toBottleIndex = idxBottle_2;
                int fromBallIndex = i;
                int toBallIndex = targetIdx;
                commands.Add(new SwitchBallCommand()
                {
                    type = type,
                    fromBottleIndex = fromBottleIndex,
                    toBottleIndex = toBottleIndex,
                    fromBallIndex = fromBallIndex,
                    toBallIndex = toBallIndex
                });
                targetIdx++;
                if (targetIdx == 4) break;
            }
            else break;
        }
        
        return commands;
    }

    public void Undo(SwitchBallCommand command)
    {
        Ball ball = new Ball();
        ball.type = command.type;
        int fromBottle = command.fromBottleIndex;
        int toBottle = command.toBottleIndex;
        int fromBall = command.fromBallIndex;
        int toBall = command.toBallIndex;
        bottles[fromBottle].Balls.RemoveAt(fromBall);
        bottles[toBottle].Balls.Insert(toBall, ball);
        gameGraphic.RefreshGraphic(bottles);
    }
    public bool CheckWinCondition()
    {
        // Debug.Log("check win condition");
        bool winFlag = true;
        for(int i = 0; i < bottles.Count; i++)
        {
            Bottle bottle = bottles[i];
            if(bottle.Balls.Count != 0 && bottle.Balls.Count != maxBallsInBottle)
            {
                winFlag = false;
                break;
            }
            if(bottle.Balls.Count == 0) continue;
            bool sameTypeFlag = true;
            int type = bottle.Balls[0].type;
            foreach (var ball in bottle.Balls)
            {
                if(ball.type != type)
                {
                    sameTypeFlag = false;
                    break;
                }
            }
            if(!sameTypeFlag)
            {
                winFlag = false;
                break;
            }
        }
        return winFlag;
    }

    [Serializable]
    public class SwitchBallCommand
    {
        public int type;
        public int fromBottleIndex;
        public int toBottleIndex;
        public int fromBallIndex;
        public int toBallIndex;
    }
    public class Bottle
    {
        public List<Ball> Balls;
    }

    public class Ball
    {
        public int type;
    }

}
