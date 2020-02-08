using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolate
{

    public enum EaseType
    {
        Linear,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc
    }

    static Vector3 Identity(Vector3 v)
    {
        return v;
    }

    static Vector3 TransformDotPosition(Transform t)
    {
        return t.position;
    }


    static IEnumerable<float> NewTimer(float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            yield return elapsedTime;
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                yield return elapsedTime;
            }
        }
    }

    public delegate Vector3 ToVector3<T>(T v);
    public delegate float Function(float a, float b, float c, float d);

    static IEnumerable<float> NewCounter(int start, int end, int step)
    {
        for (int i = start; i <= end; i += step)
        {
            yield return i;
        }
    }

    public static IEnumerator NewEase(Function ease, Vector3 start, Vector3 end, float duration)
    {
        IEnumerable<float> timer = Interpolate.NewTimer(duration);
        return NewEase(ease, start, end, duration, timer);
    }

    public static IEnumerator NewEase(Function ease, Vector3 start, Vector3 end, int slices)
    {
        IEnumerable<float> counter = Interpolate.NewCounter(0, slices + 1, 1);
        return NewEase(ease, start, end, slices + 1, counter);
    }

    static IEnumerator NewEase(Function ease, Vector3 start, Vector3 end, float total, IEnumerable<float> driver)
    {
        Vector3 distance = end - start;
        foreach (float i in driver)
        {
            yield return Ease(ease, start, distance, i, total);
        }
    }

    static Vector3 Ease(Function ease, Vector3 start, Vector3 distance, float elapsedTime, float duration)
    {
        start.x = ease(start.x, distance.x, elapsedTime, duration);
        start.y = ease(start.y, distance.y, elapsedTime, duration);
        start.z = ease(start.z, distance.z, elapsedTime, duration);
        return start;
    }

    public static Function Ease(EaseType type)
    {

        Function f = null;
        switch (type)
        {
            case EaseType.Linear: f = Interpolate.Linear; break;
            case EaseType.EaseInQuad: f = Interpolate.EaseInQuad; break;
            case EaseType.EaseOutQuad: f = Interpolate.EaseOutQuad; break;
            case EaseType.EaseInOutQuad: f = Interpolate.EaseInOutQuad; break;
            case EaseType.EaseInCubic: f = Interpolate.EaseInCubic; break;
            case EaseType.EaseOutCubic: f = Interpolate.EaseOutCubic; break;
            case EaseType.EaseInOutCubic: f = Interpolate.EaseInOutCubic; break;
            case EaseType.EaseInQuart: f = Interpolate.EaseInQuart; break;
            case EaseType.EaseOutQuart: f = Interpolate.EaseOutQuart; break;
            case EaseType.EaseInOutQuart: f = Interpolate.EaseInOutQuart; break;
            case EaseType.EaseInQuint: f = Interpolate.EaseInQuint; break;
            case EaseType.EaseOutQuint: f = Interpolate.EaseOutQuint; break;
            case EaseType.EaseInOutQuint: f = Interpolate.EaseInOutQuint; break;
            case EaseType.EaseInSine: f = Interpolate.EaseInSine; break;
            case EaseType.EaseOutSine: f = Interpolate.EaseOutSine; break;
            case EaseType.EaseInOutSine: f = Interpolate.EaseInOutSine; break;
            case EaseType.EaseInExpo: f = Interpolate.EaseInExpo; break;
            case EaseType.EaseOutExpo: f = Interpolate.EaseOutExpo; break;
            case EaseType.EaseInOutExpo: f = Interpolate.EaseInOutExpo; break;
            case EaseType.EaseInCirc: f = Interpolate.EaseInCirc; break;
            case EaseType.EaseOutCirc: f = Interpolate.EaseOutCirc; break;
            case EaseType.EaseInOutCirc: f = Interpolate.EaseInOutCirc; break;
        }
        return f;
    }

    public static IEnumerable<Vector3> NewBezier(Function ease, Transform[] nodes, float duration)
    {
        IEnumerable<float> timer = Interpolate.NewTimer(duration);
        return NewBezier<Transform>(ease, nodes, TransformDotPosition, duration, timer);
    }

    public static IEnumerable<Vector3> NewBezier(Function ease, Transform[] nodes, int slices)
    {
        IEnumerable<float> counter = NewCounter(0, slices + 1, 1);
        return NewBezier<Transform>(ease, nodes, TransformDotPosition, slices + 1, counter);
    }

    public static IEnumerable<Vector3> NewBezier(Function ease, Vector3[] points, float duration)
    {
        IEnumerable<float> timer = NewTimer(duration);
        return NewBezier<Vector3>(ease, points, Identity, duration, timer);
    }

    public static IEnumerable<Vector3> NewBezier(Function ease, Vector3[] points, int slices)
    {
        IEnumerable<float> counter = NewCounter(0, slices + 1, 1);
        return NewBezier<Vector3>(ease, points, Identity, slices + 1, counter);
    }

    static IEnumerable<Vector3> NewBezier<T>(Function ease, IList nodes, ToVector3<T> toVector3, float maxStep, IEnumerable<float> steps)
    {
        if (nodes.Count >= 2)
        {
            Vector3[] points = new Vector3[nodes.Count];
            foreach (float step in steps)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    points[i] = toVector3((T)nodes[i]);
                }
                yield return Bezier(ease, points, step, maxStep);
            }
        }
    }

    static Vector3 Bezier(Function ease, Vector3[] points, float elapsedTime, float duration)
    {
        for (int j = points.Length - 1; j > 0; j--)
        {
            for (int i = 0; i < j; i++)
            {
                points[i].x = ease(points[i].x, points[i + 1].x - points[i].x, elapsedTime, duration);
                points[i].y = ease(points[i].y, points[i + 1].y - points[i].y, elapsedTime, duration);
                points[i].z = ease(points[i].z, points[i + 1].z - points[i].z, elapsedTime, duration);
            }
        }
        return points[0];
    }

    public static IEnumerable<Vector3> NewCatmullRom(Transform[] nodes, int slices, bool loop)
    {
        return NewCatmullRom<Transform>(nodes, TransformDotPosition, slices, loop);
    }

    public static IEnumerable<Vector3> NewCatmullRom(Vector3[] points, int slices, bool loop)
    {
        return NewCatmullRom<Vector3>(points, Identity, slices, loop);
    }

    static IEnumerable<Vector3> NewCatmullRom<T>(IList nodes, ToVector3<T> toVector3, int slices, bool loop)
    {
        if (nodes.Count >= 2)
        {
            yield return toVector3((T)nodes[0]);

            int last = nodes.Count - 1;
            for (int current = 0; loop || current < last; current++)
            {
                if (loop && current > last)
                {
                    current = 0;
                }
                int previous = (current == 0) ? ((loop) ? last : current) : current - 1;
                int start = current;
                int end = (current == last) ? ((loop) ? 0 : current) : current + 1;
                int next = (end == last) ? ((loop) ? 0 : end) : end + 1;
                int stepCount = slices + 1;
                for (int step = 1; step <= stepCount; step++)
                {
                    yield return CatmullRom(toVector3((T)nodes[previous]),
                                     toVector3((T)nodes[start]),
                                     toVector3((T)nodes[end]),
                                     toVector3((T)nodes[next]),
                                     step, stepCount);
                }
            }
        }
    }

    static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime, float duration)
    {
        float percentComplete = elapsedTime / duration;
        float percentCompleteSquared = percentComplete * percentComplete;
        float percentCompleteCubed = percentCompleteSquared * percentComplete;

        return previous * (-0.5f * percentCompleteCubed +
                                   percentCompleteSquared -
                            0.5f * percentComplete) +
                start * (1.5f * percentCompleteCubed +
                           -2.5f * percentCompleteSquared + 1.0f) +
                end * (-1.5f * percentCompleteCubed +
                            2.0f * percentCompleteSquared +
                            0.5f * percentComplete) +
                next * (0.5f * percentCompleteCubed -
                            0.5f * percentCompleteSquared);
    }

    static float Linear(float start, float distance, float elapsedTime, float duration)
    {
        if (elapsedTime > duration) { elapsedTime = duration; }
        return distance * (elapsedTime / duration) + start;
    }

    static float EaseInQuad(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        return distance * elapsedTime * elapsedTime + start;
    }

    static float EaseOutQuad(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        return -distance * elapsedTime * (elapsedTime - 2) + start;
    }

    static float EaseInOutQuad(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 2.0f : elapsedTime / (duration / 2);
        if (elapsedTime < 1) return distance / 2 * elapsedTime * elapsedTime + start;
        elapsedTime--;
        return -distance / 2 * (elapsedTime * (elapsedTime - 2) - 1) + start;
    }

    static float EaseInCubic(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        return distance * elapsedTime * elapsedTime * elapsedTime + start;
    }

    static float EaseOutCubic(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        elapsedTime--;
        return distance * (elapsedTime * elapsedTime * elapsedTime + 1) + start;
    }

    static float EaseInOutCubic(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 2.0f : elapsedTime / (duration / 2);
        if (elapsedTime < 1) return distance / 2 * elapsedTime * elapsedTime * elapsedTime + start;
        elapsedTime -= 2;
        return distance / 2 * (elapsedTime * elapsedTime * elapsedTime + 2) + start;
    }

    static float EaseInQuart(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
    }

    static float EaseOutQuart(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        elapsedTime--;
        return -distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 1) + start;
    }

    static float EaseInOutQuart(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 2.0f : elapsedTime / (duration / 2);
        if (elapsedTime < 1) return distance / 2 * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
        elapsedTime -= 2;
        return -distance / 2 * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 2) + start;
    }

    static float EaseInQuint(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
    }

    static float EaseOutQuint(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        elapsedTime--;
        return distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 1) + start;
    }

    static float EaseInOutQuint(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 2.0f : elapsedTime / (duration / 2f);
        if (elapsedTime < 1) return distance / 2 * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
        elapsedTime -= 2;
        return distance / 2 * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 2) + start;
    }

    static float EaseInSine(float start, float distance, float elapsedTime, float duration)
    {
        if (elapsedTime > duration) { elapsedTime = duration; }
        return -distance * Mathf.Cos(elapsedTime / duration * (Mathf.PI / 2)) + distance + start;
    }

    static float EaseOutSine(float start, float distance, float elapsedTime, float duration)
    {
        if (elapsedTime > duration) { elapsedTime = duration; }
        return distance * Mathf.Sin(elapsedTime / duration * (Mathf.PI / 2)) + start;
    }

    static float EaseInOutSine(float start, float distance, float elapsedTime, float duration)
    {
        if (elapsedTime > duration) { elapsedTime = duration; }
        return -distance / 2 * (Mathf.Cos(Mathf.PI * elapsedTime / duration) - 1) + start;
    }

    static float EaseInExpo(float start, float distance, float elapsedTime, float duration)
    {
        if (elapsedTime > duration) { elapsedTime = duration; }
        return distance * Mathf.Pow(2, 10 * (elapsedTime / duration - 1)) + start;
    }

    static float EaseOutExpo(float start, float distance, float elapsedTime, float duration)
    {
        if (elapsedTime > duration) { elapsedTime = duration; }
        return distance * (-Mathf.Pow(2, -10 * elapsedTime / duration) + 1) + start;
    }

    static float EaseInOutExpo(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 2.0f : elapsedTime / (duration / 2);
        if (elapsedTime < 1) return distance / 2 * Mathf.Pow(2, 10 * (elapsedTime - 1)) + start;
        elapsedTime--;
        return distance / 2 * (-Mathf.Pow(2, -10 * elapsedTime) + 2) + start;
    }

    static float EaseInCirc(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        return -distance * (Mathf.Sqrt(1 - elapsedTime * elapsedTime) - 1) + start;
    }

    static float EaseOutCirc(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
        elapsedTime--;
        return distance * Mathf.Sqrt(1 - elapsedTime * elapsedTime) + start;
    }

    static float EaseInOutCirc(float start, float distance, float elapsedTime, float duration)
    {
        elapsedTime = (elapsedTime > duration) ? 2.0f : elapsedTime / (duration / 2);
        if (elapsedTime < 1) return -distance / 2 * (Mathf.Sqrt(1 - elapsedTime * elapsedTime) - 1) + start;
        elapsedTime -= 2;
        return distance / 2 * (Mathf.Sqrt(1 - elapsedTime * elapsedTime) + 1) + start;
    }
}
