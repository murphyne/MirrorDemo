using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Mirror : MonoBehaviour
{
    [SerializeField] private MeshRenderer mirrorRenderer;

    private readonly Dictionary<Camera, Camera> _companionCameras =
        new Dictionary<Camera, Camera>();

    private readonly Dictionary<Camera, RenderTexture> _renderTextures =
        new Dictionary<Camera, RenderTexture>();

    private void OnDisable()
    {
        foreach (var pair in _companionCameras)
        {
            var companionCamera = pair.Value;
            DestroyImmediate(companionCamera.gameObject);
        }

        _companionCameras.Clear();

        foreach (var pair in _renderTextures)
        {
            var renderTexture = pair.Value;
            renderTexture.Release();
            renderTexture.DiscardContents();
        }

        _renderTextures.Clear();
    }

    private void OnWillRenderObject()
    {
        var currentCamera = Camera.current;

#if UNITY_EDITOR
        var sceneView = UnityEditor.SceneView.currentDrawingSceneView;
        var sceneViewCamera = (sceneView == null) ? null : sceneView.camera;
        if (!currentCamera.enabled && currentCamera != sceneViewCamera) return;
#else
        if (!currentCamera.enabled) return;
#endif

        var companionCamera = GetCompanionCamera(currentCamera);
        var renderTexture = GetRenderTexture(currentCamera);

        companionCamera.CopyFrom(currentCamera);
        companionCamera.targetTexture = renderTexture;
        mirrorRenderer.sharedMaterial.mainTexture = renderTexture;

        MirrorTransform(currentCamera, companionCamera, mirrorRenderer.transform);

        if (renderTexture.width != Screen.width || renderTexture.height != Screen.height)
        {
            Debug.Log($"Resize texture {Screen.width}x{Screen.height}");
            renderTexture.Release();
            // renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            renderTexture.width = Screen.width;
            renderTexture.height = Screen.height;
            renderTexture.Create();
        }

        GL.invertCulling = true;
        companionCamera.Render();
        GL.invertCulling = false;
    }

    private Camera GetCompanionCamera(Camera originalCamera)
    {
        if (_companionCameras.TryGetValue(originalCamera, out var companionCamera))
            if (companionCamera != null)
                return companionCamera;

        companionCamera = CreateCompanionCamera(originalCamera);
        _companionCameras[originalCamera] = companionCamera;
        return companionCamera;
    }

    private Camera CreateCompanionCamera(Camera originalCamera)
    {
        var companionCameraGameObject = new GameObject();
        companionCameraGameObject.name = $"Mirror Camera [{originalCamera.name}]";
        companionCameraGameObject.transform.parent = transform;
        companionCameraGameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

        var companionCamera = companionCameraGameObject.AddComponent<Camera>();
        companionCamera.enabled = false;

        return companionCamera;
    }

    private RenderTexture GetRenderTexture(Camera originalCamera)
    {
        if (_renderTextures.TryGetValue(originalCamera, out var renderTexture))
            if (renderTexture != null)
                return renderTexture;

        renderTexture = CreateRenderTexture(originalCamera);
        _renderTextures[originalCamera] = renderTexture;
        return renderTexture;
    }

    private static RenderTexture CreateRenderTexture(Camera originalCamera)
    {
        // Debug.Log($"Create texture {Screen.width}x{Screen.height}");
        var renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        renderTexture.name = $"Mirror Texture [{originalCamera.name}]";
        // renderTexture.Create();

        return renderTexture;
    }

    public static void MirrorTransform(Camera sourceCamera, Camera targetCamera, Transform mirrorTransform)
    {
        var point = mirrorTransform.position;
        var normal = mirrorTransform.TransformDirection(Vector3.back);

        var mirrorPlane = GetPlane(point, normal);
        var mirrorMatrix = GetMirrorMatrix(mirrorPlane);
        targetCamera.worldToCameraMatrix = sourceCamera.worldToCameraMatrix * mirrorMatrix;

        var pointCamera = targetCamera.worldToCameraMatrix.MultiplyPoint(point);
        var normalCamera = targetCamera.worldToCameraMatrix.MultiplyVector(normal).normalized;
        var mirrorPlaneClip = GetPlane(pointCamera, normalCamera);
        var projectionMatrix = sourceCamera.CalculateObliqueMatrix(mirrorPlaneClip);
        targetCamera.projectionMatrix = projectionMatrix;
    }

    public static Matrix4x4 GetMirrorMatrix(Vector4 plane)
    {
        var mirrorMatrix = new Matrix4x4();

        mirrorMatrix.m00 = (1f - 2f * plane[0] * plane[0]);
        mirrorMatrix.m01 = (    -2f * plane[0] * plane[1]);
        mirrorMatrix.m02 = (    -2f * plane[0] * plane[2]);
        mirrorMatrix.m03 = (    -2f * plane[0] * plane[3]);

        mirrorMatrix.m10 = (    -2f * plane[1] * plane[0]);
        mirrorMatrix.m11 = (1f - 2f * plane[1] * plane[1]);
        mirrorMatrix.m12 = (    -2f * plane[1] * plane[2]);
        mirrorMatrix.m13 = (    -2f * plane[1] * plane[3]);

        mirrorMatrix.m20 = (    -2f * plane[2] * plane[0]);
        mirrorMatrix.m21 = (    -2f * plane[2] * plane[1]);
        mirrorMatrix.m22 = (1f - 2f * plane[2] * plane[2]);
        mirrorMatrix.m23 = (    -2f * plane[2] * plane[3]);

        mirrorMatrix.m30 = 0f;
        mirrorMatrix.m31 = 0f;
        mirrorMatrix.m32 = 0f;
        mirrorMatrix.m33 = 1f;

        return mirrorMatrix;
    }

    public static Vector4 GetPlane(Vector3 point, Vector3 normal)
    {
        var d = -Vector3.Dot(normal, point);
        var plane = new Vector4(normal.x, normal.y, normal.z, d);
        return plane;
    }
}
