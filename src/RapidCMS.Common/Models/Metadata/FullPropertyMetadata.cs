﻿using System;

namespace RapidCMS.Common.Models.Metadata
{
    internal class FullPropertyMetadata : PropertyMetadata, IFullPropertyMetadata
    {
        public FullPropertyMetadata(
            Type propertyType, 
            string propertyName, 
            Func<object, object> getter, 
            Action<object, object> setter,
            Type objectType, 
            string fingerprint) : base(propertyType, propertyName, getter, objectType, fingerprint)
        {
            Setter = setter ?? throw new ArgumentNullException(nameof(setter));
        }

        public Action<object, object> Setter { get; private set; }
    }
}
