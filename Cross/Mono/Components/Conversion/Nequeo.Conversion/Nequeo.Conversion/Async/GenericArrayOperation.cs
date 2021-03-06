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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Nequeo.Extension;

namespace Nequeo.Conversion.Async
{
    /// <summary>
    /// Asynchronous generic array operations
    /// </summary>
    /// <typeparam name="T">The type to examine.</typeparam>
    public class GenericArrayOperation<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GenericArrayOperation() { }

        private T[] _genericAsyncResult = null;

        /// <summary>
        /// Gets the async result after completion.
        /// </summary>
        public T[] TypeAsyncResult
        {
            get { return _genericAsyncResult; }
        }

        /// <summary>
        /// The async execute completion event.
        /// </summary>
        public event EventHandler AsyncComplete;

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        public async void Combine(T[] source, T[] arrayOne)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");

            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne);
            _genericAsyncResult = await data;

            if (AsyncComplete != null)
                AsyncComplete(this, new EventArgs());
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <returns>The task.</returns>
        private async Task<T[]> CombineInternalAsync(T[] source, T[] arrayOne)
        {
            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne);
            return await data;
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <returns>The combine array result.</returns>
        public T[] CombineAsync(T[] source, T[] arrayOne)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");

            var result = System.Threading.Tasks.Task.Run(() => CombineInternalAsync(source, arrayOne));
            return result.Result;
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        public async void Combine(T[] source, T[] arrayOne, T[] arrayTwo)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");
            if (arrayTwo == null) throw new System.ArgumentNullException("arrayTwo");

            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne, arrayTwo);
            _genericAsyncResult = await data;

            if (AsyncComplete != null)
                AsyncComplete(this, new EventArgs());
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <returns>The task.</returns>
        private async Task<T[]> CombineInternalAsync(T[] source, T[] arrayOne, T[] arrayTwo)
        {
            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne, arrayTwo);
            return await data;
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <returns>The combine array result.</returns>
        public T[] CombineAsync(T[] source, T[] arrayOne, T[] arrayTwo)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");
            if (arrayTwo == null) throw new System.ArgumentNullException("arrayTwo");

            var result = System.Threading.Tasks.Task.Run(() => CombineInternalAsync(source, arrayOne, arrayTwo));
            return result.Result;
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <param name="arrayThree">The third combining array.</param>
        public async void Combine(T[] source, T[] arrayOne, T[] arrayTwo, T[] arrayThree)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");
            if (arrayTwo == null) throw new System.ArgumentNullException("arrayTwo");
            if (arrayThree == null) throw new System.ArgumentNullException("arrayThree");

            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne, arrayTwo, arrayThree);
            _genericAsyncResult = await data;

            if (AsyncComplete != null)
                AsyncComplete(this, new EventArgs());
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <param name="arrayThree">The third combining array.</param>
        /// <returns>The task.</returns>
        private async Task<T[]> CombineInternalAsync(T[] source, T[] arrayOne, T[] arrayTwo, T[] arrayThree)
        {
            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne, arrayTwo, arrayThree);
            return await data;
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <param name="arrayThree">The third combining array.</param>
        /// <returns>The combine array result.</returns>
        public T[] CombineAsync(T[] source, T[] arrayOne, T[] arrayTwo, T[] arrayThree)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");
            if (arrayTwo == null) throw new System.ArgumentNullException("arrayTwo");
            if (arrayThree == null) throw new System.ArgumentNullException("arrayThree");

            var result = System.Threading.Tasks.Task.Run(() => CombineInternalAsync(source, arrayOne, arrayTwo, arrayThree));
            return result.Result;
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <param name="arrayThree">The third combining array.</param>
        /// <param name="arrayFour">The fourth combining array.</param>
        public async void Combine(T[] source, T[] arrayOne, T[] arrayTwo, T[] arrayThree, T[] arrayFour)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");
            if (arrayTwo == null) throw new System.ArgumentNullException("arrayTwo");
            if (arrayThree == null) throw new System.ArgumentNullException("arrayThree");
            if (arrayFour == null) throw new System.ArgumentNullException("arrayFour");

            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne, arrayTwo, arrayThree, arrayFour);
            _genericAsyncResult = await data;

            if (AsyncComplete != null)
                AsyncComplete(this, new EventArgs());
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <param name="arrayThree">The third combining array.</param>
        /// <param name="arrayFour">The fourth combining array.</param>
        /// <returns>The task.</returns>
        private async Task<T[]> CombineInternalAsync(T[] source, T[] arrayOne, T[] arrayTwo, T[] arrayThree, T[] arrayFour)
        {
            System.Threading.Tasks.Task<T[]> data = source.CombineParallelAsync<T>(arrayOne, arrayTwo, arrayThree, arrayFour);
            return await data;
        }

        /// <summary>
        /// Asynchronous array combining operation; appends to the end of the source array.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <param name="arrayOne">The first combining array.</param>
        /// <param name="arrayTwo">The second combining array.</param>
        /// <param name="arrayFour">The fourth combining array.</param>
        /// <param name="arrayThree">The third combining array.</param>
        /// <returns>The combine array result.</returns>
        public T[] CombineAsync(T[] source, T[] arrayOne, T[] arrayTwo, T[] arrayThree, T[] arrayFour)
        {
            // If the source object is null.
            if (source == null) throw new System.ArgumentNullException("source");
            if (arrayOne == null) throw new System.ArgumentNullException("arrayOne");
            if (arrayTwo == null) throw new System.ArgumentNullException("arrayTwo");
            if (arrayThree == null) throw new System.ArgumentNullException("arrayThree");
            if (arrayFour == null) throw new System.ArgumentNullException("arrayFour");

            var result = System.Threading.Tasks.Task.Run(() => CombineInternalAsync(source, arrayOne, arrayTwo, arrayThree, arrayFour));
            return result.Result;
        }
    }
}
