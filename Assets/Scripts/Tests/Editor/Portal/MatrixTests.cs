using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.Editor.Portal
{
    [TestFixture]
    public class MatrixTests
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

        private static IEnumerable MatrixTestCaseSource()
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
            var pGo = new GameObject("P");
            pGo.transform.position = d.pPos;
            pGo.transform.LookAt(d.pDir);

            var qGo = new GameObject("Q");
            qGo.transform.position = d.qPos;
            qGo.transform.LookAt(d.qDir);

            Matrix4x4 matrixWorldToLocal = pGo.transform.worldToLocalMatrix;
            Matrix4x4 matrixLocalToWorld = qGo.transform.localToWorldMatrix;
            Matrix4x4 matrixRotate = Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.up));
            Matrix4x4 matrixPortal = matrixLocalToWorld * matrixRotate * matrixWorldToLocal;

            Vector3 bPosActual = d.qPos + (Vector3)(matrixPortal * (d.aPos - d.pPos));

            Assert.That(bPosActual, Is.EqualTo(d.bPos).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixRotationForwardTest(Data d)
        {
            var pGo = new GameObject("P");
            pGo.transform.position = d.pPos;
            pGo.transform.LookAt(d.pDir, d.pUp - d.pPos);

            var qGo = new GameObject("Q");
            qGo.transform.position = d.qPos;
            qGo.transform.LookAt(d.qDir, d.qUp - d.qPos);

            Matrix4x4 matrixWorldToLocal = pGo.transform.worldToLocalMatrix;
            Matrix4x4 matrixLocalToWorld = qGo.transform.localToWorldMatrix;
            Matrix4x4 matrixRotate = Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.up));
            Matrix4x4 matrixPortal = matrixLocalToWorld * matrixRotate * matrixWorldToLocal;

            Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(d.aDir - d.aPos, d.aUp - d.aPos));

            Vector3 bDirActual = matrixPortal * matrixRotation * Vector3.forward;

            Assert.That(bDirActual.normalized, Is.EqualTo((d.bDir - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(MatrixTests), nameof(MatrixTestCaseSource))]
        public void MatrixRotationUpwardTest(Data d)
        {
            var pGo = new GameObject("P");
            pGo.transform.position = d.pPos;
            pGo.transform.LookAt(d.pDir, d.pUp - d.pPos);

            var qGo = new GameObject("Q");
            qGo.transform.position = d.qPos;
            qGo.transform.LookAt(d.qDir, d.qUp - d.qPos);

            Matrix4x4 matrixWorldToLocal = pGo.transform.worldToLocalMatrix;
            Matrix4x4 matrixLocalToWorld = qGo.transform.localToWorldMatrix;
            Matrix4x4 matrixRotate = Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.up));
            Matrix4x4 matrixPortal = matrixLocalToWorld * matrixRotate * matrixWorldToLocal;

            Matrix4x4 matrixRotation = Matrix4x4.Rotate(Quaternion.LookRotation(d.aDir - d.aPos, d.aUp - d.aPos));

            Vector3 bUpActual = matrixPortal * matrixRotation * Vector3.up;

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
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir);

            var pGo = new GameObject("P");
            pGo.transform.position = d.pPos;
            pGo.transform.LookAt(d.pDir);

            var qGo = new GameObject("Q");
            qGo.transform.position = d.qPos;
            qGo.transform.LookAt(d.qDir);

            global::Portal.PortalTransform(aGo.transform, bGo.transform, pGo.transform, qGo.transform);

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
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);

            var pGo = new GameObject("P");
            pGo.transform.position = d.pPos;
            pGo.transform.LookAt(d.pDir, d.pUp - d.pPos);

            var qGo = new GameObject("Q");
            qGo.transform.position = d.qPos;
            qGo.transform.LookAt(d.qDir, d.qUp - d.qPos);

            global::Portal.PortalTransform(aGo.transform, bGo.transform, pGo.transform, qGo.transform);

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
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);

            var pGo = new GameObject("P");
            pGo.transform.position = d.pPos;
            pGo.transform.LookAt(d.pDir, d.pUp - d.pPos);

            var qGo = new GameObject("Q");
            qGo.transform.position = d.qPos;
            qGo.transform.LookAt(d.qDir, d.qUp - d.qPos);

            global::Portal.PortalTransform(aGo.transform, bGo.transform, pGo.transform, qGo.transform);

            Vector3 bUpActual = bGo.transform.rotation * Vector3.up;

            Assert.That(bUpActual.normalized, Is.EqualTo((d.bUp - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }
    }
}
