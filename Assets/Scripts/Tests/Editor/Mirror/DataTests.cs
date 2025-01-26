using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor.Mirror
{
    [TestFixture]
    public class DataTests
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
    }
}
