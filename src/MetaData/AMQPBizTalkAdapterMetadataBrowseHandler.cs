/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterMetadataBrowseHandler.cs
/// Description :  This class is used while performing a connection-based browse for metadata from the target system.
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ServiceModel.Channels;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterMetadataBrowseHandler : AMQPBizTalkAdapterHandlerBase, IMetadataBrowseHandler
    {
        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterMetadataBrowseHandler class
        /// </summary>
        public AMQPBizTalkAdapterMetadataBrowseHandler(AMQPBizTalkAdapterConnection connection
            , MetadataLookup metadataLookup)
            : base(connection, metadataLookup)
        {
        }

        #region IMetadataBrowseHandler Members

        /// <summary>
        /// Retrieves an array of MetadataRetrievalNodes from the target system.
        /// The browse will return nodes starting from the childStartIndex in the path provided in absoluteName, and the number of nodes returned is limited by maxChildNodes.
        /// The method should complete within the specified timespan or throw a timeout exception.
        /// If absoluteName is null or an empty string, return nodes starting from the root + childStartIndex.
        /// If childStartIndex is zero, then return starting at the node indicated by absoluteName (or the root node if absoluteName is null or empty).
        /// </summary>
        public MetadataRetrievalNode[] Browse(string nodeId
            , int childStartIndex
            , int maxChildNodes, TimeSpan timeout)
        {
            //
            //TODO: Implement the metadata browse on the target system.
            //
            if (MetadataRetrievalNode.Root.NodeId.Equals(nodeId))
            {
                MetadataRetrievalNode node = new MetadataRetrievalNode(MetaDataHelper.rootNodeId);
                node.DisplayName = MetaDataHelper.rootNodeId;
                node.IsOperation = false;
                node.Description = "This category contains inbound and outbound Operations.";
                node.Direction = MetadataRetrievalNodeDirections.Inbound | MetadataRetrievalNodeDirections.Outbound;
                return new MetadataRetrievalNode[] { node };
            }
            if (MetaDataHelper.rootNodeId.CompareTo(nodeId) == 0)
            {
                return MetaDataHelper.GetMetadataRetrievalNodeList();
            }
            return null;
        }

        #endregion IMetadataBrowseHandler Members
    }
}
