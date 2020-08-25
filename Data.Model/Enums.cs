using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public enum RoleType
    {
        Undefined,
        User = 1,
        Seller = 100,
        Manager = 200,
        Supervisor = 500,
        Admin = 999
    }
    public enum Status
    {
        Active,
        Pending,
        InActive,
        Blocked
    }

    public enum ImageType
    {
        CompanyLogo,
        ShopLogo,
        ProductImage,
        GalleryImage
    }
    public enum ImageSize
    {
        Small = 50,
        Medium = 150,
        Large = 450
    }

    public enum OrderBy
    {
        RatingDesc,
        RatingAsc,
        NameDesc,
        NameAsc,
        PriceDesc,
        PriceAsc
    }

    public enum Available
    {
        Available,
        NotAvailable,
        OnDemand
    }
}
