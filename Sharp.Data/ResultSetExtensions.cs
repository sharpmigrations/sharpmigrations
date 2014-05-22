using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sharp.Data.Util;

namespace Sharp.Data {
    public static class ResultSetExtensions {
        public static List<T> Map<T>(this ResultSet res) where T : new() {
            var list = new List<T>();
            Type type = typeof(T);
            var columns = res.GetColumnNames();
            var columnProps =
                columns.Select(x => type.GetProperty(x, ReflectionHelper.NoRestrictions | BindingFlags.IgnoreCase))
                    .ToList();
            foreach (var row in res) {
                var obj = new T();
                for (int i = 0; i < columnProps.Count; i++) {
                    var propertyInfo = columnProps[i];
                    if (propertyInfo == null) {
                        continue;
                    }
                    object value = row[i];
                    var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

                    if (value != null && value.GetType() != targetType) {
                        value = Convert.ChangeType(value, targetType);
                    }
                    propertyInfo.SetValue(obj, value, null);
                }
                list.Add(obj);                    

            }
            return list;
        }

        private static bool IsNullableType(Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}