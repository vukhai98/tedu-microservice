## Command Migration:
add-Migration UpdateOrderTable -p Ordering.Infrastructure -StartupProject Ordering.API -OutputDir Persistence/Migrations

update-database -StartupProject Ordering.API

EntityFrameworkCore\Add-Migration UpdateOrderTable -p Ordering.Infrastructure -StartupProject Ordering.API -OutputDir Persistence/Migrations
