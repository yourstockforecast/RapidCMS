﻿using RapidCMS.Common.Attributes;

namespace RapidCMS.Common.Enums
{
    public enum DefaultButtonType
    {
        // Insert new Entity
        [DefaultIconLabel(Icon = "plus", Label = "New")]
        [Actions(UsageType.List | UsageType.Add, UsageType.List | UsageType.New, UsageType.List | UsageType.Edit, UsageType.List | UsageType.View)]
        New = 1,

        [DefaultIconLabel(Icon = "hard-drive", Label = "Insert")]
        [Actions(UsageType.New)]
        SaveNew,

        [DefaultIconLabel(Icon = "hard-drive", Label = "Update")]
        [Actions(UsageType.Edit | UsageType.Node)]
        SaveExisting,

        [DefaultIconLabel(Icon = "trash", Label = "Delete")]
        [Actions(UsageType.Edit | UsageType.Node, UsageType.View | UsageType.Node)]
        Delete,

        [DefaultIconLabel(Icon = "pencil", Label = "Edit")]
        [Actions(UsageType.List, UsageType.Node | UsageType.Edit, UsageType.Node | UsageType.View)]
        Edit,

        [DefaultIconLabel(Icon = "magnifying-glass", Label = "View")]
        [Actions(UsageType.List, UsageType.Node | UsageType.Edit, UsageType.Node | UsageType.View)]
        View,

        // Add existing Entity
        [DefaultIconLabel(Icon = "plus", Label = "Add")]
        [Actions(UsageType.List)]
        Add,

        // Remove existing Entity
        [DefaultIconLabel(Icon = "circle-x", Label = "Remove")]
        [Actions(UsageType.Node | UsageType.Edit, UsageType.Node | UsageType.View)]
        Remove,

        // Pick existing Entity to Add
        [DefaultIconLabel(Icon = "plus", Label = "Pick")]
        [Actions(UsageType.Node | UsageType.Pick)]
        Pick,

        // TODO: implement
        [DefaultIconLabel(Icon = "arrow-left", Label = "Return")]
        [Actions(UsageType.Node, UsageType.List | UsageType.Add, UsageType.List | UsageType.New, UsageType.List | UsageType.Pick)]
        Return
    }
}
