using UnityEngine;

public static class Extensions
{
    public static Vector3 AddScalar(this Vector3 vector, float x = 0, float y = 0, float z = 0)
    {
        vector.x += x;
        vector.y += y;
        vector.z += z;
        return vector;
    }

    public static void AddScalar(this ref Vector2 vector, float x = 0, float y = 0)
    {
        vector.x += x;
        vector.y += y;
    }

    public static void SetAnchors(this RectTransform rect, Vector2 anchor)
    {
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
    }
}