using Common.Models;
using DataAccess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Services;

public class QuestionRepository : RepositoryBase
{
    public async Task<List<QuestionModel>> GetAllQuestions()
    {
        var collection = ConnectToMongo<QuestionEntity>(QuestionCollections);

        var filter = Builders<QuestionEntity>.Filter.Empty;

        var result = await collection.FindAsync(filter);

        var mappedResults = result.ToList().Select(MapToModel).ToList();

        return mappedResults;
    }


    public async Task<QuestionModel> GetQuestionById(string id)
    {
        var collection = ConnectToMongo<QuestionEntity>(QuestionCollections);

        var filter = Builders<QuestionEntity>.Filter.Eq("Id", id);

        var question = await collection.Find(filter).FirstOrDefaultAsync();

        return MapToModel(question);
    }

    //Obsolete?
    //public async Task<QuestionModel> GetQuestionByIdFromQuiz(string quizId, string questionId, string questionText)
    //{
    //    var quizCollection = ConnectToMongo<QuizEntity>(QuizCollections);

    //    var filter = Builders<QuizEntity>.Filter.Eq("Id", quizId);

    //    var quizResult = await  quizCollection.FindAsync(filter);

    //    var question = quizResult
    //        .ToList()
    //        .First()
    //        .Questions
    //        .First(q => q.Id == questionId && q.QuestionText == questionText);

    //    return MapToModel(question);
    //}

    public async Task<string> CreateQuestion(QuestionModel model)
    {
        var collection = ConnectToMongo<QuestionEntity>(QuestionCollections);

        var entity = MapToEntity(model);

        await collection.InsertOneAsync(entity);

        return entity.Id;

    }

    public Task UpdateQuestion(QuestionModel model)
    {
        var collection = ConnectToMongo<QuestionEntity>(QuestionCollections);

        var filter = Builders<QuestionEntity>.Filter.Eq("Id", model.Id);

        var entity = MapToEntity(model);

        return collection.ReplaceOneAsync(filter, entity);
    }

    public Task DeleteQuestion(string id)
    {
        var collection = ConnectToMongo<QuestionEntity>(QuestionCollections);

        var filter = Builders<QuestionEntity>.Filter.Eq("Id", id);

        return collection.DeleteOneAsync(filter);
    }


    private QuestionModel MapToModel(QuestionEntity entity)
    {
        var model = new QuestionModel()
        {
            Id = entity.Id,
            QuestionText = entity.QuestionText,
            FirstAlternative = entity.FirstAlternative,
            SecondAlternative = entity.SecondAlternative,
            ThirdAlternative = entity.ThirdAlternative,
            Categories = entity.Categories
                .Select(c => new CategoryModel()
                {
                    Name = c.Name
                }).ToList()
        };

        return model;
    }
    private QuestionEntity MapToEntity(QuestionModel model)
    {
        var entity = new QuestionEntity()
        {
            Id = model.Id,
            QuestionText = model.QuestionText,
            FirstAlternative = model.FirstAlternative,
            SecondAlternative = model.SecondAlternative,
            ThirdAlternative = model.ThirdAlternative,
            Categories = model.Categories
                .Select(c => new CategoryEntity()
                {
                    Name = c.Name
                }).ToList()
        };

        return entity;
    }
}