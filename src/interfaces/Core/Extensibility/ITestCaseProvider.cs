// ***********************************************************************
// Copyright (c) 2008 Charlie Poole
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

using System.Collections;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
    /// <summary>
    /// The ITestCaseProvider interface is used by extensions
    /// that provide data for parameterized tests, along with
    /// certain flags and other indicators used in the test.
    /// </summary>
    public interface ITestCaseProvider
    {
        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        bool HasTestCasesFor(MethodInfo method);

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a paramterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IEnumerable GetTestCasesFor(MethodInfo method);
    }

    /// <summary>
    /// ITestCaseProvider2 extends ITestCaseProvider with methods
    /// that include the suite for which the test case is being
    /// built. TestCaseProviders not needing the suite can
    /// continue to implement ITestCaseBuilder.
    /// </summary>
    public interface ITestCaseProvider2 : ITestCaseProvider
    {
        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <param name="suite">The suite for which the test case is being built</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        bool HasTestCasesFor(MethodInfo method, Test suite);

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a paramterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="suite">The suite for which the test case is being built</param>
        /// <returns></returns>
        IEnumerable GetTestCasesFor(MethodInfo method, Test suite);
    }
}
