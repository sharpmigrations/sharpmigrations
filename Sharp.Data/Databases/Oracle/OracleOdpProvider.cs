using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq;
using Sharp.Data.Exceptions;
using Sharp.Data.Util;

namespace Sharp.Data.Databases.Oracle {
    public class OracleOdpProvider : DataProvider {

        private static OracleReflectionCache _reflectionCache = new OracleReflectionCache();

        public virtual OracleReflectionCache ReflectionCache {
            get {
                return _reflectionCache;
            }
        }

        public override string Name {
            get { return DataProviderNames.OracleOdp; }
        }

        public override DatabaseKind DatabaseKind {
            get { return DatabaseKind.Oracle; }
        }

        protected virtual string OracleDbTypeEnumName {
            get { return "Oracle.DataAccess.Client.OracleDbType"; }
        }

        public OracleOdpProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override void ConfigCommand(IDbCommand command, object[] parameters, bool isBulk) {
            EnsureProperties(command);
            if (parameters == null || !parameters.Any()) return;
            ReflectionCache.PropBindByName.SetValue(command, true, null);
            if (!isBulk) {
                return;
            }
            ConfigForBulkSql(command, parameters);
        }

        protected virtual void EnsureProperties(IDbCommand command) {
            if (ReflectionCache.IsCached) return;
            CacheCommandProperties(command);
            CacheParameterProperties(command);
            CacheOracleDbTypeEnumValues(command);
            ReflectionCache.IsCached = true;
        }

        protected virtual void CacheCommandProperties(IDbCommand command) {
            var oracleDbCommandType = command.GetType();
            ReflectionCache.PropBindByName = oracleDbCommandType.GetProperty("BindByName", ReflectionHelper.NoRestrictions);
            ReflectionCache.PropArrayBindCount = oracleDbCommandType.GetProperty("ArrayBindCount", ReflectionHelper.NoRestrictions);
        }

        protected virtual void CacheParameterProperties(IDbCommand command) {
            var parameter = command.CreateParameter();
            ReflectionCache.PropParameterDbType = parameter.GetType().GetProperty("OracleDbType", ReflectionHelper.NoRestrictions);
        }

        protected virtual void CacheOracleDbTypeEnumValues(IDbCommand command) {
            var assembly = command.GetType().Assembly;
            var typeEnum = assembly.GetType(OracleDbTypeEnumName);
            ReflectionCache.DbTypeRefCursor = Enum.Parse(typeEnum, "RefCursor");
            ReflectionCache.DbTypeBlob = Enum.Parse(typeEnum, "Blob");
        }

        protected virtual void ConfigForBulkSql(IDbCommand command, object[] parameters) {
            var param = parameters[0];
            if (param is In) {
                param = ((In)param).Value;
            }
            var collParam = param as ICollection;
            if (collParam == null || collParam.Count == 0) return;
            ReflectionCache.PropArrayBindCount.SetValue(command, collParam.Count, null);
        }

        public override IDbDataParameter GetParameter(In parIn, bool isBulk) {
            var par = GetParameter();
            if (parIn == null) {
                return par;
            }
            return SetDataParameterType(parIn, isBulk, par);
        }

        private IDbDataParameter SetDataParameterType(In parIn, bool isBulk, IDbDataParameter par) {
            var value = parIn.Value;
            if (isBulk) {
                var collParam = value as ICollection;
                if (collParam != null && collParam.Count != 0) {
                    var enumerator = collParam.GetEnumerator();
                    while (enumerator.MoveNext()) {
                        if ((value = enumerator.Current) != null) {
                            break;
                        }
                    }
                    if (value == null) {
                        value = "";
                    } 
                }
            }
            par.DbType = GenericDbTypeMap.GetDbType(value.GetType());
            if (par.DbType == DbType.Binary) {
                ReflectionCache.PropParameterDbType.SetValue(par, ReflectionCache.DbTypeBlob, null);                
            }
            return par;
        }
        
        public override IDbDataParameter GetParameterCursor() {
            IDbDataParameter parameter = DbProviderFactory.CreateParameter();
            ReflectionCache.PropParameterDbType.SetValue(parameter, ReflectionCache.DbTypeRefCursor, null);
            return parameter;
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
