using UnityEngine;

namespace Tests.Editor.Portal
{
    public class Data
    {
        public Vector3 aPos;
        public Vector3 aDir;
        public Vector3 aUp;
        public Vector3 bPos;
        public Vector3 bDir;
        public Vector3 bUp;
        public Vector3 pPos;
        public Vector3 pDir;
        public Vector3 pUp;
        public Vector3 qPos;
        public Vector3 qDir;
        public Vector3 qUp;

        public Data(
            Vector3 aPos = default, Vector3 aDir = default, Vector3 aUp = default,
            Vector3 bPos = default, Vector3 bDir = default, Vector3 bUp = default,
            Vector3 pPos = default, Vector3 pDir = default, Vector3 pUp = default,
            Vector3 qPos = default, Vector3 qDir = default, Vector3 qUp = default)
        {
            this.aPos = aPos; this.aDir = aDir; this.aUp = aUp;
            this.bPos = bPos; this.bDir = bDir; this.bUp = bUp;
            this.pPos = pPos; this.pDir = pDir; this.pUp = pUp;
            this.qPos = qPos; this.qDir = qDir; this.qUp = qUp;
        }

        public override string ToString()
        {
            return $"{aPos} {aDir} {aUp} {bPos} {bDir} {bUp} {pPos} {pDir} {pUp} {qPos} {qDir} {qUp}";
        }
    }
}
