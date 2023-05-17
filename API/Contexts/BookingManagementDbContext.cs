using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Contexts;

public class BookingManagementDbContext : DbContext
{
    public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : base(options)
    {

    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder Builder)
    {
        base.OnModelCreating(Builder);

        Builder.Entity<Employee>().HasIndex(e => new
        {
            e.Nik,
            e.Email,
            e.PhoneNumber
        }).IsUnique();

        //Relasi antara Education dan University (1 to many)
        Builder.Entity<Education>()
            .HasOne(u => u.University)
            .WithMany(e => e.Educations)
            .HasForeignKey(e => e.UniversityGuid);

        //Relasi antara Education dan Employee (1 to 1)
        Builder.Entity<Education>()
            .HasOne(e => e.Employee)
            .WithOne(e => e.Education)
            .HasForeignKey<Education>(e => e.Guid);

        //Relasi antara Employee dan Account (1 to 1)
        Builder.Entity<Account>()
            .HasOne(e => e.Employee)
            .WithOne(a => a.Account)
            .HasForeignKey<Account>(e => e.Guid);

        //Relasi antara rooms dan bookings (1 to many)
        Builder.Entity<Booking>()
            .HasOne(r => r.Room)
            .WithMany(b => b.Bookings)
            .HasForeignKey(r => r.RoomGuid);

        //Relasi antara Account dan AccountRoles (1 to many)
        Builder.Entity<AccountRole>()
            .HasOne(a => a.Account)
            .WithMany(ar => ar.AccountRoles)
            .HasForeignKey(ar => ar.AccountGuid);


        //Relasi antara role dan AccountRoles (1 to many)
        Builder.Entity<Role>()
            .HasMany(a => a.AccountRoles)
            .WithOne(r => r.Role)
            .HasForeignKey(a => a.RoleGuid);

        //Relasi antara employee dan bookings (1 to many)
        Builder.Entity<Booking>()
            .HasOne(e => e.Employee)
            .WithMany(b => b.Bookings)
            .HasForeignKey(r => r.EmployeeGuid);
    }
}
