using HomeProject.Models.Response;
using HomeProject.Models.Request.MediaContent;
using HomeProject.Constants;
using HomeProject.Models.Domain;
using HomeProject.Repositories.MediaContentRepository;
using FFMpegCore;

namespace HomeProject.Services.MediaContentService
{
    public class MediaContentService : IMediaContentService
    {
        private readonly IMediaContentRepository _mediaContentRepository;

        public MediaContentService(IMediaContentRepository mediaContentRepository)
        {
            _mediaContentRepository = mediaContentRepository;
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
                var result = await FFMpegArguments
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
    }
}
