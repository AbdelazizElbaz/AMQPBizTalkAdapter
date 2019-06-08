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
            return true;
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

            return true;
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
            extraTypeMetadataResolved = null;
            switch (operationId)
            {
                case MetaDataHelper.ReceiveOperationNodeId:
                    {
                        ParameterizedOperationMetadata om = new ParameterizedOperationMetadata(operationId, MetaDataHelper.ReceiveOperation);
                        om.OriginalName = MetaDataHelper.ReceiveOperation;
                        om.Description = MetaDataHelper.ReceiveOperationDescription;
                        om.OperationGroup = "AmqpInboundContract";
                        om.OperationNamespace = AMQPBizTalkAdapter.SERVICENAMESPACE;
                        om.NeverExpires = true;
                        ComplexQualifiedType cqtAmqpMessage = new ComplexQualifiedType("Types/AmqpMessageTypeMetadata");
                        
                        om.OperationResult = new OperationResult(cqtAmqpMessage, false);

                        // resolve extra typemetadata here  
                        extraTypeMetadataResolved = new TypeMetadataCollection();

                        // use a predefined schema to generate metadata for this type  
                        AmqpMessageTypeMetadata AmqpMessageMessageMetaData = new AmqpMessageTypeMetadata("Types/AmqpMessageTypeMetadata", "ReceiveMessage");
                        extraTypeMetadataResolved.Add(AmqpMessageMessageMetaData);

                        return om;
                    }
                case MetaDataHelper.SendOperationNodeId:
                    {
                        ParameterizedOperationMetadata om = new ParameterizedOperationMetadata(operationId, MetaDataHelper.SendOperation);
                        om.OriginalName = MetaDataHelper.SendOperation;
                        om.Description = MetaDataHelper.SendOperationDescription;
                        om.OperationGroup = "AmqpOutboundContract";
                        om.OperationNamespace = AMQPBizTalkAdapter.SERVICENAMESPACE;

                        ComplexQualifiedType cqtWso2MessageRequest = new ComplexQualifiedType("Types/AmqpMessageTypeMetadata");

                        OperationParameter parmMessage = new OperationParameter("SendMessageRequest", OperationParameterDirection.In, cqtWso2MessageRequest, false);
                        om.Parameters.Add(parmMessage);

                        ComplexQualifiedType cqtWso2MessageResponse = new ComplexQualifiedType("Types/AmqpMessageTypeMetadata");
                        // resolve extra typemetadata here  
                        extraTypeMetadataResolved = new TypeMetadataCollection();
                        AmqpMessageTypeMetadata AmqpMessageMetaDataRequest = new AmqpMessageTypeMetadata("Types/AmqpMessageTypeMetadata", "SendMessageRequest");
                        extraTypeMetadataResolved.Add(AmqpMessageMetaDataRequest);

                        om.OperationResult = OperationResult.Empty;
                        return om;
                    }

                default:
                    throw new AdapterException("Cannot resolve metadata for operation identifier " + operationId);
            }
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
                return null;
        }

        #endregion IMetadataResolver Members
    }
}
