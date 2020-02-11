using System;
using System.Threading.Tasks;

namespace RapidCMS.Core.Models.UI
{
    public class ElementUI
    {
        internal ElementUI(Func<object, Task<bool>> isVisible, Func<object, Task<bool>> isDisabled)
        {
            IsVisible = isVisible ?? throw new ArgumentNullException(nameof(isVisible));
            IsDisabled = isDisabled ?? throw new ArgumentNullException(nameof(isDisabled));
        }

        public Func<object, Task<bool>> IsVisible { get; private set; }
        public Func<object, Task<bool>> IsDisabled { get; private set; }
    }
}
