using RestItla.Application.DTO.Dish;
using RestItla.Domain.Entities;

namespace RestItla.Application.Interfaces.Services
{
    public interface IDishService
    : IGenericService<DishSaveDTO, DishDTO, DishUpdateDTO, DishSaveResponseDTO, DishUpdateResponseDTO, Dish, Guid>
    {
    }
}