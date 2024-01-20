using UnityEngine;

public static class AnimationCurveExtensions
{
    public static float GetDuration(this AnimationCurve curve) => curve[curve.keys.Length - 1].time;

/*    public static float GetTime(AnimationCurve curve, float value, float step = 0.01f)
    {
        float threshold = 0.01f;
        float duration = GetDuration(curve);
        for (float time = 0; time < duration; time += step)
        {
            if (Mathf.Abs(value - curve.Evaluate(time)) <= threshold)
                return time;
        }

        Debug.LogWarning("Time not found!");
        return GetTime(curve, value, step / 2f);
    }*/

    public static float GetTime(this AnimationCurve curve, float value)
    {
        bool accelerating = curve[0].value < curve[curve.length - 1].value;

        const float MAX_DISTANCE = 0.01f;

        float start = 0f, end = GetDuration(curve);
        while (start <= end)
        {
            float time = (start + end) / 2f;
            float currentValue = curve.Evaluate(time);
            if (Mathf.Abs(value - currentValue) <= MAX_DISTANCE)
                return time;
            else if (currentValue < value)
            {
                if (accelerating)
                {
                    start = time;
                }
                else
                {
                    end = time;
                }
            }
            else if (currentValue > value)
            {
                if (accelerating)
                {
                    end = time;
                }
                else
                {
                    start = time;
                }
            }
        }

        Debug.LogWarning("Time not found!");
        return -1f;
    }
}