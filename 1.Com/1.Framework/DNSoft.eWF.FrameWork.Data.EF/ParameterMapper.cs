using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace DNSoft.eWF.FrameWork.Data.EF
{
    public class ParameterMapper
    {
        public static SqlParameter[] Mapping(object target)
        {
            return Mapping(target, null);
        }

        public static SqlParameter[] Mapping(object target, params SqlParameter[] userparameters)
        {
            List<SqlParameter> parameters = _Mapping(target);
            if (userparameters != null)
            {
                parameters.AddRange(userparameters);
            }
            return parameters != null ? parameters.ToArray() : null;
        }

        public static List<SqlParameter> MappingToList(object target, params SqlParameter[] userparameters)
        {
            List<SqlParameter> parameters = _Mapping(target);
            if (userparameters != null)
            {
                parameters.AddRange(userparameters);
            }
            return parameters != null ? parameters.ToList() : null;
        }

        public static List<SqlParameter> MappingToList(object target)
        {
            return MappingToList(target, null);
        }

        private static List<SqlParameter> _Mapping(object target)
        {
            List<SqlParameter> parameters = null;
            if (target != null)
            {
                string[] nameList = GetPropertyNameList(target.GetType(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                List<PropertyInfo> plist = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).ToList();// ORMMappingCache.GetPropertyInfoList(target.GetType());
                parameters = new List<SqlParameter>(nameList.Length);
                parameters.AddRange(nameList.Select(name =>
                {
                    var pInfo = plist.Find(p => p.Name == name);
                    switch (pInfo.PropertyType.ToString())
                    {
                        case "System.Nullable`1[System.Int32]":
                            return new SqlParameter(string.Format("@{0}", name),
                              pInfo.GetValue(target, null) ??
                               System.Data.SqlTypes.SqlInt32.Null);
                        case "System.Nullable`1[System.Int64]":
                            return new SqlParameter(string.Format("@{0}", name),
                              pInfo.GetValue(target, null) ??
                               System.Data.SqlTypes.SqlInt64.Null);
                        case "System.Nullable`1[System.Int16]":
                            return new SqlParameter(string.Format("@{0}", name),
                              pInfo.GetValue(target, null) ??
                               System.Data.SqlTypes.SqlInt16.Null);
                        case "System.Nullable`1[System.Decimal]":
                            return new SqlParameter(string.Format("@{0}", name),
                            pInfo.GetValue(target, null) ??
                             System.Data.SqlTypes.SqlDecimal.Null);
                        case "System.Nullable`1[System.DateTime]":
                            return new SqlParameter(string.Format("@{0}", name),
                             pInfo.GetValue(target, null) ??
                              System.Data.SqlTypes.SqlDateTime.Null);
                        default:
                            return new SqlParameter(string.Format("@{0}", name),
                                 pInfo.GetValue(target, null) ??
                                  System.DBNull.Value);
                    }

                }));
            }
            return parameters;
        }

        /// <summary>
        /// target object의 속성중 "propertyName"가 있을 경우 그 값을 조회하여 반환합니다.
        /// </summary>
        /// <param name="target">목표 object</param>
        /// <param name="propertyName">속성이름</param>
        /// <returns></returns>
        private static object GetPropertyValue(object target, string propertyName)
        {
            object val = null;
            if (target != null)
            {
                PropertyInfo pi = FindPropertyFast(target, propertyName);
                val = pi.GetValue(target, null);
            }
            return val;
        }


        /// <summary>
        /// t Type의 속성의 이름을 string[]으로 반환합니다.
        /// </summary>
        /// <param name="t">type</param>
        /// <param name="flag">바인딩 플래그</param>
        /// <returns></returns>
        private static string[] GetPropertyNameList(Type t, BindingFlags flag)
        {
            string[] col = null;
            if (t != null)
            {
                List<PropertyInfo> plist = t.GetProperties().ToList();// ORMMappingCache.GetPropertyInfoList(t);
                col = new string[plist.Count];
                int i = 0;
                foreach (PropertyInfo p in plist)
                {
                    col[i] = p.Name;
                    i++;
                }
            }
            return col;
        }

        private static PropertyInfo FindPropertyFast(object target, string columnName)
        {
            PropertyInfo info = FindPropertyByOnlyName(target, columnName);
            return info;
        }

        /// <summary>
        /// 속성 이름으로만 target개체에서 columnname 속성을 찾아 반환합니다.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static PropertyInfo FindPropertyByOnlyName(object target, string columnName)
        {
            PropertyInfo pi = null;
            if (target != null)
            {
                pi = target.GetType().GetProperty(columnName);// ORMMappingCache.GetPropertyInfo(target.GetType(), columnName);
            }
            return pi;
        }
    }
}
