// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace MultiTenancySample.SchemaIsolationService.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
    }
}