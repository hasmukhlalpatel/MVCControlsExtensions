using System;

namespace MVCControls.Extensions
{
    internal class ViewModelProperty
    {
        public string Name { get; set; }

        public Type UnderlyingType { get; set; }

        public bool IsNullable { get; set; }

        public bool IsArray { get; set; }
        public bool IsGenericType { get; set; }
        public bool IsClass { get; set; }
        public bool IsEnum { get; set; }
        public bool IsUnknown { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, UnderlyingType != null ? UnderlyingType.FullName : "");
        }
    }
}