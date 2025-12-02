using Unity.VisualScripting;
using UnityEngine;

public class Transition : MonoBehaviour
{
    // ----------------------------
    // Values
    // ----------------------------

    public TransitionType transitionType;
    public Transform toChange;
    public float endTime;
    public bool ended;
    public AnimationCurve curve;
    float currentTime;

    Quaternion startAngle;
    Vector3 startPosition;
    Vector3 startSize;

    Quaternion newAngle;
    Vector3 newPosition;
    Vector3 newSize;

    // Classes
    // ----------------------------

    public enum TransitionType
    {
        Position,
        Angle,
        Size,
    }

    [System.Serializable]
    public class FloatTransition
    {
        public float newValue;
        public float duration;
        public AnimationCurve curve;
    }

    [System.Serializable]
    public class Vector3Transition
    {
        public Vector3 newValue;
        public float duration;
        public AnimationCurve curve;
    }

    [System.Serializable]
    public class AngleTransition
    {
        public Vector3 newValue;
        public Vector3 valueOffset;
        public float duration;
        public AnimationCurve curve;
    }

    // ----------------------------
    // Functions
    // ----------------------------

    void Update()
    {
        // Set Values
        currentTime += Time.deltaTime;
        if (currentTime >= endTime)
        {
            currentTime = endTime;
            ended = true;
        }

        // Call Functions
        if (transitionType == TransitionType.Position)
            ChangePostion();

        else if (transitionType == TransitionType.Angle)
            ChangeAngle();

        else
            ChangeSize();

        // Check If Ended
        if (ended)
            Destroy(this);
    }

    // Transition Functions
    // ----------------------------

    void ChangePostion()
    {
        
    }

    void ChangeAngle()
    {
        // Set Values
        float lerpFraction = currentTime / endTime;
        toChange.rotation = Quaternion.Lerp(startAngle, newAngle, curve.Evaluate(lerpFraction));
    }

    void ChangeSize()
    {
        float lerpFraction = currentTime / endTime;
        toChange.localScale = Vector3.Lerp(startSize, newSize, curve.Evaluate(lerpFraction));
    }

    public void FinishTransition()
    {
        // Call Functions
        if (transitionType == TransitionType.Position)
        transform.position = newPosition;

        else if (transitionType == TransitionType.Angle)
        transform.rotation = newAngle;

        else
        transform.localScale = newSize;

        // Destroy Function
        Destroy(this);
    }

    // Get Functions
    // ----------------------------

    static Transition CheckIfSameTypeExist(Transition[] transitions, TransitionType type)
    {
        for (int i = 0; i < transitions.Length; i++)
            if (transitions[i].transitionType == type)
                return transitions[i];

        return null;
    }

    // Static Function
    // ----------------------------

    public static void StartRotationTransition(Transform toChange, AngleTransition transitionParam, bool overrideOldTransition = false)
    {
        // Set Values
        Transition newTransition;

        // Add new Transition Class
        if (toChange.GetComponents<Transition>().Length > 0)
        {
            // There is Already a Transition
            var transitions = toChange.GetComponents<Transition>();
            Transition oldTransition = CheckIfSameTypeExist(transitions, TransitionType.Angle);

            if (oldTransition != null)
            {
                if(overrideOldTransition)
                {
                    oldTransition.FinishTransition();
                    newTransition = toChange.AddComponent<Transition>();
                }
                
                else
                newTransition = oldTransition;
            }
                
            else
            newTransition = toChange.AddComponent<Transition>();
        }

        // No Transition Found
        else
            newTransition = toChange.AddComponent<Transition>();

        // Set Values
        newTransition.transitionType = TransitionType.Angle;
        newTransition.endTime = transitionParam.duration;
        newTransition.curve = transitionParam.curve;
        newTransition.toChange = toChange;
        newTransition.currentTime = 0;

        newTransition.startAngle = toChange.rotation;
        newTransition.newAngle = Quaternion.Euler(transitionParam.newValue + transitionParam.valueOffset);
    }
    
    public static void StartLookAtTransition(Transform toChange, AngleTransition transitionParam, bool overrideOldTransition = false)
    {
        // Set Values
        Transition newTransition;

        // Add new Transition Class
        if (toChange.GetComponents<Transition>().Length > 0)
        {
            // There is Already a Transition
            var transitions = toChange.GetComponents<Transition>();
            Transition oldTransition = CheckIfSameTypeExist(transitions, TransitionType.Angle);

            if (oldTransition != null)
            {
                if (overrideOldTransition)
                {
                    oldTransition.FinishTransition();
                    newTransition = toChange.AddComponent<Transition>();
                }

                else
                    newTransition = oldTransition;
            }

            else
                newTransition = toChange.AddComponent<Transition>();
        }

        // No Transition Found
        else
            newTransition = toChange.AddComponent<Transition>();

        // Set Values
        newTransition.transitionType = TransitionType.Angle;
        newTransition.endTime = transitionParam.duration;
        newTransition.curve = transitionParam.curve;
        newTransition.toChange = toChange;
        newTransition.currentTime = 0;

        newTransition.startAngle = toChange.rotation;
        Quaternion lookAt = Quaternion.LookRotation(transitionParam.newValue - toChange.position);
        newTransition.newAngle = Quaternion.Euler(lookAt.eulerAngles + transitionParam.valueOffset);
    }

    public static void StartSizeTransition(Transform toChange, Vector3Transition transitionParam)
    {
        // Set Values
        Transition newTransition;

        // Add new Transition Class
        if (toChange.GetComponents<Transition>().Length > 0)
        {
            // There is Already a Transition
            var transitions = toChange.GetComponents<Transition>();
            Transition oldTransition = CheckIfSameTypeExist(transitions, TransitionType.Size);

            if (oldTransition != null)
                newTransition = oldTransition;

            else
                newTransition = toChange.AddComponent<Transition>();
        }

        // No Transition Found
        else
            newTransition = toChange.AddComponent<Transition>();

        // Set Values
        newTransition.transitionType = TransitionType.Size;
        newTransition.endTime = transitionParam.duration;
        newTransition.curve = transitionParam.curve;
        newTransition.toChange = toChange;
        newTransition.currentTime = 0;

        newTransition.startSize = toChange.localScale;
        newTransition.newSize = transitionParam.newValue;
    }
}
