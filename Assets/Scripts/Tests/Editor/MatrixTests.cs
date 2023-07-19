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
            Vector3 aPos = default, Vector3 aDir = default,
            Vector3 bPos = default, Vector3 bDir = default,
            Vector3 mPos = default, Vector3 mDir = default) =>
            new Data(
                aPos: aPos, aDir: aDir,
                bPos: bPos, bDir: bDir,
                mPos: mPos, mDir: mDir);

        private static Vector3 X_Z(float x, float z) =>
            new Vector3(x, 0, z);

        private static IEnumerable MatrixTestCaseSource()
        {
            yield return D(aPos: X_Z(-1,  3), aDir: X_Z( 0,  0), bPos: X_Z( 1,  3), bDir: X_Z( 0,  0), mPos: X_Z( 0, -2), mDir: X_Z( 1, -2));
            yield return D(aPos: X_Z(-1,  3), aDir: X_Z( 0,  0), bPos: X_Z( 1,  3), bDir: X_Z( 0,  0), mPos: X_Z( 0, -2), mDir: X_Z(-1, -2));
            yield return D(aPos: X_Z(-1,  0), aDir: X_Z( 0, -1), bPos: X_Z( 1,  0), bDir: X_Z( 0, -1), mPos: X_Z( 0,  1), mDir: X_Z(-1,  1));
            yield return D(aPos: X_Z(-2,  0), aDir: X_Z(-1,  0), bPos: X_Z( 2,  0), bDir: X_Z( 1,  0), mPos: X_Z( 0,  0), mDir: X_Z(-1,  0));
            yield return D(aPos: X_Z(-2,  3), aDir: X_Z( 3, -5), bPos: X_Z( 2,  3), bDir: X_Z(-3, -5), mPos: X_Z( 0,  0), mDir: X_Z(-1,  0));
            yield return D(aPos: X_Z(-2,  3), aDir: X_Z( 2, -5), bPos: X_Z(-2, -3), bDir: X_Z( 2,  5), mPos: X_Z( 1,  0), mDir: X_Z( 1,  1));
            yield return D(aPos: X_Z(-2,  3), aDir: X_Z( 2, -5), bPos: X_Z(-2, -3), bDir: X_Z( 2,  5), mPos: X_Z( 1,  0), mDir: X_Z( 1, -1));
            yield return D(aPos: X_Z(-1,  2), aDir: X_Z(-2, -4), bPos: X_Z( 2, -1), bDir: X_Z(-4, -2), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1));
            yield return D(aPos: X_Z(-1,  2), aDir: X_Z(-3,  0), bPos: X_Z( 2, -1), bDir: X_Z( 0, -3), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1));
            yield return D(aPos: X_Z(-1,  2), aDir: X_Z(-3,  0), bPos: X_Z( 2, -1), bDir: X_Z( 0, -3), mPos: X_Z( 1,  1), mDir: X_Z( 0,  2));
            yield return D(aPos: X_Z( 0,  3), aDir: X_Z( 1,  1), bPos: X_Z( 3,  0), bDir: X_Z( 1,  1), mPos: X_Z( 0,  0), mDir: X_Z(-1,  1));
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

            Vector3 bPosActual = matrixMirror * d.aPos;

            Assert.That(bPosActual, Is.EqualTo(d.bPos).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixRotationTest(Data d)
        {
            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir);

            Matrix4x4 matrixWorldToLocal = mGo.transform.worldToLocalMatrix;
            Matrix4x4 matrixLocalToWorld = mGo.transform.localToWorldMatrix;
            Matrix4x4 matrixScale = Matrix4x4.Scale(new Vector3(1, 1, -1));
            Matrix4x4 matrixMirror = matrixLocalToWorld * matrixScale * matrixWorldToLocal;

            Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(d.aDir - d.aPos));

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
        public void MatrixTransformRotationTest(Data d)
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

            Vector3 bDirActual = bGo.transform.rotation * Vector3.forward;

            Assert.That(bDirActual.normalized, Is.EqualTo((d.bDir - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }
    }
}
