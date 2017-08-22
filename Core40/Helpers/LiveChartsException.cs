//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Linq;

namespace LiveCharts.Helpers
{
    /// <summary>
    /// An Exception caused by LiveCharts
    /// </summary>
    /// <seealso cref="System.Exception" />
    internal class LiveChartsException : Exception
    {
        private readonly ExceptionReason _reason;
        private const string BaseUri = "https://lvcharts.net/exceptioncenter/";

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsException"/> class.
        /// </summary>
        public LiveChartsException(ExceptionReason reason)
        {
            _reason = reason;
        }

        public override string Message
        {
            get
            {
                var reason = (ReasonAttribute) typeof(ExceptionReason).GetCustomAttributes(typeof(ReasonAttribute), false).First();
                return reason.Message;
            }
        }

        public override string HelpLink
        {
            get { return BaseUri + _reason; }
            set { throw new NotImplementedException(); }
        }
    }
}
