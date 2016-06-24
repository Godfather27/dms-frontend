using System;

namespace QueryImage
{
	public class SearchResult
	{
		public string[] results = new string[10];

		public SearchResult (string[] results)
		{
			this.results = results;
		}
	}
}

