// Warning 169 (Disables the 'Never used' warning)
#pragma warning disable 169
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nequeo.DataAccess.ApplicationLogin.Data.Extension
{
    using System;
    using System.Text;
    using System.Data;
    using System.Threading;
    using System.Diagnostics;
    using System.Data.SqlClient;
    using System.Data.OleDb;
    using System.Data.Odbc;
    using System.Collections;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using System.Runtime.Serialization;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data.Linq;
    using System.Data.Linq.SqlClient;
    using System.Data.Linq.Mapping;
    using Nequeo.Data.DataType;
    using Nequeo.Data;
    using Nequeo.Data.Linq;
    using Nequeo.Data.Control;
    using Nequeo.Data.Custom;
    using Nequeo.Data.LinqToSql;
    using Nequeo.Data.DataSet;
    using Nequeo.Data.Edm;
    using Nequeo.ComponentModel;
    
    
    /// <summary>
    /// The user object class.
    /// </summary>
    public partial class User
    {
        
        #region Public Function Methods
        /// <summary>
        /// Execute the getcolumnvalues routine.
        /// </summary>
        /// <param name="columnIndicatorName">Initial value of ColumnIndicatorName.</param>
        /// <param name="columnName">Initial value of ColumnName.</param>
        /// <param name="dataBase">Initial value of DataBase.</param>
        /// <param name="tableName">Initial value of TableName.</param>
        /// <returns>The execution result.</returns>
        [FunctionRoutineAttribute("dbo.GetColumnValues", FunctionRoutineType.StoredProcedure)]
        public virtual object GetColumnValues([FunctionParameterAttribute("@ColumnIndicatorName", "varchar", -1, ParameterDirection.Input, true)] string columnIndicatorName, [FunctionParameterAttribute("@ColumnName", "varchar", -1, ParameterDirection.Input, true)] string columnName, [FunctionParameterAttribute("@DataBase", "varchar", -1, ParameterDirection.Input, true)] string dataBase, [FunctionParameterAttribute("@TableName", "varchar", -1, ParameterDirection.Input, true)] string tableName)
        {
            IExecuteFunctionResult result = Common.ExecuteFunction(Common, ((MethodInfo)(MethodInfo.GetCurrentMethod())), columnIndicatorName, columnName, dataBase, tableName);
            return ((object)(result.ReturnValue));
        }
        
        /// <summary>
        /// Execute the getuserscreenaccess routine.
        /// </summary>
        /// <param name="applicationID">Initial value of ApplicationID.</param>
        /// <param name="userID">Initial value of UserID.</param>
        /// <returns>The execution result.</returns>
        [FunctionRoutineAttribute("dbo.GetUserScreenAccess", FunctionRoutineType.StoredProcedure)]
        public virtual List<Data.Extended.GetUserScreenAccessResult> GetUserScreenAccess([FunctionParameterAttribute("@ApplicationID", "bigint", 8, ParameterDirection.Input, true)] System.Nullable<System.Int64> applicationID, [FunctionParameterAttribute("@UserID", "bigint", 8, ParameterDirection.Input, true)] System.Nullable<System.Int64> userID)
        {
            IExecuteFunctionResult result = Common.ExecuteFunction(Common, ((MethodInfo)(MethodInfo.GetCurrentMethod())), applicationID, userID);
            return ((List<Data.Extended.GetUserScreenAccessResult>)(result.ReturnValue));
        }
        
        /// <summary>
        /// Execute the inserterrorlog routine.
        /// </summary>
        /// <param name="applicationIdentifier">Initial value of ApplicationIdentifier.</param>
        /// <param name="errorCode">Initial value of ErrorCode.</param>
        /// <param name="errorDescription">Initial value of ErrorDescription.</param>
        /// <param name="processName">Initial value of ProcessName.</param>
        /// <returns>The execution result.</returns>
        [FunctionRoutineAttribute("dbo.InsertErrorLog", FunctionRoutineType.StoredProcedure)]
        public virtual object InsertErrorLog([FunctionParameterAttribute("@ApplicationIdentifier", "bigint", 8, ParameterDirection.Input, true)] System.Nullable<System.Int64> applicationIdentifier, [FunctionParameterAttribute("@ErrorCode", "varchar", 50, ParameterDirection.Input, true)] string errorCode, [FunctionParameterAttribute("@ErrorDescription", "varchar", -1, ParameterDirection.Input, true)] string errorDescription, [FunctionParameterAttribute("@ProcessName", "varchar", 200, ParameterDirection.Input, true)] string processName)
        {
            IExecuteFunctionResult result = Common.ExecuteFunction(Common, ((MethodInfo)(MethodInfo.GetCurrentMethod())), applicationIdentifier, errorCode, errorDescription, processName);
            return ((object)(result.ReturnValue));
        }
        
        /// <summary>
        /// Execute the inserttabletype_byte routine.
        /// </summary>
        /// <param name="image">Initial value of Image.</param>
        /// <returns>The execution result.</returns>
        [FunctionRoutineAttribute("dbo.InsertTableType_Byte", FunctionRoutineType.StoredProcedure)]
        public virtual object InsertTableType_Byte([FunctionParameterAttribute("@Image", "image", -1, ParameterDirection.Input, true)] byte[] image)
        {
            IExecuteFunctionResult result = Common.ExecuteFunction(Common, ((MethodInfo)(MethodInfo.GetCurrentMethod())), image);
            return ((object)(result.ReturnValue));
        }
        
        /// <summary>
        /// Execute the inserttabletype_ntext routine.
        /// </summary>
        /// <param name="nTextValue">Initial value of NTextValue.</param>
        /// <returns>The execution result.</returns>
        [FunctionRoutineAttribute("dbo.InsertTableType_NText", FunctionRoutineType.StoredProcedure)]
        public virtual object InsertTableType_NText([FunctionParameterAttribute("@NTextValue", "ntext", -1, ParameterDirection.Input, true)] string nTextValue)
        {
            IExecuteFunctionResult result = Common.ExecuteFunction(Common, ((MethodInfo)(MethodInfo.GetCurrentMethod())), nTextValue);
            return ((object)(result.ReturnValue));
        }
        #endregion
    }
}
namespace Nequeo.DataAccess.ApplicationLogin.Data.Extended
{
    using System;
    using System.Text;
    using System.Data;
    using System.Threading;
    using System.Diagnostics;
    using System.Data.SqlClient;
    using System.Data.OleDb;
    using System.Data.Odbc;
    using System.Collections;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using System.Runtime.Serialization;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data.Linq;
    using System.Data.Linq.SqlClient;
    using System.Data.Linq.Mapping;
    using Nequeo.Data.DataType;
    using Nequeo.Data;
    using Nequeo.Data.Linq;
    using Nequeo.Data.Control;
    using Nequeo.Data.Custom;
    using Nequeo.Data.LinqToSql;
    using Nequeo.Data.DataSet;
    using Nequeo.Data.Edm;
    using Nequeo.ComponentModel;
    
    
    #region GetUserScreenAccessResult Data Entity Type
    /// <summary>
    /// The getuserscreenaccessresult data object class.
    /// </summary>
    [DataContractAttribute(Name = "GetUserScreenAccessResult", IsReference = true)]
    [SerializableAttribute()]
    [KnownTypeAttribute(typeof(DataBase))]
    public partial class GetUserScreenAccessResult : DataBase
    {
        
