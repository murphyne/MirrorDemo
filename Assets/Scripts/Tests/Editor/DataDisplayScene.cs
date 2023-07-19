using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Editor
{
    public static class DataDisplayScene
    {
        public static void Render(Data data)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var colorA = new Color(0.70f, 0.20f, 0.10f);
            var colorB = new Color(0.20f, 0.40f, 0.70f);
            var colorM = new Color(0.90f, 0.40f, 0.90f);

            var meshQuad = PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Quad);
            var meshCube = PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Cube);

            var aGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // var aGo = new GameObject();
            aGo.name = "A";
            aGo.transform.position = data.aPos;
            aGo.transform.LookAt(data.aDir);
            aGo.transform.localScale = Vector3.one * 0.3f;
            var aGizmo = aGo.AddComponent<Gizmo>();
            aGizmo.meshPos = meshCube;
            aGizmo.meshDir = meshCube;
            aGizmo.scaleMeshDir = Vector3.one * 0.5f;
            aGizmo.color = colorA;
            aGizmo.pos = data.aPos;
            aGizmo.dir = data.aDir;

            var bGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // var bGo = new GameObject();
            bGo.name = "B";
            bGo.transform.position = data.bPos;
            bGo.transform.LookAt(data.bDir);
            bGo.transform.localScale = Vector3.one * 0.3f;
            var bGizmo = bGo.AddComponent<Gizmo>();
            bGizmo.meshPos = meshCube;
            bGizmo.meshDir = meshCube;
            bGizmo.scaleMeshDir = Vector3.one * 0.5f;
            bGizmo.color = colorB;
            bGizmo.pos = data.bPos;
            bGizmo.dir = data.bDir;

            var mGo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // var mGo = new GameObject();
            mGo.name = "M";
            mGo.transform.position = data.mPos;
            mGo.transform.LookAt(data.mDir);
            var mGizmo = mGo.AddComponent<Gizmo>();
            mGizmo.meshPos = meshQuad;
            mGizmo.meshDir = meshCube;
            mGizmo.scaleMeshDir = Vector3.one * 0.2f;
            mGizmo.color = colorM;
            mGizmo.pos = data.mPos;
            mGizmo.dir = data.mDir;

            var hash = DataHash.Hash(data);
            WriteFile(hash, scene);
        }

        private static void WriteFile(string hash, Scene scene)
        {
            const string dirPathRel = "Assets/Scripts/Tests/DisplayScenes";
            System.IO.Directory.CreateDirectory(dirPathRel);

            var fileName = $"test-{hash}.unity";
            var filePathRel = System.IO.Path.Combine(dirPathRel, fileName);
            EditorSceneManager.SaveScene(scene, filePathRel, false);

            var dirPathCur = System.IO.Directory.GetCurrentDirectory();
            var filePathAbs = System.IO.Path.Combine(dirPathCur, dirPathRel, fileName);
            Debug.Log(filePathAbs);
        }
    }
}
