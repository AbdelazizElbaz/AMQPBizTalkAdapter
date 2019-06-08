using Microsoft.ServiceModel.Channels.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace AMQPBizTalkAdapter
{
    internal class AmqpMessageTypeMetadata : TypeMetadata
    {
        private const String MetaDataFileName = "AMQPBizTalkAdapter.AmqpMessage.xsd";
        public AmqpMessageTypeMetadata(string typeId, string typeName) : base(typeId, typeName)
        {
            this.TypeNamespace = AMQPBizTalkAdapter.SERVICENAMESPACE + "/Types";
            this.Description = " ";
            this.CanUseCommonCache = true;
            // if the nillable is not set to true, the generated proxy wraps the operation  
            // with request and response objects  
            this.IsNillable = true;
        }

        /// <summary>  
        /// Override the base ExportXmlSchema to provide own   
        /// custom XML Schema  
        /// </summary>  
        /// <param name="schemaExportContext"></param>  
        /// <param name="metadataLookup"></param>  
        /// <param name="timeout"></param>  
        public override void ExportXmlSchema(XmlSchemaExportContext schemaExportContext, MetadataLookup metadataLookup, TimeSpan timeout)
        {
            if (schemaExportContext == null)
            {
                throw new AdapterException("Schema export context is null.");
            }
            // Read in XML Schema file or create XmlSchema object yourself  
            Stream predefinedXsdFile = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(MetaDataFileName);
            XmlReader reader = XmlReader.Create(predefinedXsdFile);
            XmlSchema schema = XmlSchema.Read(reader, null);
            if (!IsComplexTypeAlreadyDefined(schemaExportContext.SchemaSet, schema))
            {
                schemaExportContext.SchemaSet.Add(schema);
                if (!schemaExportContext.NamespacePrefixSet.ContainsKey(this.TypeNamespace))
                {
                    schemaExportContext.NamespacePrefixSet.Add(this.TypeNamespace, getUniqueNamespacePrefix(schemaExportContext, 0));
                }
            }
            reader.Close();
        }
        /// <summary>  
        /// A default value cannot be set for this type metadata.  
        /// </summary>  
        public override bool CanSetDefaultValue
        {
            get { return false; }
        }
        /// <summary>  
        /// Helper function to see if the schema is already defined in the   
        /// XmlSchemaSet.  
        /// </summary>  
        /// <param name="oldschemaset"></param>  
        /// <param name="newschema"></param>  
        /// <returns></returns>  
        public static bool IsComplexTypeAlreadyDefined(XmlSchemaSet oldschemaset, XmlSchema newschema)
        {
            // ensure correct namespace was defined in the passed-in schema  
            foreach (XmlSchema schema in oldschemaset.Schemas(newschema.TargetNamespace))
            {
                foreach (XmlSchemaObject newschemaObject in newschema.Items)
                {
                    if (newschemaObject is XmlSchemaComplexType)
                    {
                        //check for the definition of complex type in the schemaset             
                        foreach (XmlSchemaObject schemaObject in schema.Items)
                        {
                            XmlSchemaComplexType complexType = schemaObject as XmlSchemaComplexType;
                            // Definition of this Complex Type already exists  
                            if (complexType != null && String.Compare(complexType.Name, ((XmlSchemaComplexType)newschemaObject).Name, false, System.Globalization.CultureInfo.InvariantCulture) == 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>  
        /// Helper function to generate a unique namespace prefix  
        /// </summary>  
        /// <param name="schemaExportContext"></param>  
        /// <param name="startSuffix"></param>  
        /// <returns></returns>  
        private string getUniqueNamespacePrefix(XmlSchemaExportContext schemaExportContext, int startSuffix)
        {
            string defaultPrefix = "ns";
            string val = defaultPrefix + startSuffix;
            if (schemaExportContext.NamespacePrefixSet.ContainsValue(val))
            {
                return getUniqueNamespacePrefix(schemaExportContext, ++startSuffix);
            }
            else
            {
                return val;
            }
        }


    }
}
