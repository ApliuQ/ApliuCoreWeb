using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apliu.Standard.MongoDB
{
    public static class BsonDocumentHelper
    {
        /// <summary>
        /// Returns a Json string of the document, Remove _Id attribute.
        /// </summary>
        /// <returns></returns>
        public static string ToJsonNull_Id(this BsonDocument bsonDocument)
        {
            StringBuilder stringBuilder = new StringBuilder("{");
            IEnumerable<BsonElement> bsonElements = bsonDocument.Elements.Where(a => a != bsonDocument.Elements.First());
            foreach (BsonElement temp in bsonElements)
            {
                if (stringBuilder.Length > 1) stringBuilder.Append(",");
                stringBuilder.Append("\"");
                stringBuilder.Append(temp.Name);
                stringBuilder.Append("\":\"");
                stringBuilder.Append(temp.Value);
                stringBuilder.Append("\"");
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}
