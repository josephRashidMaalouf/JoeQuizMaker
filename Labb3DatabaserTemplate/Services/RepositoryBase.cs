using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class RepositoryBase
    {
        protected const string ConnectionString = "mongodb://localhost:27017";
        protected const string DataBaseName = "JosephLabb3QuizDb";
        protected const string QuizCollections = "Quizzes";
        protected const string QuestionCollections = "Questions";
        protected const string CategoryCollections = "Categories";
        protected IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DataBaseName);
            return db.GetCollection<T>(collection);
        }
    }
}
