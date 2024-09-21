using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Portal : MonoBehaviour
{
    [SerializeField] private Portal otherPortal;
    [SerializeField] private MeshRenderer portalRenderer;

    private readonly Dictionary<Camera, Camera> _portalCameras =
        new Dictionary<Camera, Camera>();

    private readonly Dictionary<Camera, RenderTexture> _renderTextures =
        new Dictionary<Camera, RenderTexture>();

    private void OnDisable()
    {
        foreach (var pair in _portalCameras)
        {
            var portalCamera = pair.Value;
            DestroyImmediate(portalCamera.gameObject);
        }

        _portalCameras.Clear();

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

        var portalCamera = GetPortalCamera(currentCamera);
        var renderTexture = GetRenderTexture(currentCamera);

        portalCamera.CopyFrom(currentCamera);
        portalCamera.targetTexture = renderTexture;
        portalRenderer.sharedMaterial.mainTexture = renderTexture;

        PortalTransform(currentCamera.transform, portalCamera.transform,
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

        portalCamera.Render();
    }

    private Camera GetPortalCamera(Camera camera)
    {
        if (_portalCameras.TryGetValue(camera, out var portalCamera))
            if (portalCamera != null)
                return portalCamera;

        portalCamera = CreatePortalCamera(camera);
        _portalCameras[camera] = portalCamera;
        return portalCamera;
    }

    private Camera CreatePortalCamera(Camera camera)
    {
        var portalCameraGameObject = new GameObject();
        portalCameraGameObject.name = $"Portal Camera [{camera.name}]";
        portalCameraGameObject.transform.parent = transform;
        portalCameraGameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

        var portalCamera = portalCameraGameObject.AddComponent<Camera>();
        portalCamera.enabled = false;

        return portalCamera;
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
        renderTexture.name = $"Portal Texture [{camera.name}]";
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
