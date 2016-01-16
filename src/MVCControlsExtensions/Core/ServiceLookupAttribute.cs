using System;

namespace MVCControls.Core
{
    /// <summary>
    /// Service Lookup Attribute to specify on the property where to look for the lookup. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    internal sealed class ServiceLookupAttribute : Attribute
    {
        public Type LookupType { get; set; }

        public string MethodName { get; set; }
    }

    /// <summary>
    /// Dispaly Text based on lookup specified property value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    internal sealed class LookupTextAttribute : Attribute
    {
        public string LookupIdPropertyName { get; set; }
    }
}