using UnityEngine;

namespace Tests.Editor
{
    public class DataDisplayTexture
    {
        private const int XMinWorld = -5, XMaxWorld = 5;
        private const int ZMinWorld = -5, ZMaxWorld = 5;
        private const int XLenWorld = XMaxWorld - XMinWorld;
        private const int ZLenWorld = ZMaxWorld - ZMinWorld;

        private const int CellLenPixel = 9;
        private const int XMinPixel = 0;
        private const int ZMinPixel = 0;
        private const int XMaxPixel = CellLenPixel * XLenWorld + XLenWorld;
        private const int ZMaxPixel = CellLenPixel * ZLenWorld + ZLenWorld;

        private static readonly int X0Pixel = (CellLenPixel + 1) * Mathf.Abs(XMinWorld);
        private static readonly int Z0Pixel = (CellLenPixel + 1) * Mathf.Abs(ZMinWorld);

        private static readonly Color ColorBack = new Color(0.90f, 0.90f, 0.90f);
        private static readonly Color ColorGrid = new Color(0.85f, 0.85f, 0.85f);
        private static readonly Color ColorAxis = new Color(0.75f, 0.75f, 0.75f);
        private static readonly Color ColorDot = new Color(0.50f, 0.50f, 0.50f);
        private static readonly Color ColorA = new Color(0.70f, 0.20f, 0.10f);
        private static readonly Color ColorB = new Color(0.20f, 0.40f, 0.70f);
        private static readonly Color ColorM = new Color(0.90f, 0.40f, 0.90f);

        private const int TextureSourceWidth = XMaxPixel + 1;
        private const int TextureSourceHeight = ZMaxPixel + 1;

        private readonly Data _data;

        public DataDisplayTexture(Data data)
        {
            _data = data;
        }

        public void Render()
        {
            var aPosWorld = _data.aPos;
            var bPosWorld = _data.bPos;
            var aDirWorld = _data.aDir;
            var bDirWorld = _data.bDir;
            var mPosCWorld = _data.mPos;
            var mDirFWorld = _data.mDir;
            var mDirLWorld = mPosCWorld + Vector3.Cross(mDirFWorld - mPosCWorld, _data.mUp);
            var mDirRWorld = mPosCWorld + Vector3.Cross(mDirFWorld - mPosCWorld, _data.mUp * -1);

            var texture = new Texture2D(TextureSourceWidth, TextureSourceHeight, TextureFormat.RGBA32, false);

            DrawBackground(texture, ColorBack);
            DrawGrid(texture, ColorGrid);
            DrawAxes(texture, ColorAxis);
            DrawDotGrid(texture, ColorDot);

            var aPosXPixel = (int)ToXPixel(aPosWorld.x);
            var bPosXPixel = (int)ToXPixel(bPosWorld.x);
            var aPosZPixel = (int)ToZPixel(aPosWorld.z);
            var bPosZPixel = (int)ToZPixel(bPosWorld.z);
            var aDirXPixel = (int)ToXPixel(aDirWorld.x);
            var bDirXPixel = (int)ToXPixel(bDirWorld.x);
            var aDirZPixel = (int)ToZPixel(aDirWorld.z);
            var bDirZPixel = (int)ToZPixel(bDirWorld.z);

            var mPosCXPixel = (int)ToXPixel(mPosCWorld.x);
            var mPosCZPixel = (int)ToZPixel(mPosCWorld.z);
            var mDirFXPixel = (int)ToXPixel(mDirFWorld.x);
            var mDirFZPixel = (int)ToZPixel(mDirFWorld.z);
            var mDirLXPixel = (int)ToXPixel(mDirLWorld.x);
            var mDirLZPixel = (int)ToZPixel(mDirLWorld.z);
            var mDirRXPixel = (int)ToXPixel(mDirRWorld.x);
            var mDirRZPixel = (int)ToZPixel(mDirRWorld.z);

            // Draw points.
            DrawA(texture, aPosXPixel, aPosZPixel, ColorA);
            DrawB(texture, bPosXPixel, bPosZPixel, ColorB);
            DrawPlus(texture, mPosCXPixel, mPosCZPixel, ColorM);

            // Draw directions.
            DrawLine(texture, aPosXPixel, aPosZPixel, aDirXPixel, aDirZPixel, ColorA);
            DrawLine(texture, bPosXPixel, bPosZPixel, bDirXPixel, bDirZPixel, ColorB);
            DrawLine(texture, mDirLXPixel, mDirLZPixel, mDirRXPixel, mDirRZPixel, ColorM);
            DrawLine(texture, mPosCXPixel, mPosCZPixel, mDirFXPixel, mDirFZPixel, ColorM);

            var targetTexture = Resize(texture, 10);
            var bytes = targetTexture.EncodeToPNG();
            var hash = DataHash.Hash(_data);

            WriteFile(bytes, hash);
        }

