using System.Collections.Generic;

namespace MVCControls.Extensions
{
    internal class ViewModelClass
    {
        public string Name { get; set; }

        public List<ViewModelProperty> Properties { get; set; }

        public override string ToString()
        {
            return string.Format("{0} class with {1} properties", Name, Properties != null ? Properties.Count : 0);
        }
    }
}