        private bool _Suspended;
        
        private System.Nullable<System.Int64> _ScreenCode;
        
        private System.Nullable<System.Int64> _AccessCode;
        
        #region Extensibility Method Definitions
        /// <summary>
        /// On create data entity.
        /// </summary>
		partial void OnCreated();

        /// <summary>
        /// On load data entity.
        /// </summary>
		partial void OnLoaded();

		#endregion
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GetUserScreenAccessResult()
        {
            OnCreated();
        }
        
        #region Public Column Properties
        /// <summary>
        /// Gets sets, the suspended property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the suspended property for the object.")]
        [DataMemberAttribute(Name = "Suspended")]
        [XmlElementAttribute(ElementName = "Suspended", IsNullable = false)]
        [SoapElementAttribute(IsNullable = false)]
        public bool Suspended
        {
            get
            {
                return this._Suspended;
            }
            set
            {
                if ((this._Suspended != value))
                {
                    this._Suspended = value;
                }
            }
        }
        
        /// <summary>
        /// Gets sets, the screencode property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the screencode property for the object.")]
        [DataMemberAttribute(Name = "ScreenCode")]
        [XmlElementAttribute(ElementName = "ScreenCode", IsNullable = true)]
        [SoapElementAttribute(IsNullable = true)]
        public System.Nullable<System.Int64> ScreenCode
        {
            get
            {
                return this._ScreenCode;
            }
            set
            {
                if ((this._ScreenCode != value))
                {
                    this._ScreenCode = value;
                }
            }
        }
        
        /// <summary>
        /// Gets sets, the accesscode property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the accesscode property for the object.")]
        [DataMemberAttribute(Name = "AccessCode")]
        [XmlElementAttribute(ElementName = "AccessCode", IsNullable = true)]
        [SoapElementAttribute(IsNullable = true)]
        public System.Nullable<System.Int64> AccessCode
        {
            get
            {
                return this._AccessCode;
            }
            set
            {
                if ((this._AccessCode != value))
                {
                    this._AccessCode = value;
                }
            }
        }
        #endregion
        
        /// <summary>
        /// Create a new getuserscreenaccessresult data entity.
        /// </summary>
        /// <param name="suspended">Initial value of Suspended.</param>
        /// <returns>The Data.Extended.GetUserScreenAccessResult entity.</returns>
        public static Data.Extended.GetUserScreenAccessResult CreateGetUserScreenAccessResult(bool suspended)
        {
            Data.Extended.GetUserScreenAccessResult getUserScreenAccessResult = new Data.Extended.GetUserScreenAccessResult();
            getUserScreenAccessResult.Suspended = suspended;
            return getUserScreenAccessResult;
        }
    }
    #endregion
}

#pragma warning restore 169
