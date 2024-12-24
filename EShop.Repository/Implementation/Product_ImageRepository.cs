using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.ViewModel;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EShop.Repository.Implementation
{
    public class Product_ImageRepository : Repository<Product_Image, Product_ImageViewModel, EShopContext>, IProduct_ImageRepository
    {
        private readonly IUnitOfWork<EShopContext> _unitOfWork;
        private readonly DbSet<Image> _imageService;
        public Product_ImageRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _imageService = _unitOfWork.Set<Image>();
        }

        public async Task<IEnumerable<Product_ImageViewModel>> GetPaginatedResult(Int64? productId, Int64? productOptionId)
        {
            IEnumerable<Product_ImageViewModel> result = [];

            try
            {
                var sparam = new SqlParameter[] {
                    new SqlParameter("@ProductId", productId == null? DBNull.Value :productId ),
                    new SqlParameter("@ProductOptionId", productOptionId == null? DBNull.Value :productOptionId)
                };

                var r = await GetProcedureAsync<Product_ImageViewModel>("Product_Image_Get", sparam);

                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }
        public async Task<Product_ImageViewModel> InsertUpdateAsync(Product_ImageViewModel model)
        {
            var result = new Product_ImageViewModel();
            try
            {
                result = model;

                if (!string.IsNullOrWhiteSpace(model.ImageUrl))
                {
                    var imageEntity = new Image
                    {
                        AltText = model.ImageAlt,
                        Url = model.ImageUrl,
                        CreateDate = DateTime.Now,
                        ModifiedBy = model.ModifiedBy,
                        Confirmed = true
                    };

                    _imageService.Add(imageEntity);
                    if (await _unitOfWork.SaveAsync() > 0)
                    {
                        model.ImageId = imageEntity.Id;
                        if (model.Id > 0)
                        {
                            return await UpdateAsync(model);
                        }
                        else
                        {
                            return await AddAsync(model);
                        }
                    }
                }
                else if (model.Id.HasValue)
                {
                    var old = await GetByIdAsync(model.Id.Value);
                    model.ImageId = old?.ImageId;
                    return await UpdateAsync(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}
