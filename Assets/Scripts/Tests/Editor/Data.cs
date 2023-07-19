using UnityEngine;

namespace Tests.Editor
{
    public class Data
    {
        public Vector3 aPos;
        public Vector3 aDir;
        public Vector3 bPos;
        public Vector3 bDir;
        public Vector3 mPos;
        public Vector3 mDir;

        public Data(
            Vector3 aPos = default, Vector3 aDir = default,
            Vector3 bPos = default, Vector3 bDir = default,
            Vector3 mPos = default, Vector3 mDir = default)
        {
            this.aPos = aPos; this.aDir = aDir;
            this.bPos = bPos; this.bDir = bDir;
            this.mPos = mPos; this.mDir = mDir;
        }

        public override string ToString()
        {
            return $"{aPos} {aDir} {bPos} {bDir} {mPos} {mDir}";
        }
    }
}
