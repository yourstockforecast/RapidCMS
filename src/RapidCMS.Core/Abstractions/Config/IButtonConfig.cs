using System;
using System.Threading.Tasks;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Enums;

namespace RapidCMS.Core.Abstractions.Config
{
    public interface IButtonConfig
    {
        /// <summary>
        /// Label displayed on button.
        /// </summary>
        string? Label { get; set; }

        /// <summary>
        /// Icon displayed on button in front of label.
        /// </summary>
        string? Icon { get; set; }

        /// <summary>
        /// Marks the button as Primary, and makes it more visually stand out.
        /// </summary>
        bool IsPrimary { get; set; }

        /// <summary>
        /// Sets an expression which determine whether this should be disabled.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IButtonConfig DisableWhen(Func<IButtonContext, bool> predicate);

        /// <summary>
        /// Sets an expression which determine whether this should be disabled.
        /// </summary>
        /// <param name="predicate"></param>
        IButtonConfig DisableWhen(Func<IButtonContext, Task<bool>> predicate);
    }

    public interface IPaneButtonConfig : IButtonConfig
    { 
        /// <summary>
        /// Default crud type that is returned by the pane.
        /// </summary>
        CrudType? CrudType { get; set; }
    }
}
