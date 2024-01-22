using Common.Models;
using DataAccess.Entities;
using MongoDB.Driver;

namespace DataAccess.Services;

public class CategoryRepository : RepositoryBase
{
    
    public async Task<List<CategoryModel>> GetAllCategories()
    {
        var collection = ConnectToMongo<CategoryEntity>(CategoryCollections);

        var filter = Builders<CategoryEntity>.Filter.Empty;

        var results = await collection.FindAsync(filter);

        var mappedResults = results
            .ToList()
            .Select(MapToModel)
            .OrderBy(category => category.Name)
            .ToList();

        return mappedResults;
    }

    public Task CreateCategories(IEnumerable<CategoryModel> categories)
    {
        var collection = ConnectToMongo<CategoryEntity>(CategoryCollections);

        var mappedCategories = categories.Select(MapToEntity);

        return collection.InsertManyAsync(mappedCategories);
    }


    private CategoryModel MapToModel(CategoryEntity entity)
    {
        var model = new CategoryModel
        {
            Name = entity.Name
        };

        return model;
    }

    private CategoryEntity MapToEntity(CategoryModel model)
    {
        var entity = new CategoryEntity
        {
            Name = model.Name
        };

        return entity;
    }
}