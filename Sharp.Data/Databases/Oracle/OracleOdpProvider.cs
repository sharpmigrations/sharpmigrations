using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Sharp.Data.Databases;
using Sharp.Data.Util;

namespace Sharp.Data.Providers {
    public class OracleOdpProvider : DataProvider {

        private static PropertyInfo _propOracleDbType;
        private static Type _oracleRefCursorType; 

        public OracleOdpProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override IDbDataParameter GetParameterCursor() {
            IDbDataParameter parameter = DbProviderFactory.CreateParameter();
            EnsureProperties(parameter);
            _propOracleDbType.SetValue(parameter, _oracleRefCursorType, null);
            return parameter;
        }

        private void EnsureProperties(IDbDataParameter parameter) {
            if(_propOracleDbType != null) return;
            Type parameterType = parameter.GetType();
            Assembly assembly = parameterType.Assembly;
            _oracleRefCursorType = assembly.GetType("Oracle.DataAccess.Client.OracleDbType.RefCursor");
            _propOracleDbType = parameterType.GetProperty("OracleDbType", ReflectionHelper.NoRestrictions);
        }
    }
}
