using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor.Portal
{
    public class TestsData
    {
        private static Data D(
            Vector3 aPos = default, Vector3 aDir = default, Vector3 aUp = default,
            Vector3 bPos = default, Vector3 bDir = default, Vector3 bUp = default,
            Vector3 pPos = default, Vector3 pDir = default, Vector3 pUp = default,
            Vector3 qPos = default, Vector3 qDir = default, Vector3 qUp = default) =>
            new Data(
                aPos: aPos, aDir: aDir, aUp: aUp,
                bPos: bPos, bDir: bDir, bUp: bUp,
                pPos: pPos, pDir: pDir, pUp: pUp,
                qPos: qPos, qDir: qDir, qUp: qUp);

        private static TestCaseData TCD(string name, Data data) =>
            new TestCaseData(data).SetName(name);

        private static Vector3 XYZ(float x, float y, float z) =>
            new Vector3(x, y, z);

        private static Vector3 X_Z(float x, float z) =>
            new Vector3(x, 0, z);

        private static Vector3 XY_(float x, float y) =>
            new Vector3(x, y, 0);

        private static Vector3 _Y_(float y) =>
            new Vector3(0, y, 0);

        public static IEnumerable MatrixTestCaseSource()
        {
            yield return TCD("01", D(aPos: X_Z(-2,  3), aDir: X_Z(-2,  1), aUp: XYZ(-2, 1,  3), bPos: X_Z(-2,  3), bDir: X_Z(-2,  1), bUp: XYZ(-2,  1,  3), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: X_Z(-2, -3), qDir: X_Z(-2, -5), qUp: XYZ(-2,  1, -3)));
            yield return TCD("02", D(aPos: X_Z(-2,  3), aDir: X_Z(-2,  1), aUp: XYZ(-2, 1,  3), bPos: X_Z( 2,  3), bDir: X_Z( 2,  1), bUp: XYZ( 2,  1,  3), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: X_Z( 2, -3), qDir: X_Z( 2, -5), qUp: XYZ( 2,  1, -3)));
            yield return TCD("03", D(aPos: X_Z(-2,  5), aDir: X_Z(-2,  3), aUp: XYZ(-2, 1,  5), bPos: X_Z( 2,  3), bDir: X_Z( 2,  1), bUp: XYZ( 2,  1,  3), pPos: X_Z(-2, -1), pDir: X_Z(-2,  1), pUp: XYZ(-2,  1, -1), qPos: X_Z( 2, -3), qDir: X_Z( 2, -5), qUp: XYZ( 2,  1, -3)));
            yield return TCD("04", D(aPos: X_Z(-2,  3), aDir: X_Z(-2,  1), aUp: XYZ(-2, 1,  3), bPos: X_Z( 2, -3), bDir: X_Z( 2, -1), bUp: XYZ( 2,  1, -3), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: X_Z( 2,  3), qDir: X_Z( 2,  5), qUp: XYZ( 2,  1,  3)));

            yield return TCD("05", D(aPos: X_Z(-2,  3), aDir: X_Z(-2,  1), aUp: XYZ(-2, 1,  3), bPos: XYZ( 2,  -3,  3), bDir: XYZ( 2, -1,  3), bUp: XYZ( 2, -3,  2), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: XYZ( 2,  3,  3), qDir: XYZ( 2,  5,  3), qUp: XYZ( 2,  3,  2)));

            yield return TCD("06", D(aPos: X_Z(-2,  3), aDir: X_Z(-4,  1), aUp: XYZ(-2, 1,  3), bPos: X_Z(-2,  3), bDir: X_Z(-4,  1), bUp: XYZ(-2,  1,  3), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: X_Z(-2, -3), qDir: X_Z(-2, -5), qUp: XYZ(-2,  1, -3)));
            yield return TCD("07", D(aPos: X_Z(-2,  3), aDir: X_Z(-4,  1), aUp: XYZ(-2, 1,  3), bPos: X_Z( 2,  3), bDir: X_Z( 0,  1), bUp: XYZ( 2,  1,  3), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: X_Z( 2, -3), qDir: X_Z( 2, -5), qUp: XYZ( 2,  1, -3)));

            yield return TCD("08", D(aPos: X_Z(-2,  3), aDir: XYZ(-2,  2,  1), aUp: XYZ(-2, 2,  5), bPos: X_Z(-2,  3), bDir: XYZ(-2,  2,  1), bUp: XYZ(-2,  2,  5), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: X_Z(-2, -3), qDir: X_Z(-2, -5), qUp: XYZ(-2,  1, -3)));
            yield return TCD("09", D(aPos: X_Z(-2,  3), aDir: XYZ(-2,  2,  1), aUp: XYZ(-2, 2,  5), bPos: X_Z( 2,  3), bDir: XYZ( 2,  2,  1), bUp: XYZ( 2,  2,  5), pPos: X_Z(-2, -3), pDir: X_Z(-2, -1), pUp: XYZ(-2,  1, -3), qPos: X_Z( 2, -3), qDir: X_Z( 2, -5), qUp: XYZ( 2,  1, -3)));
        }
    }
}
