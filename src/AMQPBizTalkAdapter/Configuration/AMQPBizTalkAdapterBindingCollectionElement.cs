/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterBindingCollectionElement.cs
/// Description :  Binding Collection Element class which implements the StandardBindingCollectionElement
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    /// <summary>
    /// Initializes a new instance of the AMQPBizTalkAdapterBindingCollectionElement class
    /// </summary>
    public class AMQPBizTalkAdapterBindingCollectionElement : StandardBindingCollectionElement<AMQPBizTalkAdapterBinding,
        AMQPBizTalkAdapterBindingElement>
    {
    }
}

