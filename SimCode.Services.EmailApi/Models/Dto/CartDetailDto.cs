﻿namespace SimCode.Services.EmailApi.Models.Dto
{
    public class CartDetailDto
    {
        public int CartDetailId { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDto CartHeader { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
