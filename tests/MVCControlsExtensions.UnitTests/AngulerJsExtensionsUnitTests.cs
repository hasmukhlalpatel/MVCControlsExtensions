using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCControls.Extensions.Ng;
using MVCControlsExtensions.UnitTests.Extensions;

namespace MVCControlsExtensions.UnitTests
{
    [TestClass]
    public class AngulerJsExtensionsUnitTests
    {
        private ViewDataDictionary _viewData;
        private HtmlHelper<NgTestModel> _htmlHelper;

        [TestInitialize]
        public void Init()
        {
            _viewData = new ViewDataDictionary()
            {
                Model = new NgTestModel()
                {
                    Name = "Test Name"
                }
            };

            _htmlHelper = HttpHelper.CreateHtmlHelper<NgTestModel>(_viewData);
            HttpHelper.SetCurrent();
            _htmlHelper.SetNgBindingWithModelName(true);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var results = NgControlExtensions.NgDisplayFor(_htmlHelper, x => x.Name);

        }

        [TestMethod]
        public void TestNgTextBoxFor()
        {
            const string strExpected = "<input data-ng-model=\"NgTestModel.Name\" id=\"Name\" name=\"Name\" type=\"text\" value=\"Test Name\" />";

            var results = NgControlExtensions.NgTextBoxFor(_htmlHelper, x => x.Name);
            Assert.AreEqual(strExpected, results.ToString());

        }

        [TestMethod]
        public void TestNgTextAreaFor()
        {
            const string strExpected = "<textarea cols=\"20\" data-ng-model=\"NgTestModel.Name\" id=\"Name\" name=\"Name\" rows=\"2\">\r\nTest Name</textarea>";
            
            var results = NgControlExtensions.NgTextAreaFor(_htmlHelper, x => x.Name);
            Assert.AreEqual(strExpected, results.ToString());
        }

        [TestMethod]
        public void TestKOPasswordFor()
        {
            const string strExpected = "<input data-ng-model=\"NgTestModel.Name\" id=\"Name\" name=\"Name\" type=\"password\" />";

            var results = _htmlHelper.NgPasswordFor(x => x.Name);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKORadioButtonFor()
        {
            const string strExpected = "<table ><tr><td><label><input checked=\"checked\" data-ng-model=\"NgTestModel.TestEnum\" id=\"TestEnum\" name=\"TestEnum\" type=\"radio\" value=\"Test1\" />Test1</label></td><td><label><input data-ng-model=\"NgTestModel.TestEnum\" id=\"TestEnum\" name=\"TestEnum\" type=\"radio\" value=\"Test2\" />Test2</label></td></tr></table>";

            var results = _htmlHelper.NgRadioButtonFor(x => x.TestEnum);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKODropDownListFor()
        {
            const string strExpected = "<select data-ng-model=\"NgTestModel.TestEnum\" id=\"TestEnum\" name=\"TestEnum\"><option value=\"Test1\">Test1</option>\r\n<option value=\"Test2\">Test2</option>\r\n</select>";

            var results = _htmlHelper.NgDropDownListFor(x => x.TestEnum);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKOHiddenFor()
        {
            const string strExpected = "<input data-ng-model=\"NgTestModel.Id\" id=\"Id\" name=\"Id\" type=\"hidden\" value=\"0\" />";
            var results = _htmlHelper.NgHiddenFor(x => x.Id);
            Assert.AreEqual(strExpected, results.ToString());
        }

    }

    public class NgTestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }

        public NgTestEnum TestEnum { get; set; }
    }

    public enum NgTestEnum
    {
        Test1 = 1,
        Test2 = 2
    }
}
