using Common.Models;
using DataAccess.Entities;
using MongoDB.Driver;
using System;

namespace DataAccess.Services;

public class QuizRepository : RepositoryBase
{
    public async Task<List<QuizModel>> GetAllQuizzes()
    {
        var quizCollection = ConnectToMongo<QuizEntity>(QuizCollections);

        var filter = Builders<QuizEntity>.Filter.Empty;

        var results = await quizCollection.FindAsync(filter);

        var mappedResults = results.ToList().Select(MapToModel).ToList();

        return mappedResults;
    }

    public async Task<QuizModel> GetQuizById(string id)
    {
        var quizCollection = ConnectToMongo<QuizEntity>(QuizCollections);

        var filter = Builders<QuizEntity>.Filter.Eq("Id", id);

        var entity = await quizCollection.Find(filter).FirstOrDefaultAsync();

        return MapToModel(entity);
    }

    public async Task<string> CreateQuiz(QuizModel quizModel)
    {
        var quiz = new QuizEntity
        {
            Title = quizModel.Title,
            Description = quizModel.Description
        };

        var quizCollection = ConnectToMongo<QuizEntity>(QuizCollections);

        await quizCollection.InsertOneAsync(quiz);

        return quiz.Id;
    }

    public Task UpdateQuiz(QuizModel quizModel)
    {
        var quizCollection = ConnectToMongo<QuizEntity>(QuizCollections);

        var quizEntity = MapToEntity(quizModel);

        var filter = Builders<QuizEntity>.Filter.Eq("Id", quizEntity.Id);
        var replaceOptions = new ReplaceOptions { IsUpsert = true };

        return quizCollection.ReplaceOneAsync(filter, quizEntity, replaceOptions);
    }

    

    public Task DeleteQuiz(QuizModel quizModel)
    {
        var quizCollection = ConnectToMongo<QuizEntity>(QuizCollections);

        var filter = Builders<QuizEntity>.Filter.Eq("Id", quizModel.Id);

        return quizCollection.DeleteOneAsync(filter);
    }

    private QuizModel MapToModel(QuizEntity entity)
    {
        var model = new QuizModel()
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Questions = entity.Questions
                .Select(x => new QuestionModel()
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    FirstAlternative = x.FirstAlternative,
                    SecondAlternative = x.SecondAlternative,
                    ThirdAlternative = x.ThirdAlternative,
                    Categories = x.Categories
                        .Select(c => new CategoryModel()
                        {
                            Name = c.Name
                        }).ToList()
                }).ToList()
        };

        return model;
    }
    private QuizEntity MapToEntity(QuizModel quizModel)
    {
        var quizEntity = new QuizEntity()
        {
            Id = quizModel.Id,
            Title = quizModel.Title,
            Description = quizModel.Description,
            Questions = quizModel.Questions
                .Select(q => new QuestionEntity()
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    FirstAlternative = q.FirstAlternative,
                    SecondAlternative = q.SecondAlternative,
                    ThirdAlternative = q.ThirdAlternative,
                    Categories = q.Categories
                        .Select(c => new CategoryEntity()
                        {
                            Name = c.Name
                        }).ToList()
                }).ToList()
        };

        return quizEntity;
    }
}