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
                    var cell = Cell(_data, x, z);

                    stringBuilder.Append(cell);
                }

                stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }

        private static string Cell(Data data, int x, int z)
        {
            bool aPX = x == data.aPos.x;
            bool aPZ = z == data.aPos.z;
            bool aDX = x == data.aDir.x;
            bool aDZ = z == data.aDir.z;
            bool bPX = x == data.bPos.x;
            bool bPZ = z == data.bPos.z;
            bool bDX = x == data.bDir.x;
            bool bDZ = z == data.bDir.z;
            bool mPX = x == data.mPos.x;
            bool mPZ = z == data.mPos.z;
            bool mDX = x == data.mDir.x;
            bool mDZ = z == data.mDir.z;
            bool xMin = x == XMin;
            bool xMax = x == XMax;
            bool zMin = z == ZMin;
            bool zMax = z == ZMax;
            bool x0 = x == X0;
            bool z0 = z == Z0;

            var cell = false ? string.Empty
                : (aPX && aPZ && bPX && bPZ) ? "A⁄B"
                : (aDX && aDZ && bDX && bDZ) ? "a⁄b"
                : (aDX && aDZ && mDX && mDZ) ? "a⁄m"
                : (bDX && bDZ && mDX && mDZ) ? "b⁄m"
                : (mPX && mPZ && mDX && mDZ) ? "M⁄m"
                : (aPX && aPZ) ? " A "
                : (bPX && bPZ) ? " B "
                : (aDX && aDZ) ? " a "
                : (bDX && bDZ) ? " b "
                : (mPX && mPZ) ? " M "
                : (mDX && mDZ) ? " m "
                : (x0 && zMin) ? $"{ZMin,2} "
                : (x0 && zMax) ? $"{ZMax,2} "
                : (z0 && xMin) ? $"{XMin,2} "
                : (z0 && xMax) ? $"{XMax,2} "
                : (x0 && z0) ? "─┼─"
                : (z0) ? "───"
                : (x0) ? " │ "
                : " · ";
            return cell;
        }
    }
}
