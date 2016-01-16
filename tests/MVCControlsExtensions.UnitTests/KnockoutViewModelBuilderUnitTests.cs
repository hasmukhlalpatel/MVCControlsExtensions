using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCControls.Extensions;

namespace MVCControlsExtensions.UnitTests
{
    [TestClass]
    public class KnockoutViewModelBuilderUnitTests
    {

        [TestInitialize]
        public void Init()
        {
            ViewModelBuilder.ViewModelDictionary.Clear();
        }

        [TestMethod]
        public void GenerateViewModel()
        {
            ViewModelBuilder.BuildViewModel(typeof(StudentClass), t => t.Name.Replace("ViewModel", ""));

            var koVmBuilder = new ViewModelBuilder(new KoViewModelGenerator());

            var viewmodelStr = koVmBuilder.GenerateViewModel(typeof(StudentClass));
            var sbViewmodel = new StringBuilder(viewmodelStr);
            sbViewmodel.Replace('\t', ' ').Replace('\n', ' ').Replace(" ","");
            var minifiedStr = sbViewmodel.ToString();
            var expectedStr =
                "varStudentClass=function(){self=this;self.Id=ko.observable();self.Name=ko.observable();self.Update=function(data){self.Id((data==null?0:(data.Id==null?0:data.Id)));self.Name((data==null?'':(data.Name==null?'':data.Name)));}}";
            Assert.AreEqual(expectedStr, minifiedStr);
        }
    }
}