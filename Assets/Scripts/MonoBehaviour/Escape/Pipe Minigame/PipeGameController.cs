using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PipeGameController : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    bool listeningForEvents;
    [SerializeField] List<Pipe> pipes = new List<Pipe>();

    [Header("Game Controller")]
    [SerializeField] PipeMiniGameProfile profile;
    [Space(10)]

    [SerializeField] UnityEvent onCompletion;
    [SerializeField] UnityEvent onFail;

    [Header("Prefabs References")]
    [SerializeField] Transform leftWall;
    [SerializeField] Transform centerWall;
    [SerializeField] Transform rightWall;
    [Space(10)]

    [SerializeField] GameObject oneWay;
    [SerializeField] GameObject twoWayStraight;
    [SerializeField] GameObject twoWayCorner;
    [SerializeField] GameObject threeWay;
    [SerializeField] GameObject fourWay;

    [Header("Audio Profiles")]
    [SerializeField] SoundProfile powerOnProfile;
    [SerializeField] SoundProfile explosionProfile;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] string fireTrigger;

    // ---------------------------
    // Functions
    // ---------------------------

    public void StartMiniGame()
    {
        // Set Value
        pipes = new List<Pipe>();

        // Play Animation
        animator.SetTrigger(fireTrigger);

        leftWall.gameObject.SetActive(true);
        centerWall.gameObject.SetActive(true);
        rightWall.gameObject.SetActive(true);

        // Create Pipes
        for (int i = 0; i < profile.pipes.Length; i++)
        {
            GameObject prefab = oneWay;
            Transform wall = leftWall;

            // Select Type
            if (profile.pipes[i].pipeType == Pipe.PipeType.TwoWay)
                prefab = twoWayStraight;

            else if (profile.pipes[i].pipeType == Pipe.PipeType.TwoWayCorner)
                prefab = twoWayCorner;

            else if (profile.pipes[i].pipeType == Pipe.PipeType.ThreeWay)
                prefab = threeWay;

            else if (profile.pipes[i].pipeType == Pipe.PipeType.FourWay)
                prefab = fourWay;

            // Select Wall
            if (profile.pipes[i].wall == PipeMiniGameProfile.SelectedWall.Center)
                wall = centerWall;

            else if (profile.pipes[i].wall == PipeMiniGameProfile.SelectedWall.Right)
                wall = rightWall;


            // Set Value
            GameObject pipeObject = Instantiate(prefab, wall);
            Pipe pipe = pipeObject.GetComponent<Pipe>();

            pipe.SetValueOnPipe(profile.pipes[i], this);
        }

        // Invoke Fail Mini Game
        Invoke("MiniGameFailed", profile.gameDuration);

        // Set Value
        listeningForEvents = true;
    }

    void MiniGameCompleted()
    {
        // Set Values
        listeningForEvents = false;
        SoundPlayer.CreateSoundPlayer(powerOnProfile);

        CancelInvoke("MiniGameFailed");

        // Invoke On Completion
        onCompletion?.Invoke();
    }
    
    void MiniGameFailed()
    {
        // Set Values
        listeningForEvents = false;
        SoundPlayer.CreateSoundPlayer(explosionProfile);

        // Invoke On Completion
        onFail?.Invoke();
    }

    public void UpdatePipes()
    {
        if(listeningForEvents)
        {
            // Get Values
            Pipe[] startPipes = GetPipeStartPoints();
            List<Pipe> activePipes = new List<Pipe>();
            activePipes.AddRange(startPipes);

            // Loop through all Connect Pipes to Start
            for (int i = 0; i < startPipes.Length; i++)
                startPipes[i].CheckForConnections(ref activePipes);

            // Set Active/Unactive all Pipes
            for (int i = 0; i < pipes.Count; i++)
            {
                if (activePipes.Contains(pipes[i]))
                    pipes[i].Activate();

                else
                    pipes[i].Desactivate();
            }

            // Check if Completed
            if (IsAllPipesActive(GetRequiredPipes()))
            {
                // Call Functions
                MiniGameCompleted();
                Debug.LogError("HOOOORAAAAYYYYY");
            }
        }
    }

    // Set Functions
    // ---------------------------

    public void AddToPipeList(Pipe pipe)
    {
        // Add Value
        if (!pipes.Contains(pipe))
            pipes.Add(pipe);
    }

    // Get Functions
    // ---------------------------

    Pipe[] GetRequiredPipes()
    {
        // Set Values
        List<Pipe> requiredPipes = new List<Pipe>();

        for (int i = 0; i < pipes.Count; i++)
            if (pipes[i].GetIsRequired())
                requiredPipes.Add(pipes[i]);

        return requiredPipes.ToArray();
    }

    Pipe[] GetPipeStartPoints()
    {
        // Set Values
        List<Pipe> startPipes = new List<Pipe>();

        for (int i = 0; i < pipes.Count; i++)
            if (pipes[i].GetIsAlwaysActive())
                startPipes.Add(pipes[i]);

        return startPipes.ToArray();
    }

    bool IsAllPipesActive(Pipe[] pipes)
    {
        // Set Value
        bool condition = true;

        for (int i = 0; i < pipes.Length; i++)
            if (!pipes[i].GetIsActive())
                condition = false;

        // Return Value
        return condition;
    }

    public bool GetIsActive()
    {
        // Return Value
        return listeningForEvents;
    }

    public Pipe GetPipeAt(Vector2Int coordinate)
    {
        // Set Value
        Pipe pipe = null;

        // Find Pipe
        for(int i = 0;i<pipes.Count;i++)
        if(pipes[i].GetTilePosition() == coordinate)
        pipe = pipes[i];

        // Return Value
        return pipe;
    }
}