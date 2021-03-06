// ***********************************************************************
// Copyright (c) 2014 Charlie Poole, Rob Prouse
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


namespace NUnit.Framework.Internal.Commands
{
    /// <summary>
    /// TestActionItem wraps a single execution of an ITestAction.
    /// It's primary purpose is to track whether the BeforeTest
    /// method has been called and suppress calling the
    /// AfterTest method if it has not. This is necessary when
    /// ITestActions are used before and after a CompositeWorkItem,
    /// since the OneTimeSetUpCommand and OneTimeTearDownCommand
    /// are separate command chains. By sharing a TestActionItem
    /// between the setup and teardown chains, the two calls can
    /// be coordinated.
    /// </summary>
    public class TestActionItem
    {
        private readonly ITestAction _action;

        /// <summary>
        /// Construct a TestActionItem
        /// </summary>
        /// <param name="action">The ITestAction to be included</param>
        public TestActionItem(ITestAction action)
        {
            _action = action;
        }

        /// <summary>
        /// Get flag indicating if the BeforeTest entry was already called.
        /// </summary>
        public bool BeforeTestWasRun { get; private set; }

        /// <summary>
        /// Run the BeforeTest method of the action and remember that it has been run.
        /// </summary>
        /// <param name="test">The test to which the action applies</param>
        public void BeforeTest(Interfaces.ITest test)
        {
            BeforeTestWasRun = true;
            _action.BeforeTest(test);
        }

        /// <summary>
        /// Run the AfterTest action, but only if the BeforeTest
        /// action was actually run.
        /// </summary>
        /// <param name="test">The test to which the action applies</param>
        public void AfterTest(Interfaces.ITest test)
        {
            if (BeforeTestWasRun)
                _action.AfterTest(test);
        }
    }
}
