using AutoMapper;

using RestItla.Application.DTO.Dish;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Repositories;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Entities;
using RestItla.Domain.Enum;

namespace RestItla.Application.Sevices
{
    internal class DishService
    : GenericService<DishSaveDTO, DishDTO, DishUpdateDTO, DishSaveResponseDTO, DishUpdateResponseDTO, Dish, Guid>,
      IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;
        private readonly IDishIngredientRepository _dishIngredientRepository;

        protected override string[] PropertiesToInclude => new string[] { "Ingredients" };

        public DishService(IDishRepository dishRepository,
                           IMapper mapper,
                           IDishIngredientRepository dishIngredientRepository)
        : base(dishRepository, mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
            _dishIngredientRepository = dishIngredientRepository;
        }

        public override async Task<DishSaveResponseDTO> Add(DishSaveDTO vm)
        {
            DishSaveResponseDTO dishSaveResponseDTO = await base.Add(vm);
            IEnumerable<DishIngredient> dishIngredients = vm.Ingredients.Select(uid => new DishIngredient()
            {
                DishId = dishSaveResponseDTO.Id,
                IngredientId = uid
            });

            await _dishIngredientRepository.Create(dishIngredients.ToArray());
            return dishSaveResponseDTO;
        }

        public override async Task<Result<DishUpdateResponseDTO>> Update(DishUpdateDTO vm, Guid id)
        {
            // I would spend more time refactoring for code deduplication
            // that may only happen this two times.
            if (await _dishRepository.IdExists(id))
            {
                HashSet<Guid> nonExistantGuids = await _dishRepository.CheckIds(vm.IngredientsToAdd);
                if (nonExistantGuids.Any())
                {
                    return ErrorType.NotFound
                                    .Because($"{string.Join(", ", nonExistantGuids)} do not exist");
                }
                var dishOrdersToRemove = vm.IngredientsToRemove.Select(ToDishIngredient).ToArray();
                var dishOrdersToAdd = vm.IngredientsToAdd.Select(ToDishIngredient).ToArray();

                await _dishIngredientRepository.Create(dishOrdersToAdd);
                await _dishIngredientRepository.Remove(dishOrdersToRemove);

                Dish? dish = await _dishRepository.GetByIdWith(id, PropertiesToInclude);
                if (dish is null)
                {
                    return ErrorType.NotFound
                                    .Because("Cannot find this resource.");
                }

                return _mapper.Map<DishUpdateResponseDTO>(
                    dish
                );
            }

            return ErrorType.NotFound
                            .Because("Cannot find this resource.");

            DishIngredient ToDishIngredient(Guid d)
            {
                return new DishIngredient()
                {
                    DishId = id,
                    IngredientId = d
                };
            }
        }
    }
}