﻿// ***********************************************************************
// Copyright (c) 2009 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.TestUtilities;

namespace NUnit.Framework.Attributes
{
    [TestFixture("hello", "hello", "goodbye")]
    [TestFixture("zip", "zip")]
    [TestFixture(42, 42, 99)]
    public class ParameterizedTestFixture
    {
        private string eq1;
        private string eq2;
        private string neq;
        
        public ParameterizedTestFixture(string eq1, string eq2, string neq)
        {
            this.eq1 = eq1;
            this.eq2 = eq2;
            this.neq = neq;
        }

        public ParameterizedTestFixture(string eq1, string eq2)
            : this(eq1, eq2, null) { }

        public ParameterizedTestFixture(int eq1, int eq2, int neq)
        {
            this.eq1 = eq1.ToString();
            this.eq2 = eq2.ToString();
            this.neq = neq.ToString();
        }

        [Test]
        public void TestEquality()
        {
            Assert.AreEqual(eq1, eq2);
            if (eq1 != null && eq2 != null)
                Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }

        [Test]
        public void TestInequality()
        {
            Assert.AreNotEqual(eq1, neq);
            if (eq1 != null && neq != null)
                Assert.AreNotEqual(eq1.GetHashCode(), neq.GetHashCode());
        }
    }

    [TestFixture(42)]
    public class ParameterizedTestFixtureWithDataSources
    {
        private int answer;

        object[] myData = { new int[] { 6, 7 }, new int[] { 3, 14 } };

        public ParameterizedTestFixtureWithDataSources(int val)
        {
            this.answer = val;
        }

        [Test, TestCaseSource("myData")]
        public void CanAccessTestCaseSource(int x, int y)
        {
            Assert.That(x * y, Is.EqualTo(answer));
        }

#if CLR_2_0
        IEnumerable GenerateData()
        {
            for(int i = 1; i <= answer; i++)
                if ( answer%i == 0 )
                    yield return new int[] { i, answer/i  };
        }

        [Test, TestCaseSource("GenerateData")]
        public void CanGenerateDataFromParameter(int x, int y)
        {
            Assert.That(x * y, Is.EqualTo(answer));
        }
#endif

        int[] intvals = new int[] { 1, 2, 3 };

        [Test]
        public void CanAccessValueSource(
            [ValueSource("intvals")] int x)
        {
            Assert.That(answer % x == 0);
        }
    }

    public class ParameterizedTestFixtureNamingTests
    {
        Test fixture;
        Test[] instances;

        [SetUp]
        public void MakeFixture()
        {
            fixture = TestBuilder.MakeFixture(typeof(NUnit.TestData.ParameterizedTestFixture));
        }

        [Test]
        public void TopLevelSuiteIsNamedCorrectly()
        {
            Assert.That(fixture.TestName.Name, Is.EqualTo("ParameterizedTestFixture"));
            Assert.That(fixture.TestName.FullName, Is.EqualTo("NUnit.TestData.ParameterizedTestFixture"));
        }

        [Test]
        public void SuiteHasCorrectNumberOfInstances()
        {
            Assert.That(fixture.Tests.Count, Is.EqualTo(2));
        }

        [Test]
        public void FixtureInstancesAreNamedCorrectly()
        {
            ArrayList names = new ArrayList();
            ArrayList fullnames = new ArrayList();
            foreach (Test test in fixture.Tests)
            {
                names.Add(test.TestName.Name);
                fullnames.Add(test.TestName.FullName);
            }

            Assert.That(names, Is.EquivalentTo(new string[] {
                "ParameterizedTestFixture(1)", "ParameterizedTestFixture(2)" }));
            Assert.That(fullnames, Is.EquivalentTo(new string[] {
                "NUnit.TestData.ParameterizedTestFixture(1)", "NUnit.TestData.ParameterizedTestFixture(2)" }));
        }

        [Test]
        public void MethodWithoutParamsIsNamedCorrectly()
        {
            Test instance = (Test)fixture.Tests[0];
            Test method = TestFinder.Find("MethodWithoutParams", instance, false);
            Assert.That(method, Is.Not.Null );
            Assert.That(method.TestName.FullName, Is.EqualTo(instance.TestName.FullName + ".MethodWithoutParams"));
        }

        [Test]
        public void MethodWithParamsIsNamedCorrectly()
        {
            Test instance = (Test)fixture.Tests[0];
            Test method = TestFinder.Find("MethodWithParams", instance, false);
            Assert.That(method, Is.Not.Null);
            
            Test testcase = (Test)method.Tests[0];
            Assert.That(testcase.TestName.Name, Is.EqualTo("MethodWithParams(10,20)"));
            Assert.That(testcase.TestName.FullName, Is.EqualTo(instance.TestName.FullName + ".MethodWithParams(10,20)"));
        }

        //[Test]
        //public void M
    }
}