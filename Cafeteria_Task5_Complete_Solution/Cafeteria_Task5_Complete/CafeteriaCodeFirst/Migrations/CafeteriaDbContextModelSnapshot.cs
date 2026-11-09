using CafeteriaCodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CafeteriaCodeFirst.Migrations;

[DbContext(typeof(CafeteriaDbContext))]
public class CafeteriaDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        SnapshotModel.Configure(modelBuilder);
    }
}
