using System.Linq.Expressions;
using Foodi.UserServiceProject.Models.ResponseModels;
using HomeProject.Constants;
using HomeProject.Models.Domain;
using HomeProject.Models.Request.Profile;
using HomeProject.Repositories.ProfileRepository;

namespace HomeProject.Services.ProfileService
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<ResponseModel<object>> Create(ProfileInDto request)
        {
            try
            {
                if (request == null)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = StringResources.BadRequest
                    };
                }

                var imageBytes = new byte[0];

                if (!(request.Image == null || request.Image.Length == 0))
                {
                    using var ms = new MemoryStream();
                    await request.Image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }


                await _profileRepository.AddAsync(new ProfileModel
                {
                    Name = request.Name,
                    Rating = request.Rating,
                    ProfileUrl = request.ProfileUrl,
                    Image = request.Image?.FileName,
                    ImageFile = imageBytes.Length > 1 ? imageBytes : null
                });
                await _profileRepository.CompleteAsync();

                return new ResponseModel<object>
                {
                    Status = true,
                    Message = StringResources.CreateSuccess
                };
            }
            catch (Exception ex)
            {
                throw new Exception(StringResources.InternalServerError, ex);
            }
        }

        public async Task<ResponseModel<object>> Update(int id, ProfileUpdateDto request)
        {
            try
            {
                var profile = await _profileRepository.GetAsync(id);
                
                if (profile == null)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = StringResources.NotFound
                    };
                }

                var imageBytes = new byte[0];

                if (request.IsNew && !(request.Image == null || request.Image.Length == 0))
                {
                    using var ms = new MemoryStream();
                    await request.Image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                profile.Name = request.Name;
                profile.Rating = request.Rating;
                profile.ProfileUrl = request.ProfileUrl;
                profile.Image = imageBytes.Length > 1 ? request.Image?.FileName : profile.Image;
                profile.ImageFile = imageBytes.Length > 1 ? imageBytes : profile.ImageFile;
                profile.UpdatedAt = DateTime.UtcNow;

                await _profileRepository.UpdateAsync(profile);
                await _profileRepository.CompleteAsync();

                return new ResponseModel<object>
                {
                    Status = true,
                    Message = StringResources.UpdateSuccess
                };
            }
            catch (Exception ex)
            {
                throw new Exception(StringResources.InternalServerError, ex);
            }
        }

        public async Task<ResponseModel<object>> Delete(int id)
        {
            try
            {
                var profile = await _profileRepository.GetAsync(id);

                if (profile == null)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = StringResources.NotFound
                    };
                }

                profile.IsActive = false;
                profile.UpdatedAt = DateTime.UtcNow;

                await _profileRepository.UpdateAsync(profile);
                await _profileRepository.CompleteAsync();

                return new ResponseModel<object>
                {
                    Status = true,
                    Message = StringResources.UpdateSuccess
                };
            }
            catch (Exception ex)
            {
                throw new Exception(StringResources.InternalServerError, ex);
            }
        }

        public async Task<ResponseModel<ProfileModel?>> GetById(int id)
        {
            try
            {
                var profile = await _profileRepository.GetAsync(id);

                if (profile == null)
                {
                    return new ResponseModel<ProfileModel?>
                    {
                        Status = false,
                        Message = StringResources.NotFound
                    };
                }

                return new ResponseModel<ProfileModel?>
                {
                    Status = true,
                    Message = StringResources.Success,
                    Data = profile
                };
            }
            catch (Exception ex)
            {
                throw new Exception(StringResources.InternalServerError, ex);
            }
        }

        public async Task<PaginatedResponseModel<ProfileModel>> Get(ProfileFilter request)
        {
            try
            {
                var paginatedResponseModel = new PaginatedResponseModel<ProfileModel>();

                    var filters = new List<Expression<Func<ProfileModel, bool>>>();

                filters.Add(x => x.IsActive == request.IsActive);

                if (request.Name != null)
                {
                    filters.Add(x => x.Name.ToLower().Contains(request.Name.ToLower()));
                }
                if (request.Rating != null)
                {
                    filters.Add(x => x.Rating >= request.Rating);
                }

                var findAll = await _profileRepository.GetAllAsync(filters, GetOrderBy(), (request.PageNumber - 1) * request.ItemsPerPage, request.ItemsPerPage);
                paginatedResponseModel.Items = findAll.ToList();
                paginatedResponseModel.TotalItems = await _profileRepository.CountAsync(filters);
                paginatedResponseModel.TotalPages = (int)Math.Ceiling((double)paginatedResponseModel.TotalItems / request.ItemsPerPage);

                paginatedResponseModel.PageNumber = request.PageNumber;
                paginatedResponseModel.ItemsPerPage = request.ItemsPerPage;

                return paginatedResponseModel;
            }
            catch (Exception ex)
            {
                throw new Exception(StringResources.InternalServerError, ex);
            }
        }

        private static Func<IQueryable<ProfileModel>, IOrderedQueryable<ProfileModel>> GetOrderBy()
        {
            Func<IQueryable<ProfileModel>, IOrderedQueryable<ProfileModel>>? orderBy = null;
            orderBy = models => models.OrderByDescending(ss => ss.Rating);
            return orderBy;
        }
    }
}
