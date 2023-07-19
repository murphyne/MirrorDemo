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
            yield return D(aPos: X_Z(-1,  3), aDir: X_Z( 0,  0), aUp: XYZ(-1, 1,  3), bPos: X_Z( 1,  3), bDir: X_Z( 0,  0), bUp: XYZ( 1,  1,  3), mPos: X_Z( 0, -2), mDir: X_Z( 1, -2), mUp: XYZ( 0,  1, -2));
            yield return D(aPos: X_Z(-1,  3), aDir: X_Z( 0,  0), aUp: XYZ(-1, 1,  3), bPos: X_Z( 1,  3), bDir: X_Z( 0,  0), bUp: XYZ( 1,  1,  3), mPos: X_Z( 0, -2), mDir: X_Z(-1, -2), mUp: XYZ( 0,  1, -2));
            yield return D(aPos: X_Z(-1,  0), aDir: X_Z( 0, -1), aUp: XYZ(-1, 1,  0), bPos: X_Z( 1,  0), bDir: X_Z( 0, -1), bUp: XYZ( 1,  1,  0), mPos: X_Z( 0,  1), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  1));
            yield return D(aPos: X_Z(-2,  0), aDir: X_Z(-1,  0), aUp: XYZ(-2, 1,  0), bPos: X_Z( 2,  0), bDir: X_Z( 1,  0), bUp: XYZ( 2,  1,  0), mPos: X_Z( 0,  0), mDir: X_Z(-1,  0), mUp: XYZ( 0,  1,  0));
            yield return D(aPos: X_Z(-2,  3), aDir: X_Z( 3, -5), aUp: XYZ(-2, 1,  3), bPos: X_Z( 2,  3), bDir: X_Z(-3, -5), bUp: XYZ( 2,  1,  3), mPos: X_Z( 0,  0), mDir: X_Z(-1,  0), mUp: XYZ( 0,  1,  0));
            yield return D(aPos: X_Z(-2,  3), aDir: X_Z( 2, -5), aUp: XYZ(-2, 1,  3), bPos: X_Z(-2, -3), bDir: X_Z( 2,  5), bUp: XYZ(-2,  1, -3), mPos: X_Z( 1,  0), mDir: X_Z( 1,  1), mUp: XYZ( 1,  1,  0));
            yield return D(aPos: X_Z(-2,  3), aDir: X_Z( 2, -5), aUp: XYZ(-2, 1,  3), bPos: X_Z(-2, -3), bDir: X_Z( 2,  5), bUp: XYZ(-2,  1, -3), mPos: X_Z( 1,  0), mDir: X_Z( 1, -1), mUp: XYZ( 1,  1,  0));
            yield return D(aPos: X_Z(-1,  2), aDir: X_Z(-2, -4), aUp: XYZ(-1, 1,  2), bPos: X_Z( 2, -1), bDir: X_Z(-4, -2), bUp: XYZ( 2,  1, -1), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  0));
            yield return D(aPos: X_Z(-1,  2), aDir: X_Z(-3,  0), aUp: XYZ(-1, 1,  2), bPos: X_Z( 2, -1), bDir: X_Z( 0, -3), bUp: XYZ( 2,  1, -1), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  0));
            yield return D(aPos: X_Z(-1,  2), aDir: X_Z(-3,  0), aUp: XYZ(-1, 1,  2), bPos: X_Z( 2, -1), bDir: X_Z( 0, -3), bUp: XYZ( 2,  1, -1), mPos: X_Z( 1,  1), mDir: X_Z( 0,  2), mUp: XYZ( 1,  1,  1));
            yield return D(aPos: X_Z( 0,  3), aDir: X_Z( 1,  1), aUp: XYZ( 0, 1,  3), bPos: X_Z( 3,  0), bDir: X_Z( 1,  1), bUp: XYZ( 3,  1,  0), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1), mUp: XYZ( 0,  1,  0));
            yield return D(aPos: X_Z(-2,  0), aDir: X_Z(-1,  0), aUp: XYZ(-2, 1,  0), bPos: X_Z( 4,  0), bDir: X_Z( 3,  0), bUp: XYZ( 4,  1,  0), mPos: X_Z( 1,  0), mDir: X_Z( 0,  0), mUp: XYZ( 1,  1,  0));

            yield return D(aPos: XY_(-3,  0), aDir: XY_(-2,  0), aUp: XY_(-3,  1), bPos: XY_( 3,  0), bDir: XY_( 2,  0), bUp: XY_( 3,  1), mPos: XY_( 0,  2), mDir: XY_(-1,  2), mUp: XY_( 0,  3));

            yield return D(aPos: XY_(-3,  0), aDir: XY_(-3,  1), aUp: XY_(-4,  0), bPos: XY_(-3,  4), bDir: XY_(-3,  3), bUp: XY_(-4,  4), mPos: XY_( 0,  2), mDir: XY_( 0,  1), mUp: XY_(-1,  2));
            yield return D(aPos: XY_(-3,  0), aDir: XY_(-2,  1), aUp: XY_(-4,  1), bPos: XY_(-3,  4), bDir: XY_(-2,  3), bUp: XY_(-4,  3), mPos: XY_( 0,  2), mDir: XY_( 0,  1), mUp: XY_(-1,  2));
            yield return D(aPos: XY_(-3,  0), aDir: XY_(-2,  1), aUp: XY_(-4,  1), bPos: XY_( 2,  5), bDir: XY_( 1,  4), bUp: XY_( 1,  6), mPos: XY_( 0,  2), mDir: XY_(-1,  1), mUp: XY_(-1,  3));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void DataSceneTest(Data d)
        {
            DataDisplayScene.Render(d);
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void DataTextureTest(Data d)
        {
            DataDisplayTexture.Render(d);
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void DataTextTest(Data d)
        {
            var result = DataDisplayText.Render(d);
            Debug.Log(result);
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void DataHashTest(Data d)
        {
            var result = DataHash.Hash(d);
            Debug.Log(result);
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixPositionTest(Data d)
        {
            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir);

            Matrix4x4 matrixWorldToLocal = mGo.transform.worldToLocalMatrix;
            Matrix4x4 matrixLocalToWorld = mGo.transform.localToWorldMatrix;
            Matrix4x4 matrixScale = Matrix4x4.Scale(new Vector3(1, 1, -1));
            Matrix4x4 matrixMirror = matrixLocalToWorld * matrixScale * matrixWorldToLocal;

            Vector3 bPosActual = d.mPos + (Vector3)(matrixMirror * (d.aPos - d.mPos));

            Assert.That(bPosActual, Is.EqualTo(d.bPos).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixRotationForwardTest(Data d)
        {
            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            Matrix4x4 matrixWorldToLocal = mGo.transform.worldToLocalMatrix;
            Matrix4x4 matrixLocalToWorld = mGo.transform.localToWorldMatrix;
            Matrix4x4 matrixScale = Matrix4x4.Scale(new Vector3(1, 1, -1));
            Matrix4x4 matrixMirror = matrixLocalToWorld * matrixScale * matrixWorldToLocal;

            Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(d.aDir - d.aPos, d.aUp - d.aPos));

            LogRotation(matrixWorldToLocal * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixWorldToLocal * matrixRotation);
            LogRotation(matrixWorldToLocal * matrixLocalToWorld * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixScale * matrixWorldToLocal * matrixRotation);
            LogRotation(matrixWorldToLocal * matrixScale * matrixLocalToWorld * matrixRotation);
            LogRotation(matrixScale * matrixLocalToWorld * matrixWorldToLocal * matrixRotation);
            LogRotation(matrixScale * matrixWorldToLocal * matrixLocalToWorld * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixWorldToLocal * matrixScale * matrixRotation);
            LogRotation(matrixWorldToLocal * matrixLocalToWorld * matrixScale * matrixRotation);

            void LogRotation(Matrix4x4 matrix) =>
                Debug.Log(matrix * Vector3.forward);

            Vector3 bDirActual = matrixMirror * matrixRotation * Vector3.forward;

            Assert.That(bDirActual.normalized, Is.EqualTo((d.bDir - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixRotationUpwardTest(Data d)
        {
            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            Matrix4x4 matrixWorldToLocal = mGo.transform.worldToLocalMatrix;
            Matrix4x4 matrixLocalToWorld = mGo.transform.localToWorldMatrix;
            Matrix4x4 matrixScale = Matrix4x4.Scale(new Vector3(1, 1, -1));
            Matrix4x4 matrixMirror = matrixLocalToWorld * matrixScale * matrixWorldToLocal;

            Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(d.aDir - d.aPos, d.aUp - d.aPos));

            LogRotation(matrixWorldToLocal * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixWorldToLocal * matrixRotation);
            LogRotation(matrixWorldToLocal * matrixLocalToWorld * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixScale * matrixWorldToLocal * matrixRotation);
            LogRotation(matrixWorldToLocal * matrixScale * matrixLocalToWorld * matrixRotation);
            LogRotation(matrixScale * matrixLocalToWorld * matrixWorldToLocal * matrixRotation);
            LogRotation(matrixScale * matrixWorldToLocal * matrixLocalToWorld * matrixRotation);
            LogRotation(matrixLocalToWorld * matrixWorldToLocal * matrixScale * matrixRotation);
            LogRotation(matrixWorldToLocal * matrixLocalToWorld * matrixScale * matrixRotation);

            void LogRotation(Matrix4x4 matrix) =>
                Debug.Log(matrix * Vector3.up);

            Vector3 bUpActual = matrixMirror * matrixRotation * Vector3.up;

            Assert.That(bUpActual.normalized, Is.EqualTo((d.bUp - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixTransformPositionTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir);

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir);

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir);

            Mirror.MirrorTransform(aGo.transform, bGo.transform, mGo.transform);

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

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            Mirror.MirrorTransform(aGo.transform, bGo.transform, mGo.transform);

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

            var bGo = new GameObject("B");
            bGo.transform.position = d.bPos;
            bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            Mirror.MirrorTransform(aGo.transform, bGo.transform, mGo.transform);

            Vector3 bUpActual = bGo.transform.rotation * Vector3.up;

            Assert.That(bUpActual.normalized, Is.EqualTo((d.bUp - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }
    }
}
