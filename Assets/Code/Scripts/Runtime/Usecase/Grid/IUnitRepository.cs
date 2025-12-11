public interface IUnitRepository
{
    public UnitDto GetManufactures(int id);

    public UnitDto GetWeapons(int id);

    public UnitDto GetXenosites(int id);
}