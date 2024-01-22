using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataAccess.Entities;

public class QuestionEntity : EntityBase
{
    public string QuestionText { get; set; }
    public (string answerText, bool isCorrect) FirstAlternative { get; set; }
    public (string answerText, bool isCorrect) SecondAlternative { get; set; }
    public (string answerText, bool isCorrect) ThirdAlternative { get; set; }
    public List<CategoryEntity> Categories { get; set; } = new();

}