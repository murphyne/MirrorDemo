using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.Editor.Mirror
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void DataSceneTest(Data d)
        {
            new DataDisplayScene(d, TestContext.CurrentContext.Test.Name).Render();
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void DataTextureTest(Data d)
        {
            new DataDisplayTexture(d, TestContext.CurrentContext.Test.Name).Render();
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void DataTextTest(Data d)
        {
            var result = new DataDisplayText(d).Render();
            Debug.Log(result);
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void MatrixPositionTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            // bGo.transform.position = d.bPos;
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir);

            var point = mGo.transform.position;
            var normal = mGo.transform.TransformDirection(Vector3.back);

            var mirrorPlane = global::Mirror.GetPlane(point, normal);
            var mirrorMatrix = global::Mirror.GetMirrorMatrix(mirrorPlane);
            bCam.worldToCameraMatrix = aCam.worldToCameraMatrix * mirrorMatrix;

            Vector3 bPosActual = bCam.cameraToWorldMatrix.MultiplyPoint(Vector3.zero);

            Assert.That(bPosActual, Is.EqualTo(d.bPos).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void MatrixRotationForwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            var point = mGo.transform.position;
            var normal = mGo.transform.TransformDirection(Vector3.back);

            var mirrorPlane = global::Mirror.GetPlane(point, normal);
            var mirrorMatrix = global::Mirror.GetMirrorMatrix(mirrorPlane);
            bCam.worldToCameraMatrix = aCam.worldToCameraMatrix * mirrorMatrix;

            Vector3 bDirActual = bCam.cameraToWorldMatrix.MultiplyVector(Vector3.back);

            Assert.That(bDirActual.normalized, Is.EqualTo((d.bDir - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void MatrixRotationUpwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            var point = mGo.transform.position;
            var normal = mGo.transform.TransformDirection(Vector3.back);

            var mirrorPlane = global::Mirror.GetPlane(point, normal);
            var mirrorMatrix = global::Mirror.GetMirrorMatrix(mirrorPlane);
            bCam.worldToCameraMatrix = aCam.worldToCameraMatrix * mirrorMatrix;

            Vector3 bUpActual = bCam.cameraToWorldMatrix.MultiplyVector(Vector3.up);

            Assert.That(bUpActual.normalized, Is.EqualTo((d.bUp - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void MatrixTransformPositionTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir);

            global::Mirror.MirrorTransform(aCam, bCam, mGo.transform);

            Vector3 bPosActual = bCam.cameraToWorldMatrix.MultiplyPoint(Vector3.zero);

            Assert.That(bPosActual, Is.EqualTo(d.bPos).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void MatrixTransformRotationForwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            global::Mirror.MirrorTransform(aCam, bCam, mGo.transform);

            Vector3 bDirActual = bCam.cameraToWorldMatrix.MultiplyVector(Vector3.back);

            Assert.That(bDirActual.normalized, Is.EqualTo((d.bDir - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MatrixTestCaseSource))]
        public void MatrixTransformRotationUpwardTest(Data d)
        {
            var aGo = new GameObject("A");
            aGo.transform.position = d.aPos;
            aGo.transform.LookAt(d.aDir, d.aUp - d.aPos);
            var aCam = aGo.AddComponent<Camera>();

            var bGo = new GameObject("B");
            // bGo.transform.position = d.bPos;
            // bGo.transform.LookAt(d.bDir, d.bUp - d.bPos);
            var bCam = bGo.AddComponent<Camera>();

            var mGo = new GameObject("M");
            mGo.transform.position = d.mPos;
            mGo.transform.LookAt(d.mDir, d.mUp - d.mPos);

            global::Mirror.MirrorTransform(aCam, bCam, mGo.transform);

            Vector3 bUpActual = bCam.cameraToWorldMatrix.MultiplyVector(Vector3.up);

            Assert.That(bUpActual.normalized, Is.EqualTo((d.bUp - d.bPos).normalized).Using(Vector3EqualityComparer.Instance));
        }
    }
}
