using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility
{
    public class Config : IConfig
    {
        /// <inheritdoc />
        [Description("Whether to enabled or disable plugin")]
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        [Description("Whether to enabled/disable debug")]
        public bool IsDebugEnabled { get; set; } = false;
    }
}
