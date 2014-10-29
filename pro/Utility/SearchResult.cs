using System;

namespace Utility
{
	public class SearchResult
	{
		public string KeyContent;

		public string KeyWords;

		public DateTime RecvTime;

		public DateTime UpdateTime;

		public long TotalSize;

		public string HashKey;

		public long HashId;

		public int FileCnt;

		public int Type;

		public bool ICOrE;

		public string BodyPreview;

		public SearchResult()
		{
		}
	}
}