using Microsoft.ServiceModel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    internal class MetaDataHelper
    {
        public const string rootNodeId = "amqp";
        public const string ReceiveOperation = "ReceiveMessage";
        public const string ReceiveOperationNodeId = "amqp://biztalkadapter.ampq/2018/ReceiveMessage";
        public const string ReceiveOperationDescription = "This operation receive Message from MessageQueue.";
        public const string SendOperation = "SendMessage";
        public const string SendOperationNodeId = "amqp://biztalkadapter.ampq/2018/SendMessage";
        public const string SendOperationDescription = "This operation Send to MessageQueue.";
        public static MetadataRetrievalNode[] GetMetadataRetrievalNodeList()
        {
            // Inbound operations  
            MetadataRetrievalNode inOpNode = new MetadataRetrievalNode(MetaDataHelper.ReceiveOperation);
            inOpNode.DisplayName = MetaDataHelper.ReceiveOperation;
            inOpNode.Description = MetaDataHelper.ReceiveOperationDescription;
            inOpNode.Direction = MetadataRetrievalNodeDirections.Inbound;
            inOpNode.NodeId = MetaDataHelper.ReceiveOperationNodeId;
            inOpNode.IsOperation = true;


            // Outbound operations  
            MetadataRetrievalNode outOpNode = new MetadataRetrievalNode(MetaDataHelper.SendOperation);
            outOpNode.DisplayName = MetaDataHelper.SendOperation;
            outOpNode.Description = MetaDataHelper.SendOperationDescription;
            outOpNode.Direction = MetadataRetrievalNodeDirections.Outbound;
            outOpNode.NodeId = MetaDataHelper.SendOperationNodeId;
            outOpNode.IsOperation = true;


            return new MetadataRetrievalNode[] { inOpNode, outOpNode };

        }
    }
}
