/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterMetadataResolverHandler.cs
/// Description :  This class is used for performing a connection-based retrieval of metadata from the target system.
/// ----------------------------------------------------------------------------------------------------------- 

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterMetadataResolverHandler : AMQPBizTalkAdapterHandlerBase, IMetadataResolverHandler
    {
        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterMetadataResolverHandler class
        /// </summary>
        public AMQPBizTalkAdapterMetadataResolverHandler(AMQPBizTalkAdapterConnection connection
            , MetadataLookup metadataLookup)
            : base(connection, metadataLookup)
        {
        }

        #region IMetadataResolver Members

        /// <summary>
        /// Returns a value indicating whether the specified operation metadata is valid
        /// The DateTime field is provided to indicate when this specific operation metadata was last retrieved from the source system.
        /// The method should complete within the specified timespan or throw a timeout exception.
        /// </summary>
        public bool IsOperationMetadataValid(string operationId, DateTime lastUpdatedTimestamp, TimeSpan timeout)
        {
            //
            //TODO: Check the validity of the operation metadata on the target system.
            //      Examples include identifying any relevant changes in the target system, expiration of validity time interval etc.
            //
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Returns a value indicating whether the specified type metadata is valid.
        /// The DateTime field is provided to indicate when this specific type metadata was last retrieved from the source system.
        /// The method should complete within the specified timespan or throw a timeout exception.
        /// </summary>
        public bool IsTypeMetadataValid(string typeId, DateTime lastUpdatedTimestamp, TimeSpan timeout)
        {
            //
            //TODO: Check the validity of the type metadata on the target system.
            //      Metadata validity might constitute things like changes, expiry and etc.

            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Returns an OperationMetadata object resolved from absolute name of the operation metadata object.
        /// The method should complete within the specified time span or throw a timeout exception.
        /// </summary>
        public OperationMetadata ResolveOperationMetadata(string operationId, TimeSpan timeout, out TypeMetadataCollection extraTypeMetadataResolved)
        {
            extraTypeMetadataResolved = null;

            //
            //TODO: Retrieve operation metadata from the target system and present it 
            //      as implementation of the OperationMetadata base class.
            //      Operation metadata represents operation signature, return type and etc.
            //
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Returns a TypeMetadata object resolved from the absolute name of the type metadata object.
        /// The method should complete within the specified time span or throw a timeout exception.
        /// </summary>
        public TypeMetadata ResolveTypeMetadata(string typeId, TimeSpan timeout, out TypeMetadataCollection extraTypeMetadataResolved)
        {
            extraTypeMetadataResolved = null;

            //
            //TODO: Retrieve type metadata from the target system and present it 
            //      as an implementation of the OperationMetadata base class.
            //      Type metadata represents target system supported types 
            //      as passed and returned through operation parameters.
            //
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        #endregion IMetadataResolver Members
    }
}
