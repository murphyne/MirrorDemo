using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera mirrorCamera;
    [SerializeField] private MeshRenderer mirrorRenderer;

    private Transform _playerCameraTransform;
    private Transform _mirrorCameraTransform;
    private Matrix4x4 _mirrorMatrix;

    private RenderTexture _renderTexture;

    private void Start()
    {
        _playerCameraTransform = playerCamera.transform;
        _mirrorCameraTransform = mirrorCamera.transform;

        _renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        mirrorCamera.targetTexture = _renderTexture;
        mirrorRenderer.material.mainTexture = _renderTexture;

        _mirrorMatrix = Matrix4x4.identity;
        _mirrorMatrix.m11 *= -1;
        _mirrorMatrix.m22 *= -1;
    }

    private void LateUpdate()
    {
        mirrorCamera.CopyFrom(playerCamera);
        mirrorCamera.targetTexture = _renderTexture;

        Matrix4x4 mirrorWorldToLocal = mirrorRenderer.worldToLocalMatrix;
        Matrix4x4 mirrorLocalToWorld = mirrorRenderer.localToWorldMatrix;
        Matrix4x4 mirrorMatrix = mirrorLocalToWorld * _mirrorMatrix * mirrorWorldToLocal;

        var m = mirrorMatrix * _playerCameraTransform.localToWorldMatrix;
        _mirrorCameraTransform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
        Debug.Log($"{m.GetColumn(3)} {m.rotation}");

        mirrorCamera.Render();
    }
}
