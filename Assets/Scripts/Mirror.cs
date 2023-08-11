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

        MirrorTransform(currentCamera.transform, mirrorCamera.transform, mirrorRenderer.transform);

        if (renderTexture.width != Screen.width || renderTexture.height != Screen.height)
        {
            Debug.Log($"Resize texture {Screen.width}x{Screen.height}");
            renderTexture.Release();
            // renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            renderTexture.width = Screen.width;
            renderTexture.height = Screen.height;
            renderTexture.Create();
        }

        mirrorCamera.Render();
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

    public static void MirrorTransform(Transform sourceTransform, Transform targetTransform, Transform mirrorTransform)
    {
        Matrix4x4 mirrorWorldToLocal = mirrorTransform.worldToLocalMatrix;
        Matrix4x4 mirrorLocalToWorld = mirrorTransform.localToWorldMatrix;
        Matrix4x4 matrixScale = Matrix4x4.Scale(new Vector3(1, 1, -1));
        Matrix4x4 mirrorMatrix = mirrorLocalToWorld * matrixScale * mirrorWorldToLocal;

        // https://forum.unity.com/threads/how-to-assign-matrix4x4-to-transform.121966/
        targetTransform.position = mirrorTransform.position + (Vector3)(mirrorMatrix * (sourceTransform.position - mirrorTransform.position));
        targetTransform.localScale = new Vector3(
            mirrorMatrix.GetColumn(0).magnitude,
            mirrorMatrix.GetColumn(1).magnitude,
            mirrorMatrix.GetColumn(2).magnitude);

        Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(sourceTransform.forward, sourceTransform.up));
        targetTransform.rotation = Quaternion.LookRotation(
            mirrorMatrix * matrixRotation * Vector3.forward,
            mirrorMatrix * matrixRotation * Vector3.up);
    }
}
