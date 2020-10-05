using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gesisa.Sii.Hub.CsvToXmlConvert.Tools
{
	public class GenericCompare<T> : IEqualityComparer<T>
	{
		private Func<T, object>[] _expression { get; set; }
		public GenericCompare(params Func<T, object>[] expr )
		{
			this._expression = expr;
		}
		public bool Equals(T x, T y)
		{
			foreach (var expr in _expression)
			{
				var first = expr.Invoke(x);
				var sec = expr.Invoke(y);
				if (first == null || !first.Equals(sec))
					return false;
			}
			return true;
		}
		public int GetHashCode(T obj)
		{
			List<object> x = new List<object>();
			foreach (var expr in _expression)
			{
				x.Add(expr.Invoke(obj));
			}
			return RSHash(x.ToArray());
		}
		/// <summary> 
		/// This is a simple hashing function from Robert Sedgwicks Hashing in C book.
		/// Also, some simple optimizations to the algorithm in order to speed up
		/// its hashing process have been added. from: www.partow.net
		/// </summary>
		/// <param name="input">array of objects, parameters combination that you need
		/// to get a unique hash code for them</param>
		/// <returns>Hash code</returns>
		public static int RSHash(params object[] input)
		{
			const int b = 378551;
			int a = 63689;
			int hash = 0;

			// If it overflows then just wrap around
			unchecked
			{
				for (int i = 0; i < input.Length; i++)
				{
					if (input[i] != null)
					{
						hash = hash * a + input[i].GetHashCode();
						a = a * b;
					}
				}
			}

			return hash;
		}
	}
}
