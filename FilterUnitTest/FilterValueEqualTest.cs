using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter;

namespace FilterUnitTest
{
    [TestClass]
    public class FilterValueEqualTest
    {
        [TestMethod]
        public void EqualTest()
        {
            Assert.AreEqual((IFilterValue)new FilterValue<int?>(), new FilterValue<int?>());

            Assert.AreNotEqual(new FilterValue<int?>{LeftValue = 1}, new FilterValue<int?>());

            Assert.AreNotEqual(new FilterValue<int?> {Values = null}, new FilterValue<int?>());

            Assert.AreNotEqual(new FilterValue<int?>(), null);
        }
    }
}
