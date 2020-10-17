using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryModels
{
    public class Genre : FullAuditModel
    {
        [Required]
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; }
        public virtual List<ItemGenre> GenreItems { get; set; } = new List<ItemGenre>();
    }
}
