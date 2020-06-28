namespace AMQPBizTalkAdapter.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"amqp://biztalkadapter.ampq/2018/",@"AmqpMessageEnveloppe")]
    [BodyXPath(@"/*[local-name()='AmqpMessageEnveloppe' and namespace-uri()='amqp://biztalkadapter.ampq/2018/']/*[local-name()='Result' and namespace-uri()='amqp://biztalkadapter.ampq/2018/']")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"AmqpMessageEnveloppe"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"AMQPBizTalkAdapter.Schemas.AmqpMessage", typeof(global::AMQPBizTalkAdapter.Schemas.AmqpMessage))]
    public sealed class Schema1 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""amqp://biztalkadapter.ampq/2018/Types"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""amqp://biztalkadapter.ampq/2018/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""AMQPBizTalkAdapter.Schemas.AmqpMessage"" namespace=""amqp://biztalkadapter.ampq/2018/Types"" />
  <xs:annotation>
    <xs:appinfo>
      <b:schemaInfo is_envelope=""yes"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" />
      <b:references>
        <b:reference targetNamespace=""amqp://biztalkadapter.ampq/2018/Types"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""AmqpMessageEnveloppe"" nillable=""true"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo body_xpath=""/*[local-name()='AmqpMessageEnveloppe' and namespace-uri()='amqp://biztalkadapter.ampq/2018/']/*[local-name()='Result' and namespace-uri()='amqp://biztalkadapter.ampq/2018/']"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Result"" nillable=""true"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""1"" maxOccurs=""unbounded"" name=""ReceiveMessage"" type=""ReceiveMessage"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public Schema1() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "AmqpMessageEnveloppe";
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
