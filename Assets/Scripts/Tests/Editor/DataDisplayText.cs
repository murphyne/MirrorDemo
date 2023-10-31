using System.Collections.Generic;
using System.Text;

namespace Tests.Editor
{
    public class DataDisplayText
    {
        private const int XMin = -5, XMax = 5, X0 = 0;
        private const int ZMin = -5, ZMax = 5, Z0 = 0;

        private readonly Data _data;

        public DataDisplayText(Data data)
        {
            _data = data;
        }

        public string Render()
        {
            var stringBuilder = new StringBuilder();

            for (int z = ZMin; z <= ZMax; z++)
            {
                for (int x = XMin; x <= XMax; x++)
                {
                    var cellString = InitCellString(x, z);
                    var symbols = GetSymbols(_data, x, z);
                    var symbolsString = GetSymbolsString(symbols);
                    cellString = FillString(cellString, symbolsString);

                    stringBuilder.Append(cellString);
                }

                stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }

        private static IReadOnlyList<char> GetSymbols(Data data, int x, int z)
        {
            var symbols = new List<char>();

            if (x == data.aPos.x && z == data.aPos.z) symbols.Add('A');
            if (x == data.aDir.x && z == data.aDir.z) symbols.Add('a');
            if (x == data.bPos.x && z == data.bPos.z) symbols.Add('B');
            if (x == data.bDir.x && z == data.bDir.z) symbols.Add('b');
            if (x == data.mPos.x && z == data.mPos.z) symbols.Add('M');
            if (x == data.mDir.x && z == data.mDir.z) symbols.Add('m');

            return symbols;
        }

        private static string InitCellString(int x, int z)
        {
            bool xMin = x == XMin;
            bool xMax = x == XMax;
            bool zMin = z == ZMin;
            bool zMax = z == ZMax;
            bool x0 = x == X0;
            bool z0 = z == Z0;

            return false ? string.Empty
                : (x0 && zMin) ? $"{ZMin,2} "
                : (x0 && zMax) ? $"{ZMax,2} "
                : (z0 && xMin) ? $"{XMin,2} "
                : (z0 && xMax) ? $"{XMax,2} "
                : (x0 && z0) ? "─┼─"
                : (z0) ? "───"
                : (x0) ? " │ "
                : " · ";
        }

        private static string GetSymbolsString(IReadOnlyList<char> chars)
        {
            if (chars.Count == 0)
                return "   ";

            if (chars.Count == 1)
                return $" {chars[0]} ";

            if (chars.Count == 2)
                return $"{chars[0]}/{chars[1]}";

            if (chars.Count == 3)
                return $"{chars[0]}{chars[1]}{chars[2]}";

            return "...";
        }

        private static string FillString(string original, string substitute)
        {
            var originalChars = original.ToCharArray();

            for (var i = 0; i < originalChars.Length; i++)
            {
                var substituteChar = substitute[i];
                if (!char.IsWhiteSpace(substituteChar))
                    originalChars[i] = substituteChar;
            }

            return new string(originalChars);
        }
    }
}
