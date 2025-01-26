using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor.Mirror
{
    public class TestsData
    {
        private static Data D(
            Vector3 aPos = default, Vector3 aDir = default, Vector3 aUp = default,
            Vector3 bPos = default, Vector3 bDir = default, Vector3 bUp = default,
            Vector3 mPos = default, Vector3 mDir = default, Vector3 mUp = default) =>
            new Data(
                aPos: aPos, aDir: aDir, aUp: aUp,
                bPos: bPos, bDir: bDir, bUp: bUp,
                mPos: mPos, mDir: mDir, mUp: mUp);

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
            yield return TCD("01", D(aPos: X_Z(-1,  3), aDir: X_Z( 0,  0), aUp: XYZ(-1, 1,  3), bPos: X_Z( 1,  3), bDir: X_Z( 0,  0), bUp: XYZ( 1,  1,  3), mPos: X_Z( 0, -2), mDir: X_Z( 1, -2), mUp: XYZ( 0,  1, -2)));
            yield return TCD("02", D(aPos: X_Z(-1,  3), aDir: X_Z( 0,  0), aUp: XYZ(-1, 1,  3), bPos: X_Z( 1,  3), bDir: X_Z( 0,  0), bUp: XYZ( 1,  1,  3), mPos: X_Z( 0, -2), mDir: X_Z(-1, -2), mUp: XYZ( 0,  1, -2)));
            yield return TCD("03", D(aPos: X_Z(-1,  0), aDir: X_Z( 0, -1), aUp: XYZ(-1, 1,  0), bPos: X_Z( 1,  0), bDir: X_Z( 0, -1), bUp: XYZ( 1,  1,  0), mPos: X_Z( 0,  1), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  1)));
            yield return TCD("04", D(aPos: X_Z(-2,  0), aDir: X_Z(-1,  0), aUp: XYZ(-2, 1,  0), bPos: X_Z( 2,  0), bDir: X_Z( 1,  0), bUp: XYZ( 2,  1,  0), mPos: X_Z( 0,  0), mDir: X_Z(-1,  0), mUp: XYZ( 0,  1,  0)));
            yield return TCD("05", D(aPos: X_Z(-2,  3), aDir: X_Z( 3, -5), aUp: XYZ(-2, 1,  3), bPos: X_Z( 2,  3), bDir: X_Z(-3, -5), bUp: XYZ( 2,  1,  3), mPos: X_Z( 0,  0), mDir: X_Z(-1,  0), mUp: XYZ( 0,  1,  0)));
            yield return TCD("06", D(aPos: X_Z(-2,  3), aDir: X_Z( 2, -5), aUp: XYZ(-2, 1,  3), bPos: X_Z(-2, -3), bDir: X_Z( 2,  5), bUp: XYZ(-2,  1, -3), mPos: X_Z( 1,  0), mDir: X_Z( 1,  1), mUp: XYZ( 1,  1,  0)));
            yield return TCD("07", D(aPos: X_Z(-2,  3), aDir: X_Z( 2, -5), aUp: XYZ(-2, 1,  3), bPos: X_Z(-2, -3), bDir: X_Z( 2,  5), bUp: XYZ(-2,  1, -3), mPos: X_Z( 1,  0), mDir: X_Z( 1, -1), mUp: XYZ( 1,  1,  0)));
            yield return TCD("08", D(aPos: X_Z(-1,  2), aDir: X_Z(-2, -4), aUp: XYZ(-1, 1,  2), bPos: X_Z( 2, -1), bDir: X_Z(-4, -2), bUp: XYZ( 2,  1, -1), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  0)));
            yield return TCD("09", D(aPos: X_Z(-1,  2), aDir: X_Z(-3,  0), aUp: XYZ(-1, 1,  2), bPos: X_Z( 2, -1), bDir: X_Z( 0, -3), bUp: XYZ( 2,  1, -1), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  0)));
            yield return TCD("10", D(aPos: X_Z(-1,  2), aDir: X_Z(-3,  0), aUp: XYZ(-1, 1,  2), bPos: X_Z( 2, -1), bDir: X_Z( 0, -3), bUp: XYZ( 2,  1, -1), mPos: X_Z( 1,  1), mDir: X_Z( 0,  2), mUp: XYZ( 1,  1,  1)));
            yield return TCD("11", D(aPos: X_Z( 0,  3), aDir: X_Z( 1,  1), aUp: XYZ( 0, 1,  3), bPos: X_Z( 3,  0), bDir: X_Z( 1,  1), bUp: XYZ( 3,  1,  0), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  0)));
            yield return TCD("12", D(aPos: X_Z(-2,  0), aDir: X_Z(-1,  0), aUp: XYZ(-2, 1,  0), bPos: X_Z( 4,  0), bDir: X_Z( 3,  0), bUp: XYZ( 4,  1,  0), mPos: X_Z( 1,  0), mDir: X_Z( 0,  0), mUp: XYZ( 1,  1,  0)));

            yield return TCD("13", D(aPos: XY_(-3,  0), aDir: XY_(-2,  0), aUp: XY_(-3,  1), bPos: XY_( 3,  0), bDir: XY_( 2,  0), bUp: XY_( 3,  1), mPos: XY_( 0,  2), mDir: XY_(-1,  2), mUp: XY_( 0,  3)));

            yield return TCD("14", D(aPos: XY_(-3,  0), aDir: XY_(-3,  1), aUp: XY_(-4,  0), bPos: XY_(-3,  4), bDir: XY_(-3,  3), bUp: XY_(-4,  4), mPos: XY_( 0,  2), mDir: XY_( 0,  1), mUp: XY_(-1,  2)));
            yield return TCD("15", D(aPos: XY_(-3,  0), aDir: XY_(-2,  1), aUp: XY_(-4,  1), bPos: XY_(-3,  4), bDir: XY_(-2,  3), bUp: XY_(-4,  3), mPos: XY_( 0,  2), mDir: XY_( 0,  1), mUp: XY_(-1,  2)));
            yield return TCD("16", D(aPos: XY_(-3,  0), aDir: XY_(-2,  1), aUp: XY_(-4,  1), bPos: XY_( 2,  5), bDir: XY_( 1,  4), bUp: XY_( 1,  6), mPos: XY_( 0,  2), mDir: XY_(-1,  1), mUp: XY_(-1,  3)));
        }
    }
}
