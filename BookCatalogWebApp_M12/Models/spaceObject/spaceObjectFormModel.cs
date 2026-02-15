using System.ComponentModel.DataAnnotations;
using static BookCatalogWebApp_M12.Data.DataConstants.spaceObject;

namespace BookCatalogWebApp_M12.Models.Book
{
    /// <summary>
    /// Модел за формата при създаване или редактиране на книга.
    /// Включва валидационни съобщения.
    /// </summary>
    public class spaceObjectFormModel
    {
        /// <summary> Заглавие с изискване за дължина. </summary>
        [Required(ErrorMessage = "Заглавието е задължително.")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,
          ErrorMessage = "Заглавието трябва да е между {2} и {1} символа.")]
        public string Name { get; set; } = null!;
        /// <summary> Описание с минимална дължина за по-добра информативност. </summary>
        [Required(ErrorMessage = "Описанието е задължително.")]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessage = "Описанието трябва да е поне {2} символа.")]
        public string Description { get; set; } = null!;

        /// <summary> Избраната категория от падащото меню. </summary>
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        /// <summary> Списък от налични категории за попълване на падащото меню. </summary>
        public IEnumerable<spaceObjectCategoryViewModel> Categories { get; set; } = new List<spaceObjectCategoryViewModel>();

        /// <summary> Текстово поле за въвеждане на име на автор. </summary>
        [Display(Name = "Author Name")]
        public string FounderName { get; set; } = null!;
    }
}
