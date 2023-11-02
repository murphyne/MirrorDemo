using System.Collections.Generic;
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
        private const int PaddingLenPixel = 6;
        private static readonly int X0Pixel = (int)ToXPixel(0);
        private static readonly int Z0Pixel = (int)ToZPixel(0);
        private const int XMinPixel = PaddingLenPixel;
        private const int ZMinPixel = PaddingLenPixel;
        private const int XMaxPixel = XMinPixel + (CellLenPixel + 1) * XLenWorld;
        private const int ZMaxPixel = ZMinPixel + (CellLenPixel + 1) * ZLenWorld;
        private const int XLenPixel = XMaxPixel - XMinPixel + 1;
        private const int ZLenPixel = ZMaxPixel - ZMinPixel + 1;

        private static readonly Color ColorBack = new Color(0.90f, 0.90f, 0.90f);
        private static readonly Color ColorGrid = new Color(0.85f, 0.85f, 0.85f);
        private static readonly Color ColorAxis = new Color(0.80f, 0.80f, 0.80f);
        private static readonly Color ColorDots = new Color(0.70f, 0.70f, 0.70f);
        private static readonly Color ColorNums = new Color(0.60f, 0.60f, 0.60f);
        private static readonly Color ColorA = new Color(0.70f, 0.20f, 0.10f);
        private static readonly Color ColorB = new Color(0.20f, 0.40f, 0.70f);
        private static readonly Color ColorM = new Color(0.90f, 0.40f, 0.90f);

        private const string AtlasSymbols = " 0123456789+-AB";
        private static readonly string[,] AtlasPixels = new[,]
        {
            // Disable "wrap lines" and squint you eyes.
            { "   ","███"," █ ","███","███","█ █","███","███","███","███","███","   ","   "," █ ","██ " },
            { "   ","█ █","██ ","  █","  █","█ █","█  ","█  ","  █","█ █","█ █"," █ ","   ","█ █","█ █" },
            { "   ","█ █"," █ ","███","███","███","███","███"," █ ","███","███","███","███","███","██ " },
            { "   ","█ █"," █ ","█  ","  █","  █","  █","█ █","█  ","█ █","  █"," █ ","   ","█ █","█ █" },
            { "   ","███","███","███","███","  █","███","███","█  ","███","███","   ","   ","█ █","██ " },
        };

        private static readonly int SymbolHeight = AtlasPixels.GetLength(0);
        private static readonly int SymbolWidth = AtlasPixels[0,0].Length;

        private const int TextureSourceWidth = PaddingLenPixel + XLenPixel + PaddingLenPixel;
        private const int TextureSourceHeight = PaddingLenPixel + ZLenPixel + PaddingLenPixel;

        private readonly Data _data;
        private readonly string _name;

        public DataDisplayTexture(Data data, string name)
        {
            _data = data;
            _name = name;
        }

        public void Render()
        {
            var aPosWorld = _data.aPos;
            var bPosWorld = _data.bPos;
            var aDirWorld = _data.aDir;
            var bDirWorld = _data.bDir;
            var mPosCWorld = _data.mPos;
            var mDirFWorld = _data.mDir;
            var mDirLWorld = mPosCWorld + Vector3.Cross(mDirFWorld - mPosCWorld, _data.mUp - mPosCWorld);
            var mDirRWorld = mPosCWorld + Vector3.Cross(_data.mUp - mPosCWorld, mDirFWorld - mPosCWorld);

            var texture = new Texture2D(TextureSourceWidth, TextureSourceHeight, TextureFormat.RGBA32, false);

            DrawBackground(texture, ColorBack);
            DrawGrid(texture, ColorGrid);
            DrawAxes(texture, ColorAxis);
            DrawDotGrid(texture, ColorDots);
            DrawLimits(texture, ColorNums);

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
            DrawSymbol(texture, aPosXPixel, aPosZPixel, 'A', ColorA);
            DrawSymbol(texture, bPosXPixel, bPosZPixel, 'B', ColorB);
            DrawSymbol(texture, mPosCXPixel, mPosCZPixel, '+', ColorM);

            // Draw directions.
            DrawLine(texture, aPosXPixel, aPosZPixel, aDirXPixel, aDirZPixel, ColorA);
            DrawLine(texture, bPosXPixel, bPosZPixel, bDirXPixel, bDirZPixel, ColorB);
            DrawLine(texture, mDirLXPixel, mDirLZPixel, mDirRXPixel, mDirRZPixel, ColorM);
            DrawLine(texture, mPosCXPixel, mPosCZPixel, mDirFXPixel, mDirFZPixel, ColorM);

            var targetTexture = Resize(texture, 10);
            var bytes = targetTexture.EncodeToPNG();

            WriteFile(bytes, _name);
        }

        private static void WriteFile(byte[] bytes, string name)
        {
            const string dirPathRel = "Assets/Scripts/Tests/DisplayTextures";
            System.IO.Directory.CreateDirectory(dirPathRel);

            var fileName = $"test-{name}.png";
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
            for (int zPixel = 0; zPixel <= TextureSourceHeight; zPixel++)
            {
                for (int xPixel = 0; xPixel <= TextureSourceWidth; xPixel++)
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

        private static void DrawLimits(Texture2D texture, Color color)
        {
            DrawSymbols(texture, X0Pixel, ZMaxPixel, $"{ZMaxWorld,2} ", color);
            DrawSymbols(texture, X0Pixel, ZMinPixel, $"{ZMinWorld,2} ", color);
            DrawSymbols(texture, XMaxPixel, Z0Pixel, $"{XMaxWorld,2} ", color);
            DrawSymbols(texture, XMinPixel, Z0Pixel, $"{XMinWorld,2} ", color);
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

        private static void DrawSymbols(Texture2D texture, int x, int y, string symbols, Color color)
        {
            const int symbolsSpacing = 1;

            for (int i = 0; i < symbols.Length; i++)
            {
                var xOffset = (i - 1) * (SymbolWidth + symbolsSpacing);
                DrawSymbol(texture, x + xOffset, y, symbols[i], color);
            }
        }

        private static void DrawSymbol(Texture2D texture, int x, int y, char symbol, Color color)
        {
            for (int symbolX = 0; symbolX < SymbolWidth; symbolX++)
            {
                for (int symbolY = 0; symbolY < SymbolHeight; symbolY++)
                {
                    if (ReadAtlas(symbol, symbolX, symbolY))
                    {
                        texture.SetPixel(x - 1 + symbolX, y - 2 + symbolY, color);
                    }
                }
            }
        }

        private static bool ReadAtlas(char symbol, int x, int y)
        {
            var lineIndex = SymbolHeight - 1 - y;
            var symbolIndex = AtlasSymbols.IndexOf(symbol);
            if (symbolIndex == -1) throw new KeyNotFoundException("The given symbol was not present in the atlas.");

            var pixel = AtlasPixels[lineIndex,symbolIndex][x];
            var mask = !char.IsWhiteSpace(pixel);
            return mask;
        }

        private static float ToXPixel(float xWorld) =>
            Mathf.Lerp(XMinPixel, XMaxPixel, Mathf.InverseLerp(XMinWorld, XMaxWorld, xWorld));

        private static float ToZPixel(float zWorld) =>
            Mathf.Lerp(ZMinPixel, ZMaxPixel, Mathf.InverseLerp(ZMinWorld, ZMaxWorld, zWorld));
    }
}
