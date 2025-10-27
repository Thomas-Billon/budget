using Budget.Server.Api.Categories.Models.Requests;
using System.Diagnostics.CodeAnalysis;

namespace Budget.Server.Core.Categories
{
    public class CategoryCreateParameters
    {
        public required CategoryCreateRequest Request { get; set; }

        public bool IsParentCategoryValid { get; set; } = false;


        public CategoryCreateParameters() { }

        [SetsRequiredMembers]
        public CategoryCreateParameters(CategoryCreateRequest request, bool isParentCategoryValid)
        {
            Request = request;
            IsParentCategoryValid = isParentCategoryValid;
        }
    }

    public class CategoryUpdateParameters
    {
        public required CategoryUpdateRequest Request { get; set; }

        public bool IsParentCategoryValid { get; set; } = false;


        public CategoryUpdateParameters() { }

        [SetsRequiredMembers]
        public CategoryUpdateParameters(CategoryUpdateRequest request, bool isParentCategoryValid)
        {
            Request = request;
            IsParentCategoryValid = isParentCategoryValid;
        }
    }

    public class CategoryPatchParameters
    {
        public required CategoryPatchRequest Request { get; set; }

        public bool IsParentCategoryValid { get; set; } = false;


        public CategoryPatchParameters() { }

        [SetsRequiredMembers]
        public CategoryPatchParameters(CategoryPatchRequest request, bool isParentCategoryValid)
        {
            Request = request;
            IsParentCategoryValid = isParentCategoryValid;
        }
    }
}
