/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterMetadataSearchHandler.cs
/// Description :  This class is used for performing a connection-based search for metadata from the target system
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ServiceModel.Channels;
using System.Linq;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterMetadataSearchHandler : AMQPBizTalkAdapterHandlerBase, IMetadataSearchHandler
    {
        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterMetadataSearchHandler class
        /// </summary>
        public AMQPBizTalkAdapterMetadataSearchHandler(AMQPBizTalkAdapterConnection connection
            , MetadataLookup metadataLookup)
            : base(connection, metadataLookup)
        {
        }

        #region IMetadataSearchHandler Members

        /// <summary>
        /// Retrieves an array of MetadataRetrievalNodes (see Microsoft.ServiceModel.Channels) from the target system.
        /// The search will begin at the path provided in absoluteName, which points to a location in the tree of metadata nodes.
        /// The contents of the array are filtered by SearchCriteria and the number of nodes returned is limited by maxChildNodes.
        /// The method should complete within the specified timespan or throw a timeout exception.  If absoluteName is null or an empty string, return nodes starting from the root.
        /// If SearchCriteria is null or an empty string, return all nodes.
        /// </summary>
        public MetadataRetrievalNode[] Search(string nodeId
            , string searchCriteria
            , int maxChildNodes, TimeSpan timeout)
        {
            //
            //TODO: Search for metadata on the target system.
            //
            if (searchCriteria == null || string.IsNullOrEmpty(searchCriteria.Trim()))
                return MetaDataHelper.GetMetadataRetrievalNodeList();

            var list = from node in MetaDataHelper.GetMetadataRetrievalNodeList()
                       where node.DisplayName.ToLowerInvariant().Contains(searchCriteria.ToLowerInvariant())
                       select node;

            return list.ToArray();
        }

        #endregion IMetadataSearchHandler Members
    }
}
