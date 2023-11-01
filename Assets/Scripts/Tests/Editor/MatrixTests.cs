using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.Editor
{
    [TestFixture]
    public class MatrixTests
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

        private static IEnumerable MatrixTestCaseSource()
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

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void DataSceneTest(Data d)
        {
            new DataDisplayScene(d, TestContext.CurrentContext.Test.Name).Render();
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void DataTextureTest(Data d)
        {
            new DataDisplayTexture(d, TestContext.CurrentContext.Test.Name).Render();
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void DataTextTest(Data d)
        {
            var result = new DataDisplayText(d).Render();
            Debug.Log(result);
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixPositionTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir);

            var point = mGo.transform.position;
            var normal = mGo.transform.TransformDirection(Vector3.back);

            var mirrorPlane = Mirror.GetPlane(point, normal);
            var mirrorMatrix = Mirror.GetMirrorMatrix(mirrorPlane);
            bCam.worldToCameraMatrix = aCam.worldToCameraMatrix * mirrorMatrix;

            Vector3 bPosActual = bGo.transform.position;

            Assert.That(bPosActual, Is.EqualTo(d.bPos).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixRotationForwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            var point = mGo.transform.position;
            var normal = mGo.transform.TransformDirection(Vector3.back);

            var mirrorPlane = Mirror.GetPlane(point, normal);
            var mirrorMatrix = Mirror.GetMirrorMatrix(mirrorPlane);
            bCam.worldToCameraMatrix = aCam.worldToCameraMatrix * mirrorMatrix;

            Vector3 bDirActual = bGo.transform.rotation * Vector3.forward;

            Assert.That(bDirActual.normalized, Is.EqualTo((d.bDir - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixRotationUpwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            var point = mGo.transform.position;
            var normal = mGo.transform.TransformDirection(Vector3.back);

            var mirrorPlane = Mirror.GetPlane(point, normal);
            var mirrorMatrix = Mirror.GetMirrorMatrix(mirrorPlane);
            bCam.worldToCameraMatrix = aCam.worldToCameraMatrix * mirrorMatrix;

            Vector3 bUpActual = bGo.transform.rotation * Vector3.up;

            Assert.That(bUpActual.normalized, Is.EqualTo((d.bUp - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixTransformPositionTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir);

            Mirror.MirrorTransform(aCam, bCam, mGo.transform);

            Vector3 bPosActual = bGo.transform.position;

            Assert.That(bPosActual, Is.EqualTo(d.bPos).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixTransformRotationForwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            Mirror.MirrorTransform(aCam, bCam, mGo.transform);

            Vector3 bDirActual = bGo.transform.rotation * Vector3.forward;

            Assert.That(bDirActual.normalized, Is.EqualTo((d.bDir - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixTransformRotationUpwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            Mirror.MirrorTransform(aCam, bCam, mGo.transform);

            Vector3 bUpActual = bGo.transform.rotation * Vector3.up;

            Assert.That(bUpActual.normalized, Is.EqualTo((d.bUp - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }
    }
}
