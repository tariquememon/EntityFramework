using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryModels
{
    public class CategoryColor : IIdentityModel
    {
        [Key, ForeignKey("Category")]
        [Required]
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [StringLength(InventoryModelsConstants.MAX_COLORVALUE_LENGTH)]
        public string ColorValue { get; set; }
        public virtual Category Category { get; set; }
    }
}
