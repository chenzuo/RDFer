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

#if !NO_WEB && !NO_ASP

using System;
using System.Configuration;
using System.Web;
using VDS.RDF.Configuration;
using VDS.RDF.Web.Configuration;
using VDS.RDF.Web.Configuration.Resource;

namespace VDS.RDF.Web
{
    /// <summary>
    /// HTTP Handler for serving Graphs in ASP.Net applications
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used to serve a Graph at a base URL with any URL under that being handled by this Handler.  The Graph is served to the user in one of their acceptable MIME types if possible, if they don't accept any MIME type we can serve then they get a 406 Not Acceptable
    /// </para>
    /// <para>
    /// This Handler is configured using the new Configuration API introduced in the 0.3.0 release.  This requires just one setting to be defined in the &lt;appSettings&gt; section of your Web.config file which points to a Configuration Graph like so:
    /// <code>&lt;add key="dotNetRDFConfig" value="~/App_Data/config.ttl" /&gt;</code>
    /// The Configuration Graph must then contain Triples like the following to specify a Graph to be served:
    /// <code>
    /// &lt;dotnetrdf:/folder/graph/*&gt; a dnr:HttpHandler ;
    ///                                   dnr:type "VDS.RDF.Web.WildcardGraphHandler" ;
    ///                                   dnr:usingGraph _:graph .
    ///                                 
    /// _:graph a dnr:Graph ;
    ///         dnr:type "VDS.RDF.Graph" ;
    ///         dnr:fromFile "yourGraph.rdf" .
    /// </code>
    /// </para>
    /// </remarks>
    public class WildcardGraphHandler : GraphHandler
    {
        private String _cachePath;

        /// <summary>
        /// Loads the Handler Configuration
        /// </summary>
        /// <param name="context">HTTP Context</param>
        /// <returns></returns>
        protected override BaseGraphHandlerConfiguration LoadConfig(HttpContext context)
        {
            //Check the Configuration File is specified
            String configFile = context.Server.MapPath(ConfigurationManager.AppSettings["dotNetRDFConfig"]);
            if (configFile == null) throw new DotNetRdfConfigurationException("Unable to load Graph Handler Configuration as the Web.Config file does not specify a 'dotNetRDFConfig' AppSetting to specify the RDF configuration file to use");
            IGraph g = WebConfigurationLoader.LoadConfigurationGraph(context, configFile);

            //Then check there is configuration associated with the expected URI
            INode objNode = WebConfigurationLoader.FindObject(g, context.Request.Url, out this._cachePath);
            if (objNode == null) throw new DotNetRdfConfigurationException("Unable to load Graph Handler Configuration as the RDF configuration file does not have any configuration associated with an appropriate wildcard URI");

            //Is our Configuration already cached?
            Object temp = context.Cache[this._cachePath];
            if (temp != null)
            {
                if (temp is BaseGraphHandlerConfiguration)
                {
                    return (BaseGraphHandlerConfiguration)temp;
                }
                else
                {
                    context.Cache.Remove(this._cachePath);
                }
            }

            GraphHandlerConfiguration config = new GraphHandlerConfiguration(context, g, objNode);

            //Finally cache the Configuration before returning it
            if (config.CacheSliding)
            {
                context.Cache.Add(this._cachePath, config, new System.Web.Caching.CacheDependency(configFile), System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, config.CacheDuration, 0), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                context.Cache.Add(this._cachePath, config, new System.Web.Caching.CacheDependency(configFile), DateTime.Now.AddMinutes(config.CacheDuration), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return config;
        }

        /// <summary>
        /// Updates the Handlers configuration
        /// </summary>
        /// <param name="context">HTTP Context</param>
        protected override void UpdateConfig(HttpContext context)
        {
            if (this._config.CacheDuration == 0)
            {
                if (context.Cache[this._cachePath] != null) context.Cache.Remove(this._cachePath);
            }
        }
    }
}

#endif