namespace HelloShop.ProductService.PermissionProviders;

 public static class CatalogPermissions
 {
     public const string GroupName = "Catalog";

     public static class Products
     {
         public const string Default = GroupName + ".Products";
         public const string Details = Default + ".Details";
         public const string Create = Default + ".Create";
         public const string Update = Default + ".Update";
         public const string Delete = Default + ".Delete";
     }

     public static class Brands
     {
         public const string Default = GroupName + ".Brands";
         public const string Details = Default + ".Details";
         public const string Create = Default + ".Create";
         public const string Update = Default + ".Update";
         public const string Delete = Default + ".Delete";
     }
 }