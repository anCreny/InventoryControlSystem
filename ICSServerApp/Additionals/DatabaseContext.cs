using ICSServerApp.Models;
using ICSServerApp.Models.Database;
using Microsoft.EntityFrameworkCore;
namespace ICSServerApp.Additionals;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Goal> Goals { get; set; } = null!;
    public DbSet<Cell> Cells { get; set; } = null!;
    public DbSet<DayTask> DayTasks { get; set; } = null!;
    public DbSet<CellsGoalLink> Links { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().HasData(
            new[]{
            new User()
            {
                Id = 1,
                FullName = "Creny",
                Login = "admin",
                Password = "admin",
                AccessRight = "admin"
            },
            new User
            {
                Id = 2,
                FullName = "Кожухарь Владимир Сергеевич",
                Login = "Вова",
                Password = "123456",
                AccessRight = "operator"
            }
            }
            );

        modelBuilder.Entity<DayTask>().HasData(
            new[]
            {
                new DayTask{ Id = 1, PaintsNVarnishes = 3, Wood = 1, StartTime = "9:00", Type = GoalType.Input},
                new DayTask{ Id = 2, PaintsNVarnishes = 0, Wood = 2, StartTime = "12:20", Type = GoalType.Output}
            }
        );
        
        

        modelBuilder.Entity<Cell>().HasData(
            new[]
            {
                new Cell{ Id = 1, Li = 'A', Ni = 1, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 2, Li = 'A', Ni = 2, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 3, Li = 'A', Ni = 3, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 4, Li = 'A', Ni = 4, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 5, Li = 'A', Ni = 5, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                
                new Cell{ Id = 6, Li = 'B', Ni = 1, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 7, Li = 'B', Ni = 2, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 8, Li = 'B', Ni = 3, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 9, Li = 'B', Ni = 4, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 10, Li = 'B', Ni = 5, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                
                new Cell{ Id = 11, Li = 'C', Ni = 1, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 12, Li = 'C', Ni = 2, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 13, Li = 'C', Ni = 3, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 14, Li = 'C', Ni = 4, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 15, Li = 'C', Ni = 5, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                
                new Cell{ Id = 16, Li = 'D', Ni = 1, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 17, Li = 'D', Ni = 2, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 18, Li = 'D', Ni = 3, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 19, Li = 'D', Ni = 4, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 20, Li = 'D', Ni = 5, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty},
                
                new Cell{ Id = 21, Li = 'E', Ni = 1, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 22, Li = 'E', Ni = 2, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 23, Li = 'E', Ni = 3, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 24, Li = 'E', Ni = 4, Type = CellType.Store, ProductType = null, CellStatus = CellStatus.Empty},
                new Cell{ Id = 25, Li = 'E', Ni = 5, Type = CellType.Path, ProductType = null, CellStatus = CellStatus.Empty}

            }
        );
    }
}