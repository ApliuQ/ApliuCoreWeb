using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apliu.Standard.MongoDB
{
    public class MongoDBHelper
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        private String _connectionStr = String.Empty;
        /// <summary>
        /// 数据库名称
        /// </summary>
        private String _databaseName = String.Empty;
        /// <summary>
        /// 集合名称
        /// </summary>
        private String _collectionName = String.Empty;

        /// <summary>
        /// Represents a database in MongoDB.
        /// </summary>
        private IMongoDatabase _mongoDatabase;
        /// <summary>
        /// The client interface to MongoDB.
        /// </summary>
        private IMongoClient _mongoClient;

        //private IMongoCollection<T> _mongoCollection;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connectionStr">数据库链接字符串</param>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        public MongoDBHelper(String connectionStr, String databaseName, String collectionName)
        {
            _connectionStr = connectionStr;
            _databaseName = databaseName;
            _collectionName = collectionName;
            Initialization();
        }

        /// <summary>
        /// 初始化数据库链接
        /// </summary>
        private void Initialization()
        {
            try
            {
                MongoUrlBuilder mongoUrlBuilder = new MongoUrlBuilder(_connectionStr);
                _mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());//创建并实例化客户端
                _mongoDatabase = _mongoClient.GetDatabase(_databaseName);//实例化数据库
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取数据库中指定集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>()
        {
            return _mongoDatabase.GetCollection<T>(_collectionName);
        }

        /// <summary>
        /// 获取数据库中指定集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(String collectionName)
        {
            return _mongoDatabase.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// 获取字典中的键值对查询条件对象（值完全相等）
        /// </summary>
        /// <param name="findDictionary"></param>
        /// <returns></returns>
        private FilterDefinition<BsonDocument> GetFilterEquals(Dictionary<String, Object> findDictionary)
        {
            List<FilterDefinition<BsonDocument>> listFilterDefinitions = new List<FilterDefinition<BsonDocument>>() { };
            foreach (KeyValuePair<String, Object> tempPair in findDictionary)
            {
                listFilterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(tempPair.Key, tempPair.Value));
            }
            return Builders<BsonDocument>.Filter.And(listFilterDefinitions);
        }

        /// <summary>
        /// 获取字典中的键值对查询条件对象（大于或等于指定值）
        /// </summary>
        /// <param name="findDictionary"></param>
        /// <returns></returns>
        private FilterDefinition<BsonDocument> GetFilterGreater(Dictionary<String, Object> findDictionary)
        {
            List<FilterDefinition<BsonDocument>> listFilterDefinitions = new List<FilterDefinition<BsonDocument>>() { };
            foreach (KeyValuePair<String, Object> tempPair in findDictionary)
            {
                listFilterDefinitions.Add(Builders<BsonDocument>.Filter.Gte(tempPair.Key, tempPair.Value));
            }
            return Builders<BsonDocument>.Filter.And(listFilterDefinitions);
        }

        /// <summary>
        /// 获取字典中的键值对查询条件对象（小于或等于指定值）
        /// </summary>
        /// <param name="findDictionary"></param>
        /// <returns></returns>
        private FilterDefinition<BsonDocument> GetFilterLess(Dictionary<String, Object> findDictionary)
        {
            List<FilterDefinition<BsonDocument>> listFilterDefinitions = new List<FilterDefinition<BsonDocument>>() { };
            foreach (KeyValuePair<String, Object> tempPair in findDictionary)
            {
                listFilterDefinitions.Add(Builders<BsonDocument>.Filter.Lte(tempPair.Key, tempPair.Value));
            }
            return Builders<BsonDocument>.Filter.And(listFilterDefinitions);
        }

        /// <summary>
        /// 获取字典中的键值对查询条件对象（包含指定值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private FilterDefinition<BsonDocument> GetFilterIn(String key, IEnumerable<Object> values)
        {
            return Builders<BsonDocument>.Filter.In(key, values);
        }

        /// <summary>
        /// 获取字典中的键值对更新数据对象
        /// </summary>
        /// <param name="updateDictionary"></param>
        /// <returns></returns>
        private UpdateDefinition<BsonDocument> GetUpdateDefinition(Dictionary<String, Object> updateDictionary)
        {
            List<UpdateDefinition<BsonDocument>> listUpdateDefinition = new List<UpdateDefinition<BsonDocument>>() { };
            foreach (KeyValuePair<String, Object> tempPair in updateDictionary)
            {
                listUpdateDefinition.Add(Builders<BsonDocument>.Update.Set(tempPair.Key, tempPair.Value));
            }
            return Builders<BsonDocument>.Update.Combine(listUpdateDefinition);
        }

        /// <summary>
        /// 根据指定条件查找数据 Builders<BsonDocument>.Filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<T> FindObject<T>(FilterDefinition<BsonDocument> filter)
        {
            List<T> listResult = null;
            try
            {
                IMongoCollection<BsonDocument> mongoCollection = GetCollection<BsonDocument>();
                List<BsonDocument> findResult = mongoCollection.Find(filter).ToList();
                if (findResult != null)
                {
                    listResult = new List<T>() { };
                    foreach (BsonDocument tempBosnDoc in findResult)
                    {
                        T t = JsonConvert.DeserializeObject<T>(tempBosnDoc.ToJsonNull_Id());
                        listResult.Add(t);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return listResult;
        }

        /// <summary>
        /// 向集合中插入Json字符串
        /// </summary>
        /// <param name="jsonData"></param>
        public void InsertJson(String jsonData)
        {
            IMongoCollection<BsonDocument> mongoCollection = GetCollection<BsonDocument>();
            if (BsonDocument.TryParse(jsonData, out BsonDocument bsonDocument))
            {
                mongoCollection.InsertOne(bsonDocument);
            }
        }

        /// <summary>
        /// 向集合中插入Object对象（需可序列化）
        /// </summary>
        /// <param name="objData"></param>
        public void InsertObject(Object objData)
        {
            InsertJson(JsonConvert.SerializeObject(objData));
        }

        public void InsertByteArry(Byte[] byteArry)
        {
            IMongoCollection<BsonDocument> mongoCollection = GetCollection<BsonDocument>();
            mongoCollection.InsertOne(new BsonDocument("UseData", BsonValue.Create(byteArry)));
        }

        public Byte[] FindOneAndDeleteByteArry()
        {
            IMongoCollection<BsonDocument> mongoCollection = GetCollection<BsonDocument>();
            BsonDocument findResult = mongoCollection.FindOneAndDelete(Builders<BsonDocument>.Filter.Empty);
            Byte[] byteResult = null;
            if (findResult != null && findResult.Elements.Count() > 1)
            {
                byteResult = findResult.ElementAt(1).Value.AsByteArray;
            }
            return byteResult;
        }

        /// <summary>
        /// 根据指定键值查找数据（值完全相等）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<T> FindObjectList<T>(Dictionary<String, Object> findDictionary)
        {
            List<T> listResult = null;
            try
            {
                FilterDefinition<BsonDocument> filter = GetFilterEquals(findDictionary);
                listResult = FindObject<T>(filter);
            }
            catch (Exception)
            {
                throw;
            }
            return listResult;
        }

        /// <summary>
        /// 根据指定键值查找数据（大于或等于指定值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<T> FindObjectGreater<T>(Dictionary<String, Object> findDictionary)
        {
            List<T> listResult = null;
            try
            {
                FilterDefinition<BsonDocument> filter = GetFilterGreater(findDictionary);
                listResult = FindObject<T>(filter);
            }
            catch (Exception)
            {
                throw;
            }
            return listResult;
        }

        /// <summary>
        /// 根据指定键值查找数据（小于或等于指定值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<T> FindObjectLess<T>(Dictionary<String, Object> findDictionary)
        {
            List<T> listResult = null;
            try
            {
                FilterDefinition<BsonDocument> filter = GetFilterLess(findDictionary);
                listResult = FindObject<T>(filter);
            }
            catch (Exception)
            {
                throw;
            }
            return listResult;
        }

        /// <summary>
        /// 根据指定键值查找数据（包含指定值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public List<T> FindObjectIn<T>(String key, IEnumerable<Object> values)
        {
            List<T> listResult = null;
            try
            {
                FilterDefinition<BsonDocument> filter = GetFilterIn(key, values);
                listResult = FindObject<T>(filter);
            }
            catch (Exception)
            {
                throw;
            }
            return listResult;
        }

        /// <summary>
        /// 返回所有结果，然后使用Linq进行筛选
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> FindAllLinq<T>()
        {
            List<T> listResult = null;
            try
            {
                IMongoCollection<BsonDocument> mongoCollection = GetCollection<BsonDocument>();
                List<BsonDocument> findResult = mongoCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList();
                if (findResult != null)
                {
                    listResult = new List<T>() { };
                    foreach (BsonDocument tempBosnDoc in findResult)
                    {
                        T t = JsonConvert.DeserializeObject<T>(tempBosnDoc.ToJsonNull_Id());
                        listResult.Add(t);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return listResult;
        }

        /// <summary>
        /// 根据指定键值查找数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T FindObjectOne<T>(Dictionary<String, Object> findDictionary) where T : class
        {
            T objResult = default(T);
            try
            {
                List<T> listResult = FindObjectList<T>(findDictionary);
                if (listResult != null && listResult.Count > 0)
                {
                    objResult = listResult[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objResult;
        }

        /// <summary>
        /// 根据指定键值查找数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public String FindJsonOne(Dictionary<String, Object> findDictionary)
        {
            String jsonResult = null;
            try
            {
                Object objResult = FindObjectOne<Object>(findDictionary);
                if (objResult != null)
                {
                    jsonResult = JsonConvert.SerializeObject(objResult);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return jsonResult;
        }

        /// <summary>
        /// 删除指定键值, 并返回受影响的行数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Int64 DeleteMany(Dictionary<String, Object> findDictionary)
        {
            IMongoCollection<BsonDocument> mongoCollection = GetCollection<BsonDocument>();
            FilterDefinition<BsonDocument> filter = GetFilterEquals(findDictionary);
            DeleteResult deleteResult = mongoCollection.DeleteMany(filter);
            return deleteResult.DeletedCount;
        }

        public Int64 UpdateMany(Dictionary<String, Object> findDic, Dictionary<String, Object> updateDic)
        {
            if (updateDic.Count <= 0) return 0;
            IMongoCollection<BsonDocument> mongoCollection = GetCollection<BsonDocument>();
            FilterDefinition<BsonDocument> filter = GetFilterEquals(findDic);
            UpdateDefinition<BsonDocument> updateDefinition = GetUpdateDefinition(updateDic);
            UpdateResult updateResult = mongoCollection.UpdateMany(filter, updateDefinition);
            return updateResult.ModifiedCount;
        }
    }
}
