public float GetCurveTimeForValue(AnimationCurve curveToCheck, float value, int accuracy)
{

    float startTime = curveToCheck.keys[0].time;
    float endTime = curveToCheck.keys[curveToCheck.length - 1].time;
    float nearestTime = startTime;
    float step = endTime - startTime;

    for (int i = 0; i < accuracy; i++)
    {

        float valueAtNearestTime = curveToCheck.Evaluate(nearestTime);
        float distanceToValueAtNearestTime = Mathf.Abs(value - valueAtNearestTime);

        float timeToCompare = nearestTime + step;
        float valueAtTimeToCompare = curveToCheck.Evaluate(timeToCompare);
        float distanceToValueAtTimeToCompare = Mathf.Abs(value - valueAtTimeToCompare);

        if (distanceToValueAtTimeToCompare < distanceToValueAtNearestTime)
        {
            nearestTime = timeToCompare;
            valueAtNearestTime = valueAtTimeToCompare;
        }
        step = Mathf.Abs(step * 0.5f) * Mathf.Sign(value - valueAtNearestTime);
    }

    return nearestTime;
}
