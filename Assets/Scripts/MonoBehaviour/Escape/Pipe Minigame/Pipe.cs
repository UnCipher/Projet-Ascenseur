using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    PipeGameController controller;

    [Header("Pipe Properties")]
    public PipeType pipeType;
    [Space(5)]

    [SerializeField] Vector2Int tilePosition;
    [SerializeField] Vector2Int visualTilePosition;
    [SerializeField] PipeRotation pipeRotation;
    [Space(5)]

    [SerializeField] float turnDuration = .125f;
    [SerializeField] float turnAnimation = .175f;

    [SerializeField] bool isActive;
    [SerializeField] bool isAlwaysActive;
    [SerializeField] bool cantChange;
    [SerializeField] bool required;
    [Space(5)]

    [SerializeField] Connectable connectable;

    [Header("Audio Profiles")]
    [SerializeField] SoundProfile onRotate;
    [SerializeField] SoundProfile onActivation;
    [SerializeField] SoundProfile onDeactivation;

    [Header("References")]
    [SerializeField] Transform visual;
    [Space(5)]

    [SerializeField] GameObject offModel;
    [SerializeField] GameObject onModel;
    [Space(50)]

    [SerializeField] bool test;
    bool turning;

    // Classes
    // ---------------------------

    public enum PipeType
    {
        OneWay,
        TwoWay,
        TwoWayCorner,
        ThreeWay,
        FourWay,
    }

    public enum PipeRotation
    {
        Up,
        Right,
        Down,
        Left,
    }

    [System.Serializable]
    public class Connectable
    {
        public bool up;
        public bool right;
        public bool down;
        public bool left;
    }

    // ---------------------------
    // Functions
    // ---------------------------

    void FixedUpdate()
    {
        if(test)
        {
            test = false;
            RotatePipe();
        }
    }

    public void SetValueOnPipe(PipeMiniGameProfile.PipeProperties profile, PipeGameController pipeController)
    {
        // Set Values
        controller = pipeController;
        controller.AddToPipeList(this);
        
        tilePosition = profile.tilePosition;
        visualTilePosition = profile.visualTilePosition;
        pipeRotation = profile.pipeRotation;

        isAlwaysActive = profile.isAlwaysActive;
        required = profile.required;

        // Conditionnal Values
        if (isAlwaysActive || required)
            cantChange = true;

        if (isAlwaysActive)
            isActive = true;

        // Change Visual
        offModel.SetActive(!isActive);
        onModel.SetActive(isActive);

        // Call Functions
        UpdateConnectivity();
        SetTransform();
    }

    void SetTransform()
    {
        // Set Rotation 
        visual.localEulerAngles = new Vector3(0, 0, (int)pipeRotation * -90);

        // Set Position
        transform.localPosition = new Vector3(visualTilePosition.x, visualTilePosition.y, 0);
    }

    public void CheckForConnections(ref List<Pipe> activePipes)
    {
        // Up
        if (controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, GetRotationWithOffset(PipeRotation.Up, (int)pipeRotation))) != null)
        {
            PipeRotation direction = GetRotationWithOffset(PipeRotation.Up, (int)pipeRotation);
            Pipe pipe = controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, direction));
            if (pipe.GetConnectivity(direction))
            {
                if(!activePipes.Contains(pipe))
                {
                    activePipes.Add(pipe);
                    pipe.CheckForConnections(ref activePipes);
                }
            }
        }

        // Right
        if (pipeType == PipeType.TwoWayCorner || pipeType == PipeType.ThreeWay || pipeType == PipeType.FourWay)
        {
            if (controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, GetRotationWithOffset(PipeRotation.Right, (int)pipeRotation))) != null)
            {
                PipeRotation direction = GetRotationWithOffset(PipeRotation.Right, (int)pipeRotation);
                Pipe pipe = controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, direction));
                if (pipe.GetConnectivity(direction))
                {
                    if(!activePipes.Contains(pipe))
                    {
                        activePipes.Add(pipe);
                        pipe.CheckForConnections(ref activePipes);
                    }
                }
            }
        }

        // Down
        if (pipeType == PipeType.TwoWay || pipeType == PipeType.ThreeWay || pipeType == PipeType.FourWay)
        {
            if (controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, GetRotationWithOffset(PipeRotation.Down, (int)pipeRotation))) != null)
            {
                PipeRotation direction = GetRotationWithOffset(PipeRotation.Down, (int)pipeRotation);
                Pipe pipe = controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, direction));
                if (pipe.GetConnectivity(direction))
                {
                    if(!activePipes.Contains(pipe))
                    {
                        activePipes.Add(pipe);
                        pipe.CheckForConnections(ref activePipes);
                    }
                }
            }
        }

        // Left
        if (pipeType == PipeType.FourWay)
        {
            if (controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, GetRotationWithOffset(PipeRotation.Left, (int)pipeRotation))) != null)
            {
                PipeRotation direction = GetRotationWithOffset(PipeRotation.Left, (int)pipeRotation);
                Pipe pipe = controller.GetPipeAt(GetTilePositionBasedOnDirection(tilePosition, direction));
                if (pipe.GetConnectivity(direction))
                {
                    if(!activePipes.Contains(pipe))
                    {
                        activePipes.Add(pipe);
                        pipe.CheckForConnections(ref activePipes);
                    }
                }
            }
        }
    }

    public void Activate()
    {
        if(!isActive)
        {
            // Set Value
            isActive = true;

            // Create Sound
            SoundPlayer.CreateSoundPlayer(onActivation, transform);

            // Change Visual
            offModel.SetActive(!isActive);
            onModel.SetActive(isActive);
        }
    }
    
    public void Desactivate()
    {
        if(isActive)
        {
            // Set Value
            isActive = false;

            // Create Sound
            SoundPlayer.CreateSoundPlayer(onDeactivation, transform);

            // Change Visual
            offModel.SetActive(!isActive);
            onModel.SetActive(isActive);
        }
    }

    public void RotatePipe()
    {
        if (!cantChange && controller.GetIsActive())
        {
            StopAllCoroutines();
            StartCoroutine("Rotation");
        }
    }
    
    IEnumerator Rotation()
    {
        // Set Values
        pipeRotation = GetRotationWithOffset(pipeRotation, 1);

        // Set Rotation
        Transition.AngleTransition transitionParam = new Transition.AngleTransition()
        {
            newValue = new Vector3(0, 0, (int)pipeRotation * -90),
            curve = AnimationCurve.EaseInOut(0,0,1,1),
            duration = turnAnimation,
        };

        Transition.StartRotationTransition(visual, transitionParam, true);

        yield return new WaitForSeconds(turnDuration);

        // Create Sound
        SoundPlayer.CreateSoundPlayer(onRotate, transform);

        // Call For Update
        UpdateConnectivity();
        controller.UpdatePipes();
    }

    void UpdateConnectivity()
    {
        // Reset Values
        connectable.up = false;
        connectable.right = false;
        connectable.down = false;
        connectable.left = false;

        // Up
        GetConnectivityReference(GetRotationWithOffset(PipeRotation.Up, (int)pipeRotation)) = true;

        // Right
        if (pipeType == PipeType.TwoWayCorner || pipeType == PipeType.ThreeWay || pipeType == PipeType.FourWay)
        GetConnectivityReference(GetRotationWithOffset(PipeRotation.Right, (int)pipeRotation)) = true;

        // Down
        if (pipeType == PipeType.TwoWay || pipeType == PipeType.ThreeWay || pipeType == PipeType.FourWay)
        GetConnectivityReference(GetRotationWithOffset(PipeRotation.Down, (int)pipeRotation)) = true;

        // Left
        if (pipeType == PipeType.FourWay)
        GetConnectivityReference(GetRotationWithOffset(PipeRotation.Left, (int)pipeRotation)) = true;
    }

    // Set Functions
    // ---------------------------

    // Get Functions
    // ---------------------------
 
    Vector2Int GetTilePositionBasedOnDirection(Vector2Int tilePos, PipeRotation direction)
    {
        // Set Value
        Vector2Int position = Vector2Int.left;

        if (direction == PipeRotation.Up)
            position = Vector2Int.up;

        else if (direction == PipeRotation.Right)
            position = Vector2Int.right;

        else if (direction == PipeRotation.Down)
            position = Vector2Int.down;

        // Return Position
        return tilePos + position;
    }

    PipeRotation GetRotationWithOffset(PipeRotation rotation, int offset)
    {
        // If Outside of Bound
        int value = ((int)rotation + offset) % 4;
        if (value < 0)
            value += 4;

        // Return Value
        return (PipeRotation)value;
    }

    public bool GetConnectivity(PipeRotation direction)
    {
        // Return Value
        if (direction == PipeRotation.Up)
            return connectable.down;

        else if (direction == PipeRotation.Right)
            return connectable.left;

        else if (direction == PipeRotation.Down)
            return connectable.up;

        else
            return connectable.right;

    }

    public ref bool GetConnectivityReference(PipeRotation direction)
    {
        if(direction == PipeRotation.Up)
            return ref connectable.up;

        else if(direction == PipeRotation.Right)
            return ref connectable.right;

        else if(direction == PipeRotation.Down)
            return ref connectable.down;

        else
            return ref connectable.left;
    }

    public bool GetIsActive()
    {
        // Return Value
        return isActive;
    }

    public bool GetIsAlwaysActive()
    {
        // Return Value
        return isAlwaysActive;
    }

    public bool GetIsRequired()
    {
        // Return Value
        return required;
    }

    public Vector2Int GetTilePosition()
    {
        // Return Value
        return tilePosition;
    }
}