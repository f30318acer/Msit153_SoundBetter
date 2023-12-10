using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.ViewModels
{
    /// <summary>
    /// 購物車用Model
    /// </summary>
    public class ShoppingCartVM
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string ProductDesc { get; set; }

        /// <summary>
        /// 商品數量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        public decimal? ProductPrice { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ProductThumbnailPath { get; set; }

        /// <summary>
        /// 商品起始日期
        /// </summary>
        public DateTime? ProductStartDate { get; set; }

        /// <summary>
        /// 商品起始日期
        /// </summary>
        public DateTime? ProductEndDate { get; set; }

        /// <summary>
        /// 商品狀態
        /// <para>0000 : 正常</para>
        /// <para>9999 : 失敗</para>
        /// </summary>
        public int ProductStatus { get; set; }
    }
}


