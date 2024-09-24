using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Portal : MonoBehaviour
{
    [SerializeField] private Portal otherPortal;
    [SerializeField] private MeshRenderer portalRenderer;

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
        portalRenderer.sharedMaterial.mainTexture = renderTexture;

        PortalTransform(currentCamera.transform, companionCamera.transform,
            this.transform, otherPortal.transform);

        if (renderTexture.width != Screen.width || renderTexture.height != Screen.height)
        {
            Debug.Log($"Resize texture {Screen.width}x{Screen.height}");
            renderTexture.Release();
            // renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            renderTexture.width = Screen.width;
            renderTexture.height = Screen.height;
            renderTexture.Create();
        }

        companionCamera.Render();
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
        companionCameraGameObject.name = $"Portal Camera [{originalCamera.name}]";
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
        renderTexture.name = $"Portal Texture [{originalCamera.name}]";
        // renderTexture.Create();

        return renderTexture;
    }

    public static void PortalTransform(Transform sourceTransform, Transform targetTransform,
        Transform inPortalTransform, Transform outPortalTransform)
    {
        Matrix4x4 matrixRotate = Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.up));
        Matrix4x4 portalMatrix = outPortalTransform.localToWorldMatrix * matrixRotate * inPortalTransform.worldToLocalMatrix;

        // https://forum.unity.com/threads/how-to-assign-matrix4x4-to-transform.121966/
        targetTransform.position = outPortalTransform.position + (Vector3)(portalMatrix * (sourceTransform.position - inPortalTransform.position));

        targetTransform.localScale = new Vector3(
            portalMatrix.GetColumn(0).magnitude,
            portalMatrix.GetColumn(1).magnitude,
            portalMatrix.GetColumn(2).magnitude);

        Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(sourceTransform.forward, sourceTransform.up));
        targetTransform.rotation = Quaternion.LookRotation(
            portalMatrix * matrixRotation * Vector3.forward,
            portalMatrix * matrixRotation * Vector3.up);
    }
}
