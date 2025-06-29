using System.Collections.Generic;
using System.Linq;
using ShopServices.Core.Models;

namespace GoodsGroups.API
{
    internal static class CategoryMapper
    {
        internal static Contracts.Category GetCategoryDto(Category coreCategory)
        {
            return new Contracts.Category
            {
                Id = coreCategory.Id,
                Name = coreCategory.Name,
                Brand = coreCategory.Brand,
                Params = coreCategory.Params,
                Url = coreCategory.Url,
                ImageUrl = coreCategory.ImageUrl,
                Created = coreCategory.Created!.Value,
                Updated = coreCategory.Updated,
                Archived = coreCategory.Archived
            };
        }

        internal static Category GetCoreCategory(Contracts.Category categoryDto)
        {
            return new Category
            (
                id: categoryDto.Id,
                name: categoryDto.Name.Trim(),
                brand: categoryDto.Brand?.Trim(),
                parameters: categoryDto.Params,
                url: categoryDto.Url,
                imageUrl: categoryDto.ImageUrl,
                archived: categoryDto.Archived
            );
        }

        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Category) - IEnumerable(Contracts.Category)
        /// </summary>
        /// <param name="categoriesList"> список категорий IEnumerable(Core.Models.Category) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Category> GetCategoriesDtos(this IEnumerable<Category> categoriesList)
        {
            return categoriesList.Select(coreCategory => new Contracts.Category
            {
                Id = coreCategory.Id,
                Brand = coreCategory.Brand,
                Name = coreCategory.Name,
                Params = coreCategory.Params,
                Url = coreCategory.Url,
                ImageUrl = coreCategory.ImageUrl,
                Created = coreCategory.Created!.Value,
                Updated = coreCategory.Updated,
                Archived = coreCategory.Archived
            });
        }
    }
}