

namespace Amap
{
    public abstract class AmapHttpResponse
    {
        /// <summary>
        /// 返回结果状态值
        /// </summary>
        /// <remarks>
        /// 返回值为 0 或 1，0 表示请求失败；1 表示请求成功。
        /// </remarks>
        public int Status { get; set; }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        /// <remarks>
        /// 当 status 为 0 时，info 会返回具体错误原因，否则返回“OK”。
        /// </remarks>
        public string Info { get; set; }

        /// <summary>
        /// 返回状态码
        /// </summary>
        public string InfoCode { get; set; }

        public bool IsSuccess()
        {
            return Status == 1;
        }

        public string GetErrorMessage()
        {
            return this.Info;
        }
    }
}
