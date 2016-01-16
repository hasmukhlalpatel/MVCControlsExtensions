using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVCControls.Extensions.Ko;
using MVCControlsExtensions.UnitTests.Extensions;

namespace MVCControlsExtensions.UnitTests
{
    [TestClass]
    public class KnockoutJsExtensionsUnitTests
    {
        private HtmlHelper<KoTestModel> _htmlHelper;
        private ViewDataDictionary _viewData;

        [TestInitialize]
        public void Init()
        {
            _viewData = new ViewDataDictionary()
            {
                Model = new KoTestModel()
                {
                    Name = "Test Name"
                }
            };
            _htmlHelper = HttpHelper.CreateHtmlHelper<KoTestModel>(_viewData);
            HttpHelper.SetCurrent();
            _htmlHelper.SetKOBindingWithModelName(true);
        }

        [TestMethod]
        public void TestKOTextBoxFor()
        {
            const string strExpected =
                "<input data-bind=\"value: KoTestModel.Name\" id=\"Name\" name=\"Name\" type=\"text\" value=\"Test Name\" />";

            var results = _htmlHelper.KOTextBoxFor(x => x.Name);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKOTextAreaFor()
        {
            const string strExpected =
                "<textarea cols=\"20\" data-bind=\"value: KoTestModel.Name\" id=\"Name\" name=\"Name\" rows=\"2\">\r\nTest Name</textarea>";

            var results = _htmlHelper.KOTextAreaFor(x => x.Name);
            Assert.AreEqual(strExpected, results.ToString());
        }

        [TestMethod]
        public void TestKOPasswordFor()
        {
            const string strExpected =
                "<input data-bind=\"value: KoTestModel.Name\" id=\"Name\" name=\"Name\" type=\"password\" />";

            var results = _htmlHelper.KOPasswordFor(x => x.Name);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKODateTextBoxFor()
        {
            const string strExpected =
                "<input class=\"date\" id=\"DateOfBirth\" name=\"DateOfBirth\" type=\"text\" value=\"01/01/0001\" />";

            var results = _htmlHelper.KODateTextBoxFor(x => x.DateOfBirth);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKOCheckBoxFor()
        {
            const string strExpected =
                "<label ><input data-bind=\"checked: KoTestModel.IsActive\" id=\"IsActive\" name=\"IsActive\" type=\"checkbox\" value=\"true\" /><input name=\"IsActive\" type=\"hidden\" value=\"false\" />IsActive</label>";

            var results = _htmlHelper.KOCheckBoxFor(x => x.IsActive);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKORadioButtonFor()
        {
            const string strExpected = "<table ><tr><td><label><input checked=\"checked\" data-bind=\"checked: KoTestModel.TestEnum\" id=\"TestEnum\" name=\"TestEnum\" type=\"radio\" value=\"Test1\" />Test1</label></td><td><label><input data-bind=\"checked: KoTestModel.TestEnum\" id=\"TestEnum\" name=\"TestEnum\" type=\"radio\" value=\"Test2\" />Test2</label></td></tr></table>";

            var results = _htmlHelper.KORadioButtonFor(x => x.TestEnum);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKODropDownListFor()
        {
            const string strExpected = "<select data-bind=\"value: KoTestModel.TestEnum\" id=\"TestEnum\" name=\"TestEnum\"><option value=\"Test1\">Test1</option>\r\n<option value=\"Test2\">Test2</option>\r\n</select>";

            var results = _htmlHelper.KODropDownListFor(x => x.TestEnum);
            Assert.AreEqual(strExpected, results.ToString());
        }


        [TestMethod]
        public void TestKOHiddenFor()
        {
            const string strExpected ="<input data-bind=\"value: KoTestModel.Id\" id=\"Id\" name=\"Id\" type=\"hidden\" value=\"0\" />";
            var results = _htmlHelper.KOHiddenFor(x => x.Id);
            Assert.AreEqual(strExpected, results.ToString());
        }

    }

    public class KoTestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }

        public TestEnum TestEnum { get; set; }
    }

    public enum TestEnum
    {
        Test1 = 1,
        Test2 = 2
    }

    public class TestController : Controller
    {
        public TestController()
        {
            ;
        }
    }
}

