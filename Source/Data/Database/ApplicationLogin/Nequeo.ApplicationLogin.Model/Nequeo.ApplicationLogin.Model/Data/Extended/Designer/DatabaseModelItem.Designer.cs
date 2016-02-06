//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nequeo.DataAccess.ApplicationLogin.Data.Extended
{
    using System;
    using System.Linq;
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
    using Nequeo.Data.DataType;
    using Nequeo.Data;
    using Nequeo.Data.Linq;
    using Nequeo.Data.Control;
    using Nequeo.Data.Custom;
    using Nequeo.Data.LinqToSql;
    using Nequeo.Data.DataSet;
    using Nequeo.Data.Edm;
    using Nequeo.ComponentModel;
    
    
    #region TableEntity Data Type
    /// <summary>
    /// The tableentity data object class.
    /// </summary>
    [DataContractAttribute(Name = "TableEntity", IsReference = true)]
    [SerializableAttribute()]
    [KnownTypeAttribute(typeof(DataBase))]
    public partial class TableEntity : DataBase
    {
        
        private string _TableName;
        
        private string _TableOwner;
        
        private Data.Extended.TableEntityDataColumn[] _TableEntityDataColumn;
        
        private Data.Extended.TableEntityForeignKey[] _TableEntityForeignKey;
        
        private Data.Extended.TableEntityReference[] _TableEntityReference;
        
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
        public TableEntity()
        {
            OnCreated();
        }
        
        /// <summary>
        /// Gets sets, the tablename property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the tablename property for the object.")]
        [DataMemberAttribute(Name = "TableName")]
        [XmlElementAttribute(ElementName = "TableName", IsNullable = false)]
        [SoapElementAttribute(IsNullable = false)]
        public string TableName
        {
            get
            {
                return this._TableName;
            }
            set
            {
                if ((this._TableName != value))
                {
                    this._TableName = value;
                }
            }
        }
        
        /// <summary>
        /// Gets sets, the tableowner property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the tableowner property for the object.")]
        [DataMemberAttribute(Name = "TableOwner")]
        [XmlElementAttribute(ElementName = "TableOwner", IsNullable = false)]
        [SoapElementAttribute(IsNullable = false)]
        public string TableOwner
        {
            get
            {
                return this._TableOwner;
            }
            set
            {
                if ((this._TableOwner != value))
                {
                    this._TableOwner = value;
                }
            }
        }
        
        /// <summary>
        /// Gets sets, the tableentitydatacolumn property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the tableentitydatacolumn property for the object.")]
        [DataMemberAttribute(Name = "TableEntityDataColumn")]
        [XmlArrayAttribute(ElementName = "TableEntityDataColumn", IsNullable = true)]
        [SoapElementAttribute(IsNullable = true)]
        public Data.Extended.TableEntityDataColumn[] TableEntityDataColumn
        {
            get
            {
                return this._TableEntityDataColumn;
            }
            set
            {
                if ((this._TableEntityDataColumn != value))
                {
                    this._TableEntityDataColumn = value;
                }
            }
        }
        
        /// <summary>
        /// Gets sets, the tableentityforeignkey property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the tableentityforeignkey property for the object.")]
        [DataMemberAttribute(Name = "TableEntityForeignKey")]
        [XmlArrayAttribute(ElementName = "TableEntityForeignKey", IsNullable = true)]
        [SoapElementAttribute(IsNullable = true)]
        public Data.Extended.TableEntityForeignKey[] TableEntityForeignKey
        {
            get
            {
                return this._TableEntityForeignKey;
            }
            set
            {
                if ((this._TableEntityForeignKey != value))
                {
                    this._TableEntityForeignKey = value;
                }
            }
        }
        
        /// <summary>
        /// Gets sets, the tableentityreference property for the object.
        /// </summary>
        [CategoryAttribute("Column")]
        [DescriptionAttribute("Gets sets, the tableentityreference property for the object.")]
        [DataMemberAttribute(Name = "TableEntityReference")]
        [XmlArrayAttribute(ElementName = "TableEntityReference", IsNullable = true)]
        [SoapElementAttribute(IsNullable = true)]
        public Data.Extended.TableEntityReference[] TableEntityReference
        {
            get
            {
                return this._TableEntityReference;
            }
            set
            {
                if ((this._TableEntityReference != value))
                {
                    this._TableEntityReference = value;
                }
            }
        }
        
        /// <summary>
        /// Create a new tableentity data entity.
        /// </summary>
        /// <param name="tableName">Initial value of TableName.</param>
        /// <param name="tableOwner">Initial value of TableOwner.</param>
        /// <returns>The Data.Extended.TableEntity entity.</returns>
        public static Data.Extended.TableEntity CreateTableEntity(string tableName, string tableOwner)
        {
            Data.Extended.TableEntity tableEntity = new Data.Extended.TableEntity();
            tableEntity.TableName = tableName;
            tableEntity.TableOwner = tableOwner;
            return tableEntity;
        }
    }
    #endregion
}