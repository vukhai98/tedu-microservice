## Command Migration:
add-Migration Initdataproject -p Portal.Infrastructure -StartupProject Portal.API -OutputDir Persistence/Migrationsad

update-database -StartupProject Portal.API