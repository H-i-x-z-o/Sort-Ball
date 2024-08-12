using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class GameGraphic : MonoBehaviour
{
    [SerializeField] private GameLogic gameLogic;
    public Transform PoolBottleTranform;
    public BottleGraphic bottleGraphicPrefab;
    public BallGraphic PreviewBall1, PreviewBall2;
    public BottleSpawner bottleSpawner;
    public List<BottleGraphic> bottleGraphics = new List<BottleGraphic>();


    

    private void Awake()
    {
        gameLogic = GetComponent<GameLogic>();
        bottleSpawner = GetComponent<BottleSpawner>();
        PreviewBall1 = Instantiate(PreviewBall1);
        PreviewBall2 = Instantiate(PreviewBall2);
        PreviewBall1.name = "PreviewBall_1";
        PreviewBall2.name = "PreviewBall_2";
        PreviewBall1.gameObject.SetActive(false);
        PreviewBall2.gameObject.SetActive(false);
    }

    public void OnClickBottle(int index)
    {
        if(isSwapping)  return;
        if(PreviewBall1.gameObject.activeSelf && PreviewBall2.gameObject.activeSelf) return;
        if (gameLogic.selectedBotleIndex == -1)
        {
            // Debug.Log("Chose bottle " + index);
            gameLogic.selectedBotleIndex = index;
            StartCoroutine(MoveBallUp(index));
        }
        else
        {
            if(gameLogic.selectedBotleIndex == index)
            {
                gameLogic.selectedBotleIndex = -1;
                StartCoroutine(MoveBallDown(index));
                return;
            }

            if(!gameLogic.CheckCanSwap(gameLogic.selectedBotleIndex, index))
            {
                StartCoroutine(MoveBallDown(gameLogic.selectedBotleIndex));
                gameLogic.selectedBotleIndex = index;
                StartCoroutine(MoveBallUp(gameLogic.selectedBotleIndex));
                return;
            }
            StartCoroutine(SwapBallCoroutine(gameLogic.selectedBotleIndex, index));
            gameLogic.SwapBalls(gameLogic.selectedBotleIndex, index);
            gameLogic.selectedBotleIndex = -1;
        }
        // gameLogic.PrintBottles();
    }

    public void CreateBottleGraphic(List<GameLogic.Bottle> bottles)
    {
        bottleGraphics.Clear();
        ClearChild(PoolBottleTranform);
        bottleGraphics = bottleSpawner.SpawnBottles(bottles.Count, bottleGraphicPrefab, PoolBottleTranform);
        for (int i = 0; i < bottles.Count; i++)
        {
            List<int> ballsType = new List<int>();
            foreach (var ball in bottles[i].Balls)
            {
                ballsType.Add(ball.type);
            }
            bottleGraphics[i].SetBottleGraphic(ballsType.ToArray());
        }

    }

    private void ClearChild(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshGraphic(List<GameLogic.Bottle> bottles)
    {
        PreviewBall1.gameObject.SetActive(false);
        PreviewBall2.gameObject.SetActive(false);
        for(int i = 0; i < bottleGraphics.Count; i++)
        {
            GameLogic.Bottle gb = bottles[i];
            BottleGraphic bottleGraphic = bottleGraphics[i];
            List<int> ballTypes = new List<int>();
            foreach (var ball in gb.Balls)
            {
                ballTypes.Add(ball.type);
            }
            
            bottleGraphic.SetBottleGraphic(ballTypes.ToArray());
        }
    }

    public IEnumerator MoveBallUp(int bottleIndex)
    {
        isSwapping = true;
        List<GameLogic.Ball> ballList = gameLogic.bottles[bottleIndex].Balls;
        if(ballList.Count == 0)
        {
            isSwapping = false;
            yield break;
        }
        GameLogic.Ball ballTop = ballList[ballList.Count - 1];
        Vector3 bottleUpPos = bottleGraphics[bottleIndex].GetBottleUpPosition();
        bottleGraphics[bottleIndex].SetGraphicNone(ballList.Count - 1);
        BallGraphic fakePreviewBall;
        if(!PreviewBall1.gameObject.activeSelf)  fakePreviewBall = PreviewBall1;
        else fakePreviewBall = PreviewBall2;
        fakePreviewBall.gameObject.SetActive(true);
        fakePreviewBall.SetColor(ballTop.type);
        fakePreviewBall.transform.position = bottleGraphics[bottleIndex].GetBallPosition(ballList.Count - 1);

        fakePreviewBall.transform.DOMove(bottleUpPos, 0.2f).SetEase(Ease.Linear);
        SoundManager.Instance.PlaySound("popSound");
        yield return new WaitForSeconds(0.2f);

        // while(Vector3.Distance(fakePreviewBall.transform.position,bottleUpPos) > 0.005f)
        // {
        //      fakePreviewBall.transform.position = Vector3.MoveTowards(fakePreviewBall.transform.position, bottleUpPos, 20 * Time.deltaTime);
        //      yield return null;
        // }

        // yield return new WaitForFixedUpdate();
        isSwapping = false;
    }

    public IEnumerator MoveBallDown(int bottleIndex)
    {       
        isSwapping = true;
        List<GameLogic.Ball> ballList = gameLogic.bottles[bottleIndex].Balls;
        if(ballList.Count == 0)
        {
            isSwapping = false;
            yield break;
        }
        Vector3 UpPos = bottleGraphics[bottleIndex].GetBottleUpPosition();
        Vector3 downPos = bottleGraphics[bottleIndex].GetBallPosition(ballList.Count - 1);
        BallGraphic fakePreviewBall;
        if(PreviewBall1.gameObject.activeSelf)  fakePreviewBall = PreviewBall1;
        else fakePreviewBall = PreviewBall2;
        fakePreviewBall.transform.position = UpPos;
        
        fakePreviewBall.transform.DOMove(downPos, 0.5f).SetEase(Ease.Linear).SetEase(Ease.OutBounce);
        SoundManager.Instance.PlaySound("popSound");
        yield return new WaitForSeconds(0.5f);

        // while (Vector3.Distance(fakePreviewBall.transform.position,downPos) > 0.005f)
        // {
        //     Debug.Log("ball down");
        //     fakePreviewBall.transform.position = Vector3.MoveTowards(fakePreviewBall.transform.position, downPos, 20 * Time.deltaTime);
        //     yield return null;
        // }

        GameLogic.Ball ball = ballList[ballList.Count - 1];
        bottleGraphics[bottleIndex].SetGraphic(ballList.Count-1, ball.type);
        fakePreviewBall.gameObject.SetActive(false);
        isSwapping = false;
    }

    private bool isSwapping = false;
    private bool ballOnTop = false;

    private IEnumerator SwapBallCoroutine(int fromBottleIndex, int toBottleIndex)
    {
        List<GameLogic.SwitchBallCommand> commands = gameLogic.CheckSwapBall(fromBottleIndex, toBottleIndex);

        if(commands.Count == 0)
        {
            Debug.LogError("Can't swap ball");
        }
        else
        {
            ballOnTop = true;
            bool soundPlayed = false;
            foreach (var command in commands)
            {
                StartCoroutine(SwapBall(command));
                ballOnTop = false;
                yield return new WaitForSeconds(0.1f);
                if(soundPlayed) continue;
                soundPlayed = true;
                SoundManager.Instance.PlaySound("swapSound");
            }
            
            LevelManager.MoveCommand moveCommand = new LevelManager.MoveCommand();
            moveCommand.Commands = commands;
            LevelManager.Instance.AddMoveState(moveCommand);
            bool isWin = gameLogic.CheckWinCondition();
            if(isWin) LevelManager.Instance.IsWin();
        }
    }

    
    private IEnumerator SwapBall(GameLogic.SwitchBallCommand command)
    {                          
        // tat graphic tai vi tri from
        // tao fake ball tai vi tri from
        // di chuyen fake ball den vi tri to
        // doi ball that va fake ball
        //=======================================================================================================

        isSwapping = true;
        bottleGraphics[command.fromBottleIndex].SetGraphicNone(command.fromBallIndex);   // tat graphic tai vi tri from
        BallGraphic fakeBall = Instantiate(PreviewBall1, bottleGraphics[command.fromBottleIndex].GetBallPosition(command.fromBallIndex), Quaternion.identity);   // tao fake ball tai vi tri from
        fakeBall.gameObject.SetActive(true);
        fakeBall.SetColor(command.type);
 
        Queue<Vector3> queuePos = new Queue<Vector3>();   // di chuyen fake ball den vi tri to
        if (ballOnTop)
        {
            fakeBall.transform.position = bottleGraphics[command.fromBottleIndex].GetBottleUpPosition();
            if(PreviewBall1.gameObject.activeSelf)  PreviewBall1.gameObject.SetActive(false);
            else PreviewBall2.gameObject.SetActive(false);
        }
        else queuePos.Enqueue(bottleGraphics[command.fromBottleIndex].GetBottleUpPosition());
        queuePos.Enqueue(bottleGraphics[command.toBottleIndex].GetBottleUpPosition());
        queuePos.Enqueue(bottleGraphics[command.toBottleIndex].GetBallPosition(command.toBallIndex));
        while (queuePos.Count > 0)
        {
            Vector3 targetPos = queuePos.Dequeue();
            fakeBall.transform.DOMove(targetPos, 0.1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.1f); 
            // while (Vector3.Distance(fakeBall.transform.position, targetPos) > 0.001f)
            // {
            //     fakeBall.transform.position = Vector3.MoveTowards(fakeBall.transform.position, targetPos, 20 * Time.deltaTime);
            //     yield return null;
            // }
        }
        yield return null;
        Destroy(fakeBall.gameObject);
        bottleGraphics[command.toBottleIndex].SetGraphic(command.toBallIndex, command.type);
        isSwapping = false;
        yield return new WaitForSeconds(0.18f);
    }
}
