namespace AMQPBizTalkAdapter.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"SendMessageRequest", @"ReceiveMessage"})]
    public sealed class AmqpMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xsd:schema xmlns=""amqp://biztalkadapter.ampq/2018/Types"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""amqp://biztalkadapter.ampq/2018/Types"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <xsd:element name=""SendMessageRequest"" type=""SendMessageRequest"" />
  <xsd:element name=""ReceiveMessage"" type=""ReceiveMessage"" />
  <xsd:complexType name=""SendMessageRequest"">
    <xsd:sequence>
      <xsd:element minOccurs=""1"" maxOccurs=""1"" name=""RoutingKey"" type=""xsd:string"" />
      <xsd:element minOccurs=""1"" maxOccurs=""1"" name=""Body"" type=""xsd:string"" />
      <xsd:element name=""BasicProperties"" type=""BasicProperties"" />
    </xsd:sequence>
  </xsd:complexType>
  <xsd:complexType name=""ReceiveMessage"">
    <xsd:sequence>
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""ConsumerTag"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""DeliveryTag"" type=""xsd:unsignedLong"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""Redelivered"" type=""xsd:boolean"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""Exchange"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""RoutingKey"" type=""xsd:string"" />
      <xsd:element minOccurs=""1"" maxOccurs=""1"" name=""Body"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""BasicProperties"" type=""BasicProperties"" />
    </xsd:sequence>
  </xsd:complexType>
  <xsd:complexType name=""BasicProperties"">
    <xsd:sequence>
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""ContentType"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""ContentEncoding"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""DeliveryMode"" type=""xsd:byte"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""Priority"" type=""xsd:byte"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""CorrelationId"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""ReplyTo"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""Expiration"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""MessageId"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""Type"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""UserId"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""AppId"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""ClusterId"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""Timestamp"" type=""xsd:long"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""ProtocolClassId"" type=""xsd:int"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""ProtocolClassName"" type=""xsd:string"" />
      <xsd:element minOccurs=""0"" maxOccurs=""1"" name=""Headers"">
        <xsd:complexType>
          <xsd:sequence>
            <xsd:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Item"">
              <xsd:complexType>
                <xsd:sequence>
                  <xsd:element minOccurs=""1"" maxOccurs=""1"" name=""Key"" type=""xsd:string"" />
                  <xsd:element minOccurs=""1"" maxOccurs=""1"" name=""Value"" type=""xsd:string"" />
                </xsd:sequence>
              </xsd:complexType>
            </xsd:element>
          </xsd:sequence>
        </xsd:complexType>
      </xsd:element>
    </xsd:sequence>
  </xsd:complexType>
</xsd:schema>";
        
        public AmqpMessage() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "SendMessageRequest";
                _RootElements[1] = "ReceiveMessage";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
        
        [Schema(@"amqp://biztalkadapter.ampq/2018/Types",@"SendMessageRequest")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"SendMessageRequest"})]
        public sealed class SendMessageRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public SendMessageRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "SendMessageRequest";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"amqp://biztalkadapter.ampq/2018/Types",@"ReceiveMessage")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ReceiveMessage"})]
        public sealed class ReceiveMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ReceiveMessage() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ReceiveMessage";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
    }
}
