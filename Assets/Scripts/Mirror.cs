using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private MeshRenderer mirrorRenderer;

    private Camera _mirrorCamera;

    private Transform _playerCameraTransform;
    private Transform _mirrorCameraTransform;

    private RenderTexture _renderTexture;

    private void Start()
    {
        _mirrorCamera = CreateMirrorCamera();

        _playerCameraTransform = playerCamera.transform;
        _mirrorCameraTransform = _mirrorCamera.transform;

        // Debug.Log($"Create texture {Screen.width}x{Screen.height}");
        _renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        // _renderTexture.Create();

        _mirrorCamera.targetTexture = _renderTexture;
        mirrorRenderer.material.mainTexture = _renderTexture;
    }

    private void LateUpdate()
    {
        _mirrorCamera.CopyFrom(playerCamera);
        _mirrorCamera.targetTexture = _renderTexture;

        MirrorTransform(_playerCameraTransform, _mirrorCameraTransform, mirrorRenderer.transform);

        if (_renderTexture.width != Screen.width || _renderTexture.height != Screen.height)
        {
            Debug.Log($"Resize texture {Screen.width}x{Screen.height}");
            _renderTexture.Release();
            // _renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            _renderTexture.width = Screen.width;
            _renderTexture.height = Screen.height;
            _renderTexture.Create();
        }

        _mirrorCamera.Render();
    }

    private Camera CreateMirrorCamera()
    {
        var mirrorCameraGameObject = new GameObject();
        mirrorCameraGameObject.name = $"Mirror Camera";
        mirrorCameraGameObject.transform.parent = transform;
        mirrorCameraGameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

        var mirrorCamera = mirrorCameraGameObject.AddComponent<Camera>();
        mirrorCamera.enabled = false;

        return mirrorCamera;
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
