﻿/*

Copyright Robert Vesse 2009-10
rvesse@vdesign-studios.com

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.RDF.Query
{
    /// <summary>
    /// Selector which Selects All Objects
    /// </summary>
    /// <typeparam name="T">Type you are performing Selection upon</typeparam>
    [Obsolete("ISelector interface is considered obsolete and will be removed in the 0.7.0 release", false)]
    public class AllSelector<T> : ISelector<T>
    {
        /// <summary>
        /// Accepts All Objects
        /// </summary>
        /// <param name="obj">Object to Test</param>
        /// <returns></returns>
        public bool Accepts(T obj)
        {
            return true;
        }

    }

    /// <summary>
    /// Selector which Selects No Objects
    /// </summary>
    /// <typeparam name="T">Type you are performing Selection upon</typeparam>
    [Obsolete("ISelector interface is considered obsolete and will be removed in the 0.7.0 release", false)]
    public class NoneSelector<T> : ISelector<T>
    {

        /// <summary>
        /// Accepts No Objects
        /// </summary>
        /// <param name="obj">Object to Test</param>
        /// <returns></returns>
        public bool Accepts(T obj)
        {
            return false;
        }

    }

    #region Unary Boolean Operators

    /// <summary>
    /// Selector which performs Not on the underlying Selectors results
    /// </summary>
    /// <typeparam name="T">Type you are performing selection upon</typeparam>
    [Obsolete("ISelector interface is considered obsolete and will be removed in the 0.7.0 release", false)]
    public class NotSelector<T> : ISelector<T>
    {
        private ISelector<T> _selector;

        /// <summary>
        /// Creates a new Not Selector using the given Selector
        /// </summary>
        /// <param name="selector">The Selector whose results you wish to Not</param>
        [Obsolete("ISelector interface is considered obsolete and will be removed in the 0.7.0 release", false)]
        public NotSelector(ISelector<T> selector) {
            this._selector = selector;
        }

        /// <summary>
        /// Accepts any Object which are rejected by the Underlying Selector
        /// </summary>
        /// <param name="obj">Object to test</param>
        /// <returns></returns>
        public bool Accepts(T obj)
        {
            return (!this._selector.Accepts(obj));
        }
    }

    #endregion
}