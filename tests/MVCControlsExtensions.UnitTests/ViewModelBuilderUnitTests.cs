using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCControls.Extensions;

namespace MVCControlsExtensions.UnitTests
{
    [TestClass]
    public class ViewModelBuilderUnitTests
    {

        [TestInitialize]
        public void Init()
        {
            ViewModelBuilder.ViewModelDictionary.Clear();
        } 

        [TestMethod]
        public void TestBuildViewModel()
        {
            ViewModelBuilder.BuildViewModel(typeof (Student), t => t.Name.Replace("ViewModel", ""));

            Assert.IsTrue(ViewModelBuilder.ViewModelDictionary.ContainsKey(typeof(Student)), "Should have Student type in the list");
            Assert.IsTrue(ViewModelBuilder.ViewModelDictionary.ContainsKey(typeof(StudentClass)), "Should have StudentClass in the list");
        }

        [TestMethod]
        public void GetGeneratedViewModelClass()
        {
            ViewModelBuilder.BuildViewModel(typeof(Student), t => t.Name.Replace("ViewModel", ""));
            var viewModelClass = ViewModelBuilder.ViewModelDictionary[typeof (Student)];
            Assert.IsNotNull(viewModelClass);
            Assert.AreSame(typeof(Student).Name, viewModelClass.Name);
        }

        [TestMethod]
        public void GetGeneratedViewModelClassProperties()
        {
            ViewModelBuilder.BuildViewModel(typeof(Student), t => t.Name.Replace("ViewModel", ""));
            var viewModelClass = ViewModelBuilder.ViewModelDictionary[typeof(Student)];

            Assert.AreEqual(5, viewModelClass.Properties.Count);
        }

    }
}
