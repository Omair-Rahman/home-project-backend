using HomeProject.Models.Response;

namespace HomeProject.Services.CommonService
{
    public interface ICommonService
    {
        ResponseModel<byte[]> GenerateStringImage(string name);
    }
}
