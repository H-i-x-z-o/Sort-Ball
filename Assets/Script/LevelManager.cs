using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameLogic gameLogic;
    public PopUp popUp;
    public ResourcesManager.Level CurrentLevel;
    public State state = State.PLAY;
    public int currentMode;
    public int currentLevelIndex = 0;
    public List<MoveCommand> StateMove;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayLevel(0,1);
    }

    public void PlayLevel(int mode, int level)
    {
        currentMode = mode;
        currentLevelIndex = level - 1;
        CurrentLevel = ResourcesManager.Instance.GetLevel(currentMode, currentLevelIndex);
        LoadLevel(CurrentLevel);
    }

    public void IsWin()
    {
        state = State.WIN;
        popUp.LevelCompletePopUp();
        string key = "Mode " + currentMode + " Level " + (currentLevelIndex + 1);
        PrefManager.SetState(key, PrefManager.state.Completed);
        key = "Mode " + currentMode + " Level " + (currentLevelIndex + 2);
        PrefManager.SetState(key, PrefManager.state.Unlocked);
        SoundManager.Instance.PlaySound("winSound");
        // TODO: Show win pop up

    }

    public void NextLevel()
    {
        string key = "Mode " + currentMode + " Level " + (currentLevelIndex + 1);
        PrefManager.SetState(key, PrefManager.state.Completed);
        currentLevelIndex++;
        if(currentLevelIndex > 100) 
        {
            currentLevelIndex = 0;
            currentMode++;
            if (currentMode > 2)
            {
                ExitButton();
            }
        }
        key = "Mode " + currentMode + " Level " + (currentLevelIndex + 1);
        PrefManager.SetState(key, PrefManager.state.Unlocked);
        CurrentLevel = ResourcesManager.Instance.GetLevel(currentMode, currentLevelIndex);
        LoadLevel(CurrentLevel);
        popUp.HidePopUp();
        state = State.PLAY;
    }

    public void MenuButton()
    {
        state = State.PAUSE;
        popUp.OnCickExitButton();
    }

    public void SkipButton()
    {
        state = State.PAUSE;
        popUp.OnClickSkipButton();
    }

    public void HidePopUp()
    {
        popUp.HidePopUp();
        state = State.PLAY;
    }

    public void ExitButton()
    {
        GameManager.Instance.LoadScene(0);
    }

    public State GetStateGame()
    {
        return state;
    }


    public void AddMoveState(MoveCommand movedCommand)
    {
        MoveCommand UndoCommand = new MoveCommand();
        for(int i = 0; i < movedCommand.Commands.Count;  i++)
        {
            GameLogic.SwitchBallCommand temp = new GameLogic.SwitchBallCommand();
            temp.fromBallIndex = movedCommand.Commands[i].toBallIndex;
            temp.toBallIndex = movedCommand.Commands[i].fromBallIndex;
            temp.fromBottleIndex = movedCommand.Commands[i].toBottleIndex;
            temp.toBottleIndex = movedCommand.Commands[i].fromBottleIndex;
            temp.type = movedCommand.Commands[i].type;
            UndoCommand.Commands.Add(temp);
            // Debug.Log("fromBallIndex: " + movedCommand.Commands[i].fromBallIndex + " toBallIndex: " + movedCommand.Commands[i].toBallIndex);
            // Debug.Log("fromBottleIndex: " + movedCommand.Commands[i].fromBottleIndex + " toBottleIndex: " + movedCommand.Commands[i].toBottleIndex);
            // Debug.Log("fromBallIndex: " + temp.fromBallIndex + " toBallIndex: " + temp.toBallIndex);
            // Debug.Log("fromBottleIndex: " + temp.fromBottleIndex + " toBottleIndex: " + temp.toBottleIndex);
        }
        StateMove.Add(UndoCommand);
    }

    public void Undo()
    {
        if(StateMove.Count == 0) return;
        MoveCommand undoCommand = StateMove[StateMove.Count - 1];
        // Debug.Log(StateMove.Count);
        for(int i = undoCommand.Commands.Count - 1; i >= 0; i--)
        {
            // Debug.Log("undo fromBallIndex: " + undoCommand.Commands[i].fromBallIndex + " toBallIndex: " + undoCommand.Commands[i].toBallIndex);
            // Debug.Log("undo fromBottleIndex: " + undoCommand.Commands[i].fromBottleIndex + " toBottleIndex: " + undoCommand.Commands[i].toBottleIndex);
            gameLogic.Undo(undoCommand.Commands[i]);
        }
        StateMove.RemoveAt(StateMove.Count - 1);
    }

    public void LoadLevel(ResourcesManager.Level level)
    {
        gameLogic.selectedBotleIndex = -1;
        StateMove = new List<MoveCommand>();
        UIManager.Instance.SetLevelText(currentLevelIndex + 1);
        int bottleCount = level.Bottles.Length;
        List<int[]> bottleArray = new List<int[]>();
        for(int i = 0; i < bottleCount; i++)
        {
            int[] ballType = new int[level.Bottles[i].values.Length];
            string ballTypeString = "Bottle " + (i + 1) + ": ";
            for (int j = 0; j < ballType.Length; j++)
            {
                ballType[j] = level.Bottles[i].values[j] + 1;
                ballTypeString += ballType[j] + " ";
            }
            // Debug.Log(ballTypeString);
            bottleArray.Add(ballType);
        }
        gameLogic.LoadLevel(bottleArray);
        if(PrefManager.HasKey("Tutorial") == false)
        {
            PrefManager.SetInt("Tutorial", 1);
            popUp.ShowTutorial();
        }
        // popUp.ShowTutorial();
    }

    public void RestartLevel()
    {
        LoadLevel(CurrentLevel);
        state = State.PLAY;
    }
    public enum State
    {
        PLAY,
        PAUSE,
        WIN,
        LOSE
    }

    [Serializable]
    public class MoveCommand
    {
        public List<GameLogic.SwitchBallCommand> Commands = new List<GameLogic.SwitchBallCommand>();
    }
}
