﻿/*  Company :       Nequeo Pty Ltd, http://www.nequeo.com.au/
 *  Copyright :     Copyright © Nequeo Pty Ltd 2010 http://www.nequeo.com.au/
 * 
 *  File :          
 *  Purpose :       
 * 
 */

#region Nequeo Pty Ltd License
/*
    Permission is hereby granted, free of charge, to any person
    obtaining a copy of this software and associated documentation
    files (the "Software"), to deal in the Software without
    restriction, including without limitation the rights to use,
    copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the
    Software is furnished to do so, subject to the following
    conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
    HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Data;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;

namespace Nequeo.Data.Edm.Async
{
    /// <summary>
    /// Asynchronouns generic data table access layer.
    /// </summary>
    /// <typeparam name="TDataContext">The linq connection context type.</typeparam>
    /// <typeparam name="TLinqEntity">The linq entity type.</typeparam>
    public sealed class AsyncSelectIQueryableEdmItems<TDataContext, TLinqEntity> : Nequeo.Threading.AsynchronousResult<IQueryable<TLinqEntity>>
        where TDataContext : System.Data.Entity.DbContext, new()
        where TLinqEntity : class, new()
    {
        #region Asynchronous Operations
        /// <summary>
        /// Start the asynchronous query request operation.
        /// </summary>
        /// <param name="linqIQueryable">The base generic data access layer.</param>
        /// <param name="callback">The asynchronous call back method.</param>
        /// <param name="state">The state object value.</param>
        /// <param name="queryString">The primary key value.</param>
        /// <param name="values">The values associated with the predicate string.</param>
        public AsyncSelectIQueryableEdmItems(ISelectEdmGenericBase<TDataContext, TLinqEntity> linqIQueryable,
            AsyncCallback callback, object state, string queryString, params System.Data.Objects.ObjectParameter[] values)
            : base(callback, state)
        {
            _values = values;
            _queryString = queryString;
            _linqIQueryable = linqIQueryable;

            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncSelectThread1));
            Thread.Sleep(20);
        }

        /// <summary>
        /// Start the asynchronous query request operation.
        /// </summary>
        /// <param name="linqIQueryable">The base generic data access layer.</param>
        /// <param name="callback">The asynchronous call back method.</param>
        /// <param name="state">The state object value.</param>
        public AsyncSelectIQueryableEdmItems(ISelectEdmGenericBase<TDataContext, TLinqEntity> linqIQueryable,
            AsyncCallback callback, object state)
            : base(callback, state)
        {
            _linqIQueryable = linqIQueryable;

            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncSelectThread2));
            Thread.Sleep(20);
        }

        /// <summary>
        /// Start the asynchronous query request operation.
        /// </summary>
        /// <param name="linqIQueryable">The base generic data access layer.</param>
        /// <param name="callback">The asynchronous call back method.</param>
        /// <param name="state">The state object value.</param>
        /// <param name="predicate">The predicate expression.</param>
        public AsyncSelectIQueryableEdmItems(ISelectEdmGenericBase<TDataContext, TLinqEntity> linqIQueryable,
            AsyncCallback callback, object state, Expression<Func<TLinqEntity, bool>> predicate)
            : base(callback, state)
        {
            _predicate = predicate;
            _linqIQueryable = linqIQueryable;

            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncSelectThread3));
            Thread.Sleep(20);
        }

        private Expression<Func<TLinqEntity, bool>> _predicate = null;
        private System.Data.Objects.ObjectParameter[] _values = null;
        private string _queryString = null;
        private ISelectEdmGenericBase<TDataContext, TLinqEntity> _linqIQueryable = null;

        /// <summary>
        /// The async query request method.
        /// </summary>
        /// <param name="stateInfo">The thread object state.</param>
        private void AsyncSelectThread1(Object stateInfo)
        {
            // Get the query result.
            IQueryable<TLinqEntity> data = _linqIQueryable.SelectIQueryableItems(_queryString, _values);

            // If data exits then indicate that the asynchronous
            // operation has completed and send the result to the
            // client, else indicate that the asynchronous operation
            // has failed and did not complete.
            if (data != null)
                base.Complete(data, true);
            else
                base.Complete(false);
        }

        /// <summary>
        /// The async query request method.
        /// </summary>
        /// <param name="stateInfo">The thread object state.</param>
        private void AsyncSelectThread2(Object stateInfo)
        {
            // Get the query result.
            IQueryable<TLinqEntity> data = _linqIQueryable.SelectIQueryableItems();

            // If data exits then indicate that the asynchronous
            // operation has completed and send the result to the
            // client, else indicate that the asynchronous operation
            // has failed and did not complete.
            if (data != null)
                base.Complete(data, true);
            else
                base.Complete(false);
        }

        /// <summary>
        /// The async query request method.
        /// </summary>
        /// <param name="stateInfo">The thread object state.</param>
        private void AsyncSelectThread3(Object stateInfo)
        {
            // Get the query result.
            IQueryable<TLinqEntity> data = _linqIQueryable.SelectIQueryableItems(_predicate);

            // If data exits then indicate that the asynchronous
            // operation has completed and send the result to the
            // client, else indicate that the asynchronous operation
            // has failed and did not complete.
            if (data != null)
                base.Complete(data, true);
            else
                base.Complete(false);
        }
        #endregion
    }

    /// <summary>
    /// Asynchronouns generic data table access layer.
    /// </summary>
    /// <typeparam name="TDataContext">The linq connection context type.</typeparam>
    /// <typeparam name="TLinqEntity">The linq entity type.</typeparam>
    /// <typeparam name="TypeLinqEntity">The type to examine.</typeparam>
    public sealed class AsyncSelectIQueryableEdmItems<TDataContext, TLinqEntity, TypeLinqEntity> : Nequeo.Threading.AsynchronousResult<IQueryable<TypeLinqEntity>>
        where TDataContext : System.Data.Entity.DbContext, new()
        where TLinqEntity : class, new()
        where TypeLinqEntity : class, new()
    {
        #region Asynchronous Operations
        /// <summary>
        /// Start the asynchronous query request operation.
        /// </summary>
        /// <param name="linqIQueryable">The base generic data access layer.</param>
        /// <param name="callback">The asynchronous call back method.</param>
        /// <param name="state">The state object value.</param>
        /// <param name="queryString">The primary key value.</param>
        /// <param name="values">The values associated with the predicate string.</param>
        public AsyncSelectIQueryableEdmItems(ISelectEdmGenericBase<TDataContext, TLinqEntity> linqIQueryable,
            AsyncCallback callback, object state, string queryString, params System.Data.Objects.ObjectParameter[] values)
            : base(callback, state)
        {
            _values = values;
            _queryString = queryString;
            _linqIQueryable = linqIQueryable;

            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncSelectThread1));
            Thread.Sleep(20);
        }

        /// <summary>
        /// Start the asynchronous query request operation.
        /// </summary>
        /// <param name="linqIQueryable">The base generic data access layer.</param>
        /// <param name="callback">The asynchronous call back method.</param>
        /// <param name="state">The state object value.</param>
        public AsyncSelectIQueryableEdmItems(ISelectEdmGenericBase<TDataContext, TLinqEntity> linqIQueryable,
            AsyncCallback callback, object state)
            : base(callback, state)
        {
            _linqIQueryable = linqIQueryable;

            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncSelectThread2));
            Thread.Sleep(20);
        }
        /// <summary>
        /// Start the asynchronous query request operation.
        /// </summary>
        /// <param name="linqIQueryable">The base generic data access layer.</param>
        /// <param name="callback">The asynchronous call back method.</param>
        /// <param name="state">The state object value.</param>
        /// <param name="predicate">The predicate expression.</param>
        public AsyncSelectIQueryableEdmItems(ISelectEdmGenericBase<TDataContext, TLinqEntity> linqIQueryable,
            AsyncCallback callback, object state, Expression<Func<TypeLinqEntity, bool>> predicate)
            : base(callback, state)
        {
            _predicate = predicate;
            _linqIQueryable = linqIQueryable;

            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncSelectThread3));
            Thread.Sleep(20);
        }

        private Expression<Func<TypeLinqEntity, bool>> _predicate = null;
        private System.Data.Objects.ObjectParameter[] _values = null;
        private string _queryString = null;
        private ISelectEdmGenericBase<TDataContext, TLinqEntity> _linqIQueryable = null;

        /// <summary>
        /// The async query request method.
        /// </summary>
        /// <param name="stateInfo">The thread object state.</param>
        private void AsyncSelectThread1(Object stateInfo)
        {
            // Get the query result.
            IQueryable<TypeLinqEntity> data = _linqIQueryable.SelectIQueryableItems<TypeLinqEntity>();

            // If data exits then indicate that the asynchronous
            // operation has completed and send the result to the
            // client, else indicate that the asynchronous operation
            // has failed and did not complete.
            if (data != null)
                base.Complete(data, true);
            else
                base.Complete(false);
        }

        /// <summary>
        /// The async query request method.
        /// </summary>
        /// <param name="stateInfo">The thread object state.</param>
        private void AsyncSelectThread2(Object stateInfo)
        {
            // Get the query result.
            IQueryable<TypeLinqEntity> data = _linqIQueryable.SelectIQueryableItems<TypeLinqEntity>();

            // If data exits then indicate that the asynchronous
            // operation has completed and send the result to the
            // client, else indicate that the asynchronous operation
            // has failed and did not complete.
            if (data != null)
                base.Complete(data, true);
            else
                base.Complete(false);
        }

        /// <summary>
        /// The async query request method.
        /// </summary>
        /// <param name="stateInfo">The thread object state.</param>
        private void AsyncSelectThread3(Object stateInfo)
        {
            // Get the query result.
            IQueryable<TypeLinqEntity> data = _linqIQueryable.SelectIQueryableItems<TypeLinqEntity>(_predicate);

            // If data exits then indicate that the asynchronous
            // operation has completed and send the result to the
            // client, else indicate that the asynchronous operation
            // has failed and did not complete.
            if (data != null)
                base.Complete(data, true);
            else
                base.Complete(false);
        }
        #endregion
    }
}
