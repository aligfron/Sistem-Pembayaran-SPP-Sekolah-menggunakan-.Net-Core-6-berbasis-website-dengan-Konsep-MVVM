namespace SPP_Sekolah.AddOns

{
    public class BreadcrumbItem
    {
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
