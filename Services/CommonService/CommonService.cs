using HomeProject.Models.Response;

namespace HomeProject.Services.CommonService
{
    public class CommonService : ICommonService
    {
        public CommonService()
        {

        }
        public ResponseModel<byte[]> GenerateStringImage(string name)
        {
            try
            {
                throw new NotImplementedException("This method is not implemented yet.");
            }
            catch (Exception ex)
            {
                return new ResponseModel<byte[]>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }
    }
}
