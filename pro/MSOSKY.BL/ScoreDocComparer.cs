using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;

namespace MSOSKY.BL
{
    public class ScoreDocComparer : IEqualityComparer<ScoreDoc>
    {
        IndexSearcher search;
        public ScoreDocComparer(IndexSearcher s)
        {
            search=s;
        }

        public bool Equals(ScoreDoc x, ScoreDoc y)
        {
            if (object.ReferenceEquals(x,y)) return true;
            if(object.ReferenceEquals(x,null) || object.ReferenceEquals(y,null)) return false;
            var xd = search.Doc(x.Doc);
            var yd = search.Doc(y.Doc);
            if (xd==yd) return true;
            return xd.Get("keyContent") == yd.Get("keyContent") ;
            
        }

        public int GetHashCode(ScoreDoc obj)
        {
            return obj.GetHashCode();
        }
    }
}
