using System.Text;
using UnityEngine;

namespace Tests.Editor
{
    public static class DataHash
    {
        public static string Hash(Data data)
        {
            var res = new StringBuilder();

            res.Append(ToChar(data.aPos.x));
            res.Append(ToChar(data.aPos.y));
            res.Append(ToChar(data.aPos.z));
            res.Append(ToChar(data.aDir.x));
            res.Append(ToChar(data.aDir.y));
            res.Append(ToChar(data.aDir.z));
            res.Append(ToChar(data.aUp.x));
            res.Append(ToChar(data.aUp.y));
            res.Append(ToChar(data.aUp.z));
            res.Append(ToChar(data.bPos.x));
            res.Append(ToChar(data.bPos.y));
            res.Append(ToChar(data.bPos.z));
            res.Append(ToChar(data.bDir.x));
            res.Append(ToChar(data.bDir.y));
            res.Append(ToChar(data.bDir.z));
            res.Append(ToChar(data.bUp.x));
            res.Append(ToChar(data.bUp.y));
            res.Append(ToChar(data.bUp.z));
            res.Append(ToChar(data.mPos.x));
            res.Append(ToChar(data.mPos.y));
            res.Append(ToChar(data.mPos.z));
            res.Append(ToChar(data.mDir.x));
            res.Append(ToChar(data.mDir.y));
            res.Append(ToChar(data.mDir.z));
            res.Append(ToChar(data.mUp.x));
            res.Append(ToChar(data.mUp.y));
            res.Append(ToChar(data.mUp.z));

            return res.ToString();
        }

        private static char ToChar(float value)
        {
            return false ? 'X'
                : Eq(value, -5) ? 'v'
                : Eq(value, -4) ? 'w'
                : Eq(value, -3) ? 'x'
                : Eq(value, -2) ? 'y'
                : Eq(value, -1) ? 'z'
                : Eq(value,  0) ? '0'
                : Eq(value,  1) ? 'a'
                : Eq(value,  2) ? 'b'
                : Eq(value,  3) ? 'c'
                : Eq(value,  4) ? 'd'
                : Eq(value,  5) ? 'e'
                : '_';
        }

        private static bool Eq(float f1, float f2)
        {
            const double tolerance = 0.1f;
            return Mathf.Abs(f1 - f2) < tolerance;
        }
    }
}
