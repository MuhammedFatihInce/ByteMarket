using AutoMapper;
using AutoMapper.QueryableExtensions;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Category;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Category;
using ByteMarket.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class CategoryManager : ICategoryService
	{
		private readonly ICategoryWriteRepository _categoryWriteRepository;
		private readonly ICategoryReadRepository _categoryReadRepository;
		private readonly IMapper _mapper;
		private readonly ICategoryImageFileService _categoryImageFileService;

		public CategoryManager(ICategoryWriteRepository categoryWriteRepository, IMapper mapper, ICategoryReadRepository categoryReadRepository, ICategoryImageFileService categoryImageFileService)
		{
			_categoryWriteRepository = categoryWriteRepository;
			_mapper = mapper;
			_categoryReadRepository = categoryReadRepository;
			_categoryImageFileService = categoryImageFileService;
		}
		public async Task<IDataResult<string>> AddAsync(CreateCategoryDto categoryDto)
		{
			var category = _mapper.Map<Category>(categoryDto);

			var result = await _categoryWriteRepository.AddAsync(category);

			if (result)
			{
				await _categoryWriteRepository.SaveAsync();
				return new SuccessDataResult<string>(category.Id.ToString(),"Kategori başarılıyla eklendi.");
			}

			return new ErrorDataResult<string>("Kategori eklenirken bir hata oluştu.");
		}

		public async Task<IDataResult<List<ListCategoryDto>>> GetAllAsync()
		{
			var categoryDtos = await _categoryReadRepository.GetAll(false)
				.ProjectTo<ListCategoryDto>(_mapper.ConfigurationProvider)
				.ToListAsync();

			if (categoryDtos == null || !categoryDtos.Any())
			{
				return new ErrorDataResult<List<ListCategoryDto>>("Listelenecek kategori bulunamadı.");
			}

			return new SuccessDataResult<List<ListCategoryDto>>(categoryDtos, "Kategoriler başarıyla listelendi.");
		}

		public async Task<IResult> DeleteAsync(string id)
		{
			await _categoryImageFileService.DeleteImageByCategoryIdAsync(id);
			await _categoryWriteRepository.RemoveAsync(id);
			await _categoryWriteRepository.SaveAsync();

			return new SuccessResult("Kategori başarıyla silindi.");
		}

		public async Task<IResult> UpdateAsync(UpdateCategoryDto updateCategoryDto)
		{
			var category = await _categoryReadRepository.GetByIdAsync(updateCategoryDto.Id);

			if (category == null) return new ErrorResult("Kategori bulunamadı.");

			_mapper.Map(updateCategoryDto, category);

			_categoryWriteRepository.Update(category);
			await _categoryWriteRepository.SaveAsync();

			return new SuccessResult("Kategori bilgileri güncellendi.");
		}

		public async Task<IDataResult<SingleCategoryDto>> GetByIdAsync(string id)
		{
			var category = await _categoryReadRepository.GetAll(false)
				.Include(c => c.Products)
				.Include(c=> c.CategoryImageFile)
				.FirstOrDefaultAsync(c => c.Id == Guid.Parse(id));

			if (category == null)
			{
				return new ErrorDataResult<SingleCategoryDto>("Kategori bulunamadı.");
			}

			var categoryDto = _mapper.Map<SingleCategoryDto>(category);
			return new SuccessDataResult<SingleCategoryDto>(categoryDto, "Kategori başarıyla getirildi.");
		}

	}
}
