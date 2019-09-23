using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Common.Helper
{
    public static class UtilConvert
    {
        public static int ObjToInt(this object thisValue)
        {
            int result = 0;
            if (thisValue == null)
            {
                return 0;
            }
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return result;
        }

        public static int ObjToInt(this object thisValue, int errorValue)
        {
            int result = 0;
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static double ObjToMoney(this object thisValue)
        {
            double result = 0.0;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return 0.0;
        }

        public static double ObjToMoney(this object thisValue, double errorValue)
        {
            double result = 0.0;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static string ObjToString(this object thisValue)
        {
            if (thisValue != null)
            {
                return thisValue.ToString().Trim();
            }
            return "";
        }

        public static string ObjToString(this object thisValue, string errorValue)
        {
            if (thisValue != null)
            {
                return thisValue.ToString().Trim();
            }
            return errorValue;
        }

        public static decimal ObjToDecimal(this object thisValue)
        {
            decimal result = default(decimal);
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return decimal.Zero;
        }

        public static decimal ObjToDecimal(this object thisValue, decimal errorValue)
        {
            decimal result = default(decimal);
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static DateTime ObjToDate(this object thisValue)
        {
            DateTime result = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out result))
            {
                result = Convert.ToDateTime(thisValue);
            }
            return result;
        }

        public static DateTime ObjToDate(this object thisValue, DateTime errorValue)
        {
            DateTime result = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static bool ObjToBool(this object thisValue)
        {
            bool result = false;
            if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return result;
        }
    }
}
