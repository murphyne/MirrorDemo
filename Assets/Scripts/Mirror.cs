using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Mirror : MonoBehaviour
{
    [SerializeField] private MeshRenderer mirrorRenderer;

    private readonly Dictionary<Camera, Camera> _mirrorCameras =
        new Dictionary<Camera, Camera>();

    private readonly Dictionary<Camera, RenderTexture> _renderTextures =
        new Dictionary<Camera, RenderTexture>();

    private void OnDisable()
    {
        foreach (var pair in _mirrorCameras)
        {
            var mirrorCamera = pair.Value;
            DestroyImmediate(mirrorCamera.gameObject);
        }

        _mirrorCameras.Clear();

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

        var mirrorCamera = GetMirrorCamera(currentCamera);
        var renderTexture = GetRenderTexture(currentCamera);

        mirrorCamera.CopyFrom(currentCamera);
        mirrorCamera.targetTexture = renderTexture;
        mirrorRenderer.sharedMaterial.mainTexture = renderTexture;

        MirrorTransform(currentCamera, mirrorCamera, mirrorRenderer.transform);

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
        mirrorCamera.Render();
        GL.invertCulling = false;
    }

    private Camera GetMirrorCamera(Camera camera)
    {
        if (_mirrorCameras.TryGetValue(camera, out var mirrorCamera))
            if (mirrorCamera != null)
                return mirrorCamera;

        mirrorCamera = CreateMirrorCamera(camera);
        _mirrorCameras[camera] = mirrorCamera;
        return mirrorCamera;
    }

    private Camera CreateMirrorCamera(Camera camera)
    {
        var mirrorCameraGameObject = new GameObject();
        mirrorCameraGameObject.name = $"Mirror Camera [{camera.name}]";
        mirrorCameraGameObject.transform.parent = transform;
        mirrorCameraGameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

        var mirrorCamera = mirrorCameraGameObject.AddComponent<Camera>();
        mirrorCamera.enabled = false;

        return mirrorCamera;
    }

    private RenderTexture GetRenderTexture(Camera camera)
    {
        if (_renderTextures.TryGetValue(camera, out var renderTexture))
            if (renderTexture != null)
                return renderTexture;

        renderTexture = CreateRenderTexture(camera);
        _renderTextures[camera] = renderTexture;
        return renderTexture;
    }

    private static RenderTexture CreateRenderTexture(Camera camera)
    {
        // Debug.Log($"Create texture {Screen.width}x{Screen.height}");
        var renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        renderTexture.name = $"Mirror Texture [{camera.name}]";
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
