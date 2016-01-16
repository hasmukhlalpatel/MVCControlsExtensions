using System;

namespace MvcExtensions.Demo.Models
{
    public class NewsViewModel : ViewModelBase
    {
        public string Summary { get; set; }

        public string Description { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}