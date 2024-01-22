using Common.Models;

namespace JoeQuizApp.Comparer;

public class CategoryEqualityComparer : IEqualityComparer<CategoryModel>
{
    public bool Equals(CategoryModel? x, CategoryModel? y)
    {
        return x.Name == y.Name;
    }

    public int GetHashCode(CategoryModel obj)
    {
        return obj.Name.GetHashCode();
    }
}