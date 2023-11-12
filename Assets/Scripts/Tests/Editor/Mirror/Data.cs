using UnityEngine;

namespace Tests.Editor.Mirror
{
    public class Data
    {
        public Vector3 aPos;
        public Vector3 aDir;
        public Vector3 aUp;
        public Vector3 bPos;
        public Vector3 bDir;
        public Vector3 bUp;
        public Vector3 mPos;
        public Vector3 mDir;
        public Vector3 mUp;

        public Data(
            Vector3 aPos = default, Vector3 aDir = default, Vector3 aUp = default,
            Vector3 bPos = default, Vector3 bDir = default, Vector3 bUp = default,
            Vector3 mPos = default, Vector3 mDir = default, Vector3 mUp = default)
        {
            this.aPos = aPos; this.aDir = aDir; this.aUp = aUp;
            this.bPos = bPos; this.bDir = bDir; this.bUp = bUp;
            this.mPos = mPos; this.mDir = mDir; this.mUp = mUp;
        }

        public override string ToString()
        {
            return $"{aPos} {aDir} {aUp} {bPos} {bDir} {bUp} {mPos} {mDir} {mUp}";
        }
    }
}