        private static void WriteFile(byte[] bytes, string hash)
        {
            const string dirPathRel = "Assets/Scripts/Tests/DisplayTextures";
            System.IO.Directory.CreateDirectory(dirPathRel);

            var fileName = $"test-{hash}.png";
            var filePathRel = System.IO.Path.Combine(dirPathRel, fileName);
            System.IO.File.WriteAllBytes(filePathRel, bytes);

            var dirPathCur = System.IO.Directory.GetCurrentDirectory();
            var filePathAbs = System.IO.Path.Combine(dirPathCur, dirPathRel, fileName);
            Debug.Log(filePathAbs);
        }

        private static Texture2D Resize(Texture2D source, int scale)
        {
            var sourceWidth = source.width;
            var sourceHeight = source.height;
            var targetWidth = sourceWidth * scale;
            var targetHeight = sourceHeight * scale;

            var target = new Texture2D(targetWidth, targetHeight);

            for (int x = 0; x < sourceWidth; x++)
            {
                for (int y = 0; y < sourceHeight; y++)
                {
                    var color = source.GetPixel(x, y);
                    DrawBlock(target, x * scale, y * scale, scale, color);
                }
            }

            return target;
        }

        private static void DrawBackground(Texture2D texture, Color color)
        {
            for (int zPixel = ZMinPixel; zPixel <= ZMaxPixel; zPixel++)
            {
                for (int xPixel = XMinPixel; xPixel <= XMaxPixel; xPixel++)
                {
                    texture.SetPixel(xPixel, zPixel, color);
                }
            }
        }

        private static void DrawGrid(Texture2D texture, Color color)
        {
            for (int zWorld = ZMinWorld; zWorld <= ZMaxWorld; zWorld++)
            {
                var zPixel = (int)ToZPixel(zWorld);
                for (int xPixel = XMinPixel; xPixel <= XMaxPixel; xPixel++)
                {
                    texture.SetPixel(xPixel, zPixel, color);
                }
            }
            for (int xWorld = XMinWorld; xWorld <= XMaxWorld; xWorld++)
            {
                var xPixel = (int)ToXPixel(xWorld);
                for (int zPixel = ZMinPixel; zPixel <= ZMaxPixel; zPixel++)
                {
                    texture.SetPixel(xPixel, zPixel, color);
                }
            }
        }

        private static void DrawAxes(Texture2D texture, Color color)
        {
            for (int xPixel = XMinPixel; xPixel <= XMaxPixel; xPixel++) texture.SetPixel(xPixel, Z0Pixel, color);
            for (int zPixel = ZMinPixel; zPixel <= ZMaxPixel; zPixel++) texture.SetPixel(X0Pixel, zPixel, color);
        }

        private static void DrawDotGrid(Texture2D texture, Color color)
        {
            for (int zWorld = ZMinWorld; zWorld <= ZMaxWorld; zWorld++)
            {
                var zPixel = (int)ToZPixel(zWorld);
                for (int xWorld = XMinWorld; xWorld <= XMaxWorld; xWorld++)
                {
                    var xPixel = (int)ToXPixel(xWorld);
                    texture.SetPixel(xPixel, zPixel, color);
                }
            }
        }

        private static void DrawPlus(Texture2D texture, int x, int y, Color color)
        {
            texture.SetPixel(x - 1, y, color);
            texture.SetPixel(x + 1, y, color);
            texture.SetPixel(x, y - 1, color);
            texture.SetPixel(x, y + 1, color);
        }

        private static void DrawA(Texture2D texture, int x, int y, Color color)
        {
            DrawLine(texture, x - 1, y + 1, x - 1, y - 2, color);
            DrawLine(texture, x + 1, y + 1, x + 1, y - 2, color);
            texture.SetPixel(x, y + 2, color);
            texture.SetPixel(x, y, color);
        }

        private static void DrawB(Texture2D texture, int x, int y, Color color)
        {
            DrawLine(texture, x - 1, y + 2, x - 1, y - 2, color);
            texture.SetPixel(x, y + 2, color);
            texture.SetPixel(x + 1, y + 1, color);
            texture.SetPixel(x, y, color);
            texture.SetPixel(x + 1, y - 1, color);
            texture.SetPixel(x, y - 2, color);
        }

        private static void DrawBlock(Texture2D texture, int x0, int y0, int size, Color color)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    texture.SetPixel(x0 + x, y0 + y, color);
                }
            }
        }

        private static void DrawLine(Texture2D texture, int x1, int y1, int x2, int y2, Color color)
        {
            float width = x2 - x1;
            float height = y2 - y1;
            float length = Mathf.Max(Mathf.Abs(width), Mathf.Abs(height));
            float dx = width / length;
            float dy = height / length;

            float xPixel = x1;
            float yPixel = y1;
            int intLength = (int)length;
            for (int i = 0; i <= intLength; i++)
            {
                texture.SetPixel(Mathf.RoundToInt(xPixel), Mathf.RoundToInt(yPixel), color);

                xPixel += dx;
                yPixel += dy;
            }
        }

        private static float ToXPixel(float xWorld) =>
            Mathf.Lerp(XMinPixel, XMaxPixel, Mathf.InverseLerp(XMinWorld, XMaxWorld, xWorld));

        private static float ToZPixel(float zWorld) =>
            Mathf.Lerp(ZMinPixel, ZMaxPixel, Mathf.InverseLerp(ZMinWorld, ZMaxWorld, zWorld));
    }
}
