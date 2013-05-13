using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Sharp.Data.Exceptions;
using Sharp.Data.Util;

namespace Sharp.Data.Databases.Oracle {
    public class OracleOdpProvider : DataProvider {

        private static PropertyInfo _propOracleDbType;
        private static Type _oracleRefCursorType;
        private static Type _oracleDbCommandType;
        private static PropertyInfo _propBindByName;

        public override string Name {
            get { return DataProviderNames.OracleOdp; }
        }

        public override DatabaseKind DatabaseKind {
            get { return DatabaseKind.Oracle; }
        }

        protected virtual string RefCursorFullName {
            get { return "Oracle.DataAccess.Client.OracleDbType.RefCursor"; }
        }

        public OracleOdpProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
            
        }

        public override void ConfigCommand(IDbCommand command) {
            EnsureCommandProperties(command);
            _propBindByName.SetValue(command, true, null);
        }

        public override IDbDataParameter GetParameterCursor() {
            IDbDataParameter parameter = DbProviderFactory.CreateParameter();
            EnsureDataParameterProperties(parameter);
            _propOracleDbType.SetValue(parameter, _oracleRefCursorType, null);
            return parameter;
        }

        private void EnsureCommandProperties(IDbCommand dbCommand) {
            if (_oracleDbCommandType != null) return;
            _oracleDbCommandType = dbCommand.GetType();
            _propBindByName = _oracleDbCommandType.GetProperty("BindByName", ReflectionHelper.NoRestrictions);
        }

        private void EnsureDataParameterProperties(IDbDataParameter parameter) {
            if(_propOracleDbType != null) return;
            
            Type parameterType = parameter.GetType();
            Assembly assembly = parameterType.Assembly;
            
            _oracleRefCursorType = assembly.GetType(RefCursorFullName);
            _propOracleDbType = parameterType.GetProperty("OracleDbType", ReflectionHelper.NoRestrictions);
        }

        public override DatabaseException CreateSpecificException(Exception exception, string sql) {
            if (exception.Message.Contains("ORA-00942")) {
                return new TableNotFoundException(exception.Message, exception, sql);
            }
            if (exception.Message.Contains("ORA-00001")) {
                return new UniqueConstraintException(exception.Message, exception, sql);
            }
            return base.CreateSpecificException(exception, sql);
        }
    }
}
