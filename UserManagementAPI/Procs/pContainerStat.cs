namespace UserManagementAPI.Procs
{
    public class ContainerBio
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Ref { get; set; } = string.Empty;
    }
    public record recContainerStat
    {
        public List<pContainerStat> statList { get; set; }
        public ContainerBio cBio { get; set; }
    }
    public class pContainerStat
    {
        public pContainerStat()
        {

        }
        public int Id { get; set; }
        public int containerId { get; set; }
        public int statId { get; set; }
        public string statName { get; set; }
        public decimal statValue { get; set; }



    }
}
