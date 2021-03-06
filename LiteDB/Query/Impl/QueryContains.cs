﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LiteDB
{
    /// <summary>
    /// Contains query do not work with index, only full scan
    /// </summary>
    internal class QueryContains : Query
    {
        private BsonValue _value;

        public QueryContains(string field, BsonValue value)
            : base(field)
        {
            _value = value;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            this.ExecuteMode = QueryExecuteMode.FullScan;

            return indexer.FindAll(index, Query.Ascending);
        }

        internal override bool ExecuteFullScan(BsonDocument doc)
        {
            var val = doc.Get(this.Field);

            if(!val.IsString) return false;

            return val.AsString.Contains(_value.AsString);
        }
    }
}
