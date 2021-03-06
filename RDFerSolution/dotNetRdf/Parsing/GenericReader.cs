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
using VDS.RDF.Storage;

namespace VDS.RDF.Parsing
{
    /// <summary>
    /// Class for reading RDF Graphs from an arbitrary Store into arbitrary Graphs
    /// </summary>
    [Obsolete("The GenericReader is considered obsolete - use the LoadGraph() method of an IGenericIOManager directly instead", true)]
    public class GenericReader
    {
        private IGenericIOManager _manager;

        /// <summary>
        /// Creates a new instance of the Generic Reader which connects to an arbitrary Store using the given Generic Manager
        /// </summary>
        /// <param name="manager">Manager for the underlying Store</param>
        public GenericReader(IGenericIOManager manager)
        {
            this._manager = manager;
        }

        /// <summary>
        /// Loads a Graph from the Store
        /// </summary>
        /// <param name="g">Graph to load into</param>
        /// <param name="graphUri">Uri of the Graph to load</param>
        public void Load(IGraph g, Uri graphUri)
        {
            this._manager.LoadGraph(g, graphUri);
        }

        /// <summary>
        /// Loads a Graph from the Store
        /// </summary>
        /// <param name="g">Graph to load into</param>
        /// <param name="graphUri">Uri of the Graph to load</param>
        public void Load(IGraph g, String graphUri)
        {
            this._manager.LoadGraph(g, graphUri);
        }
    }
}
