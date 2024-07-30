using AutoMapper;

using RestItla.Application.DTO.Dish;
using RestItla.Application.DTO.Ingredient;
using RestItla.Application.DTO.Order;
using RestItla.Application.DTO.Table;
using RestItla.Domain.Common;
using RestItla.Domain.Entities;

namespace RestItla.Application.Mappins
{
    internal class GeneralProfile
    : Profile
    {
        public GeneralProfile()
        {
            CreateMap<DishSaveDTO, Dish>()
                    .ForMember(x => x.Ingredients, o => o.Ignore())
                    .ForMember(x => x.Id, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.Ignore())
                    .ForMember(x => x.DeletedAt, o => o.Ignore())
                    .ForMember(x => x.ModifiedAt, o => o.Ignore());
            CreateMap<IngredientSaveDTO, Ingredient>()
                    .ForMember(x => x.Id, o => o.Ignore())
                    .ForMember(x => x.Dishes, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.Ignore())
                    .ForMember(x => x.DeletedAt, o => o.Ignore())
                    .ForMember(x => x.ModifiedAt, o => o.Ignore());
            CreateMap<OrderSaveDTO, Order>()
                    .ForMember(x => x.SelectedDishes, o => o.Ignore())
                    .ForMember(x => x.Id, o => o.Ignore())
                    .ForMember(x => x.Table, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.Ignore())
                    .ForMember(x => x.DeletedAt, o => o.Ignore())
                    .ForMember(x => x.ModifiedAt, o => o.Ignore());
            CreateMap<TableSaveDTO, Table>()
                    .ForMember(x => x.Id, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.Ignore())
                    .ForMember(x => x.DeletedAt, o => o.Ignore())
                    .ForMember(x => x.ModifiedAt, o => o.Ignore());

            CreateMap<Dish, DishSaveResponseDTO>();
            CreateMap<Ingredient, IngredientSaveResponseDTO>();
            CreateMap<Order, OrderSaveResponseDTO>();
            CreateMap<Table, TableSaveResponseDTO>();


            CreateMap<TableUpdateDTO, Table>()
                    .ForMember(x => x.Id, o => o.Ignore())
                    .ForMember(x => x.Status, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.Ignore())
                    .ForMember(x => x.DeletedAt, o => o.Ignore())
                    .ForMember(x => x.ModifiedAt, o => o.Ignore());

            // CreateMap<OrderUpdateDTO, Order>()
            //         .ConvertUsing(_ => new Order());

            CreateMap<Table, TableUpdateDTO>();
            // CreateMap<Order, OrderUpdateDTO>();
            CreateMap<Order, OrderUpdateResponseDTO>();
            CreateMap<Dish, DishUpdateResponseDTO>();

            CreateMap<Dish, DishDTO>();
            CreateMap<Ingredient, IngredientDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<Table, TableDTO>();

            CreateMap<Order, TableOrdersDTO>();

            CreateMap<Guid, Ingredient>().ConvertUsing(x => new Ingredient { Id = x });
            CreateMap<Guid, Dish>().ConvertUsing(x => new Dish { Id = x });
            CreateMap<Guid, Order>().ConvertUsing(x => new Order { Id = x });
            CreateMap<Guid, Table>().ConvertUsing(x => new Table { Id = x });

            CreateMap<Entity, Guid>().ConvertUsing(x => x.Id);
        }
    }
}