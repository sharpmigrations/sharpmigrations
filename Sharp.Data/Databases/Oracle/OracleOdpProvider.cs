using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Sharp.Data.Exceptions;
using Sharp.Data.Util;

namespace Sharp.Data.Databases.Oracle {
    public class OracleOdpProvider : DataProvider {

        private static PropertyInfo _propOracleDbType;
        private static Type _oracleRefCursorType;
        private static Type _oracleDbCommandType;
        private static PropertyInfo _propBindByName;
        private static PropertyInfo _propArrayBindCount;

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

        public override void ConfigCommand(IDbCommand command, object[] parameters) {
            EnsureCommandProperties(command);
            _propBindByName.SetValue(command, true, null);

            if(parameters == null || !parameters.Any()) return;
            var param = parameters[0];
            if (param is In) {
                param = ((In) param).Value;
            }
            var collParam = param as ICollection;
            if(collParam == null || collParam.Count == 0) return;
            _propArrayBindCount.SetValue(command, collParam.Count, null);
        }

        public override IDbDataParameter GetParameter() {
            var par = base.GetParameter();
            //without this, bulk insert doesn't work. Thanks Oracle.
            par.DbType = DbType.String;
            return par;
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
            _propArrayBindCount = _oracleDbCommandType.GetProperty("ArrayBindCount", ReflectionHelper.NoRestrictions);
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
