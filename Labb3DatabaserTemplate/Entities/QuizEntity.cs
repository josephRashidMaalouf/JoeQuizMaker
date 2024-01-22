using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Entities;

public class QuizEntity : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<QuestionEntity> Questions { get; set; } = new();
}