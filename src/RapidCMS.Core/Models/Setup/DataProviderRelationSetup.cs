﻿using System;

namespace RapidCMS.Core.Models.Setup
{
    internal class DataProviderRelationSetup : RelationSetup
    {
        internal DataProviderRelationSetup(Type dataCollectionType)
        {
            DataCollectionType = dataCollectionType ?? throw new ArgumentNullException(nameof(dataCollectionType));
        }

        internal Type DataCollectionType { get; set; }
    }
}
