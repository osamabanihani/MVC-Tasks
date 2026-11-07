using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using CafeteriaCodeFirst.Data;
namespace CafeteriaCodeFirst.Migrations;
[DbContext(typeof(CafeteriaDbContext))]
public partial class CafeteriaDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
        => SnapshotModel.Configure(modelBuilder);
}
