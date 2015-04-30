using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathieuJaumain.Tools
{
    public class Utils
    {
        public static double NormalizeTo360Angle(double angle)
        {
            angle = angle % 360;

            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }

        public static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("\n", string.Empty);
            hex = hex.Replace("\t", string.Empty);
            hex = hex.Replace("\r", string.Empty);
            hex = hex.Trim();


            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        public static bool IsIPv4(string addrss)
        {
            string[] parts = addrss.Split('.');
            if (parts.Length != 4)
                return false;

            foreach (string part in parts)
            {
                int i = -1;
                if (!int.TryParse(part, out i) && i > 0 && i < 256)
                    return false;
            }
            return true;
        }
    }
}
