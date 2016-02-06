//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nequeo.DataAccess.CloudInteraction.Data.Extension
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
    /// The data access context model object class.
    /// </summary>
    public partial class Extension : Disposable, IDisposable
    {
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Extension()
        {
        }
        
        #region Public Context Extension Properties
        /// <summary>
        /// Gets, the user model property for the object.
        /// </summary>
        public Data.Extension.User User
        {
            get
            {
                return new Data.Extension.User();
            }
        }
        
        /// <summary>
        /// Gets, the role model property for the object.
        /// </summary>
        public Data.Extension.Role Role
        {
            get
            {
                return new Data.Extension.Role();
            }
        }
        
        /// <summary>
        /// Gets, the oauthconsumer model property for the object.
        /// </summary>
        public Data.Extension.OAuthConsumer OAuthConsumer
        {
            get
            {
                return new Data.Extension.OAuthConsumer();
            }
        }
        
        /// <summary>
        /// Gets, the clientauthorization model property for the object.
        /// </summary>
        public Data.Extension.ClientAuthorization ClientAuthorization
        {
            get
            {
                return new Data.Extension.ClientAuthorization();
            }
        }
        
        /// <summary>
        /// Gets, the oauthtoken model property for the object.
        /// </summary>
        public Data.Extension.OAuthToken OAuthToken
        {
            get
            {
                return new Data.Extension.OAuthToken();
            }
        }
        
        /// <summary>
        /// Gets, the profile model property for the object.
        /// </summary>
        public Data.Extension.Profile Profile
        {
            get
            {
                return new Data.Extension.Profile();
            }
        }
        
        /// <summary>
        /// Gets, the profilevalue model property for the object.
        /// </summary>
        public Data.Extension.ProfileValue ProfileValue
        {
            get
            {
                return new Data.Extension.ProfileValue();
            }
        }
        
        /// <summary>
        /// Gets, the contacttype model property for the object.
        /// </summary>
        public Data.Extension.ContactType ContactType
        {
            get
            {
                return new Data.Extension.ContactType();
            }
        }
        
        /// <summary>
        /// Gets, the contact model property for the object.
        /// </summary>
        public Data.Extension.Contact Contact
        {
            get
            {
                return new Data.Extension.Contact();
            }
        }
        
        /// <summary>
        /// Gets, the client model property for the object.
        /// </summary>
        public Data.Extension.Client Client
        {
            get
            {
                return new Data.Extension.Client();
            }
        }
        
        /// <summary>
        /// Gets, the symmetriccryptokey model property for the object.
        /// </summary>
        public Data.Extension.SymmetricCryptoKey SymmetricCryptoKey
        {
            get
            {
                return new Data.Extension.SymmetricCryptoKey();
            }
        }
        
        /// <summary>
        /// Gets, the nonce model property for the object.
        /// </summary>
        public Data.Extension.Nonce Nonce
        {
            get
            {
                return new Data.Extension.Nonce();
            }
        }
        #endregion
    }
}
namespace Nequeo.DataAccess.CloudInteraction.Data
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
    /// The data access context object class.
    /// </summary>
    public partial class DataContext
    {
        
        /// <summary>
        /// Gets, the extension class.
        /// </summary>
        public Data.Extension.Extension Extension
        {
            get
            {
                return new Data.Extension.Extension();
            }
        }
    }
}