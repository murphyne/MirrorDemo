using UnityEngine;

namespace Tests
{
    public class Gizmo : MonoBehaviour
    {
        [SerializeField] public Mesh meshPos;
        [SerializeField] public Mesh meshDir;
        [SerializeField] public Color color;
        [SerializeField] public Vector3 pos;
        [SerializeField] public Vector3 dir;
        [SerializeField] public Vector3 scaleMeshDir;

        private void OnDrawGizmos()
        {
            var position = transform.position;
            var rotation = transform.rotation;
            var scale = transform.lossyScale;
            var meshPosScale = Vector3.Scale(scale, Vector3.one);
            var meshDirScale = Vector3.Scale(scale, scaleMeshDir);
            Gizmos.color = color;
            Gizmos.DrawWireMesh(meshPos, pos, rotation, meshPosScale);
            Gizmos.DrawWireMesh(meshDir, dir, rotation, meshDirScale);
            Gizmos.DrawLine(pos, dir);
        }
    }
}
