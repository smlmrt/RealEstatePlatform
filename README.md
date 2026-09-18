# RealEstatePlatform – Gayrimenkul İlan Platformu

Harita entegrasyonu ve görsel optimizasyonu bulunan, katmanlı mimariyle geliştirilmiş bir ASP.NET Core MVC emlak ilan platformu.

## Özellikler

- İlan listeleme ve detay sayfası
- Filtreleme: şehir, minimum/maksimum fiyat, oda sayısı, otopark
- Çoklu fotoğraf yüklemeli ilan ekleme (vitrin fotoğrafı desteği)
- **ImageSharp** ile görsel işleme:
  - Maksimum 1200 px genişliğe orantılı yeniden boyutlandırma
  - Sağ alt köşeye yarı saydam filigran (watermark)
  - %75 kalite ile JPEG sıkıştırma
- **Leaflet + OpenStreetMap** ile ilanın konumunu harita üzerinde gösterme (enlem/boylam)
- Generic Repository deseni ve servis katmanı

## Teknolojiler

- .NET 10 / ASP.NET Core MVC
- Entity Framework Core 10 (SQLite)
- SixLabors.ImageSharp
- Leaflet.js, OpenStreetMap
- Bootstrap

## Mimari

```
RealEstatePlatform/
├── RealEstatePlatform.Core/        # Entity'ler (Property, Location, PropertyImage, BaseEntity), IRepository
├── RealEstatePlatform.DataAccess/  # RealEstateDbContext, Generic Repository, migration'lar
├── RealEstatePlatform.Business/    # IPropertyService / PropertyService (filtreleme mantığı)
└── RealEstatePlatform.Web/         # MVC controller'lar, view'lar, ImageHelper, wwwroot/uploads
```

## Veri Modeli

- **Property:** `Title`, `Description`, `Price`, `RoomCount`, `SquareMeters`, `HasParking`, `Location`, `Images`
- **Location:** `City`, `District`, `Latitude`, `Longitude`
- **PropertyImage:** `ImagePath`, `IsMainImage`

## Sayfalar

| Adres | Açıklama |
|-------|----------|
| `/` veya `/Property` | İlan listesi ve filtreler (`?city=&minPrice=&maxPrice=&roomCount=&hasParking=`) |
| `/Property/Create` | Yeni ilan ekleme |
| `/Property/Detail/{id}` | İlan detayı, fotoğraflar ve harita |

## Kurulum ve Çalıştırma

```bash
git clone https://github.com/smlmrt/RealEstatePlatform.git
cd RealEstatePlatform
dotnet restore
dotnet ef database update --project RealEstatePlatform.DataAccess --startup-project RealEstatePlatform.Web
cd RealEstatePlatform.Web && dotnet run
```

Uygulama `http://localhost:5174` adresinde açılır.
