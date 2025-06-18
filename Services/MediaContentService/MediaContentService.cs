using HomeProject.Models.Response;
using HomeProject.Models.Request.MediaContent;
using HomeProject.Constants;
using HomeProject.Models.Domain;
using HomeProject.Repositories.MediaContentRepository;
using FFMpegCore;
using HomeProject.Models.Response.MediaContent;
using Microsoft.EntityFrameworkCore;
using HomeProject.Repositories.ProfileRepository;

namespace HomeProject.Services.MediaContentService
{
    public class MediaContentService : IMediaContentService
    {
        private readonly IMediaContentRepository _mediaContentRepository;
        private readonly IProfileRepository _profileRepository;

        public MediaContentService(
            IMediaContentRepository mediaContentRepository,
            IProfileRepository profileRepository
            )
        {
            _mediaContentRepository = mediaContentRepository;
            _profileRepository = profileRepository;
        }

        public async Task<ResponseModel<List<ContentPreviewListDto>>> GetContents()
        {
            try
            {
                var query = _mediaContentRepository.GetAllQueryable(x => x.IsActive, y => y.Profile!);
                var mediaContents = await query.Select(x => new ContentPreviewListDto
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    ProfileName = x.Profile!.Name,
                    FileName = x.FileName,
                    ContentType = /*x.ContentType*/"video/mp4",
                    PreviewData = x.PreviewData,
                    IsFavourite = x.IsFavourite
                }).ToListAsync();

                return new ResponseModel<List<ContentPreviewListDto>>
                {
                    Status = true,
                    Message = StringResources.Success,
                    Data = mediaContents
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ContentPreviewListDto>>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<List<ContentPreviewListDto>>> GetContentsByProfileId(int profileId)
        {
            try
            {
                var query = _mediaContentRepository.GetAllQueryable(x => x.ProfileId == profileId, y => y.Profile!);
                var mediaContents = await query.Select(x => new ContentPreviewListDto
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    ProfileName = x.Profile!.Name,
                    FileName = x.FileName,
                    ContentType = /*x.ContentType*/"video/mp4",
                    PreviewData = x.PreviewData,
                    IsFavourite = x.IsFavourite
                }).ToListAsync();

                return new ResponseModel<List<ContentPreviewListDto>>
                {
                    Status = true,
                    Message = StringResources.Success,
                    Data = mediaContents
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ContentPreviewListDto>>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<ContentDetailsDto>> GetFullContentsById(int id)
        {
            try
            {
                var query = _mediaContentRepository.GetAllQueryable(x => x.Id == id, y => y.Profile!);
                var mediaContents = await query.Select(x => new ContentDetailsDto
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    ProfileName = x.Profile!.Name,
                    ContentType = x.ContentType,
                    FullData = x.FullData
                }).FirstAsync();

                if (mediaContents == null)
                {
                    return new ResponseModel<ContentDetailsDto>
                    {
                        Status = false,
                        Message = StringResources.NotFound
                    };
                }

                return new ResponseModel<ContentDetailsDto>
                {
                    Status = true,
                    Message = StringResources.Success,
                    Data = mediaContents
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ContentDetailsDto>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }        

        public async Task<ResponseModel<object>> UploadWithPreview(MediaContentInDto request)
        {
            try
            {
                if (request.MediaFile == null || request.MediaFile.Length == 0)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = StringResources.NoFileFound
                    };
                }

                var fullPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + Path.GetExtension(request.MediaFile.FileName));

                using (var fs = System.IO.File.Create(fullPath))
                {
                    await request.MediaFile.CopyToAsync(fs);
                }

                var mediaInfo = await FFProbe.AnalyseAsync(fullPath);
                var durationSeconds = mediaInfo.Duration.TotalSeconds;

                if (durationSeconds < 10)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = "Video must be at least 10 seconds long."
                    };
                }

                var random = new Random();
                var startSeconds = random.Next(0, (int)(durationSeconds - 10));

                var previewPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".mp4");

                // Extract 10-second preview
                await FFMpegArguments
                    .FromFileInput(fullPath, true, options => options.Seek(TimeSpan.FromSeconds(startSeconds)))
                    .OutputToFile(previewPath, true, options => options
                        .WithDuration(TimeSpan.FromSeconds(10))
                        .WithFastStart()
                        .WithVideoCodec("libx264")
                        .WithAudioCodec("aac"))
                    .ProcessAsynchronously();

                if (!System.IO.File.Exists(previewPath))
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = "Failed to create preview."
                    };
                }

