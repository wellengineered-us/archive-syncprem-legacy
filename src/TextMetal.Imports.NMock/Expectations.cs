using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMock
{
	public class Expectations : List<IVerifyableExpectation>
	{
		public Expectations() :base()
		{
			
		}

		public Expectations(int capacity) : base(capacity)
		{
			for (var i = 0; i < capacity; i++)
			{
				Add(null);
			}
		}


	}
}
