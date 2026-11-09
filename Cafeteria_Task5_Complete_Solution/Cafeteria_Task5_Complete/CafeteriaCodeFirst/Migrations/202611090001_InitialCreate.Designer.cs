using CafeteriaCodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CafeteriaCodeFirst.Migrations;

[DbContext(typeof(CafeteriaDbContext))]
[Migration("202611090001_InitialCreate")]
public partial class InitialCreate
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        SnapshotModel.Configure(modelBuilder);
    }
}
