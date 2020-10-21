namespace InventoryModels.Dtos
{
    public class GetItemsForListingDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Notes { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = true;
        public string CategoryName { get; set; } = "";
    }
}
