using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Editor.Portal
{
    public class DataDisplayScene
    {
        private readonly Data _data;
        private readonly string _name;

        public DataDisplayScene(Data data, string name)
        {
            _data = data;
            _name = name;
        }

        public void Render()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var colorA = new Color(0.70f, 0.20f, 0.10f);
            var colorB = new Color(0.20f, 0.40f, 0.70f);
            var colorP = new Color(0.90f, 0.40f, 0.90f);
            var colorQ = new Color(0.90f, 0.40f, 0.90f);

            var meshQuad = PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Quad);
            var meshCube = PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Cube);

            var aGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // var aGo = new GameObject();
            aGo.name = "A";
            aGo.transform.position = _data.aPos;
            aGo.transform.LookAt(_data.aDir, _data.aUp - _data.aPos);
            aGo.transform.localScale = Vector3.one * 0.3f;
            var aGizmo = aGo.AddComponent<Gizmo>();
            aGizmo.meshPos = meshCube;
            aGizmo.meshDir = meshCube;
            aGizmo.scaleMeshDir = Vector3.one * 0.5f;
            aGizmo.color = colorA;
            aGizmo.pos = _data.aPos;
            aGizmo.dir = _data.aDir;
            aGizmo.up = _data.aUp;

            var bGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // var bGo = new GameObject();
            bGo.name = "B";
            bGo.transform.position = _data.bPos;
            bGo.transform.LookAt(_data.bDir, _data.bUp - _data.bPos);
            bGo.transform.localScale = Vector3.one * 0.3f;
            var bGizmo = bGo.AddComponent<Gizmo>();
            bGizmo.meshPos = meshCube;
            bGizmo.meshDir = meshCube;
            bGizmo.scaleMeshDir = Vector3.one * 0.5f;
            bGizmo.color = colorB;
            bGizmo.pos = _data.bPos;
            bGizmo.dir = _data.bDir;
            bGizmo.up = _data.bUp;

            var pGo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // var pGo = new GameObject();
            pGo.name = "P";
            pGo.transform.position = _data.pPos;
            pGo.transform.LookAt(_data.pDir, _data.pUp - _data.pPos);
            var pGizmo = pGo.AddComponent<Gizmo>();
            pGizmo.meshPos = meshQuad;
            pGizmo.meshDir = meshCube;
            pGizmo.scaleMeshDir = Vector3.one * 0.2f;
            pGizmo.color = colorP;
            pGizmo.pos = _data.pPos;
            pGizmo.dir = _data.pDir;
            pGizmo.up = _data.pUp;

            var qGo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // var qGo = new GameObject();
            qGo.name = "Q";
            qGo.transform.position = _data.qPos;
            qGo.transform.LookAt(_data.qDir, _data.qUp - _data.qPos);
            var qGizmo = qGo.AddComponent<Gizmo>();
            qGizmo.meshPos = meshQuad;
            qGizmo.meshDir = meshCube;
            qGizmo.scaleMeshDir = Vector3.one * 0.2f;
            qGizmo.color = colorQ;
            qGizmo.pos = _data.qPos;
            qGizmo.dir = _data.qDir;
            qGizmo.up = _data.qUp;

            WriteFile(scene, _name);
        }

        private static void WriteFile(Scene scene, string name)
        {
            const string dirPathRel = "Assets/Scripts/Tests/PortalData/Scenes";
            System.IO.Directory.CreateDirectory(dirPathRel);

            var fileName = $"test-{name}.unity";
            var filePathRel = System.IO.Path.Combine(dirPathRel, fileName);
            EditorSceneManager.SaveScene(scene, filePathRel, false);

            var dirPathCur = System.IO.Directory.GetCurrentDirectory();
            var filePathAbs = System.IO.Path.Combine(dirPathCur, dirPathRel, fileName);
            Debug.Log(filePathAbs);
        }
    }
}
