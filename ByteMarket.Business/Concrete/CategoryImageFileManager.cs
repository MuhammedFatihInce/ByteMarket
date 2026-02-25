
using ByteMarket.Business.Abstract;
using ByteMarket.Business.Abstract.Storage;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Category;
using ByteMarket.DataAccess.Abstract.CategoryImageFile;
using ByteMarket.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class CategoryImageFileManager: ICategoryImageFileService
	{
		private readonly IStorageService _storageService;
		private readonly ICategoryReadRepository _categoryReadRepository;
		private readonly ICategoryImageFileWriteRepository _categoryImageWriteRepository;
		private readonly ICategoryImageFileReadRepository _categoryImageReadRepository;
		public CategoryImageFileManager(IStorageService storageService, ICategoryReadRepository categoryReadRepository, ICategoryImageFileWriteRepository categoryImageFileWriteRepository, ICategoryImageFileReadRepository categoryImageReadRepository)
		{
			_storageService = storageService;
			_categoryReadRepository = categoryReadRepository;
			_categoryImageWriteRepository = categoryImageFileWriteRepository;
			_categoryImageReadRepository = categoryImageReadRepository;
		}

		public async Task<IResult> AddImageAsync(string categoryId, IFormFile file)
		{
			List<(string fileName, string pathOrContainerName)> fileResult = await _storageService.UploadAsync("photo-images-category", new FormFileCollection { file });

			var category = await _categoryReadRepository.GetByIdAsync(categoryId);

			var upload = fileResult.First();

			var categoryImage = new CategoryImageFile
			{
				FileName = upload.fileName,
				Path = upload.pathOrContainerName,
				Storage = _storageService.StorageName,
				Category = category 
			};

			var result = await _categoryImageWriteRepository.AddAsync(categoryImage);

			if (result)
			{
				await _categoryImageWriteRepository.SaveAsync();
				return new SuccessResult("Kategori görselleri başarıyla yüklendi.");
			}

			return new ErrorResult("Kategori görselleri yüklenirken bir hata oluştu.");
		}

		public async Task<IResult> DeleteImageAsync(string imageId)
		{
			var categoryImage = await _categoryImageReadRepository.GetWhere(x => x.Id == Guid.Parse(imageId))
				.Include(x => x.Category) 
				.FirstOrDefaultAsync();

			if (categoryImage.Category != null)
			{
				categoryImage.Category.CategoryImageFile = null;
			}

			await _storageService.DeleteAsync(categoryImage.Path, categoryImage.FileName);

			await _categoryImageWriteRepository.RemoveAsync(imageId);
			await _categoryImageWriteRepository.SaveAsync();

			return new SuccessResult("Resim hem diskten hem veritabanından başarıyla silindi.");
		}

		public async Task<IResult> DeleteImageByCategoryIdAsync(string categoryId)
		{
			var category = await _categoryReadRepository.GetAll(false)
				.Include(c=>c.CategoryImageFile)
				.FirstOrDefaultAsync(c => c.Id == Guid.Parse(categoryId));

			if (category == null)
				return new ErrorResult("Kategori bulunamadı.");

			if (category.CategoryImageFile != null)
			{
				await _storageService.DeleteAsync(category.CategoryImageFile.Path, category.CategoryImageFile.FileName);
				_categoryImageWriteRepository.Remove(category.CategoryImageFile);
			}

			await _categoryImageWriteRepository.SaveAsync();
			return new SuccessResult();
		}
	}
}
