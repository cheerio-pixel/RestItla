namespace RestItla.Application.DTO.Dish
{
    public class DishUpdateResponseDTO
    : DishSaveDTO
    {
        public Guid Id { get; set; }
    }
}