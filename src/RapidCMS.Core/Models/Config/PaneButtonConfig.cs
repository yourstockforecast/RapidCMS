using System;
using RapidCMS.Core.Abstractions.Config;
using RapidCMS.Core.Enums;

namespace RapidCMS.Core.Models.Config
{
    internal class PaneButtonConfig : ButtonConfig, IPaneButtonConfig
    {
        internal PaneButtonConfig(Type paneType, CrudType? crudType)
        {
            PaneType = paneType ?? throw new ArgumentNullException(nameof(paneType));
            CrudType = crudType;
        }

        internal Type PaneType { get; set; }
        public CrudType? CrudType { get; set; }
    }
}
