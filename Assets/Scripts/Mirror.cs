using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera mirrorCamera;
    [SerializeField] private MeshRenderer mirrorRenderer;

    private Transform _playerCameraTransform;
    private Transform _mirrorCameraTransform;

    private RenderTexture _renderTexture;

    private void Start()
    {
        _playerCameraTransform = playerCamera.transform;
        _mirrorCameraTransform = mirrorCamera.transform;

        // Debug.Log($"Create texture {Screen.width}x{Screen.height}");
        _renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        // _renderTexture.Create();

        mirrorCamera.targetTexture = _renderTexture;
        mirrorRenderer.material.mainTexture = _renderTexture;
    }

    private void LateUpdate()
    {
        mirrorCamera.CopyFrom(playerCamera);
        mirrorCamera.targetTexture = _renderTexture;

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

        mirrorCamera.Render();
    }

    public static void MirrorTransform(Transform sourceTransform, Transform targetTransform, Transform mirrorTransform)
    {
        Matrix4x4 mirrorWorldToLocal = mirrorTransform.worldToLocalMatrix;
        Matrix4x4 mirrorLocalToWorld = mirrorTransform.localToWorldMatrix;
        Matrix4x4 matrixScale = Matrix4x4.Scale(new Vector3(1, 1, -1));
        Matrix4x4 mirrorMatrix = mirrorLocalToWorld * matrixScale * mirrorWorldToLocal;

        // https://forum.unity.com/threads/how-to-assign-matrix4x4-to-transform.121966/
        targetTransform.position = mirrorMatrix * sourceTransform.position;
        targetTransform.localScale = new Vector3(
            mirrorMatrix.GetColumn(0).magnitude,
            mirrorMatrix.GetColumn(1).magnitude,
            mirrorMatrix.GetColumn(2).magnitude);

        Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(sourceTransform.forward));
        targetTransform.rotation = Quaternion.LookRotation(mirrorMatrix * matrixRotation * Vector3.forward);
    }
}
