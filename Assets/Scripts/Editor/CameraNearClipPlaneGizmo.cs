using UnityEditor;
using UnityEngine;

public static class CameraNearClipPlaneGizmo
{
    private const int XWest = -1, XCenter = 0, XEast = 1;
    private const int YSouth = -1, YCenter = 0, YNorth = 1;
    private const int ZNear = -1;

    private static readonly Vector3 Point00 = new Vector3(XCenter, YCenter, ZNear);
    private static readonly Vector3 Point0W = new Vector3(XWest, YCenter, ZNear);
    private static readonly Vector3 Point0E = new Vector3(XEast, YCenter, ZNear);
    private static readonly Vector3 PointS0 = new Vector3(XCenter, YSouth, ZNear);
    private static readonly Vector3 PointN0 = new Vector3(XCenter, YNorth, ZNear);
    private static readonly Vector3 PointSW = new Vector3(XWest, YSouth, ZNear);
    private static readonly Vector3 PointNW = new Vector3(XWest, YNorth, ZNear);
    private static readonly Vector3 PointNE = new Vector3(XEast, YNorth, ZNear);
    private static readonly Vector3 PointSE = new Vector3(XEast, YSouth, ZNear);

    [DrawGizmo(GizmoType.Selected)]
    private static void DrawSelectedCamera(Camera camera, GizmoType gizmoType)
    {
        var hashCode = camera.GetInstanceID();

        Gizmos.color = FromRgbHex(hashCode);

        DrawNearClipPlane(camera);
    }

    private static void DrawNearClipPlane(Camera camera)
    {
        var p00 = ClipToWorldPoint(Point00, camera);
        var p0W = ClipToWorldPoint(Point0W, camera);
        var p0E = ClipToWorldPoint(Point0E, camera);
        var pS0 = ClipToWorldPoint(PointS0, camera);
        var pN0 = ClipToWorldPoint(PointN0, camera);
        var pSW = ClipToWorldPoint(PointSW, camera);
        var pNW = ClipToWorldPoint(PointNW, camera);
        var pNE = ClipToWorldPoint(PointNE, camera);
        var pSE = ClipToWorldPoint(PointSE, camera);

        // Draw horizontal lines.
        // Gizmos.DrawLine(p00, p0W);
        // Gizmos.DrawLine(p00, p0E);

        // Draw vertical lines.
        // Gizmos.DrawLine(p00, pS0);
        // Gizmos.DrawLine(p00, pN0);

        // Draw diagonal lines.
        Gizmos.DrawLine(p00, pSW);
        Gizmos.DrawLine(p00, pNW);
        Gizmos.DrawLine(p00, pNE);
        Gizmos.DrawLine(p00, pSE);

        // Draw frame.
        Gizmos.DrawLine(p0W, pSW);
        Gizmos.DrawLine(p0W, pNW);
        Gizmos.DrawLine(p0E, pSE);
        Gizmos.DrawLine(p0E, pNE);
        Gizmos.DrawLine(pS0, pSW);
        Gizmos.DrawLine(pS0, pSE);
        Gizmos.DrawLine(pN0, pNW);
        Gizmos.DrawLine(pN0, pNE);
    }

    private static Vector3 ClipToWorldPoint(Vector3 point, Camera camera)
    {
        var point4 = new Vector4(point.x, point.y, point.z, w: 1);
        var pointWorld = camera.projectionMatrix.inverse * point4;
        pointWorld /= pointWorld.w;
        return camera.cameraToWorldMatrix.MultiplyPoint3x4(pointWorld);
    }

    private static Color32 FromRgbHex(int rgb)
    {
        byte r = (byte)(rgb >> 16 & 0xff);
        byte g = (byte)(rgb >>  8 & 0xff);
        byte b = (byte)(rgb >>  0 & 0xff);
        var color = new Color32(r, g, b, 0xff);
        return color;
    }
}
