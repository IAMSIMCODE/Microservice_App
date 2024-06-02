using System.ComponentModel.DataAnnotations.Schema;
using SimCode.Services.ShoppingCartApi.Models.Dto;

namespace SimCode.Services.ShoppingCartApi.Models
{
    public class CartDetail
    {
        public int CartDetailId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader? CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
    }
}
