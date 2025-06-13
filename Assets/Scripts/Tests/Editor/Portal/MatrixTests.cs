using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.Editor.Portal
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
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
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
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
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
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
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
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
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
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
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
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
