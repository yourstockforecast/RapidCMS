using System;
using System.Threading.Tasks;
using RapidCMS.Core.Abstractions.Config;
using RapidCMS.Core.Abstractions.Forms;

namespace RapidCMS.Core.Models.Config
{
    internal class ButtonConfig : IButtonConfig
    {
        internal string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Label { get; set; }
        public string? Icon { get; set; }
        public bool IsPrimary { get; set; }

        internal Func<IButtonContext, Task< bool>> IsDisabled { get; set; } = x => Task.FromResult(false);

        public IButtonConfig DisableWhen(Func<IButtonContext, bool> predicate)
        {
            IsDisabled = x => Task.FromResult(predicate.Invoke(x));

            return this;
        }

        public IButtonConfig DisableWhen(Func<IButtonContext, Task<bool>> predicate)
        {
            IsDisabled = predicate;

            return this;
        }
    }
}
