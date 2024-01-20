using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MotionCurve", menuName = "MotionCurve")]
public class MotionCurve : ScriptableObject
{
    public AnimationCurve
        acceleration = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)),
        deceleration = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
    [SerializeField] AnimationCurve timeline;

    private void OnValidate()
    {
        // Refresh Motion Curve
        {
            timeline = new AnimationCurve();

            // Acceleration
            foreach (var k in acceleration.keys)
            {
                timeline.AddKey(k);
            }

            // Deceleration
            float decelStartTime = acceleration.GetDuration() + 0.5f;
            Keyframe[] decelKeys = deceleration.keys;
            for (int i = 0; i < decelKeys.Length; i++)
            {
                decelKeys[i].time += decelStartTime;
                timeline.AddKey(decelKeys[i]);
            }

            // Fix Max Speed Line
            AnimationUtility.SetKeyRightTangentMode(timeline, acceleration.length - 1, AnimationUtility.TangentMode.Constant);
        }
    }
}

[System.Serializable]
public struct MotionController
{
    [SerializeField] MotionCurve curve;

    [SerializeField] float speedMax;
    float speedPercent;
    public float Speed => speedMax * speedPercent;

    float time;
    bool wasMoving;
    public void Update(bool isMoving)
    {
        AnimationCurve currentCurve = isMoving
            ? curve.acceleration
            : curve.deceleration;

        // Switch Curves
        if (isMoving != wasMoving)
        {
            time = currentCurve.GetTime(speedPercent);
        }
        time = Mathf.Clamp(time + Time.deltaTime, 0, currentCurve.GetDuration());
        speedPercent = currentCurve.Evaluate(time);

        wasMoving = isMoving;
    }
}