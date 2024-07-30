namespace RestItla.Domain.Entities
{
    public class DishIngredient
    {
        public Guid DishId { get; set; }
        public Guid IngredientId { get; set; }
    }
}