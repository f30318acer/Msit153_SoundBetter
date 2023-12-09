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
        public int ProjectCount { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        public decimal? ProjectPrice { get; set; }
    }
}