                var fullData = await System.IO.File.ReadAllBytesAsync(fullPath);
                var previewData = await System.IO.File.ReadAllBytesAsync(previewPath);

                System.IO.File.Delete(fullPath);
                System.IO.File.Delete(previewPath);

                // Save to DB
                var media = new MediaContent
                {
                    ProfileId = request.ProfileId,
                    FileName = request.MediaFile.FileName,
                    ContentType = request.MediaFile.ContentType,
                    PreviewData = previewData,
                    FullData = fullData
                };

                await _mediaContentRepository.AddAsync(media);
                await _mediaContentRepository.CompleteAsync();

                return new ResponseModel<object>
                {
                    Status = true,
                    Message = StringResources.CreateSuccess
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<object>> UploadWithPreviewV2(MediaContentInDto request)
        {
            try
            {
                if (request.MediaFile == null || request.MediaFile.Length == 0)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = StringResources.NoFileFound
                    };
                }

                var profile = await _profileRepository.GetAsync(request.ProfileId);

                if (profile == null)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = StringResources.Forbidden
                    };
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                var uniqueFileName = $"{Guid.NewGuid()}_{request.ProfileId}.mp4";
                var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save original file to disk
                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    await request.MediaFile.CopyToAsync(fs);
                }

                var mediaInfo = await FFProbe.AnalyseAsync(fullPath);
                var durationSeconds = mediaInfo.Duration.TotalSeconds;

                if (durationSeconds < 10)
                {
                    System.IO.File.Delete(fullPath); // Clean up original if not valid
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = "Video must be at least 10 seconds long."
                    };
                }

                var random = new Random();
                var startSeconds = random.Next(0, (int)(durationSeconds - 10));

                var previewFileName = $"{Guid.NewGuid()}_preview_{request.ProfileId}.mp4";
                var previewPath = Path.Combine(uploadsFolder, previewFileName);

                // Extract 10-second preview
                await FFMpegArguments
                    .FromFileInput(fullPath, true, options => options.Seek(TimeSpan.FromSeconds(startSeconds)))
                    .OutputToFile(previewPath, true, options => options
                        .WithDuration(TimeSpan.FromSeconds(10))
                        .WithFastStart()
                        .WithVideoCodec("libx264")
                        .WithAudioCodec("aac"))
                    .ProcessAsynchronously();

                if (!System.IO.File.Exists(previewPath))
                {
                    System.IO.File.Delete(fullPath);
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = "Failed to create preview."
                    };
                }

                var previewData = await System.IO.File.ReadAllBytesAsync(previewPath);
                System.IO.File.Delete(previewPath);

                // Save to DB
                var media = new MediaContent
                {
                    ProfileId = request.ProfileId,
                    FileName = request.MediaFile.FileName,
                    ContentType = request.MediaFile.ContentType,
                    FullPath = $"/uploads/{uniqueFileName}",
                    PreviewPath = $"/uploads/{previewFileName}",
                    PreviewData = previewData
                };

                profile.TotalMediaContents += 1;
                profile.UpdatedAt = DateTime.UtcNow;

                await _mediaContentRepository.AddAsync(media);
                await _profileRepository.UpdateAsync(profile);
                await _mediaContentRepository.CompleteAsync();

                return new ResponseModel<object>
                {
                    Status = true,
                    Message = StringResources.CreateSuccess
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<object>> UpdateContentIsFavourite(int id, bool isFavourite)
        {
            try
            {
                var content = await _mediaContentRepository.GetAsync(id);

                if (content == null)
                {
                    return new ResponseModel<object>
                    {
                        Status = false,
                        Message = StringResources.Forbidden
                    };
                }

                content.IsFavourite = isFavourite;
                content.UpdatedAt = DateTime.UtcNow;

                await _mediaContentRepository.UpdateAsync(content);
                await _mediaContentRepository.CompleteAsync();

                return new ResponseModel<object>
                {
                    Status = true,
                    Message = StringResources.UpdateSuccess
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }
    }
}
