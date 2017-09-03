using System;

namespace CafeLib.SignalR
{
    /// <summary>
    /// Method export attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodExportAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Method { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodExportAttribute"/> class.
        /// </summary>
        /// <param name='method'>
        /// Alias.
        /// </param>
        public MethodExportAttribute(string method)
        {
            Method = method;
        }
    }
}
