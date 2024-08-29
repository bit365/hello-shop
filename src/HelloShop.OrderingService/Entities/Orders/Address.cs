// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Entities.Orders
{
    public record Address
    {
        /// <summary>
        /// 国家
        /// </summary>
        public required string Country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public required string State { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public required string Street { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public required string ZipCode { get; set; }
    }
}
