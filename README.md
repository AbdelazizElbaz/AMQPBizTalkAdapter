
# AMQPBizTalkAdapter
recently i worked in a project that we needed to do communicates WSO2 Message brocker - which implement AMQP - and biztalk, i did not find either an official adapter or an adapter created by the community. that's why I started to create an adapter for this purpose.


# Add Adapter as binding custom

You can add a configuration in your machine.config  files (x86 and 64) or your BTSNTSvc.config and BTSNTSvc64.config

#### In <*bindingElementExtensions*> Element Add :   

    <add name="AmqpAdapter" type="AMQPBizTalkAdapter.AMQPBizTalkAdapterBindingElementExtensionElement, AMQPBizTalkAdapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d7ac4cbfa21b471c" />

   #### In <*bindingExtensions*> Element add line : 
  

    <add name="AmqpAdapterBinding" type="AMQPBizTalkAdapter.AMQPBizTalkAdapterBindingCollectionElement, AMQPBizTalkAdapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d7ac4cbfa21b471c" />

  
##### In   < *client* > add line : 

      <endpoint binding="AmqpAdapterBinding" contract="IMetadataExchange" name="Amqp" />

  
#### To use Adapter binding custom
![enter image description here](https://i.ibb.co/t2Wj4Zt/wcf-custom.png)

![enter image description here](https://i.ibb.co/XyXfDbX/binding.png)

##### Queue
amqp://server:port/Queue/QueueName

##### Topic
amqp://server:port/Topic/topicname?subscriptionIdentifier

