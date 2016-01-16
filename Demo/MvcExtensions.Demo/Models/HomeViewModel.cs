using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcExtensions.Demo.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            NewsList = new List<NewsViewModel>();
            Address = new AddressViewModel();
        }

        public NewsViewModel MainNews { get; set; }
        public AddressViewModel Address { get; set; }
        public List<NewsViewModel> NewsList { get; set; }
    }

    public class AddressViewModel : ViewModelBase
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Postcode { get; set; }      
    }

    public abstract class ViewModelBase
    {
        public int Id { get; set; }
    }
}